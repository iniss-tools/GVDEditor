using System.Globalization;
using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Formats;

/// <summary>
/// Nacitanie a ulozenie celeho grafikonu - vsetkych jeho suborov naraz.
/// </summary>
internal static class GrafikonRepository
{
    /// <summary>
    /// Nacita grafikon do noveho dokumentu. Otvoreny grafikon sa nemeni; varovania zbiera <see cref="Domain.Analysis.LoadWarnings" />.
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    /// <param name="gvd">hlavicka grafikonu (Grafikon.txt)</param>
    /// <param name="workspace">instalacia INISS, do ktorej grafikon patri</param>
    /// <exception cref="FormatException">subor grafikonu je chybny</exception>
    public static GrafikonDocument Load(string path, GVDInfo gvd, InissWorkspace workspace, LoadWarnings warnings)
    {
        var document = new GrafikonDocument();
        var context = new GrafikonContext(workspace, document, default) { Warnings = warnings };

        // poradie: vlastne stanice pred trasami vlakov, logicke tabule pred kolajami, katalogove pred textami
        document.CustomStations = new ExBindingList<Station>(CustomStationsFile.Read(path, gvd, workspace.Stations));

        var (tabtabs, catalogs, physicals, logicals) = TablesFile.Read(path);
        document.TabTabs = new ExBindingList<TableTabTab>(tabtabs);
        document.TableCatalogs = new ExBindingList<TableCatalog>(catalogs);
        document.TablePhysicals = new ExBindingList<TablePhysical>(physicals);
        document.TableLogicals = new ExBindingList<TableLogical>(logicals);

        document.Operators = new ExBindingList<Operator>(OperatorsFile.Read(path)) { FireEventOnSort = true };
        document.Tracks = new ExBindingList<Track>(TracksFile.Read(path, document.TableLogicals, warnings)) { FireEventOnSort = true };
        // kolaje s rovnakym nastupistom zdielaju jednu instanciu (TracksFile.Read)
        document.Platforms = new ExBindingList<Platform>(document.Tracks.Select(track => track.Platform).Distinct().ToList());

        (document.ReportVariants, document.ReportTypes, document.LocalLanguages) = CategoriFile.ReadLocal(path, workspace.Languages, warnings);

        var allSounds = new List<FyzSound>();
        foreach (var language in document.LocalLanguages)
            allSounds.AddRange(language.IsBasic ? workspace.Sounds : RawBankParser.ReadFyzZvukFile(workspace.RawBankDir, language));

        try
        {
            document.Radenia = RazeniFile.Read(path, allSounds, context);
        }
        catch (FileNotFoundException)
        {
            // grafikon bez radeni
        }

        document.Trains = new TrainBindingList(TrainsFile.Read(path, context)) { TrainNames = context.Workspace.TrainNames };
        document.TableTexts = new ExBindingList<TableText>(TTextsFile.Read(path, document.Trains, document.TableCatalogs));

        var modeTabs = ModeTabsFile.Read(path);
        document.TableFonts = new ExBindingList<TableFont>(modeTabs.Fonts);
        document.TableFontDir = modeTabs.FontDir;
        document.ModeTabsSections = modeTabs.Sections;

        return document;
    }

    /// <summary>
    /// Ulozi grafikon. Zapis je transakcny - ked niektory subor zlyha, vsetky subory grafikonu sa vratia
    /// do stavu pred ulozenim (inak by sa grafikon pri dalsom otvoreni hlasil ako chybny).
    /// </summary>
    /// <param name="path">priecinok grafikonu</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="context">instalacia, ukladany dokument a jazyk hlaviciek suborov</param>
    /// <exception cref="InvalidOperationException">zapis zlyhal; sprava hovori, ci sa subory podarilo vratit</exception>
    public static void Save(string path, GVDInfo gvd, GrafikonContext context)
    {
        var document = context.Document;

        var transaction = new FileTransaction(path);
        try
        {
            TrainsFile.Write(path, document.Trains, gvd, context);
            RazeniFile.Write(path, document.Radenia, document.LocalLanguages, document.ReportVariants);

            TablesFile.Write(path, document.TabTabs, document.TableCatalogs, document.TablePhysicals, document.TableLogicals);
            TTextsFile.Write(path, document.TableTexts);
            TracksFile.Write(path, document.Tracks);
            InfoGvdFile.Write(path, gvd);
            OperatorsFile.Write(path, document.Operators);
            ModeTabsFile.Write(path, document.TableFonts, document.TableFontDir, document.ModeTabsSections);
            CategoriFile.WriteLocal(path, document.ReportVariants, document.ReportTypes, document.LocalLanguages);
            CustomStationsFile.Write(path, document.CustomStations, gvd, context.CommentLanguage);
        }
        catch (Exception exception)
        {
            Log.Exception(exception);

            var message = transaction.TryRollback()
                ? string.Format(CultureInfo.CurrentCulture, Resources.FMain_Uloženie_grafikonu_zlyhalo_zmeny_boli_vrátené, exception.Message)
                : string.Format(CultureInfo.CurrentCulture, Resources.FMain_Uloženie_grafikonu_zlyhalo_a_nepodarilo_sa_obnoviť,
                    exception.Message, transaction.BackupPath);

            throw new InvalidOperationException(message, exception);
        }

        transaction.Commit();
    }

    /// <summary>
    /// Zalozi subory noveho, prazdneho grafikonu (Subor → Novy).
    /// </summary>
    /// <param name="path">priecinok noveho grafikonu</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="context">instalacia, novy dokument (<see cref="GrafikonDocument.CreateNew" />) a jazyk hlaviciek</param>
    /// <param name="dataDir">priecinok DATA (vyrovnavacia pamat stavovych diagramov)</param>
    /// <param name="stateDgm">predloha stavoveho diagramu</param>
    public static void CreateNew(string path, GVDInfo gvd, GrafikonContext context, string dataDir, StateDgmTemplate stateDgm)
    {
        var document = context.Document;

        TrainsFile.Write(path, document.Trains, gvd, context);
        TablesFile.Write(path, document.TabTabs, document.TableCatalogs, document.TablePhysicals, document.TableLogicals);
        TTextsFile.Write(path, document.TableTexts);
        TracksFile.Write(path, document.Tracks);
        InfoGvdFile.Write(path, gvd);
        OperatorsFile.Write(path, document.Operators);
        ModeTabsFile.Write(path, document.TableFonts, document.TableFontDir, document.ModeTabsSections);
        StateDgmFile.WriteTemplate(path, dataDir, stateDgm);
        CategoriFile.WriteLocal(path, document.ReportVariants, document.ReportTypes, document.LocalLanguages);
        RazeniFile.WriteRazeniDefault(path);
        RazeniFile.WriteDefault(path);
    }
}

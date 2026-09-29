using System.Collections;
using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;
using static ToolsCore.Iniss.Tools.PathUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Radenia vlakov (Razeni1.txt) a prazdny Razeni.txt.
/// </summary>
internal static class RazeniFile
{
    /// <summary>
    /// Nainicializuje informacie o radeniach vlakov
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="sounds">zvuky zo zvukovej banky</param>
    /// <returns>radenia vlakov</returns>
    /// <param name="context">instalacia INISS a dokument grafikonu</param>
    public static List<Radenie> Read(string path, List<FyzSound> sounds, GrafikonContext context)
    {
        var fileRazeni1 = CombinePath(path, FILE_RAZENI1)!;

        var radeniaList = new List<Radenie>();

        using var razeni1F = new CsvFileReader(fileRazeni1);
        var riadok = 1;
        var row = new CsvRow();

        Radenie? radenie = null;

        while (true)
        {
            var status = razeni1F.ReadRow(row);
            if (LineIsEmpty(status))
            {
                riadok++;
                continue;
            }

            if (LineIsEOF(status))
            {
                if (radenie != null)
                {
                    var sb = new StringBuilder();
                    foreach (var sound in radenie.Sounds) sb.Append(sound.Text + " ");

                    radenie.Text = sb.ToString().Trim();
                    radeniaList.Add(radenie);
                }

                break;
            }

            try
            {
                if (row[0].StartsWith('#'))
                {
                    if (radenie != null)
                    {
                        var sb = new StringBuilder();
                        foreach (var sound in radenie.Sounds) sb.Append(sound.Text + " ");

                        radenie.Text = sb.ToString().Trim();
                        radeniaList.Add(radenie);
                    }

                    radenie = new Radenie();
                    var array = row[0].Split(new[] { ':' }, 2);
                    radenie.CisloVlaku = array[0].Substring(1);
                    if (array.Length > 1 && array[1].Length > 0)
                    {
                        var id = ParseIntOrDefault(array[1], -1);
                        if (id == -1)
                        {
                            radenie = null;
                            riadok++;
                            continue;
                        }

                        radenie.DestStation = context.StationFromID(id.ToString(CultureInfo.InvariantCulture));
                    }

                    radenie.ChosenReports = ReportType.Parse(context.Document.ReportTypes, row[1], context.Document.ReportVariants);

                    // prazdne datumy = radenie plati bez obmedzenia (INISS datum nekontroluje)
                    var from = row.Count > 2 ? row[2].Trim() : "";
                    var to = row.Count > 3 ? row[3].Trim() : "";
                    if (from.Length == 0 && to.Length == 0)
                    {
                        radenie.Validity = null;
                        radenie.DatObm = "";
                    }
                    else
                    {
                        var validity = new ValidityPeriod(ParseDateOnlyAlts(from), ParseDateOnlyAlts(to));
                        radenie.Validity = validity;
                        var dateLimit = new DateLimit(validity.From, validity.To, insertMarks: false);
                        var bit = new BitArray((row.Count > 4 ? row[4] : "").Select(c => c == '1').ToArray());
                        radenie.DatObm = dateLimit.BitArrayToText(bit);
                    }
                }
                else
                {
                    if (radenie == null) throw new FormatException(Resources.Razeni_MissingBasics);
                    var file = row[0];
                    var array = file.Split('/');

                    FyzSound? zvuk = null;
                    if (array.Length >= 3)
                    {
                        var lang = context.Document.LocalLanguages.FirstOrDefault(jazyk => jazyk.Key.EqualsIgnoreCase(array[0]));
                        if (lang == null)
                            throw new FormatException($"Jazyk {array[0]} neexistuje.");

                        // INISS odkaz rozlisuje podla klucov skupiny a zvuku (nie nazvov), bez ohladu na velkost pismen
                        zvuk = sounds.FirstOrDefault(sound =>
                            array[1].EqualsIgnoreCase(sound.Group.Key) && array[2].EqualsIgnoreCase(sound.Key) && lang == sound.Group.Language);
                    }
                    else if (array.Length == 2)
                    {
                        // starsi dvojdielny zapis bez jazyka (Skupina/meno) - INISS ho pouzije pri kazdom jazyku;
                        // my ho priradime k zakladnemu jazyku, pripadne k prvemu, kde nahravka existuje
                        var candidates = sounds.Where(sound =>
                            array[0].EqualsIgnoreCase(sound.Group.Key) && array[1].EqualsIgnoreCase(sound.Key)).ToList();
                        zvuk = candidates.FirstOrDefault(sound => sound.Group.Language.IsBasic) ?? candidates.FirstOrDefault();
                    }
                    else
                        throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Razeni_BadSoundRef, file));

                    if (zvuk == null)
                    {
                        // chybajuca nahravka nezhodi cely grafikon - INISS ju tiez len preskoci
                        LoadWarnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.Razeni_SoundMissing, FILE_RAZENI1, riadok, file));
                        riadok++; // continue obchadza pocitadlo na konci cyklu - dalsie hlasenia by mali zle cislo riadka
                        continue;
                    }

                    radenie.Sounds.Add(zvuk);
                }
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, FORMAT_EX, FILE_RAZENI1, riadok) + e.Message, e);
            }

            riadok++;
        }

        return radeniaList;
    }

    /// <summary>
    /// Vytvori novy subor, ktory bude sluzit na ukladanie informacii o radeniach vlakov
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    public static void WriteDefault(string path) => File.Create(CombinePath(path, FILE_RAZENI1)!).Dispose();

    /// <summary>
    /// Zapise informacie o radeniach vlakov
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    /// <param name="radenia">radenia vlakov</param>
    /// <param name="languages">jazykove mutacie hlaseni</param>
    /// <param name="reportVariants">varianty hlaseni grafikonu (poradie urcuje pismeno typu)</param>
    public static void Write(string path, IEnumerable<Radenie> radenia, IEnumerable<FyzLanguage> languages, IList<ReportVariant> reportVariants)
    {
        var fileRazeni1 = CombinePath(path, FILE_RAZENI1)!;

        using var razeni1F = new CsvFileWriter(fileRazeni1);
        var otherLangs = new List<FyzLanguage>(2);
        foreach (var lang in languages)
            if (!lang.IsBasic)
                otherLangs.Add(lang);

        foreach (var radenie in radenia)
        {
            var row = new CsvRow();
            row.Insert(0,
                radenie.DestStation != null
                    ? $"#{radenie.CisloVlaku}:{radenie.DestStation.ID}"
                    : $"#{radenie.CisloVlaku}");
            var sb = new StringBuilder();

            // INISS berie variant podla poradia sekcii VARIANT_nn (KEY necita): prvy = velke pismeno, dalsi = male
            foreach (var reportType in radenie.ChosenReports)
            {
                foreach (var variant in reportType.Variants)
                {
                    var index = reportVariants.IndexOf(variant);
                    var first = index < 0 ? variant.Key == 0 : index == 0;
                    sb.Append(first ? reportType.Type.Char : reportType.Type.Char.ToLowerInvariant());
                }
            }

            row.Insert(1, sb.ToString());
            if (radenie.Validity is { } validity)
            {
                row.Insert(2, validity.From.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
                row.Insert(3, validity.To.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
                var dateRem = new DateLimit(validity.From, validity.To, insertMarks: false);
                var bits = dateRem.TextToBitArray(radenie.DatObm);
                row.Insert(4, BitArrayToString(bits));
            }
            else
            {
                // bez obdobia platnosti - zapise sa tak, ako bolo nacitane (prazdne polia)
                row.Insert(2, "");
                row.Insert(3, "");
                row.Insert(4, "");
            }
            razeni1F.WriteRow(row);

            var sbZvuk = new StringBuilder();
            var sbL1 = new StringBuilder();
            var sbL2 = new StringBuilder();
            var sbL3 = new StringBuilder();

            foreach (var sound in radenie.Sounds)
            {
                var rowsound = new CsvRow();
                rowsound.Insert(0, $"{sound.Language.Key}/{sound.Group.Key}/{sound.Key}");
                razeni1F.WriteRow(rowsound);
                if (sound.Language.IsBasic)
                {
                    sbZvuk.Append(sound.Name + " ");
                    sbL1.Append(sound.Text + " ");
                }

                switch (otherLangs.Count)
                {
                    case 1:
                    {
                        if (sound.Language == otherLangs[0]) 
                            sbL2.Append(sound.Text + " ");
                        break;
                    }
                    case 2:
                    {
                        if (sound.Language == otherLangs[0]) 
                            sbL2.Append(sound.Text + " ");

                        if (sound.Language == otherLangs[1]) 
                            sbL3.Append(sound.Text + " ");
                        break;
                    }
                }
            }

            var rowcomment = new CsvRow();
            rowcomment.Insert(0, $";{radenie.DatObm.Quote().UTFtoANSI()}");
            rowcomment.Insert(1, sbL1.ToString().Trim().Quote().UTFtoANSI());
            rowcomment.Insert(2, sbZvuk.ToString().Trim().Quote().UTFtoANSI());
            rowcomment.Insert(3, ""); //ich weiss nicht was ist das
            rowcomment.Insert(4, "");
            rowcomment.Insert(5, sbL2.ToString().Trim().Quote().UTFtoANSI());
            rowcomment.Insert(6, sbL3.ToString().Trim().Quote().UTFtoANSI());
            razeni1F.WriteRow(rowcomment);
        }
    }

    /// <summary>
    /// Vytvori novy subor, ktory bude sluzit na ukladanie informacii o radeniach vlakov
    /// </summary>
    /// <param name="path">cesta do priecinka s datami</param>
    public static void WriteRazeniDefault(string path) => File.Create(CombinePath(path, FILE_RAZENI)!).Dispose();
}

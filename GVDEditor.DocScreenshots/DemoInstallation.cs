using System.Globalization;
using System.Reflection;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.StateDgm;
using ToolsCore.Tools;

namespace GVDEditor.DocScreenshots;

/// <summary>
///     Fiktívna inštalácia INISS pre snímky do dokumentácie – stanica Dolné Mesto na vymyslenej trati.
///     Súbory zapisuje tými istými zapisovačmi ako GVDEditor pri založení a uložení grafikonu,
///     takže sú platné bez ručného písania. Stanice majú čísla 99xxxxx, mimo rozsah skutočných staníc.
/// </summary>
internal static class DemoInstallation
{
    public const string GvdDirName = "DolneMesto.2027";
    public const string ExeName = "INISS - Dolné Mesto.exe";

    private static readonly DateTime ValidFrom = new(2026, 12, 13);
    private static readonly DateTime ValidTo = new(2027, 12, 11);

    // stanice trate od západu na východ, odbočka z Dolného Mesta na juh
    private static readonly (string Id, string Name)[] StationList =
    [
        ("9900010", "Veľká Ves"),
        ("9900020", "Horné Lúky"),
        ("9900030", "Brezová Dolina"),
        ("9900100", "Dolné Mesto"),
        ("9900110", "Lipová"),
        ("9900120", "Podhradie"),
        ("9900130", "Sklené Pole"),
        ("9900140", "Hraničná"),
        ("9900040", "Kamenný Brod"),
        ("9900050", "Nové Záhorie"),
    ];

    // vlastná stanica - zastávka, pre ktorú zvuková banka nemá nahrávku (Stanice.txt)
    private static readonly (string Id, string Name)[] CustomStationList =
    [
        ("9900115", "Lesná zastávka"),
    ];

    private static readonly string[] TrainNames =["Brezovan", "Lipovan", "Podhradčan"];

    /// <summary>
    ///     Zostaví inštaláciu do <paramref name="root" /> (existujúci obsah zmaže) a vráti cestu k priečinku grafikonu.
    /// </summary>
    public static string Build(string root, List<string> log)
    {
        // cesta sa zobrazuje v titulku a v nastaveniach, preto predvolene C:\INISS ako v návodoch;
        // zmazať sa smie len priečinok, ktorý vytvoril harness (so značkou), nikdy skutočná inštalácia
        var marker = Path.Combine(root, ".docshots");
        if (Directory.Exists(root))
        {
            if (!File.Exists(marker))
                throw new InvalidOperationException(
                    $"Priečinok {root} existuje a nevytvoril ho harness – nezmaže sa. Zadaj iný cez --work.");
            Directory.Delete(root, true);
        }

        Directory.CreateDirectory(root);
        File.WriteAllText(marker, "Ukážková inštalácia INISS pre GVDEditor.DocScreenshots – pri ďalšom spustení sa zmaže.");

        var dataDir = Path.Combine(root, "DATA");
        var bankDir = Path.Combine(root, "RAWBANK");
        Directory.CreateDirectory(dataDir);
        Directory.CreateDirectory(bankDir);

        // INISS.exe len ako prázdny zástupca - GVDEditor z neho berie ponuku Spustiť
        File.WriteAllBytes(Path.Combine(root, ExeName), []);

        var languages = BuildSoundBank(bankDir);

        // zapisovače globálnych súborov pracujú s GlobData.DataDir - nastaví sa pred PrepareGlobalData
        SetGlobData(nameof(GlobData.INISSDir), root);
        SetGlobData(nameof(GlobData.DataDir), dataDir);
        SetGlobData(nameof(GlobData.RawBankDir), bankDir);

        TxtParser.WriteLanguages(languages);
        TxtParser.WriteTrainTypesDefaults();
        TxtParser.WriteZpozdeniDefault();

        // zvukový okruh stanice a testovací okruh TEST (sprístupní v INISSe tlačidlo Test/stop)
        File.WriteAllLines(Path.Combine(dataDir, FileConsts.FILE_AUDIO),
        [
            "9900100,Dolné Mesto,Dolné Mesto,Hlásenie,",
            "TEST,Test,Test,TestHlas,"
        ], ToolsCore.Tools.Encodings.Win1250);

        var dir = new DirList { DirName = GvdDirName, FullPath = Path.Combine(dataDir, GvdDirName), TablePort = 2, ReportPort = 3 };
        TxtParser.WriteDirList([dir]);

        GlobData.PrepareGlobalData(root);

        var home = Station("9900100");
        var gvd = new GVDInfo
        {
            ThisStation = new Station(home.ID, home.Name),
            StartValidData = ValidFrom,
            EndValidData = ValidTo,
            StartValidTimeTable = ValidFrom,
            EndValidTimeTable = ValidTo,
            CreateData = ValidFrom.AddMonths(-1),
            IsRegionText = true,
            Category = 1,
            VLIndex = -1,
            OnlyCityVLIndex = -999,
        };

        Directory.CreateDirectory(dir.FullPath);
        WriteNewGvd(dir.FullPath, gvd);
        FillGvd(dir.FullPath, gvd);
        Verify(dir.FullPath, gvd, log);

        return dir.FullPath;
    }

    /// <summary>
    ///     Zvuková banka bez nahrávok - len zoznamy FYZBANK/FYZZVUK, z ktorých GVDEditor berie stanice a názvy vlakov.
    /// </summary>
    private static List<FyzLanguage> BuildSoundBank(string bankDir)
    {
        var sk = new FyzLanguage("SK", "Slovenčina", "FYZZVUK.DAT", @"SK\") { IsBasic = true };
        var gb = new FyzLanguage("GB", "Angličtina", "FYZZVUK.DAT", @"GB\");

        sk.Groups =
        [
            Group(sk, "R1", StationList.Select(s => (s.Id, s.Name))),
            Group(sk, "V8", TrainNames.Select(n => (n, n))),
            Group(sk, "Dodatky",
            [
                ("D1001", "Vlak je vedený náhradnou autobusovou dopravou."),
                ("D1002", "Na nástupišti prosíme dodržiavať bezpečnú vzdialenosť."),
            ]),
            Group(sk, "Poz1", [("0100", "koľaj 1"), ("0200", "koľaj 2"), ("0300", "koľaj 3"), ("0400", "koľaj 4")]),
            // nahrávky dopravcov sa volajú presne ako dopravca vo Vlastnik.txt
            Group(sk, "Dopravca", [("Regionálna železnica, a.s.", "Regionálna železnica"), ("Expres Línia, s.r.o.", "Expres Línia")]),
            Group(sk, "Slova", [("prich", "príde"), ("odch", "odíde"), ("kolaj", "na koľaj"), ("cislo", "číslo")]),
            // radenie: „Za rušňom sú radené: vozeň prvej triedy číslo jeden a vozne druhej triedy číslo dva až šesť.“
            Group(sk, "Poz7", [("zalok", "Za rušňom sú radené:"), ("nakonci", "Na konci vlaku je radený")]),
            Group(sk, "VOZY1", [("v1", "vozeň prvej triedy"), ("rest", "reštauračný vozeň")]),
            Group(sk, "VOZY4M", [("v2", "a vozne druhej triedy")]),
            Group(sk, "CISLO1", [("1", "jeden"), ("2", "dva")]),
            Group(sk, "CISLO9", [("6", "až šesť."), ("8", "až osem.")]),
        ];
        gb.Groups =
        [
            Group(gb, "R1", StationList.Select(s => (s.Id, s.Name))),
            Group(gb, "Slova", [("arr", "arrives"), ("dep", "departs")]),
        ];

        var languages = new List<FyzLanguage> { sk, gb };
        foreach (var language in languages)
        {
            Directory.CreateDirectory(Path.Combine(bankDir, language.RelativePath));
            RawBankParser.WriteFyzZvukFile(bankDir, language);
        }

        RawBankParser.WriteFyzBankFile(bankDir, languages);
        return languages;
    }

    private static FyzGroup Group(FyzLanguage language, string key, IEnumerable<(string Key, string Text)> sounds)
    {
        var group = new FyzGroup(language, key, key, key + @"\");
        foreach (var (soundKey, text) in sounds)
            group.Sounds.Add(new FyzSound(group, soundKey, soundKey, soundKey + ".WAV", "", text, 1200));
        return group;
    }

    /// <summary>
    ///     Rovnaké súbory, aké vytvorí FMain pri prvom otvorení nového grafikonu.
    /// </summary>
    private static void WriteNewGvd(string path, GVDInfo gvd)
    {
        TxtParser.WriteTrains(path, [], gvd, ReportVariant.GetDefaultValues());
        TxtParser.WriteTables(path, [], [], [], []);
        TxtParser.WriteTTexts(path, []);
        TxtParser.WriteTracks(path, [Track.None]);
        TxtParser.WriteInfoGVD(path, gvd);
        TxtParser.WriteOperators(path, [Operator.None]);
        TxtParser.WriteModeTabs(path, [], GlobData.TableFontDir);
        TxtParser.WriteStateDgm(path, StateDgmTemplate.Slovak);
        TxtParser.WriteLocalCategori(path, ReportVariant.GetDefaultValues(), ReportType.GetDefaultValuesSK(), GlobData.Languages);
        TxtParser.WriteRazeniDefault(path);
        TxtParser.WriteRazeni1Default(path);
    }

    /// <summary>
    ///     Naplní grafikon koľajami, dopravcami a vlakmi a uloží ho rovnako ako Súbor → Uložiť.
    /// </summary>
    private static void FillGvd(string path, GVDInfo gvd)
    {
        var p1 = new Platform("1", "Nástupište 1", "01");
        var p2 = new Platform("2", "Nástupište 2", "02");
        var tracks = new List<Track>
        {
            Track.None,
            new("1", "1", "Koľaj 1", p1, "0100", "1"),
            new("2", "2", "Koľaj 2", p1, "0200", "2"),
            new("3", "3", "Koľaj 3", p2, "0300", "3"),
            new("4", "4", "Koľaj 4", p2, "0400", "4"),
        };

        var regional = new Operator(1, "Regionálna železnica, a.s.");
        var express = new Operator(2, "Expres Línia, s.r.o.");
        var operators = new List<Operator> { Operator.None, regional, express };

        var skOnly = GlobData.Languages.Where(l => l.IsBasic).ToList();
        var all = GlobData.Languages.ToList();

        string[] west = ["9900010", "9900020", "9900030"];
        string[] east = ["9900110", "9900120", "9900130", "9900140"];
        string[] south = ["9900040", "9900050"];
        // osobné vlaky do Podhradia zastavujú aj na vlastnej zastávke Lesná
        string[] local = ["9900110", "9900115", "9900120"];

        var trains = new List<Train>
        {
            Tr("Os", "3601", "", Routing.Vychadzajuci, null, "05:12", [], south, tracks[2], regional, "ide v 1-5", skOnly),
            Tr("Os", "4201", "", Routing.Vychadzajuci, null, "05:50", [], local, tracks[4], regional, "ide denne", skOnly),
            Tr("R", "811", "Brezovan", Routing.Prechadzajuci, "06:02", "06:04", west, east, tracks[1], regional, "ide denne", all),
            Tr("Os", "3602", "", Routing.Konciaci, "06:40", null, south.Reverse().ToArray(), [], tracks[2], regional, "ide v 1-5", skOnly),
            Tr("Os", "4202", "", Routing.Konciaci, "07:05", null, local.Reverse().ToArray(), [], tracks[4], regional, "ide denne", skOnly),
            Tr("Os", "3603", "", Routing.Vychadzajuci, null, "07:15", [], south, tracks[2], regional, "ide denne", skOnly),
            Tr("REX", "1911", "", Routing.Prechadzajuci, "07:48", "07:50", west, east[..3], tracks[1], regional, "ide v 1-5", skOnly),
            Tr("Ex", "521", "Lipovan", Routing.Prechadzajuci, "09:10", "09:12", west, east, tracks[1], express, "ide denne", all,
                t => { t.IsMiestenkovy = true; t.IsMedzistatny = true; }),
            Tr("IC", "501", "Podhradčan", Routing.Prechadzajuci, "11:30", "11:32", west, east[..2], tracks[1], express, "ide v 5,7", all,
                t => { t.IsDialkovy = true; t.IsMiestenkovy = true; }),
            Tr("Os", "3605", "", Routing.Vychadzajuci, null, "12:40", [], south, tracks[2], regional, "ide v 6,7", skOnly),
            Tr("Os", "3606", "", Routing.Konciaci, "13:55", null, south.Reverse().ToArray(), [], tracks[2], regional, "ide denne", skOnly,
                t => t.IsNizkopodlazny = true),
            Tr("REX", "1912", "", Routing.Prechadzajuci, "16:20", "16:22", east[..3].Reverse().ToArray(), west.Reverse().ToArray(), tracks[3],
                regional, "ide v 1-5", skOnly),
            Tr("R", "812", "Brezovan", Routing.Prechadzajuci, "18:31", "18:33", east.Reverse().ToArray(), west.Reverse().ToArray(), tracks[3],
                regional, "ide denne", all),
            Tr("Ex", "522", "Lipovan", Routing.Prechadzajuci, "20:05", "20:07", east.Reverse().ToArray(), west.Reverse().ToArray(), tracks[3],
                express, "ide denne", all, t => { t.IsMiestenkovy = true; t.IsMedzistatny = true; }),
        };
        gvd.TrainCount = trains.Count;

        GlobData.Tracks = new ExControls.ExBindingList<Track>(tracks);
        GlobData.Operators = new ExControls.ExBindingList<Operator>(operators);
        GlobData.CustomStations = new ExControls.ExBindingList<Station>(
            CustomStationList.Select(s => new Station(s.Id, s.Name, IsCustom: true)).ToList());

        // ID vlaku je jeho poradie v Export3A.TXT - odkazujú naň texty tabúľ
        for (var i = 0; i < trains.Count; i++)
            trains[i].ID = i + 1;

        var tables = new DemoTables(trains, tracks);
        GlobData.TableLogicals = new ExControls.ExBindingList<TableLogical>(tables.Logicals);
        GlobData.TableFontDir = DemoTables.FontDir;

        TxtParser.WriteTrains(path, trains, gvd, ReportVariant.GetDefaultValues());
        TxtParser.WriteTables(path, tables.TabTabs, tables.Catalogs, tables.Physicals, tables.Logicals);
        TxtParser.WriteTTexts(path, tables.Texts);
        TxtParser.WriteModeTabs(path, tables.Fonts, DemoTables.FontDir);
        TxtParser.WriteTracks(path, tracks);
        TxtParser.WriteOperators(path, operators);
        TxtParser.WriteInfoGVD(path, gvd);
        TxtParser.WriteCustomStations(path, GlobData.CustomStations, gvd);
        TxtParser.WriteRazeni1(path, DemoRadenia(), GlobData.Languages);
    }

    /// <summary>
    ///     Radenie Ex 521: v pracovné dni šesť vozňov, cez víkend osem, hlási sa pri príchode a zastavení.
    /// </summary>
    private static List<Radenie> DemoRadenia()
    {
        FyzSound Snd(string group, string key) => GlobData.Sounds.First(s => s.Group.Key == group && s.Key == key);

        var types = ReportType.GetDefaultValuesSK();
        List<ChosenReportType> Reports() =>
        [
            new() { Type = types[0], Variants = ReportVariant.GetDefaultValues() },
            new() { Type = types[2], Variants = [ReportVariant.DlheHlasenie] }
        ];

        Radenie Rad(string dateLimit, string last)
        {
            List<FyzSound> sounds =
            [
                Snd("Poz7", "zalok"), Snd("VOZY1", "v1"), Snd("Slova", "cislo"), Snd("CISLO1", "1"),
                Snd("VOZY4M", "v2"), Snd("Slova", "cislo"), Snd("CISLO1", "2"), Snd("CISLO9", last)
            ];
            return new Radenie
            {
                CisloVlaku = "521",
                ZacPlatnosti = ValidFrom,
                KonPlatnosti = ValidTo,
                DatObm = dateLimit,
                Sounds = sounds,
                Text = Radenie.SoundsToString(sounds),
                ChosenReports = Reports()
            };
        }

        return [Rad("ide v 1-5", "6"), Rad("ide v 6,7", "8")];
    }

    private static Train Tr(string type, string number, string name, Routing routing, string? arrival, string? departure,
        string[] from, string[] to, Track track, Operator op, string dateLimit, List<FyzLanguage> languages, Action<Train>? setup = null)
    {
        var train = new Train
        {
            Number = number,
            Variant = -1,
            Name = name,
            Type = GlobData.TrainsTypes.First(t => t.Key == type),
            Routing = routing,
            Arrival = arrival is null ? null : DateTime.Today.Add(TimeSpan.Parse(arrival, CultureInfo.InvariantCulture)),
            Departure = departure is null ? null : DateTime.Today.Add(TimeSpan.Parse(departure, CultureInfo.InvariantCulture)),
            Track = track,
            Operator = op,
            DateLimitText = dateLimit,
            ZaciatokPlatnosti = ValidFrom,
            KoniecPlatnosti = ValidTo,
            Languages = languages,
        };

        // v kratšom hlásení len východisková a cieľová stanica, v dlhšom všetky
        var route = from.Concat(to).ToList();
        foreach (var id in from)
            train.StaniceZoSmeru.Add(RouteStation(id, route));
        foreach (var id in to)
            train.StaniceDoSmeru.Add(RouteStation(id, route));

        train.StartingStation = train.StaniceZoSmeru.FirstOrDefault();
        train.EndingStation = train.StaniceDoSmeru.LastOrDefault();

        setup?.Invoke(train);
        return train;
    }

    private static Station RouteStation(string id, List<string> route)
    {
        var station = Station(id);
        var isEnd = id == route.First() || id == route.Last();
        return new Station(station.ID, station.Name, IsInShortReport: isEnd, IsInLongReport: true);
    }

    private static (string ID, string Name) Station(string id) => StationList.Concat(CustomStationList).First(s => s.Id == id);

    /// <summary>
    ///     Načíta grafikon späť tými istými čítačmi ako GVDEditor a zapíše varovania do logu.
    /// </summary>
    private static void Verify(string path, GVDInfo gvd, List<string> log)
    {
        // poradie ako vo FMain.ProccessData: vlastné stanice pred trasami, koľaje sa odkazujú na logické tabule
        LoadWarnings.Clear();
        GlobData.CustomStations = new ExControls.ExBindingList<Station>(TxtParser.ReadCustomStations(path, gvd));
        var (tabtabs, catalogs, physicals, logicals) = TxtParser.ReadTables(path);
        GlobData.TabTabs = new ExControls.ExBindingList<TableTabTab>(tabtabs);
        GlobData.TableCatalogs = new ExControls.ExBindingList<TableCatalog>(catalogs);
        GlobData.TablePhysicals = new ExControls.ExBindingList<TablePhysical>(physicals);
        GlobData.TableLogicals = new ExControls.ExBindingList<TableLogical>(logicals);
        GlobData.Tracks = new ExControls.ExBindingList<Track>(TxtParser.ReadTracks(path));
        GlobData.Operators = new ExControls.ExBindingList<Operator>(TxtParser.ReadOperators(path));
        (GlobData.ReportVariants, GlobData.ReportTypes, GlobData.LocalLanguages) = TxtParser.ReadLocalCategori(path);
        var trains = TxtParser.ReadTrains(path);
        var texts = TxtParser.ReadTTexts(path, trains);
        var fonts = TxtParser.ReadTableFonts(path);
        var tracksWithTables = GlobData.Tracks.Count(t => t.Tables.Count > 0);
        var radenia = TxtParser.ReadRazeni1(path, GlobData.Sounds);
        log.Add($"demo: radenia {radenia.Count} ({string.Join("; ", radenia.Select(r => $"{r.CisloVlaku} {r.DatObm}: {r.Text}"))})");

        log.Add($"demo: {trains.Count} vlakov, {GlobData.Tracks.Count - 1} koľají ({tracksWithTables} s tabuľou), " +
                $"{GlobData.Operators.Count - 1} dopravcov, tabule fyz/log/kat/TabTab {physicals.Count}/{logicals.Count}/" +
                $"{catalogs.Count}/{tabtabs.Count}, texty {texts.Count}, písma {fonts.Count}, vlastné stanice {GlobData.CustomStations.Count}");
        foreach (var warning in LoadWarnings.Items)
            log.Add("demo varovanie: " + warning);
    }

    private static void SetGlobData(string property, string value) =>
        typeof(GlobData).GetProperty(property, BindingFlags.Public | BindingFlags.Static)!.SetValue(null, value);
}

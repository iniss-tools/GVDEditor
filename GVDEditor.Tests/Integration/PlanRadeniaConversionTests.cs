using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using Iniss.PlanRadenia;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Radenie vlaku z planu radenia ZSSK: hlasenie zo zvukov banky (ako v radeniach INISS) a varianty podla poznamok.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class PlanRadeniaConversionTests
{
    private static readonly DateOnly Od = new(2025, 12, 14);
    private static readonly DateOnly Do = new(2026, 12, 12);
    private static readonly Station Kosice = new("5613206", "Košice");
    private static readonly Station NoveZamky = new("5613436", "Nové Zámky");

    private static readonly FyzLanguage Sk = new("SK", "Slovensky") { IsBasic = true };
    private static readonly FyzLanguage Gb = new("GB", "Anglicky");

    private static FyzSound Sound(string group, string key, string text, FyzLanguage? language = null) =>
        new(new FyzGroup(language ?? Sk, group, group, group + "\\"), key, key, key + ".WAV", "", text, 0);

    /// <summary>Banka ako v staniciach ZSSK: druhy vozni v skupinach Vozy1-6 (M = mnozne cislo), cisla CISLO*, stanice R*.</summary>
    private static List<FyzSound> Bank(params string[] numbers)
    {
        var sounds = new List<FyzSound>
        {
            Sound("Poz7", "zalok", "za rušňom"),
            Sound("SLOVA", "jeraz", "je radený"),
            Sound("SLOVA", "jsouraz", "sú radené"),
            Sound("SLOVA", "cislo", "číslo"),
            Sound("SLOVA", "DoStan", "do stanice")
        };
        var kinds = new (string Key, string One, string Many)[]
        {
            ("v1", "vozeň prvej triedy", "vozne prvej triedy"), ("v2", "vozeň druhej triedy", "vozne druhej triedy"),
            ("lev", "ležadlový vozeň", "ležadlové vozne"), ("rv", "reštauračný vozeň", "reštauračné vozne")
        };
        string[] ends = ["", ",", "."];
        foreach (var (key, one, many) in kinds)
            for (var i = 0; i < 6; i++)
            {
                var and = i >= 3 ? "a " : "";
                sounds.Add(Sound($"Vozy{i + 1}", key, and + one + ends[i % 3]));
                sounds.Add(Sound($"Vozy{i + 1}M", key, and + many + ends[i % 3]));
            }

        string[] groups = ["CISLO1", "CISLO_", "CISLO3", "CISLO4", "CISLO5", "CISLO6", "CISLO7", "CISLO8", "CISLO9"];
        string[] prefixes = ["", "", "", "a ", "a ", "a ", "až ", "až ", "až "];
        foreach (var number in numbers)
            for (var i = 0; i < groups.Length; i++)
                sounds.Add(Sound(groups[i], number, prefixes[i] + number + ends[i % 3]));

        foreach (var station in new[] { Kosice, NoveZamky })
        {
            sounds.Add(Sound("R1", station.ID, station.Name + ","));
            sounds.Add(Sound("R3", station.ID, station.Name));
            sounds.Add(Sound("R4", station.ID, station.Name + "."));
        }

        return sounds;
    }

    /// <summary>Anglicka banka ako v staniciach ZSSK: skupiny vozni bez bodkovych variant, sloveso a poloha na konci.</summary>
    private static List<FyzSound> BankGb(params string[] numbers)
    {
        var sounds = new List<FyzSound>
        {
            Sound("Poz3", "zalok", "behind the locomotive.", Gb),
            Sound("SLOVA", "je_r", "is coupled", Gb),
            Sound("SLOVA", "jsou_r", "are coupled", Gb),
            Sound("SLOVA", "cislo", "no.", Gb),
            Sound("SLOVA", "cisla", "nos.", Gb),
            Sound("SLOVA", "DoStan", "to", Gb)
        };
        foreach (var (key, one, many) in new[] { ("v2", "second-class carriage", "second-class carriages"), ("lev", "couchette car", "couchette cars") })
            foreach (var (group, prefix, end) in new[] { ("Vozy1", "", ""), ("Vozy2", "", ","), ("Vozy4", "and ", ""), ("Vozy7", "", ""), ("Vozy8", "", ",") })
            {
                sounds.Add(Sound(group, key, prefix + one + end, Gb));
                sounds.Add(Sound(group + "M", key, prefix + many + end, Gb));
            }

        foreach (var number in numbers)
            foreach (var (group, prefix, end) in new[] { ("CISLO1", "", ""), ("CISLO_", "", ","), ("CISLO4", "and ", ""), ("CISLO7", "to ", "") })
                sounds.Add(Sound(group, number, prefix + number + end, Gb));
        sounds.Add(Sound("R1", NoveZamky.ID, NoveZamky.Name + ",", Gb));
        return sounds;
    }

    private static PlanRadeniaContext Context(List<FyzSound> bank, List<FyzSound>? english = null) => new()
    {
        Banks = english is null ? [new RadenieBank(Sk, bank)] : [new RadenieBank(Sk, bank), new RadenieBank(Gb, english)],
        From = Od,
        To = Do,
        Station = name => name == "Nové Zámky" ? NoveZamky : null,
        Reports = []
    };

    private static Train Vlak(string dateLimit = "") => new()
    {
        Type = new TrainType("R"),
        Number = "681",
        Name = "",
        Operator = Operator.None,
        ZaciatokPlatnosti = Od,
        KoniecPlatnosti = Do,
        DateLimitText = dateLimit,
        Track = new Track { Key = "1", Name = "1" },
        EndingStation = Kosice
    };

    private static PlanVehicle Car(string number, string icon) => new() { Number = number, Icons = [icon] };

    private static PlanTrain Plan(params PlanVehicle[] cars)
    {
        var plan = new PlanTrain { Type = "R", Number = "681", Locomotive = "left" };
        plan.Vehicles.Add(new PlanVehicle { IsLocomotive = true });
        plan.Vehicles.AddRange(cars);
        return plan;
    }

    private static string Keys(Radenie radenie, FyzLanguage? language = null) =>
        string.Join(" ", radenie.Sounds.Where(s => s.Group.Language == (language ?? Sk)).Select(s => $"{s.Group.Key}/{s.Key}"));

    [TestMethod]
    public void Hlasenie_RadAzAPoslednaSkupinaSoSpojkou()
    {
        var plan = Plan(Car("369", "vozeň 2. triedy"), Car("368", "vozeň 2. triedy"), Car("367", "vozeň 2. triedy"), Car("363", "ležadlový vozeň"));

        var proposals = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("369", "367", "363")));

        Assert.HasCount(1, proposals);
        var radenie = proposals[0].Radenie;
        Assert.AreEqual("Poz7/zalok SLOVA/jsouraz Vozy1M/v2 SLOVA/cislo CISLO1/369 CISLO7/367 Vozy4/lev SLOVA/cislo CISLO3/363", Keys(radenie));
        Assert.AreEqual("za rušňom sú radené vozne druhej triedy číslo 369 až 367 a ležadlový vozeň číslo 363.", radenie.Text);
        Assert.IsNull(radenie.Validity, "radenie plati cele obdobie grafikonu");
        Assert.IsTrue(proposals[0].Take);
    }

    [TestMethod]
    public void Hlasenie_CiarkaMedziSkupinamiADveCisla()
    {
        var plan = Plan(Car("1", "vozeň 1. triedy"), Car("2", "vozeň 2. triedy"), Car("4", "vozeň 2. triedy"), Car("5", "reštauračný vozeň"));

        var radenie = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("1", "2", "4", "5")))[0].Radenie;

        Assert.AreEqual("Poz7/zalok SLOVA/jsouraz Vozy1/v1 SLOVA/cislo CISLO_/1 Vozy1M/v2 SLOVA/cislo CISLO1/2 CISLO4/4 Vozy4/rv SLOVA/cislo CISLO3/5",
            Keys(radenie));
    }

    [TestMethod]
    public void Hlasenie_CielVozniZPoznamky()
    {
        var plan = Plan(Car("364", "vozeň 2. triedy"), Car("365", "vozeň 2. triedy"), Car("366", "vozeň 2. triedy"));
        plan.Notes.Add(new PlanNote { Kind = PlanNoteKind.Destination, Cars = ["364", "365"], Station = "Nové Zámky" });

        var radenie = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("364", "365", "366")))[0].Radenie;

        Assert.AreEqual(
            "Poz7/zalok SLOVA/jsouraz Vozy1M/v2 SLOVA/cislo CISLO1/364 CISLO4/365 SLOVA/DoStan R3/5613436 Vozy4/v2 SLOVA/cislo CISLO1/366 SLOVA/DoStan R4/5613206",
            Keys(radenie));
    }

    [TestMethod]
    public void Hlasenie_AnglickaVetaPoSlovenskej()
    {
        var plan = Plan(Car("369", "vozeň 2. triedy"), Car("368", "vozeň 2. triedy"), Car("367", "vozeň 2. triedy"), Car("363", "ležadlový vozeň"));

        var proposal = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("369", "367", "363"), BankGb("369", "367", "363")))[0];

        Assert.AreEqual("Vozy7M/v2 SLOVA/cisla CISLO1/369 CISLO7/367 Vozy4/lev SLOVA/cislo CISLO1/363 SLOVA/jsou_r Poz3/zalok", Keys(proposal.Radenie, Gb));
        Assert.AreSame(Sk, proposal.Radenie.Sounds[0].Group.Language, "zakladny jazyk prvy");
        Assert.AreEqual(
            "za rušňom sú radené vozne druhej triedy číslo 369 až 367 a ležadlový vozeň číslo 363. " +
            "second-class carriages nos. 369 to 367 and couchette car no. 363 are coupled behind the locomotive.", proposal.Radenie.Text);
    }

    [TestMethod]
    public void Hlasenie_AnglickyCielACiarkaZaPrvouSkupinou()
    {
        var plan = Plan(Car("364", "vozeň 2. triedy"), Car("365", "vozeň 2. triedy"), Car("366", "ležadlový vozeň"), Car("367", "vozeň 2. triedy"));
        plan.Notes.Add(new PlanNote { Kind = PlanNoteKind.Destination, Cars = ["364", "365"], Station = "Nové Zámky" });
        var english = BankGb("364", "365", "366", "367");
        english.Add(Sound("R1", Kosice.ID, Kosice.Name + ",", Gb));

        var radenie = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("364", "365", "366", "367"), english))[0].Radenie;

        Assert.AreEqual(
            "Vozy7M/v2 SLOVA/cisla CISLO1/364 CISLO4/365 SLOVA/DoStan R1/5613436 Vozy1/lev SLOVA/cislo CISLO1/366 Vozy4/v2 SLOVA/cislo CISLO1/367 " +
            "SLOVA/DoStan R1/5613206 SLOVA/jsou_r Poz3/zalok", Keys(radenie, Gb));
    }

    [TestMethod]
    public void Hlasenie_ChybajuciZvukVDalsomJazyku_JazykSaVynecha()
    {
        var plan = Plan(Car("381", "vozeň 2. triedy"));

        var proposal = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("381"), BankGb()))[0];

        Assert.AreEqual("", Keys(proposal.Radenie, Gb));
        Assert.AreEqual("Poz7/zalok SLOVA/jeraz Vozy1/v2 SLOVA/cislo CISLO3/381", Keys(proposal.Radenie));
        Assert.IsTrue(proposal.Take, "zakladny jazyk je uplny");
        Assert.IsTrue(proposal.Issues.Any(i => i.Kind == PlanRadeniaIssueKind.LanguageMissing && i.Text == "GB: CISLO1/381"));
    }

    [TestMethod]
    public void Hlasenie_JazykBezSlovRadenia_JazykSaVynecha()
    {
        var plan = Plan(Car("381", "vozeň 2. triedy"));

        var proposal = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank("381"), [Sound("CISLO1", "381", "381", Gb)]))[0];

        Assert.AreEqual("", Keys(proposal.Radenie, Gb));
        Assert.IsTrue(proposal.Issues.Any(i => i.Kind == PlanRadeniaIssueKind.LanguageNoWords && i.Text == "GB"));
    }

    [TestMethod]
    public void Hlasenie_VozneBezCisla()
    {
        var plan = Plan(Car("", "vozeň 2. triedy"), Car("", "vozeň 2. triedy"));

        var radenie = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank()))[0].Radenie;

        Assert.AreEqual("Poz7/zalok SLOVA/jsouraz Vozy3M/v2", Keys(radenie));
    }

    [TestMethod]
    public void Varianty_VozenSObmedzenim()
    {
        var plan = Plan(Car("21", "vozeň 2. triedy"), Car("22", "vozeň 1. triedy"));
        plan.Notes.Add(new PlanNote { Kind = PlanNoteKind.Restriction, Cars = ["22"], Rest = "ide do 15.IX." });
        var from = new DateOnly(2026, 9, 1);
        var to = new DateOnly(2026, 9, 30);

        var proposals = PlanRadeniaConversion.Propose(plan, [Vlak()], from, to, Context(Bank("21", "22")));

        Assert.HasCount(2, proposals);
        var both = proposals.Single(p => p.Radenie.Sounds.Any(s => s.Key == "22")).Radenie;
        var one = proposals.Single(p => p.Radenie.Sounds.All(s => s.Key != "22")).Radenie;
        Assert.AreEqual(new ValidityPeriod(from, to), both.Validity);
        Assert.AreEqual("ide do 15.IX.", both.DatObm);
        Assert.AreEqual("ide od 16.IX.", one.DatObm);
        Assert.AreEqual("Poz7/zalok SLOVA/jeraz Vozy1/v2 SLOVA/cislo CISLO3/21", Keys(one));
    }

    [TestMethod]
    public void ChybajuciZvuk_RadenieSaNepreberie()
    {
        var plan = Plan(Car("381", "vozeň 2. triedy"));

        var proposal = PlanRadeniaConversion.Propose(plan, [Vlak()], Od, Do, Context(Bank()))[0];

        Assert.IsFalse(proposal.Take);
        Assert.IsTrue(proposal.Issues.Any(i => i.Kind == PlanRadeniaIssueKind.MissingSound && i.Text == "CISLO3/381"));
    }

    [TestMethod]
    [DataRow("ide v ⑤, ⑦, 2., 30.IV., nejde od 20.XII. do 3.I.", "ide v 5, 7, 2., 30.IV., nejde od 20.XII. do 3.I.")]
    [DataRow("nejde od 12.X. v ① - ④, ⑥, 15.XI.,", "nejde od 12.X. v 1-4, 6, 15.XI.")]
    [DataRow("od 7.I. do 30.VI. v ⚒", "od 7.I. do 30.VI. v X")]
    public void Poznamka_DniVTvareDatumovehoObmedzenia(string rest, string expected) =>
        Assert.AreEqual(expected, PlanRadeniaConversion.DayText(rest));

    [TestMethod]
    [DataRow("Bratislava hl.st.", "Bratislava hlavná stanica", true)]
    [DataRow("Praha hl.n.", "Praha hl.n.", true)]
    [DataRow("Kraków Główny", "Krakow Glowny", true)]
    [DataRow("Nové Zámky", "Nové Mesto", false)]
    [DataRow("Kúty", "Kúty zastávka", false)]
    public void Stanica_RovnakaAjSoSkratkou(string a, string b, bool same) => Assert.AreEqual(same, PlanStations.Same(a, b));

    [TestMethod]
    public void Stanica_KandidatiSoSkratkouAjBezDiakritiky()
    {
        var candidates = PlanStations.Candidates([new Station("1", "Bratislava hl.st."), new Station("2", "Kraków Główny")]);

        Assert.AreEqual("1", candidates["Bratislava hlavná stanica"].ID);
        Assert.AreEqual("2", candidates["Krakow Glowny"].ID);
        Assert.AreEqual("1", PlanStations.Resolve("Bratislava hlavná stanica", candidates)?.ID);
    }
}

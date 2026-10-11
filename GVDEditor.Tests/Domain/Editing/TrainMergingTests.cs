using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Tests.Domain.Editing;

/// <summary>
/// Aktualizacia vlakov grafikonu importovanymi vlakmi.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainMergingTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly TrainType R = new("R");
    private static readonly DateOnly Od = new(2025, 12, 14);
    private static readonly DateOnly Do = new(2026, 12, 12);

    private static Train Vlak(TrainType type, string number, string dateLimit = "", int departure = 8 * 60, string name = "") => new()
    {
        Type = type,
        Number = number,
        Name = name,
        Operator = Operator.None,
        ZaciatokPlatnosti = Od,
        KoniecPlatnosti = Do,
        DateLimitText = dateLimit,
        Departure = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(departure)),
        Track = new Track { Key = "1", Name = "1" }
    };

    [TestMethod]
    public void Parovanie_PodlaDruhuCislaANazvu()
    {
        var os1 = Vlak(Os, "1");
        var r1 = Vlak(R, "1");
        var os2 = Vlak(Os, "2");
        var novyOs1 = Vlak(Os, "1");
        var novyR1 = Vlak(R, "1", name: "Tatran");
        var novyOs3 = Vlak(Os, "3");

        var pairs = TrainMerging.Match([os1, r1, os2], [novyOs1, novyR1, novyOs3]);

        Assert.HasCount(1, pairs);
        Assert.AreSame(os1, pairs[0].Original);
        Assert.AreSame(novyOs1, pairs[0].Imported);
    }

    [TestMethod]
    public void Parovanie_VariantyPodlaSpolocnychDniACasov()
    {
        var vPracovneDni = Vlak(Os, "1", "ide v 1-5");
        var vSoboty = Vlak(Os, "1", "ide v 6");
        var novySoboty = Vlak(Os, "1", "ide v 6");
        var novyPracovne = Vlak(Os, "1", "ide v 1-5", departure: 8 * 60 + 5);

        var pairs = TrainMerging.Match([vPracovneDni, vSoboty], [novySoboty, novyPracovne]);

        Assert.HasCount(2, pairs);
        Assert.AreSame(novyPracovne, pairs.Single(p => ReferenceEquals(p.Original, vPracovneDni)).Imported);
        Assert.AreSame(novySoboty, pairs.Single(p => ReferenceEquals(p.Original, vSoboty)).Imported);
    }

    [TestMethod]
    [DataRow("TATRAN /617", "TATRAN")]
    [DataRow("REGIOJET /1021", "REGIOJET")]
    [DataRow("SLOVAKIA /97443", "SLOVAKIA")]
    [DataRow("A /1 /2", "A")]
    [DataRow("Kriváň", "Kriváň")]
    [DataRow("Šariš/Tatry", "Šariš/Tatry")]
    [DataRow("", "")]
    public void Nazov_BezCiselZaLomkou(string name, string expected) => Assert.AreEqual(expected, TrainMerging.BaseName(name));

    [TestMethod]
    public void Parovanie_StaryNazovSCislomZaLomkou()
    {
        var povodny = Vlak(R, "964", name: "TATRAN /617");
        var novy = Vlak(R, "964", name: "TATRAN");

        Assert.HasCount(1, TrainMerging.Match([povodny], [novy]));
    }

    [TestMethod]
    public void Parovanie_KazdyVlakNajviacRaz()
    {
        var povodny = Vlak(Os, "1");
        var a = Vlak(Os, "1");
        var b = Vlak(Os, "1");

        var pairs = TrainMerging.Match([povodny], [a, b]);

        Assert.HasCount(1, pairs);
        Assert.AreSame(a, pairs[0].Imported);
    }

    [TestMethod]
    public void Prenesenie_RucneUdajeOstanuUdajeZImportuVyhravaju()
    {
        var kolaj = new Track { Key = "3", Name = "3" };
        var dopravca = new Operator(749, "ZSSK");
        var povodny = Vlak(Os, "1");
        povodny.Track = kolaj;
        povodny.TrackDeparture = kolaj;
        povodny.Variant = 2;
        povodny.Operator = dopravca;
        povodny.IsMotorovy = true;
        povodny.IsNizkopodlazny = true;
        povodny.LockoutNumber = 5;
        povodny.LineArrival = "S1";
        povodny.Doplnky.Add(new Dodatok { Name = "1003", ChosenReports = [] });
        povodny.Doplnky.Add(new Dodatok { Name = "1004", ChosenReports = [] });

        var novy = Vlak(Os, "1");
        novy.IsMiestenkovy = true;
        novy.LockoutNumber = 7;
        novy.LineDeparture = "R2";
        novy.Doplnky.Add(new Dodatok { Name = "1004", ChosenReports = [] });
        novy.Doplnky.Add(new Dodatok { Name = "1010", ChosenReports = [] });

        TrainMerging.Preserve(povodny, novy);

        Assert.AreSame(kolaj, novy.Track);
        Assert.AreSame(kolaj, novy.TrackDeparture);
        Assert.AreEqual(2, novy.Variant);
        Assert.AreSame(dopravca, novy.Operator);
        Assert.IsTrue(novy.IsMotorovy);
        Assert.IsTrue(novy.IsNizkopodlazny);
        Assert.IsTrue(novy.IsMiestenkovy);
        Assert.AreEqual(7, novy.LockoutNumber, "vyluka z importu");
        Assert.AreEqual("S1", novy.LineArrival);
        Assert.AreEqual("R2", novy.LineDeparture);
        CollectionAssert.AreEqual(new[] { "1003", "1004", "1010" }, novy.Doplnky.Select(d => d.Name).ToArray());
        Assert.AreNotSame(povodny.Doplnky[0], novy.Doplnky[0], "dodatok vlaku je kopia");
    }
}

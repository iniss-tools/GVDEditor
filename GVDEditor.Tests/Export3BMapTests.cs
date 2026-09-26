using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Export3 3.01 niekedy nevypise poznamku do Export3C (pocet 0), hoci mapa dni v Export3B vlak obmedzuje.
///     Obmedzenie sa potom musi zobrat z mapy, inak by vlak po nacitani isiel denne a ulozenie by ho stratilo.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class Export3BMapTests
{
    // 01.01.2026 je stvrtok; ide len cez vikendy 3.-4. a 10.-11.1.
    private const string WEEKENDS = "00110000011000";

    [TestMethod]
    public void ReadTrains_BezPoznamkyVExport3C_BerieObmedzenieZMapyExport3B()
    {
        var dir = Directory.CreateTempSubdirectory("gvdexport3b");
        var typesProp = typeof(GlobData).GetProperty(nameof(GlobData.TrainsTypes), BindingFlags.Public | BindingFlags.Static)!;
        var oldTypes = GlobData.TrainsTypes;
        var (oldOperators, oldTracks, oldRadenia, oldStations, oldCustom) =
            (GlobData.Operators, GlobData.Tracks, GlobData.Radenia, GlobData.Stations, GlobData.CustomStations);
        try
        {
            DateLimit.Loc = DateLimit.Locale.Sk;
            typesProp.SetValue(null, new ExBindingList<TrainType>([new TrainType("Os")]));
            GlobData.Operators = [];
            GlobData.Tracks = [];
            GlobData.Radenia = [];
            GlobData.Stations = [];
            GlobData.CustomStations = [];

            Write(dir, FileConsts.FILE_GRAFIKON,
                "IDSTATION=1", "NAMESTATION=\"Test\"",
                "START_VALID_TIMETABLE=01.01.2026", "END_VALID_TIMETABLE=14.01.2026",
                "START_VALID_DATA=01.01.2026", "END_VALID_DATA=14.01.2026", "CREATE_DATA=01.01.2026");
            Write(dir, FileConsts.FILE_EXPORT3A, new[] { 1, 2, 3, 4, 5 }
                .Select(i => $"{i},\"{100 + i}\",\"\",\"Os\",-1,V,,,8:00,,,-1,-1").ToArray());
            Write(dir, FileConsts.FILE_EXPORT3B,
                $"1,01.01.2026,14.01.2026,{WEEKENDS}",             // vikendy, pocet 0
                "2,01.01.2026,14.01.2026,11111111111111",          // kazdy den
                "3,01.01.2026,14.01.2026,0011000001100",           // mapa kratsia nez obdobie
                $"4,01.01.2026,14.01.2026,{WEEKENDS}",             // poznamka v 3C ma prednost
                $"5,01.01.2026,14.01.2026,{WEEKENDS}");            // pocet 1, ale prazdne pole
            Write(dir, FileConsts.FILE_EXPORT3C,
                "1,0,", "2,0,", "3,0,", "4,1,\"ide 1.I.\"", "5,1,\"\"");
            foreach (var file in new[] { FileConsts.FILE_VZORY, FileConsts.FILE_STAHLASB, FileConsts.FILE_STAHLASC, FileConsts.FILE_VLAKY, FileConsts.FILE_POZICE })
                Write(dir, file);

            var trains = TxtParser.ReadTrains(dir.FullName);

            var limit = new DateLimit(new DateTime(2026, 1, 1), new DateTime(2026, 1, 14), insertMarks: false);
            var expected = limit.BitArrayToText(Utils.StringToBitArray(WEEKENDS));
            Assert.AreNotEqual("ide denne", expected);
            Assert.AreEqual(expected, trains[0].DateLimitText);
            Assert.AreEqual("ide denne", trains[1].DateLimitText);
            Assert.AreEqual("ide denne", trains[2].DateLimitText);
            Assert.AreEqual("ide 1.I.", trains[3].DateLimitText);
            Assert.AreEqual(expected, trains[4].DateLimitText);

            // pri ulozeni sa mapa prepocita z textu - musi vyjst rovnaka
            var bits = new DateLimit(trains[0].ZaciatokPlatnosti, trains[0].KoniecPlatnosti).TextToBitArray(trains[0].DateLimitText);
            Assert.AreEqual(WEEKENDS, string.Concat(bits.Cast<bool>().Select(b => b ? '1' : '0')));
        }
        finally
        {
            typesProp.SetValue(null, oldTypes);
            (GlobData.Operators, GlobData.Tracks, GlobData.Radenia, GlobData.Stations, GlobData.CustomStations) =
                (oldOperators, oldTracks, oldRadenia, oldStations, oldCustom);
            dir.Delete(true);
        }
    }

    private static void Write(DirectoryInfo dir, string file, params string[] lines) =>
        File.WriteAllLines(Path.Combine(dir.FullName, file), lines, Encodings.Win1250);
}

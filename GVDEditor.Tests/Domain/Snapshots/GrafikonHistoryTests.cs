using System.ComponentModel;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Snapshots;

namespace GVDEditor.Tests.Domain.Snapshots;

[TestClass]
public class GrafikonHistoryTests
{
    private static readonly Track Track1 = new("1", "1", "Koľaj 1", Platform.None, "", "1");
    private static readonly Track Track2 = new("2", "2", "Koľaj 2", Platform.None, "", "2");

    private static (GrafikonDocument Document, GVDDirectory Directory, GrafikonHistory History) Open()
    {
        var document = new GrafikonDocument { Tracks = [Track.None, Track1, Track2] };
        document.Trains.Add(new Train { Number = "601", Name = "", Track = Track1 });
        var directory = new GVDDirectory(new DirList { DirName = "G" }, new GVDInfo { ThisStation = new Station("99", "Dolné Mesto") });
        return (document, directory, new GrafikonHistory(() => GrafikonState.Capture(document, directory), saved: true));
    }

    [TestMethod]
    public void Spat_VratiZmenuAjPridanyVlakAZnovaJuZopakuje()
    {
        var (document, _, history) = Open();
        var train = document.Trains[0];

        train.Track = Track2;
        Assert.IsTrue(history.Commit("Úprava vlaku 601"));
        document.Trains.Add(new Train { Number = "603", Name = "", Track = Track1 });
        Assert.IsTrue(history.Commit("Nový vlak 603"));

        Assert.AreEqual("Nový vlak 603", history.UndoLabel);
        history.Undo();
        Assert.HasCount(1, document.Trains);
        Assert.AreSame(Track2, train.Track);
        history.Undo();
        Assert.AreSame(Track1, train.Track, "ten isty objekt vlaku - odkazy ostanu platne");
        Assert.IsFalse(history.CanUndo);
        Assert.IsTrue(history.IsSaved);

        history.Redo(2);
        Assert.HasCount(2, document.Trains);
        Assert.AreSame(Track2, train.Track);
        Assert.IsFalse(history.IsSaved);
    }

    [TestMethod]
    public void Commit_BezZmenyKrokNevznikneANovaUpravaZahodiZnova()
    {
        var (document, _, history) = Open();

        Assert.IsFalse(history.Commit("nic"));
        document.Trains[0].Number = "605";
        history.Commit("A");
        history.Undo();
        document.Trains[0].Name = "Tatran";
        history.Commit("B");

        Assert.IsFalse(history.CanRedo);
        CollectionAssert.AreEqual(new[] { "B" }, history.UndoLabels.ToArray());
        Assert.AreEqual("601", document.Trains[0].Number);
    }

    [TestMethod]
    public void Commit_RovnakeHodnotyVNovychObjektochNieSuKrok()
    {
        var (document, _, history) = Open();
        var text = new TableText { Key = "CIEL", Name = "Cieľ", Comment = "", Trains = [new TableTrain { Train = document.Trains[0], Text = "Žilina", FontID = -1 }] };
        document.TableTexts.Add(text);
        Assert.IsTrue(history.Commit("text"));

        // napr. pregenerovanie textov - novy zoznam aj zaznamy s rovnakymi hodnotami
        text.Trains = [new TableTrain { Train = document.Trains[0], Text = "Žilina", FontID = -1 }];
        Assert.IsFalse(history.Commit("rovnake texty"));
        // pridanie a odobratie zmeni len pocitadlo zmien zoznamu
        document.Trains.Add(new Train { Number = "603", Name = "" });
        document.Trains.RemoveAt(1);
        Assert.IsFalse(history.Commit("nic"));
        text.Trains[0].Text = "Košice";
        Assert.IsTrue(history.Commit("iny text"));
        text.Trains = [new TableTrain { Train = new Train { Number = "601", Name = "" }, Text = "Košice", FontID = -1 }];
        Assert.IsTrue(history.Commit("iny vlak"), "rovnake hodnoty, ale vlak mimo grafikonu");
    }

    [TestMethod]
    public void Ulozenie_PoKrokochSpatAZnovaPozna_UlozenyStav()
    {
        var (document, _, history) = Open();
        document.Trains[0].Number = "605";
        history.Commit("A");
        history.MarkSaved();
        document.Trains[0].Number = "607";
        history.Commit("B");

        Assert.IsFalse(history.IsSaved);
        history.Undo();
        Assert.IsTrue(history.IsSaved);
        history.Undo();
        Assert.IsFalse(history.IsSaved);
        // nova uprava po kroku spat - ulozeny stav bol medzi krokmi znova, uz sa neda dosiahnut
        document.Trains[0].Name = "x";
        history.Commit("C");
        history.Undo();
        Assert.IsFalse(history.IsSaved);
    }

    [TestMethod]
    public void Kapacita_NajstarsieKrokyVypadnu()
    {
        var (document, _, history) = Open();
        for (var i = 0; i < GrafikonHistory.Capacity + 5; i++)
        {
            document.Trains[0].Number = i.ToString(System.Globalization.CultureInfo.InvariantCulture);
            history.Commit("krok " + i);
        }

        Assert.AreEqual(GrafikonHistory.Capacity, history.Count);
        history.Undo(GrafikonHistory.Capacity);
        Assert.AreEqual("4", document.Trains[0].Number);
        Assert.IsFalse(history.IsSaved, "ulozeny povodny stav vypadol");
    }

    [TestMethod]
    public void Spat_HlavickaGrafikonuAOdberyZoznamuOstanu()
    {
        var (document, directory, history) = Open();
        var changes = 0;
        ListChangedEventHandler handler = (_, _) => changes++;

        directory.GVD.ThisStation = new Station("98", "Horné Mesto");
        history.Commit("Lokálne nastavenia");
        // odber pridany po zapamatani (napr. vazba okna) obnova nezrusi
        document.Trains.ListChanged += handler;
        history.Undo();
        document.Trains.ResetBindings();

        Assert.AreEqual("Dolné Mesto", directory.GVD.ThisStation.Name);
        Assert.AreEqual(1, changes);
    }
}

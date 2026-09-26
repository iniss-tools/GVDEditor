using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Transakcne ukladanie grafikonu (Subor → Ulozit) - pri chybe sa priecinok vrati do povodneho stavu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class FileTransactionTests
{
    private string _dir = null!;

    [TestInitialize]
    public void Init()
    {
        _dir = Path.Combine(Path.GetTempPath(), "FileTransactionTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
        File.WriteAllText(Path.Combine(_dir, "EXPORT3A.txt"), "povodny");
        File.WriteAllText(Path.Combine(_dir, "VYLUKA.txt"), "vyluka");
    }

    [TestCleanup]
    public void Cleanup()
    {
        foreach (var file in Directory.GetFiles(_dir))
            File.SetAttributes(file, FileAttributes.Normal);
        Directory.Delete(_dir, true);
    }

    [TestMethod]
    public void Rollback_VratiZmeneneAOdstraniNoveSubory()
    {
        var transaction = new FileTransaction(_dir);
        File.WriteAllText(Path.Combine(_dir, "EXPORT3A.txt"), "novy");
        File.WriteAllText(Path.Combine(_dir, "RAZENI1.txt"), "novy subor");

        Assert.IsTrue(transaction.TryRollback());
        Assert.AreEqual("povodny", File.ReadAllText(Path.Combine(_dir, "EXPORT3A.txt")));
        Assert.IsFalse(File.Exists(Path.Combine(_dir, "RAZENI1.txt")));
        Assert.IsFalse(Directory.Exists(transaction.BackupPath));
    }

    [TestMethod]
    public void Rollback_NezmenenySuborLenNaCitanie_NevadiObnoveniu()
    {
        // zapis zlyhal prave na subore len na citanie - ten ostal nezmeneny a obnova ho nesmie prepisovat
        var readOnly = Path.Combine(_dir, "VYLUKA.txt");
        File.SetAttributes(readOnly, FileAttributes.ReadOnly);

        var transaction = new FileTransaction(_dir);
        File.WriteAllText(Path.Combine(_dir, "EXPORT3A.txt"), "novy");

        Assert.IsTrue(transaction.TryRollback());
        Assert.AreEqual("povodny", File.ReadAllText(Path.Combine(_dir, "EXPORT3A.txt")));
    }

    [TestMethod]
    public void Commit_ZahodiZalohu()
    {
        var transaction = new FileTransaction(_dir);
        File.WriteAllText(Path.Combine(_dir, "EXPORT3A.txt"), "novy");

        transaction.Commit();
        Assert.AreEqual("novy", File.ReadAllText(Path.Combine(_dir, "EXPORT3A.txt")));
        Assert.IsFalse(Directory.Exists(transaction.BackupPath));
    }
}

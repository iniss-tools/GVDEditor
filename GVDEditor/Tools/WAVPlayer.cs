using System.Media;
using System.Threading;

namespace GVDEditor.Tools;

/// <summary>
///     Trieda umoznujuca prehravanie zvukovych suborov .WAV.
/// </summary>
//TODO Trieda prehrava zvuky s pauzami.
internal class WavPlayer
{
    private readonly BackgroundWorker _worker = new();

    /// <summary>
    ///     Vytvori novu instanciu triedy <see cref="WavPlayer"/>.
    /// </summary>
    /// <param name="sounds">zoznam zvukov, ktore sa maju prehrat</param>
    /// <param name="wordPause">pauza medzi zvukmi</param>
    public WavPlayer(string[] sounds, int wordPause)
    {
        Sounds = sounds;
        WordPause = wordPause;

        _worker.DoWork += PlayingSoundsDoWork;
    }

    /// <summary>
    ///     Cesty ku zvukovym suborom .WAV, ktore sa maju prehrat.
    /// </summary>
    public string[] Sounds { get; }

    /// <summary>
    ///    Pauza medzi zvukmi v milisekundach.
    /// </summary>
    public int WordPause { get; }

    /// <summary>
    ///     Zacne prehravanie zvukovych suborov (asynchronne).
    /// </summary>
    public void StartPlay() => _worker.RunWorkerAsync();

    private void PlayingSoundsDoWork(object sender, DoWorkEventArgs e)
    {
        var soundps = new List<SoundPlayer>();
        foreach (var sound in Sounds)
        {
            var p = new SoundPlayer(sound);
            p.Load();
            soundps.Add(p);
        }

        foreach (var p in soundps)
        {
            p.PlaySync();
            if (WordPause > 0) Thread.Sleep(WordPause);
        }
    }
}
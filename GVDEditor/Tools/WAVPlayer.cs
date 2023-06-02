using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace GVDEditor.Tools;

/// <summary>
///     Trieda umoznujuca prehravanie zvukovych suborov .WAV.
/// </summary>
internal sealed class WavPlayer : IDisposable
{
    private WaveOutEvent _outputDevice;
    private AudioFileReader[] _audios;

    /// <summary>
    ///     Vytvori novu instanciu triedy <see cref="WavPlayer"/>.
    /// </summary>
    /// <param name="sounds">zoznam zvukov, ktore sa maju prehrat</param>
    /// <param name="soundsOffset">pauza medzi zvukmi</param>
    public WavPlayer(string[] sounds, int soundsOffset)
    {
        Sounds = sounds;
        SoundsOffset = soundsOffset;
    }

    /// <summary>
    ///     Cesty ku zvukovym suborom .WAV, ktore sa maju prehrat.
    /// </summary>
    public string[] Sounds { get; }

    /// <summary>
    ///    Pauza medzi zvukmi v milisekundach.
    /// </summary>
    public int SoundsOffset { get; }

    /// <summary>
    ///     Zacne prehravanie zvukovych suborov (asynchronne).
    /// </summary>
    public void StartPlay()
    {
        try
        {
            var readers = new ISampleProvider[Sounds.Length];
            _audios = new AudioFileReader[Sounds.Length];
            for (var i = 0; i < readers.Length; i++)
            {
                _audios[i] = new AudioFileReader(Sounds[i]);
                readers[i] = new WdlResamplingSampleProvider(_audios[i], 352000);

                if (i == readers.Length - 1)
                    break;

                switch (SoundsOffset)
                {
                    case < 0:
                        readers[i] = new OffsetSampleProvider(readers[i]) { Take = TimeSpan.FromMilliseconds(_audios[i].TotalTime.TotalMilliseconds - (-SoundsOffset)) };
                        break;
                    case > 0:
                        readers[i] = new OffsetSampleProvider(readers[i]) { LeadOut = TimeSpan.FromMilliseconds(SoundsOffset) };
                        break;
                }
            }

            var playlist = new ConcatenatingSampleProvider(readers);
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += (_, _) => Dispose();
            _outputDevice.Init(playlist);
            _outputDevice.Play();
        }
        catch
        {
            //ignored (Napr. ak sa ma prehrat .EWA subor)
            Dispose();
        }
    }

    public void Dispose()
    {
        _outputDevice?.Dispose();
        foreach (var audio in _audios)
        {
            audio.Dispose();
        }
    }
}
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Serilog;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Trieda umoznujuca prehravanie zvukovych suborov .WAV a .EWA (dekoduje sa v pamati).
/// </summary>
internal sealed class WavPlayer : IDisposable
{
    private WaveOutEvent? _outputDevice;
    private WaveStream[]? _audios;

    /// <summary>
    /// Vytvori novu instanciu triedy <see cref="WavPlayer"/>.
    /// </summary>
    /// <param name="sounds">zoznam zvukov, ktore sa maju prehrat</param>
    /// <param name="soundsOffset">pauza medzi zvukmi</param>
    public WavPlayer(string[] sounds, int soundsOffset)
    {
        Sounds = sounds;
        SoundsOffset = soundsOffset;
    }

    /// <summary>
    /// Cesty ku zvukovym suborom .WAV/.EWA, ktore sa maju prehrat.
    /// </summary>
    public string[] Sounds { get; }

    /// <summary>
    /// Pauza medzi zvukmi v milisekundach.
    /// </summary>
    public int SoundsOffset { get; }

    /// <summary>
    /// Zacne prehravanie zvukovych suborov (asynchronne).
    /// </summary>
    public void StartPlay()
    {
        try
        {
            // zvuk, ktory sa neda otvorit (chybajuci subor v banke), sa preskoci - prehra sa zvysok hlasenia
            var audios = new List<WaveStream>();
            var providers = new List<ISampleProvider>();
            foreach (var sound in Sounds)
            {
                try
                {
                    var audio = Open(sound);
                    audios.Add(audio);
                    providers.Add(ToMono(audio.ToSampleProvider()));
                }
                catch (Exception e) when (e is IOException or UnauthorizedAccessException or FormatException or MmException)
                {
                    Log.Warning(e, "Zvuk {Path} sa neda prehrat, preskakuje sa", sound);
                }
            }

            _audios = [.. audios];
            if (audios.Count == 0)
                return;

            var readers = new ISampleProvider[audios.Count];
            for (var i = 0; i < readers.Length; i++)
            {
                var audio = audios[i];
                readers[i] = new WdlResamplingSampleProvider(providers[i], 352000);

                if (i == readers.Length - 1)
                    break;

                switch (SoundsOffset)
                {
                    case < 0:
                        readers[i] = new OffsetSampleProvider(readers[i]) { Take = TimeSpan.FromMilliseconds(audio.TotalTime.TotalMilliseconds - (-SoundsOffset)) };
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
        catch (Exception e)
        {
            // prehravanie je len ukazka - chyba (chybajuci subor, neplatny WAV, zvukove zariadenie) nema zhodit okno
            Log.Warning(e, "Prehratie {Count} zvukov zlyhalo", Sounds.Length);
            Dispose();
        }
    }

    /// <summary>
    /// Zvuky retazi <see cref="ConcatenatingSampleProvider"/>, ktory vyzaduje rovnaky pocet kanalov - stereo sa zmiesa do mono.
    /// </summary>
    private static ISampleProvider ToMono(ISampleProvider provider)
        => provider.WaveFormat.Channels == 2 ? new StereoToMonoSampleProvider(provider) : provider;

    /// <summary>
    /// Otvori subor zvuku; .EWA sa dekoduje do pamate, komprimovany WAV (napr. MP3) sa prevedie na PCM kodekom Windows.
    /// </summary>
    private static WaveStream Open(string path)
    {
        WaveFileReader reader;
        if (Path.GetExtension(path).Equals(SoundFileElement.EWAExt, StringComparison.OrdinalIgnoreCase))
        {
            var wav = new MemoryStream();
            using (var ewa = File.OpenRead(path))
                EwaCodec.Decode(ewa, wav);
            wav.Position = 0;
            reader = new WaveFileReader(wav);
        }
        else
        {
            reader = new WaveFileReader(path);
        }

        if (reader.WaveFormat.Encoding is WaveFormatEncoding.Pcm or WaveFormatEncoding.IeeeFloat or WaveFormatEncoding.Extensible)
            return reader;

        try
        {
            return WaveFormatConversionStream.CreatePcmStream(reader);
        }
        catch
        {
            reader.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        _outputDevice?.Dispose();
        if (_audios == null)
            return;

        foreach (var audio in _audios)
            audio.Dispose();
    }
}

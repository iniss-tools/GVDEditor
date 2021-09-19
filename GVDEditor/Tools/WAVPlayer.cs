using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Threading;

namespace GVDEditor.Tools
{
    internal class WAVPlayer
    {
        private readonly BackgroundWorker worker = new();

        public WAVPlayer(string[] sounds, int wordPause)
        {
            Sounds = sounds;
            WordPause = wordPause;

            worker.DoWork += PlayingSoundsDoWork;
        }

        public string[] Sounds { get; }
        public int WordPause { get; }

        public void StartPlay()
        {
            worker.RunWorkerAsync();

        }

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
}
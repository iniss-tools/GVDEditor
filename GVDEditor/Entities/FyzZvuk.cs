namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujúca fyzický zvuk zo zvukovej banky.
    /// </summary>
    public sealed class FyzZvuk
    {
        /// <summary>
        ///     Vytvori novu instanciu triedy <see cref="FyzZvuk"/>.
        /// </summary>
        /// <param name="dir">Priečinok, do ktorého fyzický zvuk patrí</param>
        /// <param name="name">Názov zvuku.</param>
        /// <param name="nazevZvukuPro">Názov zvuku (alternatívna forma).</param>
        /// <param name="fileName">Názov súboru so zvukom (vrátane prípony).</param>
        /// <param name="text"></param>
        /// <param name="language"></param>
        public FyzZvuk(FyzZvukDir dir, string name, string nazevZvukuPro, string fileName, string text, Language language)
        {
            Dir = dir;
            Name = name;
            NazevZvukuPro = nazevZvukuPro;
            FileName = fileName;
            Text = text;
            Language = language;
        }

        /// <summary>
        ///     Vrati priečinok, do ktorého fyzický zvuk patrí.
        /// </summary>
        public FyzZvukDir Dir { get; }

        /// <summary>
        ///     Vrati názov zvuku.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Vrati názov zvuku (alternatívna forma).
        /// </summary>
        public string NazevZvukuPro { get; }

        /// <summary>
        ///     Vrati názov súboru so zvukom (vrátane prípony).
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     Vrati text hlásenia.
        /// </summary>
        public string Text { get; }

        /// <summary>
        ///     Vrati jazyk hlásenia.
        /// </summary>
        public Language Language { get; }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
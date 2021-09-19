namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda obsahujuca informacie o audio linke.
    /// </summary>
    public sealed class Audio
    {
        /// <summary>
        ///     Vrati alebo nastavi stanicu audio linky (použije sa len identifikátor stanice).
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi názov audio linky.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi skrátený názov.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi názov fronty.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi mixer.
        /// </summary>
        public string Mixer { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi identifikator zvukovej karty.
        /// </summary>
        public string SoundCard { get; set; }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
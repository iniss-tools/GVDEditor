namespace GVDEditor.Entities
{
    /// <summary>
    ///     Nástupište na stanici
    /// </summary>
    public sealed record Platform
    {
        /// <summary>
        ///     Predvolené nástupište
        /// </summary>
        public static readonly Platform None = new("N", "Nedefinované", "");

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="key">kľúč nástupišťa</param>
        /// <param name="fullName">celý názov nástupišťa</param>
        /// <param name="soundName">názov zvuku nástupišťa (bez prípony)</param>
        public Platform(string key, string fullName, string soundName)
        {
            Key = key;
            FullName = fullName;
            SoundName = soundName;
        }

        /// <summary>
        ///     Kľúč nástupišťa
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Celý názov nástupišťa
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Názov zvuku nástupišťa (bez prípony)
        /// </summary>
        public string SoundName { get; set; }

        /// <summary>
        ///     This
        /// </summary>
        public Platform This => this;

        /// <summary>
        ///     Porovná nástupišťa (porovná iba ich kľúče)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsKeys(Platform other)
        {
            return other.Key == Key;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Key;
        }
    }
}
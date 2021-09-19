namespace GVDEditor.Entities
{
    /// <summary>
    ///     Rozhranie pre všetky triedy tabuľovitého typu.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        ///     Nazov tatule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Kluc tatule
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Textovy komentar ku tabuli
        /// </summary>
        public string Comment { get; set; }
    }
}
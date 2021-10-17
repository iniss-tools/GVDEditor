namespace GVDEditor.Entities
{
    /// <summary>
    ///     Definuje poziciu zaznamu tabule.
    /// </summary>
    public sealed class TablePosition
    {
        /// <summary>
        ///     Ciselna pozicia.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     Druh zaznamu.
        /// </summary>
        public TableViewType TypeView { get; set; }

        /// <summary>
        ///     Fyzicka tabula, na ktorej sa zaznam zobrazuje.
        /// </summary>
        public TablePhysical TablePhysical { get; set; }
    }
}
namespace GVDEditor.Entities
{
    /// <summary>
    ///     Vlak, ktorý sa zobrazuje na tabuli
    /// </summary>
    public sealed class TableTrain
    {
        /// <summary>
        ///     Vlak, ktorý sa zobrazuje na tabuli
        /// </summary>
        public Train Train { get; set; }

        /// <summary>
        ///     Text na tabuli
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     ID písma
        /// </summary>
        public int FontID { get; set; }
    }
}
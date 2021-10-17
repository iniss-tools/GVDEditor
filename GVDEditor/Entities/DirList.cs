using System.Drawing;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trida obsahujuca informácie o priečinku s grafikonom.
    /// </summary>
    public sealed class DirList
    {
        /// <summary>
        ///     Vrati alebo nastavi názov priečinka.
        /// </summary>
        public string DirName { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi celá cestu k priečinku s grafikonom.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi port pre vzdialené ovládanie tabúľ.
        /// </summary>
        public int? TablePort { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi port pre zabezpečenie vzdialeného hlásenia.
        /// </summary>
        public int? ReportPort { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi farbu zobrazujúcu v INISS ako farba pozadia vlaku na pracovnej ploche ako odlíšenie od vlakov iných staníc.
        /// </summary>
        public Color? BackColor { get; set; }
    }
}
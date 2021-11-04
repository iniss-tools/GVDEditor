using System.Collections.Generic;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca logicku tabulu.
    /// </summary>
    public sealed class TableLogical : ITable
    {
        /// <summary>
        ///     Vytvori novu instanciu triedy <see cref="TableLogical"/>.
        /// </summary>
        public TableLogical() => Records = new List<TableRecord>();

        /// <summary>
        ///     Typ pouzitia logickej tabule.
        /// </summary>
        public TableViewType ViewType { get; set; }

        /// <summary>
        ///     Dalsie data ohladom pouzitia tejto logickej tabule.
        /// </summary>
        public string TypeViewFlags { get; set; }

        /// <summary>
        ///     Zaznamy logickej tabule.
        /// </summary>
        public List<TableRecord> Records { get; set; }

        /// <summary>
        ///     Odkaz na seba, pouzivane pre DataSource.
        /// </summary>
        public TableLogical This => this;

        /// <summary>
        ///     Nazov logickej tatule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Kluc logickej tatule.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Textovy komentar ku tabuli.
        /// </summary>
        public string Comment { get; set; }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
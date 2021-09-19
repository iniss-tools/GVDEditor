using System.Collections;
using System.Collections.Generic;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Definuje zaznam tabule
    /// </summary>
    public sealed class TableRecord : IEnumerable
    {
        /// <summary>
        ///     Konstruktor
        /// </summary>
        public TableRecord()
        {
            Positions = new List<TablePosition>();
        }

        /// <summary>
        ///     Pozicie zaznamu
        /// </summary>
        public List<TablePosition> Positions { get; set; }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return Positions.GetEnumerator();
        }
    }
}
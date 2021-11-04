using System.Collections.Generic;
using System.Linq;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Dopravca vlaku (operátor).
    /// </summary>
    /// <param name="Id">identifikátor dopravcu</param>
    /// <param name="Name">názov dopravcu</param>
    public sealed record Operator(int Id, string Name)
    {
        /// <summary>
        ///     Predvolený dopravca.
        /// </summary>
        public static readonly Operator None = new(-1, "Žiadny");

        /// <summary>
        ///     Identifikátor dopravcu.
        /// </summary>
        public int Id { get; } = Id;

        /// <summary>
        ///     Názov dopravcu.
        /// </summary>
        public string Name { get; set; } = Name;

        /// <summary>
        ///     This.
        /// </summary>
        public Operator This => this;

        /// <summary>
        ///     Vráti dopravcu zo zadaného listu podľa identifikátora dopravcu.
        /// </summary>
        /// <param name="operators">list dopravcov</param>
        /// <param name="id">identifikátor dopravcu</param>
        /// <returns>dopravcu, ak ho nenájde, vráti <see langword="null" /></returns>
        public static Operator GetFromID(IEnumerable<Operator> operators, int id)
        {
            return operators.FirstOrDefault(dopravca => dopravca.Id == id);
        }

        /// <summary>
        ///     Vráti dopravcu zo zadaného listu podľa názvu dopravcu
        /// </summary>
        /// <param name="operators">list dopravcov</param>
        /// <param name="name">názov dopravcu</param>
        /// <returns>dopravcu, ak ho nenájde, vráti <see langword="null" /></returns>
        public static Operator GetFromName(IEnumerable<Operator> operators, string name)
        {
            return operators.FirstOrDefault(dopravca => dopravca.Name == name);
        }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
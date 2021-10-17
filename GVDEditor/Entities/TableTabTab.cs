namespace GVDEditor.Entities
{
    /// <summary>
    ///     Definuje spravanie obsahu sekcie katalogovej tabule.
    /// </summary>
    public sealed class TableTabTab
    {
        /// <summary>
        ///     Predvolený TabTab - žiadny.
        /// </summary>
        public static readonly TableTabTab Empty = new() { Key = "Žiadny", Text = "" };


        /// <summary>
        ///     Kluc TabTab.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Obsah TabTab ako text.
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is TableTabTab tab && tab.Key == Key;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Key != null ? Key.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public override string ToString() => Key;

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.TableTabTab" />
        ///     objects are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(TableTabTab left, TableTabTab right) => Equals(left, right);

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.TableTabTab" /> objects have
        ///     different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(TableTabTab left, TableTabTab right) => !Equals(left, right);
    }
}
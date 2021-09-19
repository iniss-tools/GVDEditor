using System.Collections.Generic;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Jazyková mútácia používaná pre vlak
    /// </summary>
    public sealed class Language
    {
        /// <summary>
        ///     Kľúč jazyka
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Je jazyk v stanici hlavný
        /// </summary>
        public bool IsBasic { get; set; }

        /// <summary>
        ///     Celý názov jazyka
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Názov jazyka vo zvukovej banke
        /// </summary>
        public string FyzBankName { get; set; }

        /// <summary>
        ///     Súbor FYZZVUK.dat jazyka vo zvukovej banky
        /// </summary>
        public string FileFyzZvuk { get; set; }

        /// <summary>
        ///     Relatívna cesta k priečinku zvukov jazyka vo zvukej banke
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        ///     This
        /// </summary>
        public Language This => this;

        /// <summary>
        ///     Vráti jazyk z listujazyk z listu podľa kľúča jazyka
        /// </summary>
        /// <param name="langs">list zvukov</param>
        /// <param name="key">kľúč jazyka</param>
        /// <returns>jazyk z listu, resp. <see langword="null" /> ak nebola nájdená zhoda</returns>
        public static Language GetLanguageFromKey(IEnumerable<Language> langs, string key)
        {
            foreach (var jazyk in langs)
                if (jazyk.Key == key)
                    return jazyk;

            return null;
        }

        /// <summary>
        ///     Vráti hlavný jazyk z listu zvukov
        /// </summary>
        /// <param name="langs">list jazykov</param>
        /// <returns>hlavný jazyk alebo <see langword="null" /> ak zadaný list neobsahuje hlavný jazyk</returns>
        public static Language GetBasicLanguage(IEnumerable<Language> langs)
        {
            foreach (var jazyk in langs)
                if (jazyk.IsBasic)
                    return jazyk;

            return null;
        }

        /// <summary>
        ///     Zistí, či v zadanom poli jazykov sa nachádza prvok s rovnakým kľúčom ako zadaný kľúč
        /// </summary>
        /// <param name="languages">list jazykov</param>
        /// <param name="key">kľúč jazyka</param>
        /// <returns>
        ///     <see langword="true" /> ak sa v poli nachádza prvok s rovnakým kľučom ako zadaný kľúč, inak
        ///     <see langword="false" />
        /// </returns>
        public static bool ContainsKey(List<Language> languages, string key)
        {
            if (key == null || languages == null || languages.Count == 0) 
                return false;

            foreach (var language in languages)
                if (language.Key == key)
                    return true;

            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        private bool Equals(Language other)
        {
            return Key == other.Key && IsBasic == other.IsBasic && Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Language)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Key != null ? Key.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ IsBasic.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.Language" /> objects
        ///     are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(Language left, Language right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.Language" /> objects have different
        ///     values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(Language left, Language right)
        {
            return !Equals(left, right);
        }
    }
}
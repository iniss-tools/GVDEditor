using System;
using System.Collections.Generic;
using System.Linq;
using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca stanicu/zastávku, v ktorej može zastaviť vlak.
    /// </summary>
    /// <param name="ID">Identifikátor stanice.</param>
    /// <param name="Name">Názov stanice.</param>
    /// <param name="IsInShortReport">Ci sa bude hlásiť v krátkom hlásení.</param>
    /// <param name="IsInLongReport">Ci sa bude hlásiť v dlhom hlásení.</param>
    /// <param name="IsCustom">Ci stanica nepochadza zo zvukovej banky ale zo suboru Stanice.txt.</param>
    public sealed record Station(string ID, string Name, bool IsInShortReport = false, bool IsInLongReport = false, bool IsCustom = false) : IComparable
    {
        /// <summary>
        ///     Identifikátor stanice.
        /// </summary>
        public string ID { get; set; } = ID;

        /// <summary>
        ///     Názov stanice.
        /// </summary>
        public string Name { get; set; } = Name;

        /// <summary>
        ///     Stanica sa bude hlásiť v krátkom hlásení.
        /// </summary>
        public bool IsInShortReport { get; set; } = IsInShortReport;

        /// <summary>
        ///     Stanica sa bude hlásiť v dlhom hlásení.
        /// </summary>
        public bool IsInLongReport { get; set; } = IsInLongReport;

        /// <summary>
        ///     Je použiváteľom definovaná stanica (zo súboru STANICE.TXT).
        /// </summary>
        public bool IsCustom { get; set; } = IsCustom;

        /// <inheritdoc />
        public int CompareTo(object obj) => string.Compare(Name, obj.ToString(), StringComparison.Ordinal);

        /// <summary>
        ///     Vráti stanicu z <see cref="GlobData.Stations" /> alebo <see cref="GlobData.CustomStations" />
        ///     podľa identifikátora stanice.
        /// </summary>
        /// <param name="id">Identifikátor stanice.</param>
        /// <returns><see cref="Station" />. Ak nenašlo žiadnu zhodu, vrati stanicu s nazvom zadaneho ID.</returns>
        public static Station GetFromID(string id)
        {
            var stationsWithSameId = GlobData.Stations.Where(station => station.ID == id);
            foreach (var st in stationsWithSameId) 
                return new Station(st.ID, st.Name);

            stationsWithSameId = GlobData.CustomStations.Where(customStation => customStation.ID == id);
            foreach (var cst in stationsWithSameId)
                return new Station(cst.ID, cst.Name);

            return new Station(id, id);
        }

        /// <summary>
        ///     Vráti stanicu z <see cref="GlobData.Stations" /> alebo <see cref="GlobData.CustomStations" /> podľa názvu stanice
        /// </summary>
        /// <param name="name">názov stanice</param>
        /// <returns><see cref="Station" /> alebo <see langword="null" /> ak nenašlo žiadnu zhodu</returns>
        public static Station GetFromName(string name)
        {
            name = name.Replace(".", "").Replace("-", "").ToLower();
            name = Utils.RemoveDiacritics(name);

            foreach (var st in GlobData.Stations)
            {
                var ns = st.Name.Replace(".", "").Replace("-", "").ToLower();
                ns = Utils.RemoveDiacritics(ns);
                if (ns == name) return new Station(st.ID, st.Name);
            }

            foreach (var cst in GlobData.CustomStations)
            {
                var ns = cst.Name.Replace(".", "").Replace("-", "").ToLower();
                ns = Utils.RemoveDiacritics(ns);
                if (ns == name) return new Station(cst.ID, cst.Name);
            }

            return null;
        }

        /// <summary>
        ///     Vráti stanice dostupné zo zvukovej banky (prehľadáva sa priečinok R1).
        /// </summary>
        /// <returns>list staníc.</returns>
        public static List<Station> GetStations()
        {
            return (
                from soundE 
                in GlobData.Sounds 
                where soundE.Dir.Name == "R1" 
                select new Station(soundE.Name, soundE.Text.Replace(",", ""))
                ).ToList();
        }

        /// <summary>
        ///     Vráti list staníc podľa poľa staníc zapísaných v reťazci ako identifikátory staníc.
        /// </summary>
        /// <param name="stations">pole staníc ako reťazec</param>
        /// <returns>list staníc</returns>
        /// <exception cref="ArgumentException">ak stanica neexistuje</exception>
        public static List<Station> GetStationsFromIDListString(string stations)
        {
            stations = stations.Trim().Replace(" ", "");
            var langsSplitted = stations.Split(',');
            return langsSplitted.Select(GetFromID).ToList();
        }

        /// <summary>
        ///     Vráti list staníc podľa poľa staníc zapísaných v reťazci ako názvy staníc.
        /// </summary>
        /// <param name="stations">pole staníc ako reťazec</param>
        /// <returns>list staníc</returns>
        /// <exception cref="ArgumentException">ak stanica neexistuje</exception>
        public static List<Station> GetStationsFromNameListString(string stations)
        {
            var stationsList = new List<Station>();

            stations = stations.Trim().Replace(" ", "");
            var stationsStringArr = stations.Split(',');

            foreach (var s in stationsStringArr)
            {
                var station = GetFromName(s);
                if (station == null) 
                    throw new ArgumentException($"Stanica \"{s}\" neexistuje.");
                stationsList.Add(station);
            }

            return stationsList;
        }

        /// <summary>
        ///     Skopíruje trasu vlaku.
        /// </summary>
        /// <param name="stations">list staníc.</param>
        /// <returns>skopírovaná trasa vlaku.</returns>
        public static List<Station> CopyRoute(IEnumerable<Station> stations) 
            => stations.Select(station => new Station(station)).ToList();

        /// <summary>
        ///     Porovná stanice podľa názvu staníc.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EqualsName(string name) => !string.IsNullOrEmpty(name) && name == Name;

        /// <summary>
        ///     Porovná zoznamy staníc vo všetkých vlastnostiach.
        /// </summary>
        /// <param name="st1">list staníc 1</param>
        /// <param name="st2">list staníc 2</param>
        /// <returns></returns>
        public static bool SequencesEqual(List<Station> st1, List<Station> st2)
        {
            if (st1.Count == st2.Count)
                return !st1.Where((station, i) => station != st2[i]).Any();

            return false;
        }

        /// <summary>
        ///     Zistí, sa v liste staníc nachádza stanica so zadaným názvom stanice.
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ContainsName(IEnumerable<Station> stations, string name) 
            => !string.IsNullOrEmpty(name) && stations.Any(station => station.Name == name);

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
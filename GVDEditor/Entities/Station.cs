using System;
using System.Collections.Generic;
using System.Linq;
using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Stanica/zastávka, v ktorej može zastaviť vlak
    /// </summary>
    public sealed class Station : IComparable
    {
        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="id">identifikátor stanice</param>
        /// <param name="name">názov stanice</param>
        /// <param name="inShortReport">bude sa hlásiť v krátkom hlásení</param>
        /// <param name="inLongReport">bude sa hlásiť v dlhom hlásení</param>
        /// <param name="isCustom">ci stanica nepochadza zo zvukovej banky ale zo suboru Stanice.txt</param>
        public Station(string id, string name, bool inShortReport = false, bool inLongReport = false, bool isCustom = false)
        {
            ID = id;
            Name = name;
            IsInShortReport = inShortReport;
            IsInLongReport = inLongReport;
            IsCustom = isCustom;
        }

        /// <summary>
        ///     Identifikátor stanice
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     Názov stanice
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Stanica sa bude hlásiť v krátkom hlásení
        /// </summary>
        public bool IsInShortReport { get; set; }

        /// <summary>
        ///     Stanica sa bude hlásiť v dlhom hlásení
        /// </summary>
        public bool IsInLongReport { get; set; }

        /// <summary>
        ///     Je použiváteľom definovaná stanica (zo súboru STANICE.TXT)
        /// </summary>
        public bool IsCustom { get; set; }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            return string.Compare(Name, obj.ToString(), StringComparison.Ordinal);
        }

        /// <summary>
        ///     Vráti stanicu z <see cref="GlobData.Stations" /> alebo <see cref="GlobData.CustomStations" /> podľa identifikátora
        ///     stanice
        /// </summary>
        /// <param name="id">identifikátor stanice</param>
        /// <returns><see cref="Station" /> alebo <see langword="null" /> ak nenašlo žiadnu zhodu</returns>
        public static Station GetStationFromID(string id)
        {
            foreach (var st in GlobData.Stations.Where(st => st.ID == id)) return new Station(st.ID, st.Name);

            foreach (var customStation in GlobData.CustomStations.Where(customStation => customStation.ID == id))
                return new Station(customStation.ID, customStation.Name);

            return new Station(id, id);
        }

        /// <summary>
        ///     Vráti stanicu z <see cref="GlobData.Stations" /> alebo <see cref="GlobData.CustomStations" /> podľa názvu stanice
        /// </summary>
        /// <param name="name">názov stanice</param>
        /// <returns><see cref="Station" /> alebo <see langword="null" /> ak nenašlo žiadnu zhodu</returns>
        public static Station GetStationFromName(string name)
        {
            name = name.Replace(".", "").Replace("-", "").ToLower();
            name = Utils.RemoveDiacritics(name);

            foreach (var st in GlobData.Stations)
            {
                var ns = st.Name.Replace(".", "").Replace("-", "").ToLower();
                ns = Utils.RemoveDiacritics(ns);
                if (ns == name) return new Station(st.ID, st.Name);
            }

            foreach (var customStation in GlobData.CustomStations)
            {
                var ns = customStation.Name.Replace(".", "").Replace("-", "").ToLower();
                ns = Utils.RemoveDiacritics(ns);
                if (ns == name) return new Station(customStation.ID, customStation.Name);
            }

            return null;
        }

        /// <summary>
        ///     Vráti stanice dostupné zo zvukovej banky (prehľadáva sa priečinok R1)
        /// </summary>
        /// <returns>list staníc</returns>
        public static List<Station> GetStations()
        {
            var list = new List<Station>();

            foreach (var soundE in GlobData.Sounds)
                if (soundE.Dir.Name == "R1")
                    list.Add(new Station(soundE.Name, soundE.Text.Replace(",", "")));

            return list;
        }

        /// <summary>
        ///     Kopíruje trasu vlaku
        /// </summary>
        /// <param name="stations">list staníc</param>
        /// <returns>skopírovaná trasa vlaku</returns>
        public static List<Station> CopyRoute(IEnumerable<Station> stations)
        {
            var result = new List<Station>();

            foreach (var station in stations)
                result.Add(new Station(station.ID, station.Name, station.IsInShortReport, station.IsInLongReport));

            return result;
        }

        /// <summary>
        ///     Vráti list staníc podľa poľa staníc zapísaných v reťazci ako identifikátory staníc
        /// </summary>
        /// <param name="stations">pole staníc ako reťazec</param>
        /// <returns>list staníc</returns>
        /// <exception cref="ArgumentException">ak stanica neexistuje</exception>
        public static List<Station> GetStationsFromIDListString(string stations)
        {
            var stationsList = new List<Station>();

            stations = stations.Trim().Replace(" ", "");
            var langsArrayS = stations.Split(',');

            foreach (var s in langsArrayS)
                stationsList.Add(GetStationFromID(s));

            return stationsList;
        }

        /// <summary>
        ///     Vráti list staníc podľa poľa staníc zapísaných v reťazci ako názvy staníc
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
                var station = GetStationFromName(s);

                if (station == null) throw new ArgumentException($"Stanica \"{s}\" neexistuje.");

                stationsList.Add(station);
            }

            return stationsList;
        }

        /// <summary>
        ///     Porovná stanice podľa názvu staníc
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EqualsName(string name)
        {
            return !string.IsNullOrEmpty(name) && name == Name;
        }

        /// <summary>
        ///     Porovná listi staníc vo všetkých vlastnostiah
        /// </summary>
        /// <param name="st1">list staníc 1</param>
        /// <param name="st2">list staníc 2</param>
        /// <returns></returns>
        public static bool SequencesEqual(List<Station> st1, List<Station> st2)
        {
            if (st1.Count == st2.Count)
            {
                for (var i = 0; i < st1.Count; i++)
                    if (st1[i] != st2[i])
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Zistí, sa v liste staníc nachádza stanica so zadaným názvom stanice
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ContainsName(IEnumerable<Station> stations, string name)
        {
            return !string.IsNullOrEmpty(name) && stations.Any(station => station.Name == name);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ID != null ? ID.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsInShortReport.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInLongReport.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Station stanica)
                return ID == stanica.ID && IsInLongReport == stanica.IsInLongReport &&
                       IsInShortReport == stanica.IsInShortReport;
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Station left, Station right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Station left, Station right)
        {
            return !Equals(left, right);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca kolaj
    /// </summary>
    public sealed record Track
    {
        /// <summary>
        ///     Predvolená koľaj
        /// </summary>
        public static readonly Track None = new("K", "K", "Nedefinovaná", Platform.None, "", "Nedefinovaná");

        /// <summary>
        ///     Konstruktor
        /// </summary>
        public Track()
        {
            Tabule = new List<TableLogical>();
        }

        /// <summary>
        ///     Konstruktor
        /// </summary>
        public Track(string key, string name, string fullname, Platform nastupiste, string sound, string trackName)
        {
            Tabule = new List<TableLogical>();

            Key = key;
            Name = name;
            FullName = fullname;
            Nastupiste = nastupiste;
            SoundName = sound;
            TrackName = trackName;
        }

        /// <summary>
        ///     Identifikátor kolaje
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Názov kolaje
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Cely názov kolaje
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Nastupiste kolaje
        /// </summary>
        public Platform Nastupiste { get; set; }

        /// <summary>
        ///     Názov kolaje
        /// </summary>
        public string TrackName { get; set; }

        /// <summary>
        ///     Nazov zvuku kolaje
        /// </summary>
        public string SoundName { get; set; }

        /// <summary>
        ///     Logicke tabule nachadzajuce sa na tejto kolaji
        /// </summary>
        public List<TableLogical> Tabule { get; }

        /// <summary>
        ///     Odkaz na seba, pouzite pre DataSource
        /// </summary>
        public Track This => this;

        /// <summary>
        ///     Cely názov nástupista - len pre potreby GUI
        /// </summary>
        public string NastupisteCelyNazov => Nastupiste.FullName;

        /// <summary>
        ///     Vyhlada kolaj z listu podla zadaneho identifikatora kolaje
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Track GetTrackFromId(IEnumerable<Track> tracks, string id)
        {
            return tracks.FirstOrDefault(kolaj => kolaj.Key == id);
        }

        /// <summary>
        ///     Porovna identifikatory kolaji
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsKeys(Track other)
        {
            return other.Key == Key;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Key;
        }
    }
}
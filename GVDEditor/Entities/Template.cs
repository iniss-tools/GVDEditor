using System.Collections.Generic;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca vzory tras vlakov.
    /// </summary>
    public sealed class Template
    {
        /// <summary>
        ///     Konstruktor
        /// </summary>
        public Template()
        {
            Stations = new List<Station>();
            Trains = new List<Train>();
        }

        /// <summary>
        ///     Identifikator trasy.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Stanice vo vzore trasy.
        /// </summary>
        public List<Station> Stations { get; set; }

        /// <summary>
        ///     Vlaky, ktore maju tuto trasu.
        /// </summary>
        public List<Train> Trains { get; set; }
    }
}
using System.Collections.Generic;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca vzory tras vlakov.
    /// </summary>
    public sealed class Template
    {
        /// <summary>
        ///     Identifikator trasy.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Stanice vo vzore trasy.
        /// </summary>
        public List<Station> Stations { get; set; } = new();

        /// <summary>
        ///     Vlaky, ktore maju tuto trasu.
        /// </summary>
        public List<Train> Trains { get; set; } = new();
    }
}
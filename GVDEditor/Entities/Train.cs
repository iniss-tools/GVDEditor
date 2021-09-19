using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GVDEditor.Tools;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujuca vlak
    /// </summary>
    public sealed class Train
    {
        private NumberVariant _numberVariant;
        private Routing _routing;

        /// <summary>
        ///     Konstruktor
        /// </summary>
        public Train()
        {
            StaniceZoSmeru = new List<Station>();
            StaniceDoSmeru = new List<Station>();
            Languages = new List<Language>();
            Doplnky = new List<Dodatok>();
            Radenia = new List<Radenie>();
        }

        /// <summary>
        ///     Interne id vlaku
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Cislo vlaku
        /// </summary>
        public string Number
        {
            get => _numberVariant.Number;
            set => _numberVariant.Number = value;
        }

        /// <summary>
        ///     Cislo a varianta vlaku
        /// </summary>
        public NumberVariant NumberVariant
        {
            get => _numberVariant;
            set
            {
                _numberVariant = value;
                _numberVariant.Number = Number;
                _numberVariant.Variant = Variant;
            }
        }

        /// <summary>
        ///     Typ vlaku
        /// </summary>
        public TrainType Type { get; set; }

        /// <summary>
        ///     Nazov vlaku
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Smerovanie vlaku
        /// </summary>
        public Routing Routing
        {
            get => _routing;
            set
            {
                _routing = value;
                RoutingImage = _routing.Image;
            }
        }

        /// <summary>
        ///     Smerovanie vlaku vo forme obrazku
        /// </summary>
        public Bitmap RoutingImage { get; set; }

        /// <summary>
        ///     Stanice zo smeru
        /// </summary>
        public List<Station> StaniceZoSmeru { get; }

        /// <summary>
        ///     Stanice do smeru
        /// </summary>
        public List<Station> StaniceDoSmeru { get; }

        /// <summary>
        ///     Prichod vlaku do stanice (nullable)
        /// </summary>
        public DateTime? Arrival { get; set; }

        /// <summary>
        ///     Odchod vlaku zo stanice (nullable)
        /// </summary>
        public DateTime? Departure { get; set; }

        /// <summary>
        ///     Vychodzia stanica
        /// </summary>
        public Station StartingStation { get; set; }

        /// <summary>
        ///     Konecna stanica
        /// </summary>
        public Station EndingStation { get; set; }

        /// <summary>
        ///     Kolaj na stanici v ktorej stoji vlak
        /// </summary>
        public Track Track { get; set; }

        /// <summary>
        ///     Dopravca vlaku
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        ///     Datumove obmedzenie v textovej forme
        /// </summary>
        public string DateLimitText { get; set; }

        /// <summary>
        ///     Jazykove mutacie hlasenia vlaku
        /// </summary>
        public List<Language> Languages { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak medzistatny
        /// </summary>
        public bool IsMedzistatny { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak dialkovy
        /// </summary>
        public bool IsDialkovy { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak mimoriadny
        /// </summary>
        public bool IsMimoriadny { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak miestenkovy
        /// </summary>
        public bool IsMiestenkovy { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak lozkovy
        /// </summary>
        public bool IsIbaLozkovy { get; set; }

        /// <summary>
        ///     Ci ma vlak priznak nizkopodlazny
        /// </summary>
        public bool IsNizkopodlazny { get; set; }

        /// <summary>
        ///     Zaciatok platnosti datumoveho obmedzenia
        /// </summary>
        public DateTime ZaciatokPlatnosti { get; set; }

        /// <summary>
        ///     Koniec platnosti datumoveho obmedzenia
        /// </summary>
        public DateTime KoniecPlatnosti { get; set; }

        /// <summary>
        ///     Linka na prichode
        /// </summary>
        public string LineArrival { get; set; }

        /// <summary>
        ///     Linka na odchode
        /// </summary>
        public string LineDeparture { get; set; }

        /// <summary>
        ///     Dodatkove hlasenia vlaku
        /// </summary>
        public List<Dodatok> Doplnky { get; set; }

        /// <summary>
        ///     Varianta vlaku
        /// </summary>
        public int Variant
        {
            get => _numberVariant.Variant;
            set => _numberVariant.Variant = value;
        }

        /// <summary>
        ///     Radenie vlaku
        /// </summary>
        public List<Radenie> Radenia { get; }

        /// <summary>
        ///     Vrati prvy vyskyt vlaku v zozname so specifikovanymi vlastnostami
        /// </summary>
        /// <param name="trains">zoznam vsetkych vlakov</param>
        /// <param name="trainNum">cislo vlaku</param>
        /// <param name="trainName">nazov vlaku</param>
        /// <param name="trainType">typ vlaku</param>
        /// <param name="variant">variant vlaku</param>
        /// <returns></returns>
        public static Train GetTrain(IEnumerable<Train> trains, string trainNum, string trainName, TrainType trainType, int variant)
        {
            return trains.FirstOrDefault(train =>
                train.Number == trainNum && train.Name == trainName && train.Type == trainType && train.Variant == variant);
        }

        /// <summary>
        ///     Vrati vlak zo zoznamu variant, ktory je hlavna varianta vlaku (ma najdlhsiu trasu poctom stanic)
        /// </summary>
        /// <param name="allvariants">varianty vlaku</param>
        /// <returns>hlavna varianta vlaku</returns>
        public static void ReorderVariants(List<Train> allvariants)
        {
            switch (allvariants.Count)
            {
                case 0:
                    return;
                case 1:
                    allvariants[0].Variant = -1;
                    return;
            }

            //P1: reordering
            allvariants.Sort((t1, t2) =>
            {
                var stcount1 = t1.StaniceZoSmeru.Count + t1.StaniceDoSmeru.Count;
                var stcount2 = t2.StaniceZoSmeru.Count + t2.StaniceDoSmeru.Count;
                return stcount1.CompareTo(stcount2);
            });

            //P2: changing datelimits
            //rovnaky limit pre vsetky varianty
            var limit = new DateLimit(allvariants[0].ZaciatokPlatnosti, allvariants[0].KoniecPlatnosti, bInsertMarks: false);
            for (var i = 0; i < allvariants.Count; i++) //prechadzam kazdym variantom vlaku
            {
                allvariants[i].Variant = i + 1; // nastavenie vlastnosti Variant kazdemu vlaku (zacina 1, lebo variant 0 nie je)
                if (i + 1 != allvariants.Count) //ak nie je tento variant posledny, urobi sa op. XOR medzi tymto a nasledujucim variantom
                    allvariants[i].DateLimitText = limit.TextXor(allvariants[i].DateLimitText, allvariants[i + 1].DateLimitText);
            }
        }

        /// <summary>
        ///     Zisti, ci sa vlaky zhoduju v cisle, nazve a type
        /// </summary>
        /// <param name="train1"></param>
        /// <param name="train2"></param>
        /// <returns></returns>
        public static bool IsSameVariant(Train train1, Train train2)
        {
            return train1.Number == train2.Number && train1.Name == train2.Name && train1.Type == train2.Type;
        }

        private bool Equals(Train other)
        {
            return ID == other.ID &&
                   Number == other.Number &&
                   Equals(Type, other.Type) &&
                   Name == other.Name &&
                   Equals(StaniceZoSmeru, other.StaniceZoSmeru) &&
                   Equals(StaniceDoSmeru, other.StaniceDoSmeru) &&
                   Nullable.Equals(Arrival, other.Arrival) &&
                   Nullable.Equals(Departure, other.Departure) &&
                   Equals(Track, other.Track) &&
                   Equals(Operator, other.Operator) &&
                   DateLimitText == other.DateLimitText &&
                   Equals(Languages, other.Languages) &&
                   IsMedzistatny == other.IsMedzistatny &&
                   IsDialkovy == other.IsDialkovy &&
                   IsMimoriadny == other.IsMimoriadny &&
                   IsMiestenkovy == other.IsMiestenkovy &&
                   IsIbaLozkovy == other.IsIbaLozkovy &&
                   IsNizkopodlazny == other.IsNizkopodlazny &&
                   ZaciatokPlatnosti.Equals(other.ZaciatokPlatnosti) &&
                   KoniecPlatnosti.Equals(other.KoniecPlatnosti) &&
                   LineArrival == other.LineArrival &&
                   LineDeparture == other.LineDeparture &&
                   Equals(Doplnky, other.Doplnky) &&
                   Variant == other.Variant;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Train)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ID;
                hashCode = (hashCode * 397) ^ (Number != null ? Number.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StaniceZoSmeru != null ? StaniceZoSmeru.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StaniceDoSmeru != null ? StaniceDoSmeru.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Arrival.GetHashCode();
                hashCode = (hashCode * 397) ^ Departure.GetHashCode();
                hashCode = (hashCode * 397) ^ (Track != null ? Track.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operator != null ? Operator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DateLimitText != null ? DateLimitText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Languages != null ? Languages.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsMedzistatny.GetHashCode();
                hashCode = (hashCode * 397) ^ IsDialkovy.GetHashCode();
                hashCode = (hashCode * 397) ^ IsMimoriadny.GetHashCode();
                hashCode = (hashCode * 397) ^ IsMiestenkovy.GetHashCode();
                hashCode = (hashCode * 397) ^ IsIbaLozkovy.GetHashCode();
                hashCode = (hashCode * 397) ^ IsNizkopodlazny.GetHashCode();
                hashCode = (hashCode * 397) ^ ZaciatokPlatnosti.GetHashCode();
                hashCode = (hashCode * 397) ^ KoniecPlatnosti.GetHashCode();
                hashCode = (hashCode * 397) ^ (LineArrival != null ? LineArrival.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LineDeparture != null ? LineDeparture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Doplnky != null ? Doplnky.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Variant;
                return hashCode;
            }
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.Train" /> objects are
        ///     equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(Train left, Train right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.Train" /> objects have different
        ///     values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(Train left, Train right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Vráti všetky názvy vlakov zo zvukovej banky (priečinok V8)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTrainNames()
        {
            return GlobData.Sounds.Where(soundE => soundE.Dir.Name == "V8").Select(soundE => soundE.Name).ToList();
        }
    }
}
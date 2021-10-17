using System.Collections.Generic;
using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Typ stĺpca pre imporovanie dát.
    /// </summary>
    public sealed class ImportTrainColumnType : Enumeration<ImportTrainColumnType>
    {
        private ImportTrainColumnType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        ///     Vráti všetky požadované typy stĺpcov pre importovanie dát.
        /// </summary>
        /// <returns>všetky požadované typy stĺpcov</returns>
        public static List<ImportTrainColumnType> GetRequiredValues()
        {
            return new List<ImportTrainColumnType>
            {
                Number,
                Type,
                Variant,
                Prichod, Odchod,
                Track
            };
        }

        /// <summary>
        ///     Konvertuje názov stĺpca na objekt.
        /// </summary>
        /// <param name="name">názov stĺpca ako reťazec</param>
        /// <returns>objekt <see cref="ImportTrainColumnType" /> alebo <see cref="None" /> ak sa nenašla žiadna zhoda</returns>
        public static ImportTrainColumnType ParseColumnName(string name)
        {
            if (string.IsNullOrEmpty(name)) return None;
            return name.ToLower().Replace("-", "") switch
            {
                "číslo" => Number,
                "cislo" => Number,
                "number" => Number,
                "typ" => Type,
                "type" => Type,
                "variant" => Variant,
                "varianta" => Variant,
                "názov" => Nazov,
                "nazov" => Nazov,
                "název" => Nazov,
                "nazev" => Nazov,
                "názov vlaku" => Nazov,
                "nazov vlaku" => Nazov,
                "název vlaku" => Nazov,
                "nazev vlaku" => Nazov,
                "name" => Nazov,
                "príchod" => Prichod,
                "příjezd" => Prichod,
                "prichod" => Prichod,
                "arrival" => Prichod,
                "odchod" => Odchod,
                "odjezd" => Odchod,
                "departure" => Odchod,
                "daterem" => DateRemText,
                "datum. obm." => DateRemText,
                "dátum. obm." => DateRemText,
                "datum. om." => DateRemText,
                "dátumové obmedzenie" => DateRemText,
                "datumové omezení" => DateRemText,
                "platnosť od" => PlatnostOd,
                "platnost od" => PlatnostOd,
                "platnosť do" => PlatnostDo,
                "platnost do" => PlatnostDo,
                "dopravca id" => DopravcaId,
                "dopravce id" => DopravcaId,
                "id dopravca" => DopravcaId,
                "id dopravce" => DopravcaId,
                "dopravca" => DopravcaName,
                "dopravce" => DopravcaName,
                "název dopravce" => DopravcaName,
                "názov dopravcu" => DopravcaName,
                "kolaj" => Track,
                "kolej" => Track,
                "track" => Track,
                "jazyky" => Languages,
                "languages" => Languages,
                "jazyk" => Languages,
                "language" => Languages,
                "linka odchod" => LinkaOdchod,
                "linka odjezd" => LinkaOdchod,
                "linka príchod" => LinkaPrichod,
                "linka příjezd" => LinkaPrichod,
                "linka prijezd" => LinkaPrichod,
                "linka prichod" => LinkaPrichod,
                "routing" => Routing,
                "smerovanie" => Routing,
                "směrování" => Routing,
                "smerovani" => Routing,
                "trasy" => AllStationsID,
                "všetky stanice" => AllStationsID,
                "všechny stanice" => AllStationsID,
                "vsetky stanice" => AllStationsID,
                "vsechny stanice" => AllStationsID,
                "all stations" => AllStationsID,
                "stahlasb" => StationsShortID,
                "stanice kratke hlasenie" => StationsShortID,
                "stanice krátke hlásenie" => StationsShortID,
                "stanice kratke hlaseni" => StationsShortID,
                "stanice krátke hlášeni" => StationsShortID,
                "stanice kratke" => StationsShortID,
                "stanice krátke" => StationsShortID,
                "stahlasc" => StationsLongID,
                "stanice dlhe hlasenie" => StationsLongID,
                "stanice dlhé hlásenie" => StationsLongID,
                "stanice dlouhe hlaseni" => StationsLongID,
                "stanice dlhé hlášeni" => StationsLongID,
                "stanice dlhe" => StationsLongID,
                "stanice dlhé" => StationsLongID,
                "stanice dlouhe" => StationsLongID,
                "stanice dlouhé" => StationsLongID,
                "attributes" => Attributes,
                "atributy" => Attributes,
                "atribúty" => Attributes,
                _ => None
            };
        }

        #region VALUES

#pragma warning disable 1591
        public static readonly ImportTrainColumnType None = new(0, "-");
        public static readonly ImportTrainColumnType Number = new(1, "Číslo");
        public static readonly ImportTrainColumnType Type = new(2, "Typ");
        public static readonly ImportTrainColumnType Variant = new(3, "Varianta");
        public static readonly ImportTrainColumnType Nazov = new(4, "Názov");
        public static readonly ImportTrainColumnType Prichod = new(5, "Príchod");
        public static readonly ImportTrainColumnType Odchod = new(6, "Odchod");
        public static readonly ImportTrainColumnType DateRemText = new(7, "Dátum. obm. (text)");
        public static readonly ImportTrainColumnType DateRemBitArray = new(8, "Dátum. obm. (bitarray)");
        public static readonly ImportTrainColumnType PlatnostOd = new(9, "Platnosť od");
        public static readonly ImportTrainColumnType PlatnostDo = new(10, "Platnosť do");
        public static readonly ImportTrainColumnType DopravcaId = new(11, "Dopravca (ID)");
        public static readonly ImportTrainColumnType DopravcaName = new(12, "Dopravca (Názov)");
        public static readonly ImportTrainColumnType Track = new(13, "Koľaj");
        public static readonly ImportTrainColumnType Languages = new(14, "Jazyky");
        public static readonly ImportTrainColumnType LinkaOdchod = new(15, "Linka (odchod)");
        public static readonly ImportTrainColumnType LinkaPrichod = new(16, "Linka (príchod)");
        public static readonly ImportTrainColumnType Routing = new(17, "Smerovanie");
        public static readonly ImportTrainColumnType AllStationsID = new(18, "Všetky stanice (ID stanice)");
        public static readonly ImportTrainColumnType StationsShortID = new(19, "Stanice (krátke hlásenie) (ID stanice)");
        public static readonly ImportTrainColumnType StationsLongID = new(20, "Stanice (dlhé hlásenie) (ID stanice)");
        public static readonly ImportTrainColumnType AllStationsName = new(21, "Všetky stanice (stanice stanice)");
        public static readonly ImportTrainColumnType StationsShortName = new(22, "Stanice (krátke hlásenie) (názov stanice)");
        public static readonly ImportTrainColumnType StationsLongName = new(23, "Stanice (dlhé hlásenie) (názov stanice)");
        public static readonly ImportTrainColumnType Attributes = new(24, "Vlastnosti vlaku");
#pragma warning restore 1591

        #endregion
    }
}
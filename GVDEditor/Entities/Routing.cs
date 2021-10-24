using System.Drawing;
using GVDEditor.Properties;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda reprezentujúca smerovanie vlaku
    /// </summary>
    public sealed class Routing
    {
        private Routing(string name, string symbol, Bitmap bitmap, string charSymbol)
        {
            Name = name;
            Symbol = symbol;
            Image = bitmap;
            CharSymbol = charSymbol;
        }

        /// <summary>
        ///     Názov smerovania
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Symbol smerovania ako text
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        ///     Označenie smerovania ako znak
        /// </summary>
        public string CharSymbol { get; }

        /// <summary>
        ///     Symbol smerovania ako obrázok
        /// </summary>
        public Bitmap Image { get; }

        /// <summary>
        ///     Konvertuje označenie smerovania na objekt
        /// </summary>
        /// <param name="s">označenie smerovania</param>
        /// <returns><see cref="Routing" />, ak nenájde zhodu, vráti <see langword="null" /></returns>
        public static Routing Parse(string s)
        {
            return s switch
            {
                "V" => Vychadzajuci,
                "P" => Prechadzajuci,
                "K" => Konciaci,
                _ => null
            };
        }

        /// <summary>
        ///     Konvertuje označenie smerovania na objekt a vráti, či sa operácia podarila
        /// </summary>
        /// <param name="s">označenie smerovania</param>
        /// <param name="routing">výsledné smerovanie</param>
        /// <returns><see langword="true" /> ak sa operácia podarila, inak vráti <see langword="false" /></returns>
        public static bool TryParse(string s, out Routing routing)
        {
            switch (s)
            {
                case "V":
                    routing = Vychadzajuci;
                    return true;
                case "P":
                    routing = Prechadzajuci;
                    return true;
                case "K":
                    routing = Konciaci;
                    return true;
                default:
                    routing = null;
                    return false;
            }
        }

        /// <inheritdoc />
        public override string ToString() => Symbol;

        #region VALUES

        /// <summary>
        ///     Vlak končí v stanici
        /// </summary>
        public static readonly Routing Konciaci = new("Končiaci", "->|", Resources.konecna_st, "K");

        /// <summary>
        ///     Vlak prechádza stanicou
        /// </summary>
        public static readonly Routing Prechadzajuci = new("Prechádzajúci", "<->", Resources.prechadza_st, "P");

        /// <summary>
        ///     Vlak vychádza zo stanice
        /// </summary>
        public static readonly Routing Vychadzajuci = new("Vychadzajúci", "|->", Resources.vychodzia_st, "V");

        #endregion
    }
}
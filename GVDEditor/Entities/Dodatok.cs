using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Trieda obshaujuca informacie o dodatkovom hláseni.
    /// </summary>
    public sealed class Dodatok
    {
        /// <summary>
        ///     Vytvori novu instanciu triedy <see cref="Dodatok"/>.
        /// </summary>
        public Dodatok() => ChosenReports = new List<ChosenReportType>();

        /// <summary>
        ///     Vrati alebo nastavi fyzický zvuk dodatku.
        /// </summary>
        public FyzZvuk Sound { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi názov dodatkového hlásenia (zvyčajne Dxxxx, kde x - 0-9).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Vrati alebo nastavi v akých reportoch sa má dodatok hlásiť.
        /// </summary>
        public List<ChosenReportType> ChosenReports { get; set; }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <summary>
        ///     Konveruje binárne pole na dodatkové hlásenie zo všetkými dátami.
        /// </summary>
        /// <param name="sound">Fyzický zvuk.</param>
        /// <param name="nums">Binárne pole vo forme reťazca.</param>
        /// <param name="reportTypes">Dostupné typy reportov.</param>
        /// <param name="reportVariants">Dostupné varianty reportov.</param>
        /// <param name="smerovanie">Smerovanie vlaku.</param>
        /// <returns>dodatkové hlásenie.</returns>
        /// <exception cref="FormatException"></exception>
        public static Dodatok NumsToDodatok(FyzZvuk sound, string nums, List<ReportType> reportTypes, List<ReportVariant> reportVariants,
            Routing smerovanie)
        {
            var dodatok = new Dodatok { Sound = sound, Name = sound.Name.Replace("D", "") };

            if (!Utils.IsInt(nums)) 
                throw new FormatException("Pole dodatku neobsahuje iba čísla.");

            var numChars = nums.ToCharArray();

            foreach (var c in numChars)
                if (!c.Equals('0') && !c.Equals('1'))
                    throw new FormatException("Pole dodatku musí obsahovať iba číslice 0 a 1.");

            var countVariants = reportVariants.Count;
            List<ReportType> rightTypes;

            if (smerovanie == Routing.Vychadzajuci)
                rightTypes = reportTypes.Where(t => t.BaseTrain).ToList();
            else if (smerovanie == Routing.Prechadzajuci)
                rightTypes = reportTypes.Where(t => t.PassThrough).ToList();
            else
                rightTypes = reportTypes.Where(t => t.TerminateTrain).ToList();

            var rightMapLength = rightTypes.Count * countVariants;

            if (rightMapLength != numChars.Length)
                throw new FormatException($"Mapa dodatku má nesprávnu dĺžku ({numChars.Length}). Mala by mať dĺžku {rightMapLength}.");

            var ch = 0;
            for (var i = 0; i < rightTypes.Count; i++)
            {
                var vars = new List<ReportVariant>();
                for (var j = 0; j < countVariants; j++)
                {
                    if (numChars[ch].Equals('1')) vars.Add(reportVariants[j]);
                    ch++;
                }

                dodatok.ChosenReports.Add(new ChosenReportType { Type = reportTypes[i], Variants = vars });
            }

            return dodatok;
        }

        /// <summary>
        ///     Konveruje dodatkové hlásenie na binárne pole.
        /// </summary>
        /// <param name="dodatok">Dodatkové hlásenie.</param>
        /// <param name="reportTypes">Dostupné typy reportov.</param>
        /// <param name="reportVariants">Dostupné varianty reportov.</param>
        /// <returns>binárne pole vo forme reťazca</returns>
        public static string DodatokToNums(Dodatok dodatok, IList<ReportType> reportTypes, IList<ReportVariant> reportVariants)
        {
            var nums = new StringBuilder();

            for (var i = 0; i < reportTypes.Count; i++)
                if (i < dodatok.ChosenReports.Count && dodatok.ChosenReports[i].Type == reportTypes[i])
                    foreach (var variant in reportVariants)
                        nums.Append(dodatok.ChosenReports[i].Variants.Contains(variant) ? '1' : '0');
                else
                    for (var j = 0; j < reportVariants.Count; j++)
                        nums.Append('0');

            return nums.ToString();
        }
    }
}
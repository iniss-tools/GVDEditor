using System.Collections.Generic;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML
{
    /// <summary>
    ///     Obsahuje zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu.
    /// </summary>
    public class DesktopColumns
    {
        /// <summary>
        ///     Stĺpec Odchod.
        /// </summary>
        [XmlElement("Odchod")] 
        public DesktopColumn Odchod = new() { Order = 7, Visible = true, MinWidth = 60 };

        /// <summary>
        ///     Stĺpec Východzia stanica.
        /// </summary>
        [XmlElement("StartStation")] 
        public DesktopColumn VychodziaStanica = new() { Order = 8, Visible = true, MinWidth = 120 };

        /// <summary>
        ///     Stĺpec Konečná stanica.
        /// </summary>
        [XmlElement("EndStation")] 
        public DesktopColumn KonecnaStanica = new() { Order = 9, Visible = true, MinWidth = 120 };

        /// <summary>
        ///     Stĺpec Dátumové obmedzenie.
        /// </summary>
        [XmlElement("DateLimit")] 
        public DesktopColumn DateLimit = new() { Order = 10, Visible = true, MinWidth = 300 };

        /// <summary>
        ///     Stĺpec Číslo.
        /// </summary>
        [XmlElement("Number")] 
        public DesktopColumn Number = new() { Order = 0, Visible = true, MinWidth = 60 };

        /// <summary>
        ///     Stĺpec Typ.
        /// </summary>
        [XmlElement("Type")] 
        public DesktopColumn Type = new() { Order = 1, Visible = true, MinWidth = 40 };

        /// <summary>
        ///     Stĺpec Názov.
        /// </summary>
        [XmlElement("Name")] 
        public DesktopColumn Name = new() { Order = 2, Visible = true, MinWidth = 100 };

        /// <summary>
        ///     Stĺpec Linka-Príchod.
        /// </summary>
        [XmlElement("LinkaPrichod")] 
        public DesktopColumn LinkaPrichod = new() { Order = 3, Visible = false, MinWidth = 50 };

        /// <summary>
        ///     Stĺpec Linka-Príchod.
        /// </summary>
        [XmlElement("LinkaOdchod")] 
        public DesktopColumn LinkaOdchod = new() { Order = 4, Visible = false, MinWidth = 50 };

        /// <summary>
        ///     Stĺpec Smerovanie.
        /// </summary>
        [XmlElement("Routing")] 
        public DesktopColumn Routing = new() { Order = 5, Visible = true, MinWidth = 100 };

        /// <summary>
        ///     Stĺpec Príchod.
        /// </summary>
        [XmlElement("Prichod")] 
        public DesktopColumn Prichod = new() { Order = 6, Visible = true, MinWidth = 60 };

        /// <summary>
        ///     Stĺpec Koľaj.
        /// </summary>
        [XmlElement("Track")] 
        public DesktopColumn Track = new() { Order = 11, Visible = true, MinWidth = 100 };

        /// <summary>
        ///     Stĺpec Linka-Príchod.
        /// </summary>
        [XmlElement("Operator")] 
        public DesktopColumn Operator = new() { Order = 12, Visible = true, MinWidth = 50 };

        /// <summary>
        ///     Stĺpec Ostatné.
        /// </summary>
        [XmlElement("OtherBtn")] 
        public DesktopColumn OtherBtn = new() { Order = 13, Visible = true, MinWidth = 50 };

        /// <summary>
        ///     Vráti zoznam všetkých možnźch stĺpcov pre tabuľku na pracovnej ploche programu.
        /// </summary>
        /// <returns></returns>
        public List<DesktopColumn> GetValues()
        {
            return new List<DesktopColumn>
            {
                Number, Type, Name, LinkaPrichod, LinkaOdchod, Routing, Prichod, Odchod, VychodziaStanica,
                KonecnaStanica, DateLimit, Track, Operator, OtherBtn
            };
        }

        /// <summary>
        ///     Vráti zoradený zoznam všetkých možných stĺpcov pre tabuľku na pracovnej ploche programu.
        /// </summary>
        /// <returns></returns>
        public List<DesktopColumn> GetOrderedValues()
        {
            var order = GetValues();
            var count = order.Count;

            for (var i = 0; i < count - 1; i++)
            {
                for (var j = 0; j < count - i - 1; j++)
                {
                    if (order[j].Order > order[j + 1].Order)
                    {
                        (order[j], order[j + 1]) = (order[j + 1], order[j]);
                    }
                }
            }

            return order;
        }
    }
}
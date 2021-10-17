using System.Drawing;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML
{
    /// <summary>
    ///     Obsahuje zoznam všetkých štýlov pre text v editore TabTab.
    /// </summary>
    public class TabTabEditorScheme
    {
        /// <summary>
        ///     Textový editor TabTab - Číslo.
        /// </summary>
        [XmlElement("Number")] 
        public ColorSetting Number = new() { ForeColor = Color.Purple, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Reťazec.
        /// </summary>
        [XmlElement("String")] 
        public ColorSetting String = new() { ForeColor = Color.Red, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Komentár.
        /// </summary>
        [XmlElement("Comment")] 
        public ColorSetting Comment = new() { ForeColor = Color.Green, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Znak konca riadku a prechod do ďalšieho.
        /// </summary>
        [XmlElement("OnNewLine")] 
        public ColorSetting OnNewLine = new() { ForeColor = Color.OrangeRed, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Operátor.
        /// </summary>
        [XmlElement("Operator")] 
        public ColorSetting Operator = new() { ForeColor = Color.Black, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Konštanta.
        /// </summary>
        [XmlElement("Constant")] 
        public ColorSetting Constant = new() { ForeColor = Color.DimGray, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Normálny text.
        /// </summary>
        [XmlElement("Default")] 
        public ColorSetting Default = new() { ForeColor = Color.Black, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Označenie premennej.
        /// </summary>
        [XmlElement("Var")] 
        public ColorSetting Var = new() { ForeColor = Color.DarkSlateGray, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Udalosť.
        /// </summary>
        [XmlElement("Event")]
        public ColorSetting Event = new() { ForeColor = Color.SaddleBrown, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Font v editore TabTab.
        /// </summary>
        [XmlIgnore] 
        public Font Font = new("Consolas", 10);

        /// <summary>
        ///     Textový editor TabTab - Funkcia.
        /// </summary>
        [XmlElement("Function")] 
        public ColorSetting Function = new() { ForeColor = Color.Blue, Bold = false, DisableBackColorEdit = true };

        /// <summary>
        ///     Textový editor TabTab - Identifikátor.
        /// </summary>
        [XmlElement("Identifier")] 
        public ColorSetting Identifier = new() { ForeColor = Color.Teal, Bold = false, DisableBackColorEdit = true };


        /// <summary>
        ///     Textový editor TabTab - Označenenie aktívnych zátvoriek.
        /// </summary>
        [XmlElement("SelBraces")] 
        public ColorSetting SelBraces = new() { ForeColor = Color.LightGray, BackColor = Color.BlueViolet, Bold = false };

        /// <summary>
        ///     Textový editor TabTab - Označenenie aktívnej zátvorky, ktorá nemá páru.
        /// </summary>
        [XmlElement("SelBraceBad")] 
        public ColorSetting SelBraceBad = new() { ForeColor = Color.Red, BackColor = Color.LightGray, Bold = false };


        /// <summary>
        ///     Font v editore TabTab vo formate XML.
        /// </summary>
        [XmlElement(Type = typeof(XMLFont), ElementName = "Font")]
        public XMLFont FontXML
        {
            get => XMLFont.FromFont(Font);
            set => Font = XMLFont.ToFont(value);
        }
    }
}
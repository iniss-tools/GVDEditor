using System.Drawing;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML
{
    /// <inheritdoc />
    public class GVDEditorStyle : Style
    {
		/// <summary>
        ///     Farebna schema pre druh vlaku zobrazujuceho sa v tabulke na pracovnej ploche programu
        /// </summary>
        [XmlElement("TrainType")]
        public TrainTypeColumnScheme TrainTypeColumnScheme = new();

        /// <summary>
        ///     Farebna schéma pre textový editor TabTab
        /// </summary>
        [XmlElement("TabTabEditor")]
        public TabTabEditorScheme TabTabEditorScheme = new();

        /// <inheritdoc />
        public GVDEditorStyle()
        {
        }

        /// <inheritdoc />
        public GVDEditorStyle(Style original) : base(original)
        {
        }

		/// <summary>
		///     Nastavi nastavenia farieb pre editor TabTab na predvolene hodnoty
		/// </summary>
		public static TabTabEditorScheme SetTabTabEditorSchemeDefault()
		{
			return new TabTabEditorScheme
			{
				Default = new ColorSetting
				{
					ForeColor = Color.Black,
					Bold = false,
					DisableBackColorEdit = true
				},
				Function = new ColorSetting
				{
					ForeColor = Color.Blue,
					Bold = false,
					DisableBackColorEdit = true
				},
				Identifier = new ColorSetting
				{
					ForeColor = Color.Teal,
					Bold = false,
					DisableBackColorEdit = true
				},
				Number = new ColorSetting
				{
					ForeColor = Color.Purple,
					Bold = false,
					DisableBackColorEdit = true
				},
				String = new ColorSetting
				{
					ForeColor = Color.Red,
					Bold = false,
					DisableBackColorEdit = true
				},
				Comment = new ColorSetting
				{
					ForeColor = Color.Green,
					Bold = false,
					DisableBackColorEdit = true
				},
				Var = new ColorSetting
				{
					ForeColor = Color.DarkSlateGray,
					Bold = false,
					DisableBackColorEdit = true
				},
				Event = new ColorSetting
				{
					ForeColor = Color.SaddleBrown,
					Bold = false,
					DisableBackColorEdit = true
				},
				OnNewLine = new ColorSetting
				{
					ForeColor = Color.OrangeRed,
					Bold = false,
					DisableBackColorEdit = true
				},
				Operator = new ColorSetting
				{
					ForeColor = Color.Black,
					Bold = false,
					DisableBackColorEdit = true
				},
				Constant = new ColorSetting
				{
					ForeColor = Color.DimGray,
					Bold = false,
					DisableBackColorEdit = true
				},
				SelBraces = new ColorSetting
				{
					ForeColor = Color.LightGray,
					BackColor = Color.BlueViolet,
					Bold = false
				},
				SelBraceBad = new ColorSetting
				{
					ForeColor = Color.Red,
					BackColor = Color.LightGray,
					Bold = false
				},
				Font = new Font("Consolas", 10f)
			};
		}

        /// <summary>
        ///     Nastavi nastavenia farieb pre editor TabTab na predvolene hodnoty (Dark mode)
        /// </summary>
		public static TabTabEditorScheme SetTabTabEditorSchemeDarkDefault()
        {
            return new TabTabEditorScheme
            {
                Default = new ColorSetting
                {
                    ForeColor = Color.White,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Function = new ColorSetting
                {
                    ForeColor = Color.DodgerBlue,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Identifier = new ColorSetting
                {
                    ForeColor = Color.LightGreen,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Number = new ColorSetting
                {
                    ForeColor = Color.PaleGoldenrod,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                String = new ColorSetting
                {
                    ForeColor = Color.Coral,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Comment = new ColorSetting
                {
                    ForeColor = Color.Green,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Var = new ColorSetting
                {
                    ForeColor = Color.Aquamarine,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Event = new ColorSetting
                {
                    ForeColor = Color.Goldenrod,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                OnNewLine = new ColorSetting
                {
                    ForeColor = Color.OrangeRed,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Operator = new ColorSetting
                {
                    ForeColor = Color.White,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                Constant = new ColorSetting
                {
                    ForeColor = Color.LightSlateGray,
                    Bold = false,
                    DisableBackColorEdit = true
                },
                SelBraces = new ColorSetting
                {
                    ForeColor = Color.LightGray,
                    BackColor = Color.BlueViolet,
                    Bold = false
                },
                SelBraceBad = new ColorSetting
                {
                    ForeColor = Color.Red,
                    BackColor = Color.LightGray,
                    Bold = false
                },
                Font = new Font("Consolas", 10f)
            };
        }

		/// <summary>
		///     Nastavi nastavenia farieb pre stlpec Typ vlaku na predvolene hodnoty
		/// </summary>
		public static TrainTypeColumnScheme SetTrainTypeColumnSchemeDarkDefault()
		{
			return new TrainTypeColumnScheme
			{
				Os = new ColorSetting
				{
					ForeColor = Color.White,
					BackColor = Color.Transparent,
					Bold = false
				},
				R = new ColorSetting
				{
					ForeColor = Color.Red,
					BackColor = Color.Transparent,
					Bold = true
				},
				X = new ColorSetting
				{
					ForeColor = Color.Green,
					BackColor = Color.Transparent,
					Bold = true
				},
				Sl = new ColorSetting
				{
					ForeColor = Color.OrangeRed,
					BackColor = Color.Transparent,
					Bold = false
				},
				Font = new Font("Segoe UI", 9f)
			};
		}
		
        public new static GVDEditorStyle DefaultLightStyle => new(Style.DefaultLightStyle);

        public new static GVDEditorStyle DefaultDarkStyle
        {
            get
            {
                var st = new GVDEditorStyle(Style.DefaultDarkStyle)
                {
                    TabTabEditorScheme = SetTabTabEditorSchemeDarkDefault(),
					TrainTypeColumnScheme = SetTrainTypeColumnSchemeDarkDefault()
                };
                return st;
            }
        }
	}
}
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;
using GVDEditor.XML;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor
{
    internal static class GlobData
    {
        public static string INISSDir { get; set; }
        public static string DATADir { get; set; }
        public static string RAWBANKDir { get; set; }

        public static List<string> INISSExeFiles { get; set; }

        public static List<DirList> GVDDirs { get; set; }
        public static List<Audio> Audios { get; set; }

        public static List<FyzZvuk> Sounds { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<Language> LocalLanguages { get; set; }
        public static List<Station> Stations { get; set; }
        public static List<Station> CustomStations { get; set; }

        public static List<Operator> Operators { get; set; }

        public static ExBindingList<Train> Trains { get; set; } = new();
        public static List<Track> Tracks { get; set; }
        public static List<Platform> Platforms { get; set; }

        public static List<int> Delays { get; set; }

        public static List<TrainType> TrainsTypes { get; set; }

        public static List<string> TrainNames { get; set; }

        public static List<TableTabTab> TabTabs { get; set; }
        public static List<TableCatalog> TableCatalogs { get; set; }
        public static List<TablePhysical> TablePhysicals { get; set; }
        public static List<TableLogical> TableLogicals { get; set; }
        public static List<TableText> TableTexts { get; set; }
        public static List<TableFont> TableFonts { get; set; }
        public static string TableFontDir { get; set; }

        public static List<ReportVariant> ReportVariants { get; set; }
        public static List<ReportType> ReportTypes { get; set; }

        public static List<ReportType> ReportTypesV { get; set; }
        public static List<ReportType> ReportTypesP { get; set; }
        public static List<ReportType> ReportTypesK { get; set; }

        public static List<Radenie> Radenia { get; set; }

        public static GVDEditorConfig Config { get; set; }
        public static Styles<GVDEditorStyle> Styles { get; set; }
        public static GVDEditorStyle UsingStyle { get; set; }
        public static bool PrivateFeatures { get; set; }

        public static void PrepareGlobalData(string pathtoiniss)
        {
            Audios = new List<Audio>();

            Tracks = new List<Track>();
            Platforms = new List<Platform>();

            TrainsTypes = new List<TrainType>();

            Operators = new List<Operator>();

            TabTabs = new List<TableTabTab>();
            TableCatalogs = new List<TableCatalog>();
            TablePhysicals = new List<TablePhysical>();
            TableLogicals = new List<TableLogical>();
            TableTexts = new List<TableText>();
            TableFonts = new List<TableFont>();

            ReportVariants = new List<ReportVariant>();
            ReportTypes = new List<ReportType>();

            ReportTypesV = new List<ReportType>();
            ReportTypesP = new List<ReportType>();
            ReportTypesK = new List<ReportType>();

            CustomStations = new List<Station>();

            Radenia = new List<Radenie>();

            INISSDir = pathtoiniss;
            DATADir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_DATA);
            RAWBANKDir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_RAWBANK);
            GVDDirs = TXTParser.ReadDirList();

            INISSExeFiles = new List<string>();
            var di = new DirectoryInfo(INISSDir);
            var subFiles = di.GetFiles("*.exe");
            foreach (var file in subFiles) INISSExeFiles.Add(file.Name);

            var langs = RawBankReader.ReadFyzBankFile(RAWBANKDir, out var maxLangs);
            Languages = TXTParser.ReadGlobalCategori(DATADir, langs, maxLangs);

            LocalLanguages = new List<Language>();
            Sounds = RawBankReader.ReadFyzZvukFile(RAWBANKDir, Language.GetBasicLanguage(Languages));
            try
            {
                TrainsTypes = TXTParser.ReadTrainTypes();
            }
            catch (FileNotFoundException)
            {
            }

            TrainNames = Train.GetTrainNames();
            Stations = Station.GetStations();
            Delays = TXTParser.ReadZpozdeni();

            try
            {
                Audios = TXTParser.ReadAudio();
            }
            catch (FileNotFoundException)
            {
            }
        }

        
    }
}
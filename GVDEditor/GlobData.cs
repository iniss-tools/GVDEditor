using System.Collections.Generic;
using System.IO;
using System.Text;
using GVDEditor.Entities;
using GVDEditor.Tools;
using GVDEditor.XML;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor
{
    internal static class GlobData
    {
        public static readonly Encoding ANSIEncoding = Encoding.GetEncoding(1250);

        public static string INISSDir;
        public static string DATADir;
        public static string RAWBANKDir;

        public static List<string> INISSExeFiles;

        public static List<DirList> GVDDirs;
        public static List<Audio> Audios;

        public static List<FyzZvuk> Sounds;
        public static List<Language> Languages;
        public static List<Language> LocalLanguages;
        public static List<Station> Stations;
        public static List<Station> CustomStations;

        public static List<Operator> Operators;

        public static List<Track> Tracks;
        public static List<Platform> Platforms;

        public static List<int> Delays;

        public static List<TrainType> TrainsTypes;

        public static List<string> TrainNames;

        public static List<TableTabTab> TabTabs;
        public static List<TableCatalog> TableCatalogs;
        public static List<TablePhysical> TablePhysicals;
        public static List<TableLogical> TableLogicals;
        public static List<TableText> TableTexts;
        public static List<TableFont> TableFonts;
        public static string TableFontDir;

        public static List<ReportVariant> ReportVariants;
        public static List<ReportType> ReportTypes;

        public static List<ReportType> ReportTypesV;
        public static List<ReportType> ReportTypesP;
        public static List<ReportType> ReportTypesK;

        public static List<Radenie> Radenia;

        public static GVDEditorConfig Config;
        public static Styles<GVDEditorStyle> Styles;
        public static GVDEditorStyle UsingStyle;
        public static bool PrivateFeatures;

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
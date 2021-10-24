using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ToolsCore;
using ToolsCore.Tools;
using ToolsCore.XML;
using static GVDEditor.FileConsts;
using static ToolsCore.Tools.Utils;


namespace GVDEditor.Tools
{
    /// <summary>
    ///     Trieda obsahujuca funkcie pre citanie a zapisovanie dat do .TXT suborov.
    /// </summary>
    internal static class TXTParser
    {
        private const string FORMAT_EX = "Chyba v súbore {0} na riadku {1}. ";
        private const string FORMAT_EX_AREA = "{0}: Nezadefinované pole {1}.";

        private static string CheckKey<T>(string key, string name, IEnumerable<T> items) where T : ITable
        {
            string tabtype = typeof(T) switch
            {
                { } catalog when catalog == typeof(TableCatalog) => "Katalógová tabuľa",
                { } log when log == typeof(TableLogical) => "Logická tabuľa",
                { } ph when ph == typeof(TablePhysical) => "Fyzická tabuľa",
                _ => "Tabuľa"
            };

            if (string.IsNullOrEmpty(key))
            {
                throw new FormatException($"{tabtype} s názvom {name} nemá zadaný kľúč.");
            }

            foreach (var item in items)
            {
                if (item.Key == key)
                {
                    throw new FormatException($"{tabtype} s názvom {name} má kľúč \"{key}\" zhodný s inou tabuľov.");
                }
            }

            return key;
        }

        #region COMMENTS

        /// <summary>
        ///     Vygeneruje komentar ako hlavicku k niektorym suborom
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="filename">nazov suboru</param>
        /// <param name="gvd">informacie o grafikone</param>
        /// <param name="lang">jazyk generovania komentara</param>
        /// <param name="toAnsi">true (default) ak ma pouyit ANSI kodovanie, false pre pouzitie UTF-8</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateComment(string path, string filename, GVDInfo gvd, Config.AppLanguage lang, bool toAnsi = true)
        {
            var rows = new List<string>();

            switch (lang)
            {
                case Config.AppLanguage.Slovak:
                    rows.Add(CombinePath(path, filename));
                    rows.Add(
                        $"Vygenerované programom {Application.ProductName}, verzia {Application.ProductVersion} dňa {DateTime.Today:dd.MM.yyyy} v {DateTime.Now:HH:mm}");
                    rows.Add($"Názov stanice: {gvd.ThisStation.Name} ({gvd.ThisStation.ID})");
                    rows.Add($"Platnosť grafikonu: {gvd.StartValidTimeTable:dd.MM.yyyy} - {gvd.EndValidTimeTable:dd.MM.yyyy}");
                    rows.Add($"Platnosť dát: {gvd.StartValidData:dd.MM.yyyy} - {gvd.EndValidData:dd.MM.yyyy}");
                    rows.Add($"Dáta vytvorené: {gvd.CreateData:dd.MM.yyyy}");
                    rows.Add("=============================================================================================");
                    break;
                case Config.AppLanguage.Czech:
                    rows.Add(CombinePath(path, filename));
                    rows.Add(
                        $"Vygenerované programem {Application.ProductName}, verze {Application.ProductVersion} dne {DateTime.Today:dd.MM.yyyy} v {DateTime.Now:HH:mm}");
                    rows.Add($"Název stanice: {gvd.ThisStation.Name} ({gvd.ThisStation.ID})");
                    rows.Add($"Platnost grafikonu: {gvd.StartValidTimeTable:dd.MM.yyyy} - {gvd.EndValidTimeTable:dd.MM.yyyy}");
                    rows.Add($"Platnost dat: {gvd.StartValidData:dd.MM.yyyy} - {gvd.EndValidData:dd.MM.yyyy}");
                    rows.Add($"Data vytvořené: {gvd.CreateData:dd.MM.yyyy}");
                    rows.Add("=============================================================================================");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lang), lang, null);
            }


            if (toAnsi)
                for (var i = 0; i < rows.Count; i++)
                    rows[i] = rows[i].UTFtoANSI();

            return rows;
        }

        #endregion

        #region STATEDGM

        /// <summary>
        ///     Zapise subor StateGVD do konkretneho priecinka
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="skversion">true ak ma zapisat slovenku verziu, false pre cesku verziu (zatial je funkcna iba SK verzia)</param>
        public static void WriteStateDgm(string path, bool skversion = true)
        {
            if (skversion)
                File.WriteAllText(CombinePath(path, FILE_STATEDGM), Resources.statedgmSK, Encodings.Win1250);
            else
                File.WriteAllText(CombinePath(path, FILE_STATEDGM), Resources.statedgmSK, Encodings.Win1250); //TODO cz version
        }

        #endregion

        #region DIRLIST

        /// <summary>
        ///     Vrati zoznam pouzivanych priecinkov s GVD
        /// </summary>
        /// <returns>priecinky s GVD</returns>
        public static List<DirList> ReadDirList()
        {
            var fileDirList = CombinePath(GlobData.DATADir, FILE_DIRLIST);

            var dirs = new List<DirList>();

            using var dirlistF = new CSVFileReader(fileDirList);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = dirlistF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var dirList = new DirList();
                    var dirName = row[0];
                    dirList.DirName = dirName;
                    dirList.FullPath = GlobData.DATADir + Path.DirectorySeparatorChar + dirName;

                    dirList.TablePort = ParseIntOrNull(row.ElementAtOrDefault(1));
                    dirList.ReportPort = ParseIntOrNull(row.ElementAtOrDefault(2));
                    dirList.BackColor = TryParseHEX(row.ElementAtOrDefault(4));

                    dirs.Add(dirList);
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_DIRLIST, riadok) + e.Message, e);
                }

                riadok++;
            }

            return dirs;
        }

        /// <summary>
        ///     Zapise zoznam pouzivanych priecinkoch s GVD
        /// </summary>
        /// <param name="dirs">priecinky s GVD</param>
        public static void WriteDirList(IEnumerable<DirList> dirs)
        {
            var fileDirList = CombinePath(GlobData.DATADir, FILE_DIRLIST);

            using var dirlistF = new CSVFileWriter(fileDirList);
            foreach (var dir in dirs)
            {
                var row = new CSVRow();
                row.Insert(0, dir.DirName);
                if (dir.TablePort.HasValue && dir.TablePort != 0)
                    row.Insert(1, dir.TablePort.ToString());
                else
                    row.Insert(1, "");

                if (dir.ReportPort.HasValue && dir.ReportPort != 0)
                    row.Insert(2, dir.TablePort.ToString());
                else
                    row.Insert(2, "");

                row.Insert(3, "");
                row.Insert(4, dir.BackColor.HasValue ? dir.BackColor.Value.ToHEX() : "");

                dirlistF.WriteRow(row);
            }
        }

        #endregion

        #region GVD

        /// <summary>
        ///     Vrati informacie o grafikone
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <returns>informacie o grafikone</returns>
        public static GVDInfo ReadInfoGVD(string path)
        {
            var fileGrafikon = CombinePath(path, FILE_GRAFIKON);

            var gvd = new GVDInfo();

            var config = new TXTProps(fileGrafikon);

            gvd.Category = int.Parse(config.Get("CATEGORY", "1"));
            gvd.Subcat = int.Parse(config.Get("SUBCAT", "0"));
            var id = int.Parse(config.Get("IDSTATION"));
            var name = config.Get("NAMESTATION");
            gvd.ThisStation = new Station(id.ToString(), name);
            gvd.TrainCount = int.Parse(config.Get("TRAINCOUNT", "0"));
            gvd.StartValidTimeTable = ParseDateAlts(config.Get("START_VALID_TIMETABLE"));
            gvd.EndValidTimeTable = ParseDateAlts(config.Get("END_VALID_TIMETABLE"));
            gvd.StartValidData = ParseDateAlts(config.Get("START_VALID_DATA"));
            gvd.EndValidData = ParseDateAlts(config.Get("END_VALID_DATA"));
            gvd.CreateData = ParseDateAlts(config.Get("CREATE_DATA"));
            gvd.TTIndex = int.Parse(config.Get("TT_INDEX", "0"));
            gvd.VLIndex = int.Parse(config.Get("VL_INDEX", "-1"));
            gvd.STIndex = int.Parse(config.Get("ST_INDEX", "0"));
            var isRegionText = config.Get("IS_REGION_TEXT", "1");
            gvd.IsRegionText = isRegionText == "1";
            gvd.OnlyCityVLIndex = int.Parse(config.Get("ONLY_CITY_VL_INDEX", "-999"));

            return gvd;
        }

        /// <summary>
        ///     Zapise informacie o grafikone do suboru
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="gvd">informacie o grafikone</param>
        public static void WriteInfoGVD(string path, GVDInfo gvd)
        {
            var fileGrafikon = CombinePath(path, FILE_GRAFIKON);

            var config = new TXTProps(fileGrafikon, true);

            config.Set("CATEGORY", gvd.Category);
            config.Set("SUBCAT", gvd.Subcat);
            config.Set("IDSTATION", gvd.ThisStation.ID);
            config.Set("NAMESTATION", gvd.ThisStation.Name);
            config.Set("TRAINCOUNT", gvd.TrainCount);
            config.Set("START_VALID_TIMETABLE", gvd.StartValidTimeTable.ToString("dd.MM.yyyy"));
            config.Set("END_VALID_TIMETABLE", gvd.EndValidTimeTable.ToString("dd.MM.yyyy"));
            config.Set("START_VALID_DATA", gvd.StartValidData.ToString("dd.MM.yyyy"));
            config.Set("END_VALID_DATA", gvd.EndValidData.ToString("dd.MM.yyyy"));
            config.Set("CREATE_DATA", gvd.CreateData.ToString("dd.MM.yyyy"));
            config.Set("TT_INDEX", gvd.TTIndex);
            config.Set("VL_INDEX", gvd.VLIndex);
            config.Set("ST_INDEX", gvd.STIndex);
            var isRegionText = gvd.IsRegionText ? 1 : 0;
            config.Set("IS_REGION_TEXT", isRegionText);
            config.Set("ONLY_CITY_VL_INDEX", gvd.OnlyCityVLIndex);
            config.Save();
        }

        #endregion

        #region AUDIO

        /// <summary>
        ///     Vrati informacie o audio linkach zo suboru
        /// </summary>
        /// <returns>audio linky</returns>
        public static List<Audio> ReadAudio()
        {
            var fileAudio = CombinePath(GlobData.DATADir, FILE_AUDIO);

            var audios = new List<Audio>();

            using var audioF = new CSVFileReader(fileAudio);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = audioF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var audio = new Audio
                    {
                        Station = Station.GetFromID(row[0]),
                        Name = row[1],
                        ShortName = row[2],
                        QueueName = row[3],
                        Mixer = row.ElementAtOrDefaultStr(4),
                        SoundCard = row.ElementAtOrDefaultStr(5)
                    };

                    audios.Add(audio);
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_AUDIO, riadok) + e.Message, e);
                }

                riadok++;
            }

            return audios;
        }

        /// <summary>
        ///     Zapise informacie o audio linkach do suboru
        /// </summary>
        /// <param name="audios">audio linky</param>
        public static void WriteAudio(IEnumerable<Audio> audios)
        {
            var fileAudio = CombinePath(GlobData.DATADir, FILE_AUDIO);

            using var audioF = new CSVFileWriter(fileAudio);
            foreach (var a in audios)
            {
                var row = new CSVRow();

                row.Insert(0, a.Station.ID);
                row.Insert(1, a.Name);
                row.Insert(2, a.ShortName);
                row.Insert(3, a.QueueName);
                row.Insert(4, ParseStringOrDefault(a.Mixer));
                row.Insert(5, ParseStringOrDefault(a.SoundCard));

                audioF.WriteRow(row);
            }
        }

        #endregion

        #region ZPOZDENI

        /// <summary>
        ///     Vrati mozne casy meskani
        /// </summary>
        /// <returns>meskania</returns>
        public static List<int> ReadZpozdeni()
        {
            var file = CombinePath(GlobData.DATADir, FILE_ZPOZDENI);

            var meskania = new List<int>();

            using var zpozdeniF = new CSVFileReader(file);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = zpozdeniF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var num = int.Parse(row[0]);
                    if (num is < 5 or > 480 || num % 5 != 0)
                        throw new FormatException($"Číslo {num} nie je v intervale X >= 5 a zároveň X <= 480 alebo nie je delitelné 5.");
                    meskania.Add(num);
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_ZPOZDENI, riadok) + e.Message, e);
                }

                riadok++;
            }

            return meskania;
        }

        /// <summary>
        ///     Zapise predvolene casy meskani
        /// </summary>
        public static void WriteZpozdeniDefault()
        {
            var file = CombinePath(GlobData.DATADir, FILE_ZPOZDENI);

            using var zpozdeniF = new CSVFileWriter(file);
            for (var i = 5; i <= 480; i += 5)
            {
                var row = new CSVRow();
                row.Insert(0, i.ToString());

                zpozdeniF.WriteRow(row);
            }
        }

        /// <summary>
        ///     Zapise mozne casy meskani do suboru
        /// </summary>
        /// <param name="meskania">meskania</param>
        public static void WriteZpozdeni(IEnumerable<int> meskania)
        {
            var file = CombinePath(GlobData.DATADir, FILE_ZPOZDENI);

            using var zpozdeniF = new CSVFileWriter(file);
            foreach (var meskanie in meskania)
            {
                var row = new CSVRow();
                row.Insert(0, meskanie.ToString());

                zpozdeniF.WriteRow(row);
            }
        }

        #endregion

        #region TRAIN_TYPES

        /// <summary>
        ///     Vrati kategorie vlakov
        /// </summary>
        /// <returns>kategorie vlakov</returns>
        public static List<TrainType> ReadTrainTypes()
        {
            var fileTrTypes = CombinePath(GlobData.DATADir, FILE_TRTYPES);

            var typy = new List<TrainType>();

            using var trtypesF = new CSVFileReader(fileTrTypes);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = trtypesF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    TrainType typ;

                    var s = row[0];

                    if (Regex.IsMatch(s, "^X[1-9]$") || Regex.IsMatch(s, "^R[1-9]$") || Regex.IsMatch(s, "^Os[1-9]$") ||
                        Regex.IsMatch(s, "^Sl[1-9]$"))
                    {
                        typ = new TrainType(s, row[1], row[2]);
                    }
                    else
                    {
                        if (TrainType.Validate(s))
                            typ = new TrainType(s);
                        else
                            throw new ArgumentException($"Neznámy typ vlaku {s}");
                    }

                    typy.Add(typ);
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_TRTYPES, riadok) + e.Message, e);
                }

                riadok++;
            }

            return typy;
        }

        /// <summary>
        ///     Zapise do suboru kategorie vlakov
        /// </summary>
        /// <param name="typy">kategorie vlakov</param>
        public static void WriteTrainTypes(IEnumerable<TrainType> typy)
        {
            var fileTrTypes = CombinePath(GlobData.DATADir, FILE_TRTYPES);

            using var trtypesF = new CSVFileWriter(fileTrTypes);
            foreach (var typ in typy)
            {
                var row = new CSVRow();
                if (typ.IsCustom)
                {
                    row.Add(typ.CategoryTrain);
                    row.Add(typ.Key);
                    row.Add(typ.TextInTable);
                }
                else
                {
                    row.Add(typ.Key);
                }

                trtypesF.WriteRow(row);
            }
        }

        /// <summary>
        ///     Zapise do suboru predvolene kategorie vlakov
        /// </summary>
        public static void WriteTrainTypesDefaults()
        {
            var file = CombinePath(GlobData.DATADir, FILE_TRTYPES);

            string[] types = { "Os", "Zr", "R", "Ex", "EC", "IC", "EN", "ER", "REX", "Bus", "SC" };

            using var trtypes = new CSVFileWriter(file);
            foreach (var typ in types)
            {
                var row = new CSVRow { typ };
                trtypes.WriteRow(row);
            }
        }

        #endregion

        #region GLOBAL_CATEGORI

        /// <summary>
        ///     Vrati informacie o jazykovych mutaciach hlaseni (pre vsetky GVD)
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="jazykyFromBank">jazykove mutacie z banky zvukov</param>
        /// <param name="maxLangs">maximalny pocet jazykovych mutacii (podla zvukovej banky)</param>
        /// <returns>jazyky</returns>
        public static List<Language> ReadGlobalCategori(string path, IList<Language> jazykyFromBank, int maxLangs)
        {
            var file = CombinePath(path, FILE_CATEGORI);

            var jazyky = new List<Language>();

            var categoriF = new TXTPropsAreasFields(file);

            var count = int.Parse(categoriF.Get("MAIN", "COUNT_LANGUAGES"));

            if (count > maxLangs)
                throw new ArgumentException(
                    $"V globálnom súbore CATEGORI.TXT sa nachádza viac definícií jazykov ({count}) ako je definované vo zvukovej banke ({maxLangs}).");

            for (var i = 1; i <= count; i++)
            {
                var area = $"LANGUAGE_{i.PadZeros(2)}";

                if (!categoriF.GetAreas().Contains(area))
                    throw new FormatException(string.Format(FORMAT_EX_AREA, file, area));

                var key = categoriF.Get(area, "KEY").ANSItoUTF();
                var isBasic = ParseIntOrDefault(categoriF.Get(area, "IS_BASIC", false)).ToBool();
                var name = categoriF.Get(area, "NAME").ANSItoUTF();

                foreach (var language in jazykyFromBank)
                    if (language.Key == key)
                    {
                        language.Name = name;
                        language.IsBasic = isBasic;
                        jazyky.Add(language);
                    }

                if (!Language.ContainsKey(jazykyFromBank, key))
                    throw new ArgumentException($"Neplatný kľúč jazyka {key} v súbore {file} (Nenachádza sa v zvukovej banke).");
            }

            return jazyky;
        }

        /// <summary>
        ///     Zapise informacie o jazykovych mutaciach hlaseni pouzivanych na stanici
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="jazyky">jazyky</param>
        public static void WriteLanguages(string path, List<Language> jazyky)
        {
            var file = CombinePath(path, FILE_CATEGORI);

            var categoriF = new TXTPropsAreasFields(file, true);

            categoriF.Set("MAIN", "COUNT_LANGUAGES", jazyky.Count);
            for (var i = 1; i <= jazyky.Count; i++)
            {
                var area = $"LANGUAGE_{i.PadZeros(2)}";
                categoriF.Set(area, "KEY", jazyky[i - 1].Key, WriteType.WriteStringANSI);
                var isBasic = jazyky[i - 1].IsBasic ? 1 : 0;
                categoriF.Set(area, "IS_BASIC", isBasic);
                categoriF.Set(area, "NAME", jazyky[i - 1].Name, WriteType.WriteStringANSI);
            }

            categoriF.Save();
        }

        #endregion

        #region LOCAL_CATEGORI

        /// <summary>
        ///     Nainicializuje data o variantach a typoch reportov a jazykovych mutaciach hlaseni (pre konkretne GVD)
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static (List<ReportVariant>,List<ReportType>,List<Language>) ReadLocalCategori(string path)
        {
            var file = CombinePath(path, FILE_CATEGORI);

            var variants = new List<ReportVariant>();
            var types = new List<ReportType>();
            var languages = new List<Language>();

            var categoriF = new TXTPropsAreasFields(file);

            var countV = int.Parse(categoriF.Get("MAIN", "COUNT_BASIC_REPORT_VARIANT"));
            for (var i = 1; i <= countV; i++)
            {
                var variant = new ReportVariant();
                var area = $"VARIANT_{i.PadZeros(2)}";
                variant.Key = int.Parse(categoriF.Get(area, "KEY"));
                variant.Name = categoriF.Get(area, "NAME").ANSItoUTF();

                variants.Add(variant);
            }

            var countT = int.Parse(categoriF.Get("MAIN", "COUNT_TYPE_BASIC_REPORT"));
            for (var i = 1; i <= countT; i++)
            {
                var area = $"TYPE_REPORT_{i.PadZeros(2)}";
                var key = categoriF.Get(area, "KEY").ANSItoUTF();
                var name = categoriF.Get(area, "NAME").ANSItoUTF();
                var @char = categoriF.Get(area, "CHAR").ANSItoUTF();
                var bt = int.Parse(categoriF.Get(area, "BASE_TRAIN")).ToBool();
                var pt = int.Parse(categoriF.Get(area, "PASS_THROUGH")).ToBool();
                var tt = int.Parse(categoriF.Get(area, "TERMINATE_TRAIN")).ToBool();
                var comp = int.Parse(categoriF.Get(area, "COMPLEMENT")).ToBool();
                var typ = new ReportType(key, name, @char, bt, pt, tt, comp);
                types.Add(typ);
            }

            var countL = int.Parse(categoriF.Get("MAIN", "COUNT_LANGUAGES"));
            for (var i = 1; i <= countL; i++)
            {
                var area = $"LANGUAGE_{i.PadZeros(2)}";
                var key = categoriF.Get(area, "KEY");
                var isBasic = ParseIntOrDefault(categoriF.Get(area, "IS_BASIC", false)).ToBool();
                var name = categoriF.Get(area, "NAME").ANSItoUTF();

                foreach (var lang in GlobData.Languages)
                    if (lang.Key == key)
                    {
                        lang.Name = name;
                        lang.IsBasic = isBasic;
                        languages.Add(lang);
                        break;
                    }

                if (!Language.ContainsKey(GlobData.Languages, key))
                    throw new ArgumentException($"Neplatný kľúč jazyka {key} v súbore {file}.");
            }

            return (variants, types, languages);
        }

        /// <summary>
        ///     Zapise data o variantach a typoch reportov a jazykovych mutaciach hlaseni pouzivanych na stanici (pre konkretne
        ///     GVD)
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="varianty">verianty reportov</param>
        /// <param name="types">typy reportov</param>
        /// <param name="languages">jazyky</param>
        public static void WriteLocalCategori(string path, List<ReportVariant> varianty, List<ReportType> types,
            List<Language> languages)
        {
            var file = CombinePath(path, FILE_CATEGORI);

            var categoriF = new TXTPropsAreasFields(file, true);

            categoriF.Set("MAIN", "COUNT_BASIC_REPORT_VARIANT", varianty.Count);
            categoriF.Set("MAIN", "COUNT_TYPE_BASIC_REPORT", types.Count);
            categoriF.Set("MAIN", "COUNT_LANGUAGES", languages.Count);

            for (var i = 0; i < varianty.Count; i++)
            {
                var variant = varianty[i];
                var ti = i + 1;

                var area = $"VARIANT_{ti.PadZeros(2)}";
                categoriF.Set(area, "KEY", variant.Key);
                categoriF.Set(area, "NAME", variant.Name, WriteType.WriteStringANSI);
            }


            for (var i = 0; i < types.Count; i++)
            {
                var typ = types[i];
                var ti = i + 1;

                var area = $"TYPE_REPORT_{ti.PadZeros(2)}";
                categoriF.Set(area, "KEY", typ.Key, WriteType.WriteStringANSI);
                categoriF.Set(area, "NAME", typ.Name, WriteType.WriteStringANSI);
                categoriF.Set(area, "CHAR", typ.Char, WriteType.WriteStringANSI);
                categoriF.Set(area, "BASE_TRAIN", typ.BaseTrain.ToNumber());
                categoriF.Set(area, "PASS_THROUGH", typ.PassThrough.ToNumber());
                categoriF.Set(area, "TERMINATE_TRAIN", typ.TerminateTrain.ToNumber());
                categoriF.Set(area, "COMPLEMENT", typ.Complement.ToNumber());
            }


            for (var i = 0; i < languages.Count; i++)
            {
                var language = languages[i];
                var ti = i + 1;

                var area = $"LANGUAGE_{ti.PadZeros(2)}";
                categoriF.Set(area, "KEY", language.Key, WriteType.WriteStringANSI);
                categoriF.Set(area, "IS_BASIC", language.IsBasic.ToNumber());
                categoriF.Set(area, "NAME", language.Name, WriteType.WriteStringANSI);
            }

            categoriF.Save();
        }

        #endregion

        #region TRAINS

        /// <summary>
        ///     Vrati informacie a data o vlakoch
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <returns>vlaky</returns>
        public static List<Train> ReadTrains(string path)
        {
            var fileEXP3A = CombinePath(path, FILE_EXPORT3A);
            var fileEXP3B = CombinePath(path, FILE_EXPORT3B);
            var fileEXP3C = CombinePath(path, FILE_EXPORT3C);
            var fileVlaky = CombinePath(path, FILE_VLAKY);
            var fileVzory = CombinePath(path, FILE_VZORY);
            var fileStaHlasB = CombinePath(path, FILE_STAHLASB);
            var fileStaHlasC = CombinePath(path, FILE_STAHLASC);
            var filePozice = CombinePath(path, FILE_POZICE);
            var fileForeign = CombinePath(path, FILE_FOREIGN);
            var fileDoplnky = CombinePath(path, FILE_DOPLNKY);

            var vlaky = new List<Train>();

            using (var export3AF = new CSVFileReader(fileEXP3A))
            {
                var riadok = 1;
                var row = new CSVRow();

                while (true)
                {
                    var status = export3AF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var vlak = new Train
                        {
                            ID = int.Parse(row[0]),
                            Number = row[1],
                            Name = row[2]
                        };
                        foreach (var typ in GlobData.TrainsTypes)
                            if (row[3] == typ.Key)
                                vlak.Type = typ;

                        if (vlak.Type == null)
                        {
                            var err = $"Neexistujúci typ vlaku {row[3]}";
                            throw new FormatException(err);
                        }

                        vlak.Variant = int.Parse(row[4]);
                        vlak.Routing = Routing.Parse(row[5]);
                        vlak.IsMedzistatny = row[6].Contains("M");
                        vlak.IsMiestenkovy = row[6].Contains("R");
                        vlak.IsMimoriadny = row[6].Contains("X");
                        vlak.IsDialkovy = row[6].Contains("D");
                        vlak.IsIbaLozkovy = row[6].Contains("L");
                        vlak.IsNizkopodlazny = row[6].Contains("N");
                        if (vlak.Routing == Routing.Vychadzajuci)
                        {
                            vlak.Departure = ParseTime(row[8]);
                            vlak.EndingStation = Station.GetFromID(row[10]);
                        }
                        else if (vlak.Routing == Routing.Prechadzajuci)
                        {
                            vlak.Arrival = ParseTime(row[7]);
                            vlak.Departure = ParseTime(row[8]);

                            vlak.StartingStation = Station.GetFromID(row[9]);
                            vlak.EndingStation = Station.GetFromID(row[10]);
                        }
                        else if (vlak.Routing == Routing.Konciaci)
                        {
                            vlak.Arrival = ParseTime(row[7]);
                            vlak.StartingStation = Station.GetFromID(row[9]);
                        }

                        var idOperator = ParseIntOrDefault(row[12], -1);
                        vlak.Operator = Operator.GetFromID(GlobData.Operators, idOperator);

                        vlak.LineArrival = row.ElementAtOrDefault(13);
                        vlak.LineDeparture = row.ElementAtOrDefault(14);

                        vlaky.Add(vlak);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_EXPORT3A, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var export3BF = new CSVFileReader(fileEXP3B))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = export3BF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var train = vlaky[id - 1];
                        train.ZaciatokPlatnosti = ParseDate(row[1]);
                        train.KoniecPlatnosti = ParseDate(row[2]);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(
                            string.Format(FORMAT_EX, FILE_EXPORT3B, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var export3CF = new CSVFileReader(fileEXP3C))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = export3CF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var train = vlaky[id - 1];

                        train.DateLimitText = int.Parse(row[1]) == 1 ? row[2] : "ide denne";
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_EXPORT3C, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            var vzoryList = new List<Template>();
            using (var vzoryF = new CSVFileReader(fileVzory))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = vzoryF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var vzor = new Template();
                        var id = int.Parse(row[0]);
                        vzor.ID = id;

                        var x = int.Parse(row[1]);

                        for (var i = 0; i < x; i++)
                        {
                            var idS = row[i + 2];
                            vzor.Stations.Add(Station.GetFromID(idS));
                        }

                        vzoryList.Add(vzor);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_VZORY, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var stahlasbF = new CSVFileReader(fileStaHlasB))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = stahlasbF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var template = vzoryList[id - 1];

                        var x = int.Parse(row[1]);

                        foreach (var stanica in template.Stations)
                            for (var i = 0; i < x; i++)
                                if (stanica.ID == row[i + 2])
                                    stanica.IsInShortReport = true;
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_STAHLASB, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var stahlascF = new CSVFileReader(fileStaHlasC))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = stahlascF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var template = vzoryList[id - 1];

                        var x = int.Parse(row[1]);

                        foreach (var stanica in template.Stations)
                            for (var i = 0; i < x; i++)
                                if (stanica.ID == row[i + 2])
                                    stanica.IsInLongReport = true;
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_STAHLASC, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var vlakyF = new CSVFileReader(fileVlaky))
            {
                var riadok = 1;
                var row = new CSVRow();

                var gvd = ReadInfoGVD(path);

                while (true)
                {
                    var status = vlakyF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);

                        var template = vzoryList.Find(v => v.ID == id);
                        if (template == null) throw new FormatException($"Neexistujúce ID trasy {id} v tomto súbore.");

                        var countTrains = int.Parse(row[1]);

                        for (var i = 0; i < countTrains; i++)
                        {
                            var cisloVlaku = row[2 + i * 4];
                            var vlakNazov = ParseStringOrDefault(row[3 + i * 4]);
                            var vlakTypS = ParseStringOrDefault(row[4 + i * 4]);
                            var varianta = int.Parse(row[5 + i * 4]);

                            var vlakTyp = GlobData.TrainsTypes.Find(t => t.Key == vlakTypS);
                            if (vlakTyp == null) throw new FormatException($"Neznámy typ vlaku {vlakTypS}.");

                            var vlak = Train.GetTrain(vlaky, cisloVlaku, vlakNazov, vlakTyp, varianta);
                            if (vlak == null)
                                throw new FormatException($"Neznámy vlak {cisloVlaku} {vlakTyp.Key} (nemá definíciu v EXPORT3A.txt).");

                            var skok = 0;

                            var staniceCopy = Station.CopyRoute(template.Stations);

                            for (var j = 0; j < staniceCopy.Count; j++)
                            {
                                if (template.Stations[j].ID == gvd.ThisStation.ID)
                                {
                                    skok = j + 1;
                                    break;
                                }

                                vlak.StaniceZoSmeru.Add(staniceCopy[j]);
                            }

                            for (var j = skok; j < staniceCopy.Count; j++) vlak.StaniceDoSmeru.Add(staniceCopy[j]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_VZORY, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var poziceF = new CSVFileReader(filePozice))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = poziceF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var train = vlaky[id - 1];

                        var kolaj = Track.GetFromID(GlobData.Tracks, row[1]);
                        train.Track = kolaj;
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_POZICE, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            using (var doplnkyF = new CSVFileReader(fileDoplnky))
            {
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = doplnkyF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var train = vlaky[id - 1];

                        var count = int.Parse(row[1]);
                        if (count != -1)
                            for (var i = 0; i < count; i++)
                                foreach (var sound in GlobData.Sounds)
                                {
                                    var sndName = sound.Name.Replace("D", "");
                                    if (sound.Dir.Name.EqualsIgnoreCase("DODATKY") && sndName == row[i * 2 + 2])
                                        try
                                        {
                                            train.Doplnky.Add(Dodatok.NumsToDodatok(sound, row[i * 2 + 3], GlobData.ReportTypes,
                                                GlobData.ReportVariants, train.Routing));
                                        }
                                        catch (Exception e)
                                        {
                                            throw new FormatException($"Doplnok vlaku [{id},{i}]: " + e.Message, e);
                                        }
                                }
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_DOPLNKY, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }

            try
            {
                using var foreignF = new CSVFileReader(fileForeign);
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = foreignF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var train = vlaky[id - 1];

                        if (!IsInt(row[1]))
                        {
                            var j = Language.GetLanguageFromKey(GlobData.Languages, row[1]);
                            if (j != null) train.Languages.Add(j);
                        }
                        else if (IsInt(row[1]) && int.Parse(row[1]) == 1)
                        {
                            train.Languages.AddRange(GlobData.Languages);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_FOREIGN, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }
            catch (FileNotFoundException)
            {
            }

            foreach (var vlak in vlaky)
            foreach (var rad in GlobData.Radenia)
                if (vlak.Number == rad.CisloVlaku)
                    vlak.Radenia.Add(rad);

            return vlaky;
        }

        /// <summary>
        ///     Zapise informacie a data o vlakoch
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="vlaky">vlaky</param>
        /// <param name="gvd">informacie o grafikone</param>
        /// <param name="reportVariants">varianty reportov</param>
        public static void WriteTrains(string path, IList<Train> vlaky, GVDInfo gvd, IList<ReportVariant> reportVariants)
        {
            var fileEXP3A = CombinePath(path, FILE_EXPORT3A);
            var fileEXP3B = CombinePath(path, FILE_EXPORT3B);
            var fileEXP3C = CombinePath(path, FILE_EXPORT3C);
            var fileVlaky = CombinePath(path, FILE_VLAKY);
            var fileVzory = CombinePath(path, FILE_VZORY);
            var fileStaHlasB = CombinePath(path, FILE_STAHLASB);
            var fileStaHlasC = CombinePath(path, FILE_STAHLASC);
            var filePozice = CombinePath(path, FILE_POZICE);
            var fileForeign = CombinePath(path, FILE_FOREIGN);
            var fileDoplnky = CombinePath(path, FILE_DOPLNKY);
            var fileStanice = CombinePath(path, FILE_STANICE);

            //EXPORT3A.TXT
            using (var export3aF = new CSVFileWriter(fileEXP3A))
            {
                var comments = GenerateComment(path, FILE_EXPORT3A, gvd, GlobData.Config.Language);
                foreach (var comment in comments) export3aF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());
                    row.Insert(1, vlak.Number.Quote());
                    row.Insert(2, (vlak.Name ?? "").UTFtoANSI().Quote());
                    row.Insert(3, vlak.Type.ToString().Quote());
                    row.Insert(4, vlak.Variant != -1 ? vlak.Variant.ToString() : "-1");
                    row.Insert(5, vlak.Routing.CharSymbol);

                    var priznaky = "";
                    if (vlak.IsIbaLozkovy) priznaky += "L";
                    if (vlak.IsMimoriadny) priznaky += "X";
                    if (vlak.IsMiestenkovy) priznaky += "R";
                    if (vlak.IsMedzistatny) priznaky += "M";
                    if (vlak.IsNizkopodlazny) priznaky += "N";
                    if (vlak.IsDialkovy) priznaky += "D";

                    row.Insert(6, priznaky);

                    if (vlak.Routing == Routing.Vychadzajuci)
                    {
                        row.Insert(7, "");
                        row.Insert(8, vlak.Departure?.ToString("HH:mm"));
                        row.Insert(9, gvd.ThisStation.ID);
                        row.Insert(10, vlak.StaniceDoSmeru.Last().ID);
                    }
                    else if (vlak.Routing == Routing.Konciaci)
                    {
                        row.Insert(7, vlak.Arrival?.ToString("HH:mm"));
                        row.Insert(8, "");
                        row.Insert(9, vlak.StaniceZoSmeru.First().ID);
                        row.Insert(10, gvd.ThisStation.ID);
                    }
                    else
                    {
                        row.Insert(7, vlak.Arrival?.ToString("HH:mm") ?? "");
                        row.Insert(8, vlak.Departure?.ToString("HH:mm") ?? "");
                        row.Insert(9, vlak.StaniceZoSmeru.First().ID);
                        row.Insert(10, vlak.StaniceDoSmeru.Last().ID);
                    }

                    row.Insert(11, "-1");
                    row.Insert(12, vlak.Operator.Id.ToString());

                    row.Insert(13, "");
                    row.Insert(14, "");

                    if (!string.IsNullOrEmpty(vlak.LineArrival)) row[13] = vlak.LineArrival;
                    if (!string.IsNullOrEmpty(vlak.LineDeparture)) row[14] = vlak.LineDeparture;

                    export3aF.WriteRow(row);
                    vlakId++;
                }
            }

            //EXPORT3B.TXT
            using (var export3bF = new CSVFileWriter(fileEXP3B))
            {
                var comments = GenerateComment(path, FILE_EXPORT3B, gvd, GlobData.Config.Language);
                foreach (var comment in comments) export3bF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());
                    row.Insert(1, vlak.ZaciatokPlatnosti.ToString("dd.MM.yyyy"));
                    row.Insert(2, vlak.KoniecPlatnosti.ToString("dd.MM.yyyy"));

                    var obmedzenie = new DateLimit(vlak.ZaciatokPlatnosti, vlak.KoniecPlatnosti);
                    var array = obmedzenie.TextToBitArray(vlak.DateLimitText);

                    row.Insert(3, BitArrayToString(array));
                    export3bF.WriteRow(row);
                    vlakId++;
                }
            }

            //EXPORT3C.TXT
            using (var export3cF = new CSVFileWriter(fileEXP3C))
            {
                var comments = GenerateComment(path, FILE_EXPORT3C, gvd, GlobData.Config.Language);
                foreach (var comment in comments) export3cF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());
                    row.Insert(1, "1");
                    row.Insert(2, vlak.DateLimitText.Quote());

                    export3cF.WriteRow(row);
                    vlakId++;
                }
            }

            //Zoriedenie tras vlakov
            var vzory = new List<Template>();
            var vlakTrasaDictionary = new Dictionary<Train, List<Station>>();

            foreach (var vlak in vlaky)
            {
                var stanice = new List<Station>();
                stanice.AddRange(vlak.StaniceZoSmeru);
                stanice.Add(gvd.ThisStation);
                stanice.AddRange(vlak.StaniceDoSmeru);

                vlakTrasaDictionary.Add(vlak, stanice);
            }

            var znameTrasy = new HashSet<EquatableCollection<Station>>();
            var zoradene = new Dictionary<EquatableCollection<Station>, HashSet<Train>>();

            foreach (var pair in vlakTrasaDictionary) znameTrasy.Add(new EquatableCollection<Station>(pair.Value));

            foreach (var trasa in znameTrasy)
            foreach (var vlak in vlaky)
            {
                var stanice = new List<Station>();
                stanice.AddRange(vlak.StaniceZoSmeru);
                stanice.Add(gvd.ThisStation);
                stanice.AddRange(vlak.StaniceDoSmeru);

                if (Station.SequencesEqual(stanice, trasa.ToList()))
                {
                    var staniceCL = new EquatableCollection<Station>(stanice);
                    if (zoradene.ContainsKey(staniceCL))
                    {
                        zoradene[staniceCL].Add(vlak);
                    }
                    else
                    {
                        zoradene.Add(staniceCL, new HashSet<Train>());
                        zoradene[staniceCL].Add(vlak);
                    }
                }
            }

            var o = 1;
            foreach (var pair in zoradene)
            {
                vzory.Add(new Template { ID = o, Stations = pair.Key.ToList(), Trains = pair.Value.ToList() });
                o++;
            }

            //VZORY.TXT
            using (var vzoryF = new CSVFileWriter(fileVzory))
            {
                var comments = GenerateComment(path, FILE_VZORY, gvd, GlobData.Config.Language);
                foreach (var comment in comments) vzoryF.WriteComment(comment);

                foreach (var vzor in vzory)
                {
                    var row = new CSVRow();
                    row.Insert(0, vzor.ID.ToString());
                    row.Insert(1, vzor.Stations.Count.ToString());
                    var i = 0;
                    foreach (var stanica in vzor.Stations)
                    {
                        row.Insert(i + 2, stanica.ID);
                        i++;
                    }

                    vzoryF.WriteRow(row);
                }
            }

            //VLAKY.TXT
            using (var vlakyF = new CSVFileWriter(fileVlaky))
            {
                var comments = GenerateComment(path, FILE_VLAKY, gvd, GlobData.Config.Language);
                foreach (var comment in comments) vlakyF.WriteComment(comment);

                foreach (var vzor in vzory)
                {
                    var row = new CSVRow();
                    row.Insert(0, vzor.ID.ToString());
                    row.Insert(1, vzor.Trains.Count.ToString());
                    var i = 0;
                    foreach (var vlak in vzor.Trains)
                    {
                        row.Insert(2 + i * 4, vlak.Number.Quote());
                        row.Insert(3 + i * 4, vlak.Name.Quote());
                        row.Insert(4 + i * 4, vlak.Type.Key.Quote());
                        row.Insert(5 + i * 4, vlak.Variant.ToString());
                        i++;
                    }

                    vlakyF.WriteRow(row);
                }
            }

            //STAHlasB.TXT
            using (var stahlasBF = new CSVFileWriter(fileStaHlasB))
            {
                var comments = GenerateComment(path, FILE_STAHLASB, gvd, GlobData.Config.Language);
                foreach (var comment in comments) stahlasBF.WriteComment(comment);

                foreach (var vzor in vzory)
                {
                    var row = new CSVRow();
                    row.Insert(0, vzor.ID.ToString());

                    var stanice = new List<Station>();

                    foreach (var stanica in vzor.Stations)
                        if (stanica.IsInShortReport && stanica.ID != gvd.ThisStation.ID)
                            stanice.Add(stanica);

                    row.Insert(1, stanice.Count.ToString());

                    foreach (var stanica in stanice) row.Add(stanica.ID);

                    stahlasBF.WriteRow(row);
                }
            }

            //STAHlasC.TXT
            using (var stahlasCF = new CSVFileWriter(fileStaHlasC))
            {
                var comments = GenerateComment(path, FILE_STAHLASC, gvd, GlobData.Config.Language);
                foreach (var comment in comments) stahlasCF.WriteComment(comment);

                foreach (var vzor in vzory)
                {
                    var row = new CSVRow();
                    row.Insert(0, vzor.ID.ToString());

                    var stanice = new List<Station>();

                    foreach (var stanica in vzor.Stations)
                        if (stanica.IsInLongReport && stanica.ID != gvd.ThisStation.ID)
                            stanice.Add(stanica);

                    row.Insert(1, stanice.Count.ToString());

                    foreach (var stanica in stanice)
                        row.Add(stanica.ID);

                    stahlasCF.WriteRow(row);
                }
            }

            //POZICE.TXT
            using (var poziceF = new CSVFileWriter(filePozice))
            {
                var comments = GenerateComment(path, FILE_POZICE, gvd, GlobData.Config.Language);

                foreach (var comment in comments) poziceF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());
                    row.Insert(1, vlak.Track.Key.Quote());

                    poziceF.WriteRow(row);
                    vlakId++;
                }
            }

            //DOPLNKY.TXT
            using (var doplnkyF = new CSVFileWriter(fileDoplnky))
            {
                var comments = GenerateComment(path, FILE_DOPLNKY, gvd, GlobData.Config.Language);
                foreach (var comment in comments) doplnkyF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());
                    if (vlak.Doplnky.Count == 0)
                    {
                        row.Insert(1, "-1");
                    }
                    else
                    {
                        row.Insert(1, vlak.Doplnky.Count.ToString());

                        List<ReportType> reportTypes;
                        if (vlak.Routing == Routing.Prechadzajuci)
                            reportTypes = GlobData.ReportTypesP;
                        else if (vlak.Routing == Routing.Konciaci)
                            reportTypes = GlobData.ReportTypesK;
                        else
                            reportTypes = GlobData.ReportTypesV;

                        for (var i = 0; i < vlak.Doplnky.Count; i++)
                        {
                            var snd = vlak.Doplnky[i].Sound.Name.Replace("D", "");

                            row.Insert(2 + i * 2, snd);
                            row.Insert(2 + i * 2 + 1,
                                Dodatok.DodatokToNums(vlak.Doplnky[i], reportTypes, reportVariants));
                        }
                    }

                    doplnkyF.WriteRow(row);
                    vlakId++;
                }
            }

            //FOREIGN.TXT
            using (var foreignF = new CSVFileWriter(fileForeign))
            {
                var comments = GenerateComment(path, FILE_FOREIGN, gvd, GlobData.Config.Language);
                foreach (var comment in comments) foreignF.WriteComment(comment);

                var vlakId = 0;
                foreach (var vlak in vlaky)
                {
                    var row = new CSVRow();
                    row.Insert(0, (vlakId + 1).ToString());

                    if (vlak.Languages.Count == 0)
                        row.Insert(1, "-1");
                    else if (vlak.Languages.Count == GlobData.Languages.Count)
                        row.Insert(1, "1");
                    else
                        foreach (var t in vlak.Languages)
                            if (!t.IsBasic)
                            {
                                row.Add(t.Key.Quote());
                                break;
                            }

                    foreignF.WriteRow(row);
                    vlakId++;
                }
            }

            //STANICE.TXT
            using (var staniceF = new CSVFileWriter(fileStanice))
            {
                var comments = GenerateComment(path, FILE_STANICE, gvd, GlobData.Config.Language);
                foreach (var comment in comments)
                    staniceF.WriteComment(comment);

                var allStations = new HashSet<Station> { gvd.ThisStation };

                foreach (var cs in GlobData.CustomStations)
                    allStations.Add(cs);

                foreach (var vlak in vlaky)
                {
                    foreach (var stZ in vlak.StaniceZoSmeru)
                        allStations.Add(stZ);

                    foreach (var stD in vlak.StaniceDoSmeru)
                        allStations.Add(stD);
                }

                foreach (var st in allStations)
                {
                    var row = new CSVRow();
                    row.Insert(0, st.ID);
                    row.Insert(1, st.Name.UTFtoANSI().Quote());
                    staniceF.WriteRow(row);
                }
            }
        }

        #endregion

        #region TRACKS

        /// <summary>
        ///     Nainicializuje informacie o nastupistiach a kolajach nachadzajucich sa na stanici
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static List<Track> ReadTracks(string path)
        {
            var file = CombinePath(path, FILE_POZICE_A);

            var tracks = new List<Track>();

            using var poziceAF = new CSVFileReader(file);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = poziceAF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var kolaj = new Track
                    {
                        Key = row[0],
                        Name = row[1],
                        FullName = row[2].ANSItoUTF(),
                        TrackName = row[4],
                        SoundName = row[6],
                        Nastupiste = new Platform(row[5], row[3].ANSItoUTF(), row[7])
                    };

                    for (var i = 0; i < ParseIntOrDefault(row[8]); i++)
                        foreach (var table in GlobData.TableLogicals)
                            if (table.Key == row[i + 9])
                            {
                                kolaj.Tables.Add(table);
                                break;
                            }

                    tracks.Add(kolaj);
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_POZICE_A, riadok) + e.Message, e);
                }

                riadok++;
            }

            if (!tracks.Contains(Track.None)) tracks.Insert(0, Track.None);

            return tracks;
        }

        /// <summary>
        ///     Zapise informacie o nastupistiach a kolajach nachadzajucich sa na stanici/zastavke
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="tracks">kolaje</param>
        public static void WriteTracks(string path, IEnumerable<Track> tracks)
        {
            var file = CombinePath(path, FILE_POZICE_A);

            using var poziceAF = new CSVFileWriter(file);

            foreach (var track in tracks)
            {
                if (track == Track.None) continue;

                var tabcount = track.Tables.Count;
                var row = new CSVRow(9 + 2 * tabcount)
                {
                    track.Key.Quote().UTFtoANSI(),
                    track.Name.Quote().UTFtoANSI(),
                    track.FullName.Quote().UTFtoANSI(),
                    track.Nastupiste.FullName.Quote().UTFtoANSI(),
                    track.TrackName.Quote().UTFtoANSI(),
                    track.Nastupiste.Key.Quote().UTFtoANSI(),
                    track.SoundName.Quote().UTFtoANSI(),
                    track.Nastupiste.SoundName.Quote().UTFtoANSI(),
                    track.Tables.Count.ToString()
                };

                for (var i = 0; i < tabcount; i++)
                {
                    row.Insert(9 + i, track.Tables[i].Key.Quote().UTFtoANSI());
                    row.Insert(10 + i, "0"); //TODO priority (nie su prioritne) :D
                }

                poziceAF.WriteRow(row);
            }
        }

        #endregion

        #region OPERATORS

        /// <summary>
        ///     Nainicializuje informacie o dopravcoch
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static List<Operator> ReadOperators(string path)
        {
            var file = CombinePath(path, FILE_VLASTNIK);

            var operators = new HashSet<Operator> { Operator.None };

            try
            {
                using var vlastnikF = new CSVFileReader(file);
                var riadok = 1;
                var row = new CSVRow();
                while (true)
                {
                    var status = vlastnikF.ReadRow(row);
                    if (LineIsEmpty(status))
                    {
                        riadok++;
                        continue;
                    }

                    if (LineIsEOF(status))
                        break;

                    try
                    {
                        var id = int.Parse(row[0]);
                        var nazov = row[1].ANSItoUTF();
                        operators.Add(new Operator(id, nazov));
                    }
                    catch (Exception e)
                    {
                        throw new FormatException(string.Format(FORMAT_EX, FILE_VLASTNIK, riadok) + e.Message, e);
                    }

                    riadok++;
                }
            }
            catch (FileNotFoundException)
            {
            }

            return operators.ToList();
        }

        /// <summary>
        ///     Zapise informacie o dopravcoch
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="operators">dopravcovia</param>
        public static void WriteOperators(string path, IEnumerable<Operator> operators)
        {
            var file = CombinePath(path, FILE_VLASTNIK);

            using var vlastnikF = new CSVFileWriter(file);
            foreach (var operatorV in operators)
                if (operatorV != Operator.None)
                {
                    var row = new CSVRow(2)
                    {
                        operatorV.Id.ToString(),
                        operatorV.Name.Quote().UTFtoANSI()
                    };

                    vlastnikF.WriteRow(row);
                }
        }

        #endregion

        #region CUSTOM_STATIONS

        /// <summary>
        ///     Nainicializuje vsetky pouzivatelom-definovane stanice, ktore sa nenachadzaju v zvukovej banke
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="gvd">informacie o grafikone</param>
        /// <returns>vlastne stanice</returns>
        public static List<Station> ReadCustomStations(string path, GVDInfo gvd)
        {
            var fileStanice = CombinePath(path, FILE_STANICE);

            var stanice = new List<Station>();

            using var staniceF = new CSVFileReader(fileStanice);
            var riadok = 1;
            var row = new CSVRow();
            while (true)
            {
                var status = staniceF.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                    break;

                try
                {
                    var id = int.Parse(row[0]);
                    var name = row[1].ANSItoUTF();
                    var stanica = new Station(id.ToString(), name);

                    if (!Station.ContainsName(GlobData.Stations, stanica.Name))
                    {
                        if (!stanica.EqualsName(gvd.ThisStation.Name))
                        {
                            stanica.IsCustom = true;
                            stanice.Add(stanica);
                        }
                        else
                        {
                            gvd.ThisStation.IsCustom = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_STANICE, riadok) + e.Message, e);
                }

                riadok++;
            }

            return stanice;
        }

        /// <summary>
        ///     Zapise vsetky pouzivatelom definovane stanice do STANICE.TXT
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="cstations">zoznam definovanych stanic</param>
        /// <param name="gvd">informacie o grafikone</param>
        public static void WriteCustomStations(string path, IEnumerable<Station> cstations, GVDInfo gvd)
        {
            var fileStations = CombinePath(path, FILE_STANICE);

            using var staniceF = new CSVFileWriter(fileStations);

            var comments = GenerateComment(path, FILE_STANICE, gvd, GlobData.Config.Language);
            foreach (var comment in comments) staniceF.WriteComment(comment);

            foreach (var cs in cstations)
            {
                var row = new CSVRow(2)
                {
                    cs.ID,
                    cs.Name.UTFtoANSI().Quote()
                };

                staniceF.WriteRow(row);
            }
        }

        #endregion

        #region RAZENI

        /// <summary>
        ///     Nainicializuje informacie o radeniach vlakov
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="sounds">zvuky zo zvukovej banky</param>
        /// <returns>radenia vlakov</returns>
        public static List<Radenie> ReadRazeni1(string path, List<FyzZvuk> sounds)
        {
            var fileRazeni1 = CombinePath(path, FILE_RAZENI1);

            var radeniaList = new List<Radenie>();

            using var razeni1F = new CSVFileReader(fileRazeni1);
            var riadok = 1;
            var row = new CSVRow();

            Radenie radenie = null;

            while (true)
            {
                var status = razeni1F.ReadRow(row);
                if (LineIsEmpty(status))
                {
                    riadok++;
                    continue;
                }

                if (LineIsEOF(status))
                {
                    if (radenie != null)
                    {
                        var sb = new StringBuilder();
                        foreach (var sound in radenie.Sounds) sb.Append(sound.Text + " ");

                        radenie.Text = sb.ToString().Trim();
                        radeniaList.Add(radenie);
                    }

                    break;
                }

                try
                {
                    if (row[0].StartsWith("#"))
                    {
                        if (radenie != null)
                        {
                            var sb = new StringBuilder();
                            foreach (var sound in radenie.Sounds) sb.Append(sound.Text + " ");

                            radenie.Text = sb.ToString().Trim();
                            radeniaList.Add(radenie);
                        }

                        radenie = new Radenie();
                        var array = row[0].Split(new[] { ':' }, 2);
                        radenie.CisloVlaku = array[0].Substring(1);
                        if (array.Length > 1 && array[1].Length > 0)
                        {
                            var id = ParseIntOrDefault(array[1], -1);
                            if (id == -1)
                            {
                                radenie = null;
                                continue;
                            }

                            radenie.DestStation = Station.GetFromID(id.ToString());
                        }

                        radenie.ChosenReports = ReportType.Parse(GlobData.ReportTypes, row[1]);
                        radenie.ZacPlatnosti = ParseDateAlts(row[2]);
                        radenie.KonPlatnosti = ParseDateAlts(row[3]);
                        var dateLimit = new DateLimit(radenie.ZacPlatnosti, radenie.KonPlatnosti, insertMarks: false);
                        var bit = new BitArray(row[4].Select(c => c == '1').ToArray());
                        radenie.DatObm = dateLimit.BitArrayToText(bit);
                    }
                    else
                    {
                        if (radenie == null) throw new FormatException("Chýbajú základné informácie o radení.");
                        var file = row[0];
                        var array = file.Split('/');

                        Language lang = null;
                        foreach (var jazyk in GlobData.LocalLanguages)
                            if (jazyk.Key == array[0])
                                lang = jazyk;

                        if (lang == null) 
                            throw new FormatException($"Jazyk {array[0]} neexistuje.");

                        FyzZvuk zvuk = null;
                        var formated = array[1];
                        foreach (var sound in sounds)
                            if (formated.EqualsIgnoreCase(sound.Dir.Name) && array[2].EqualsIgnoreCase(sound.Name) && lang == sound.Language)
                                zvuk = sound;

                        if (zvuk == null)
                            throw new FormatException($"Zvuk {formated}\\{array[2]} sa v zvukovej banke nenachádza.");
                        radenie.Sounds.Add(zvuk);
                    }
                }
                catch (Exception e)
                {
                    throw new FormatException(string.Format(FORMAT_EX, FILE_RAZENI1, riadok) + e.Message, e);
                }

                riadok++;
            }

            return radeniaList;
        }

        /// <summary>
        ///     Vytvori novy subor, ktory bude sluzit na ukladanie informacii o radeniach vlakov
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static void WriteRazeni1Default(string path) => File.Create(CombinePath(path, FILE_RAZENI1));

        /// <summary>
        ///     Zapise informacie o radeniach vlakov
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="radenia">radenia vlakov</param>
        /// <param name="languages">jazykove mutacie hlaseni</param>
        public static void WriteRazeni1(string path, IEnumerable<Radenie> radenia, IEnumerable<Language> languages)
        {
            var fileRazeni1 = CombinePath(path, FILE_RAZENI1);

            using var razeni1F = new CSVFileWriter(fileRazeni1);
            var otherLangs = new List<Language>(2);
            foreach (var lang in languages)
                if (!lang.IsBasic)
                    otherLangs.Add(lang);

            foreach (var radenie in radenia)
            {
                var row = new CSVRow();
                row.Insert(0,
                    radenie.DestStation != null
                        ? $"#{radenie.CisloVlaku}:{radenie.DestStation.ID}"
                        : $"#{radenie.CisloVlaku}");
                var sb = new StringBuilder();

                foreach (var reportType in radenie.ChosenReports)
                    foreach (var variant in reportType.Variants)
                        sb.Append(variant.Key == 0 ? reportType.Type.Char : reportType.Type.Char.ToLower());

                row.Insert(1, sb.ToString());
                row.Insert(2, radenie.ZacPlatnosti.ToString("dd.MM.yyyy"));
                row.Insert(3, radenie.KonPlatnosti.ToString("dd.MM.yyyy"));
                var dateRem = new DateLimit(radenie.ZacPlatnosti.Date, radenie.KonPlatnosti.Date, insertMarks: false);
                var bits = dateRem.TextToBitArray(radenie.DatObm);
                row.Insert(4, BitArrayToString(bits));
                razeni1F.WriteRow(row);

                var sbZvuk = new StringBuilder();
                var sbL1 = new StringBuilder();
                var sbL2 = new StringBuilder();
                var sbL3 = new StringBuilder();

                foreach (var sound in radenie.Sounds)
                {
                    var rowsound = new CSVRow();
                    rowsound.Insert(0, $"{sound.Language.Key}/{sound.Dir.Name}/{sound.Name}");
                    razeni1F.WriteRow(rowsound);
                    if (sound.Language.IsBasic)
                    {
                        sbZvuk.Append(sound.Name + " ");
                        sbL1.Append(sound.Text + " ");
                    }

                    switch (otherLangs.Count)
                    {
                        case 1:
                        {
                            if (sound.Language == otherLangs[0]) 
                                sbL2.Append(sound.Text + " ");
                            break;
                        }
                        case 2:
                        {
                            if (sound.Language == otherLangs[0]) 
                                sbL2.Append(sound.Text + " ");

                            if (sound.Language == otherLangs[1]) 
                                sbL3.Append(sound.Text + " ");
                            break;
                        }
                    }
                }

                var rowcomment = new CSVRow();
                rowcomment.Insert(0, $";{radenie.DatObm.Quote().UTFtoANSI()}");
                rowcomment.Insert(1, sbL1.ToString().Trim().Quote().UTFtoANSI());
                rowcomment.Insert(2, sbZvuk.ToString().Trim().Quote().UTFtoANSI());
                rowcomment.Insert(3, ""); //ich weiss nicht was ist das
                rowcomment.Insert(4, "");
                rowcomment.Insert(5, sbL2.ToString().Trim().Quote().UTFtoANSI());
                rowcomment.Insert(6, sbL3.ToString().Trim().Quote().UTFtoANSI());
                razeni1F.WriteRow(rowcomment);
            }
        }

        /// <summary>
        ///     Vytvori novy subor, ktory bude sluzit na ukladanie informacii o radeniach vlakov
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static void WriteRazeniDefault(string path) => File.Create(CombinePath(path, FILE_RAZENI));

        #endregion

        #region TABLES

        /// <summary>
        ///     Inicializuje definicie a vlastnosti tabul
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static (List<TableTabTab>, List<TableCatalog>, List<TablePhysical>, List<TableLogical>) ReadTables(string path)
        {
            var fileTabTab = CombinePath(path, FILE_TABTAB);
            var fileTCatalog = CombinePath(path, FILE_TKATALOG);
            var fileTPhysical = CombinePath(path, FILE_TPHYSIC);
            var fileTLogical = CombinePath(path, FILE_TLOGICAL);

            var tabtabs = new List<TableTabTab>();
            var tcatalogs = new List<TableCatalog>();
            var tphysicals = new List<TablePhysical>();
            var tlogicals = new List<TableLogical>();

            //TABTABS
            var tabtabF = new TXTPropsAreas(fileTabTab);
            int p = 1;
            foreach (var area in tabtabF.GetAreas())
            {
                if (string.IsNullOrWhiteSpace(area) || area == "")
                {
                    throw new FormatException($"TabTab č. {p} má neplatný názov (prázdny alebo obsahujúci iba biele znaky).");
                }

                var tabTab = new TableTabTab { Key = area, Text = tabtabF.Get(area) };
                tabtabs.Add(tabTab);
                p++;
            }

            //TCATALOGS
            var catalogF = new TXTPropsAreasFields(fileTCatalog);
            var count = int.Parse(catalogF.Get("MAIN", "COUNT"));
            for (var i = 0; i < count; i++)
            {
                var tcatalog = new TableCatalog();
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                tcatalog.Comment = catalogF.GetComment(area);

                tcatalog.Name = catalogF.Get(area, "NAME").ANSItoUTF();
                tcatalog.Key = CheckKey(catalogF.Get(area, "KEY").ANSItoUTF(), tcatalog.Name, tcatalogs);
                var manufacturer = catalogF.Get(area, "MANUFACTURER_KEY");
                tcatalog.Manufacturer = TableManufacturer.Parse(manufacturer);
                if (tcatalog.Manufacturer == null)
                {
                    throw new FormatException(
                        $"Katalógová tabuľa {tcatalog.Key} má zadaný neplatný kľúč výrobcu (MANUFACTURER_KEY): {manufacturer}.");
                }

                tcatalog.MaxRecCount = ParseIntOrDefault(catalogF.Get(area, "MAX_REC_COUNT", false));
                tcatalog.MinHeight = ParseIntOrDefault(catalogF.Get(area, "MIN_HEIGHT", false));
                tcatalog.NumSegments = ParseIntOrDefault(catalogF.Get(area, "NUM_SEGMENTS", false), 1);

                for (var j = 0; j < ParseIntOrDefault(catalogF.Get(area, "COUNT_PHYSICAL_LINE_INFO", false)); j++)
                {
                    var tj = j + 1;
                    var segment = new TableSegment
                    {
                        Height = ParseIntOrDefault(catalogF.Get(area, $"HEIGHT_{tj.PadZeros()}", false)),
                        Width = ParseIntOrDefault(catalogF.Get(area, $"WIDTH_{tj.PadZeros()}", false)),
                        Size = ParseIntOrDefault(catalogF.Get(area, $"SIZE_{tj.PadZeros()}", false))
                    };
                    tcatalog.Segments.Add(segment);
                }

                for (var j = 0; j < ParseIntOrDefault(catalogF.Get(area, "COUNT_TYPE_ITEMS", false)); j++)
                {
                    var tj = j + 1;
                    var padded = tj.PadZeros();

                    var name = catalogF.Get(area, $"TYPE_ITEMS_NAME_{padded}").ANSItoUTF();
                    var item = new TableItem
                    {
                        Name = name,
                        Key = CheckKey(catalogF.Get(area, $"TYPE_ITEMS_KEY_{padded}").ANSItoUTF(), name, tcatalog.Items),
                        Line = ParseIntOrDefault(catalogF.Get(area, $"TYPE_ITEMS_LINE_{padded}", false)),
                        Start = ParseIntOrDefault(catalogF.Get(area, $"TYPE_ITEMS_START_{padded}", false)),
                        End = ParseIntOrDefault(catalogF.Get(area, $"TYPE_ITEMS_END_{padded}", false)),
                        FontIDX = ParseIntOrDefault(catalogF.Get(area, $"TYPE_ITEMS_FONT_IDX_{padded}", false))
                    };

                    var fillSection = catalogF.Get(area, $"TYPE_ITEMS_IDX_{padded}", false);
                    item.FillSection = TableFillSection.Parse(ParseIntOrDefault(fillSection));

                    var align = catalogF.Get(area, $"TYPE_ITEMS_ALIGN_{padded}", false);
                    item.Align = TableAlign.Parse(ParseIntOrDefault(align));

                    var divType = catalogF.Get(area, $"TYPE_ITEMS_DIVTYPE_{padded}", false);
                    item.DivType = TableDivType.Parse(ParseIntOrDefault(divType));

                    if (item.FillSection == null)
                    {
                        throw new FormatException(
                            $"Katalógová tabuľa {tcatalog.Key} obsahuje pre stĺpec {item.Key} neplatnú hodnotu TYPE_ITEMS_IDX: {fillSection}.");
                    }

                    if (item.Align == null)
                    {
                        throw new FormatException(
                            $"Katalógová tabuľa {tcatalog.Key} obsahuje pre stĺpec {item.Key} neplatnú hodnotu TYPE_ITEMS_ALIGN: {align}.");
                    }

                    if (item.DivType == null)
                    {
                        throw new FormatException(
                            $"Katalógová tabuľa {tcatalog.Key} obsahuje pre stĺpec {item.Key} neplatnú hodnotu TYPE_ITEMS_DIVTYPE: {align}.");
                    }

                    var tab1 = catalogF.Get(area, $"TYPE_ITEMS_TAB1_{padded}").ANSItoUTF();
                    var tab2 = catalogF.Get(area, $"TYPE_ITEMS_TAB2_{padded}").ANSItoUTF();

                    CheckTabTab(tab1, item, tabtabs, true, tcatalog.Key);
                    CheckTabTab(tab2, item, tabtabs, false, tcatalog.Key);

                    tcatalog.Items.Add(item);
                }

                for (var j = 0; j < ParseIntOrDefault(catalogF.Get(area, "COUNT_TYPE_VIEW_TAB", false)); j++)
                {
                    var tj = j + 1;
                    var typetab = new TableViewTypeTab();

                    var viewType = catalogF.Get(area, $"TYPE_VIEW_TAB_KEY_{tj.PadZeros()}").ANSItoUTF();
                    typetab.ViewType = TableViewType.Parse(viewType);

                    if (typetab.ViewType == null)
                    {
                        throw new FormatException(
                            $"Katalógová tabuľa {tcatalog.Key} obsahuje neplatný typ zobrazenia (TYPE_VIEW_TAB_KEY): {viewType}.");
                    }

                    typetab.CountLinesRecord = catalogF.Get(area, $"TYPE_VIEW_TAB_COUNT_LINES_RECORD_{tj.PadZeros()}");
                    if (!IsInt(typetab.CountLinesRecord))
                    {
                        throw new FormatException(
                            $"Katalógová tabuľa {tcatalog.Key} obsahuje v type zobrazenia {typetab.ViewType.Key} neplatný počet riadkov - \"{typetab.CountLinesRecord}\" (počet má byť číslo).");
                    }

                    var ck = ParseIntOrDefault(catalogF.Get(area, $"TYPE_VIEW_TAB_COUNT_TYPE_MODE_{tj.PadZeros()}", false));
                    for (var k = 0; k < ck; k++)
                    {
                        var tk = k + 1;
                        var ttmi = new TableTypeModeItem();

                        var viewMode = catalogF.Get(area, $"TYPE_MODE_KEY_{tj.PadZeros()}_{tk.PadZeros()}").ANSItoUTF();
                        ttmi.ViewMode = TableViewMode.Parse(viewMode);

                        if (ttmi.ViewMode == null)
                        {
                            throw new FormatException(
                                $"Katalógová tabuľa {tcatalog.Key} obsahuje neplatný typ zobrazenia (TYPE_VIEW_TAB_KEY): {viewMode}.");
                        }

                        var cl = ParseIntOrDefault(catalogF.Get(area, $"TYPE_MODE_COUNT_TYPE_ITEM_{tj.PadZeros()}_{tk.PadZeros()}", false));
                        for (var l = 0; l < cl; l++)
                        {
                            var tl = l + 1;
                            var itemKey = catalogF.Get(area, $"TYPE_ITEM_{tj.PadZeros()}_{tk.PadZeros()}_{tl.PadZeros(2)}").ANSItoUTF();

                            string tolist = null;
                            foreach (var item in tcatalog.Items)
                                if (item.Key == itemKey)
                                    tolist = itemKey;

                            if (!string.IsNullOrEmpty(tolist))
                                ttmi.ItemsKeys.Add(itemKey);
                            else
                                throw new FormatException(
                                    $"Katalógová tabuľa {tcatalog.Key} obsahuje v type {typetab.ViewType.Key} v móde {ttmi.ViewMode.Key} neplatný stĺpec {itemKey}.");
                        }

                        typetab.TypeModeItems.Add(ttmi);
                    }

                    tcatalog.ViewTypeTabs.Add(typetab);
                }

                tcatalogs.Add(tcatalog);
            }

            //TPHYSIC
            var tphysicF = new TXTPropsAreasFields(fileTPhysical);
            var countp = int.Parse(tphysicF.Get("MAIN", "COUNT"));
            for (var i = 0; i < countp; i++)
            {
                var tphysical = new TablePhysical();
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                tphysical.Comment = tphysicF.GetComment(area);

                tphysical.Name = tphysicF.Get(area, "NAME").ANSItoUTF();
                tphysical.Key = CheckKey(tphysicF.Get(area, "KEY").ANSItoUTF(), tphysical.Name, tphysicals);
                tphysical.ID = ParseIntOrDefault(tphysicF.Get(area, "ID", false));
                tphysical.CommunicationPort = ParseIntOrDefault(tphysicF.Get(area, "COMUNICATION_PORT", false));
                tphysical.RecCount = ParseIntOrDefault(tphysicF.Get(area, "REC_COUNT", false));
                tphysical.Rem = tphysicF.Get(area, "REM").ANSItoUTF();
                tphysical.ReverseArrows = ParseStringOrDefault(tphysicF.Get(area, "REVERSE_ARROWS", false)).ANSItoUTF();
                tphysical.SaveXML = tphysicF.Get(area, "SAVE_XML", "").ANSItoUTF();

                var catname = tphysicF.Get(area, "CATALOG_KEY").ANSItoUTF();
                AssignCatalogToPhysical(tcatalogs, tphysical, catname);
                if (tphysical.TableCatalog == null)
                    throw new FormatException($"Katalógová tabuľa {catname}, ktorá bola použitá vo fyzickej tabuli {tphysical.Key}, neexistuje.");

                tphysicals.Add(tphysical);
            }

            //TLOGICAL
            var tlogicF = new TXTPropsAreasFields(fileTLogical);
            var countl = int.Parse(tlogicF.Get("MAIN", "COUNT"));
            for (var i = 0; i < countl; i++)
            {
                var tlLogical = new TableLogical();
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                tlLogical.Comment = tlogicF.GetComment(area);

                tlLogical.Name = tlogicF.Get(area, "NAME").ANSItoUTF();
                tlLogical.Key = CheckKey(tlogicF.Get(area, "KEY").ANSItoUTF(), tlLogical.Name, tlogicals);
                tlLogical.TypeViewFlags = tlogicF.Get(area, "TYPE_VIEW_FLAGS").ANSItoUTF();
                var viewtype = tlogicF.Get(area, "TYPE_VIEW").ANSItoUTF();
                tlLogical.ViewType = TableViewType.Parse(viewtype);
                if (tlLogical.ViewType == null)
                {
                    throw new FormatException($"Logická tabuľa {tlLogical.Key} obsahuje neplatný typ zobrazenia (TYPE_VIEW): {viewtype}.");
                }

                for (var j = 0; j < ParseIntOrDefault(tlogicF.Get(area, "COUNT_REC", false)); j++)
                {
                    var trecord = new TableRecord();
                    var tj = j + 1;

                    for (var k = 0; k < ParseIntOrDefault(tlogicF.Get(area, $"COUNT_POS_{tj.PadZeros()}", false)); k++)
                    {
                        var tposition = new TablePosition();
                        var tk = k + 1;

                        tposition.Position = int.Parse(tlogicF.Get(area, $"POSITION_{tj.PadZeros()}_{tk.PadZeros()}"));
                        var tv = tlogicF.Get(area, $"TYPE_VIEW_KEY_{tj.PadZeros()}_{tk.PadZeros()}").ANSItoUTF();
                        tposition.TypeView = TableViewType.Parse(tv);
                        if (tposition.TypeView == null)
                        {
                            throw new FormatException($"Logická tabuľa {tlLogical.Key} obsahuje pre neplatný kľúč typu zobrazenia (TYPE_VIEW_KEY_{tj.PadZeros()}_{tk.PadZeros()}): {tv}.");
                        }

                        var fyzname = tlogicF.Get(area, $"PHYSICAL_KEY_{tj.PadZeros()}_{tk.PadZeros()}").ANSItoUTF();
                        AssignPhysicalToPosition(tphysicals, tposition, fyzname);

                        if (tposition.Table == null)
                            throw new FormatException($"Logická tabuľa {tlLogical.Key} obsahuje neexistujúcu fyzickú tabuľu {fyzname}.");

                        trecord.Positions.Add(tposition);
                    }

                    tlLogical.Records.Add(trecord);
                }

                tlogicals.Add(tlLogical);
            }

            return (tabtabs,tcatalogs,tphysicals,tlogicals);
        }

        private static void CheckTabTab(string tabkey, TableItem item, IEnumerable<TableTabTab> tabtabs, bool tab1, string catalogKey)
        {
            if (!string.IsNullOrEmpty(tabkey))
            {
                TableTabTab tab = null;
                foreach (var tabtab in tabtabs)
                    if (tabkey == tabtab.Key)
                    {
                        tab = tabtab;
                        break;
                    }

                if (tab == null)
                    throw new FormatException(
                        $"Katalógová tabuľa {catalogKey} obsahuje pre stĺpec {item.Key} neexistujúci {(tab1 ? "TABTAB1" : "TABTAB2")} {tabkey}.");
                
                if (tab1) item.Tab1 = tab;
                else item.Tab2 = tab;
            }
            else
            {
                if (tab1) item.Tab1 = TableTabTab.Empty;
                else item.Tab2 = TableTabTab.Empty;
            }
        }

        private static void AssignPhysicalToPosition(IEnumerable<TablePhysical> tphysicals, TablePosition tposition, string fyzname)
        {
            foreach (var physical in tphysicals)
            {
                if (physical.Key == fyzname)
                {
                    tposition.Table = physical;
                    break;
                }
            }
        }

        private static void AssignCatalogToPhysical(IEnumerable<TableCatalog> tcatalogs, TablePhysical tphysical, string catname)
        {
            foreach (var tableCatalog in tcatalogs)
            {
                if (tableCatalog.Key == catname)
                {
                    tphysical.TableCatalog = tableCatalog;
                    break;
                }
            }
        }

        /// <summary>
        ///     Zapise data o tabuliach do suborov.
        /// </summary>
        /// <param name="path">Cesta do priecinka s datami.</param>
        /// <param name="TabTabs">TabTabs.</param>
        /// <param name="Catalogs">Katalogove tabule.</param>
        /// <param name="Physicals">Fyzicke tabule.</param>
        /// <param name="Logicals">Logicke tabule.</param>
        public static void WriteTables(string path, IEnumerable<TableTabTab> TabTabs, List<TableCatalog> Catalogs,
            List<TablePhysical> Physicals, List<TableLogical> Logicals)
        {
            var fileTabTab = CombinePath(path, FILE_TABTAB);
            var fileTCatalog = CombinePath(path, FILE_TKATALOG);
            var fileTPhysical = CombinePath(path, FILE_TPHYSIC);
            var fileTLogical = CombinePath(path, FILE_TLOGICAL);

            //TABTABS
            var tabtabF = new TXTPropsAreas(fileTabTab, true);
            foreach (var tabTab in TabTabs)
            {
                tabtabF.Set(tabTab.Key, tabTab.Text);
            }
            tabtabF.Save();

            //TCATALOG
            var catalogF = new TXTPropsAreasFields(fileTCatalog, true);
            catalogF.Set("MAIN", "COUNT", Catalogs.Count);
            for (var i = 0; i < Catalogs.Count; i++)
            {
                var tcatalog = Catalogs[i];
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                catalogF.SetComment(area, tcatalog.Comment);

                catalogF.Set(area, "KEY", tcatalog.Key, WriteType.WriteStringANSI);
                catalogF.Set(area, "NAME", tcatalog.Name, WriteType.WriteStringANSI);
                catalogF.Set(area, "MANUFACTURER_KEY", tcatalog.Manufacturer.Name, WriteType.WriteStringANSI);
                catalogF.Set(area, "MAX_REC_COUNT", tcatalog.MaxRecCount);
                catalogF.Set(area, "MIN_HEIGHT", tcatalog.MinHeight);
                catalogF.Set(area, "NUM_SEGMENTS", tcatalog.NumSegments);

                catalogF.Set(area, "COUNT_PHYSICAL_LINE_INFO", tcatalog.Segments.Count);
                for (var j = 0; j < tcatalog.Segments.Count; j++)
                {
                    var segment = tcatalog.Segments[j];
                    var tj = j + 1;

                    catalogF.Set(area, $"HEIGHT_{tj.PadZeros()}", segment.Height);
                    catalogF.Set(area, $"WIDTH_{tj.PadZeros()}", segment.Width);
                    catalogF.Set(area, $"SIZE_{tj.PadZeros()}", segment.Size);
                }

                catalogF.Set(area, "COUNT_TYPE_ITEMS", tcatalog.Items.Count);
                for (var j = 0; j < tcatalog.Items.Count; j++)
                {
                    var item = tcatalog.Items[j];
                    var tj = j + 1;

                    catalogF.Set(area, $"TYPE_ITEMS_KEY_{tj.PadZeros()}", item.Key, WriteType.WriteStringANSI);
                    catalogF.Set(area, $"TYPE_ITEMS_NAME_{tj.PadZeros()}", item.Name, WriteType.WriteStringANSI);
                    catalogF.Set(area, $"TYPE_ITEMS_IDX_{tj.PadZeros()}", item.FillSection.ID);
                    catalogF.Set(area, $"TYPE_ITEMS_LINE_{tj.PadZeros()}", item.Line);
                    catalogF.Set(area, $"TYPE_ITEMS_START_{tj.PadZeros()}", item.Start);
                    catalogF.Set(area, $"TYPE_ITEMS_END_{tj.PadZeros()}", item.End);
                    catalogF.Set(area, $"TYPE_ITEMS_FONT_IDX_{tj.PadZeros()}", item.FontIDX);
                    catalogF.Set(area, $"TYPE_ITEMS_ALIGN_{tj.PadZeros()}", item.Align.ID);
                    catalogF.Set(area, $"TYPE_ITEMS_DIVTYPE_{tj.PadZeros()}", item.DivType.ID);
                    catalogF.Set(area, $"TYPE_ITEMS_TAB1_{tj.PadZeros()}", item.Tab1 == TableTabTab.Empty ? "" : item.Tab1.Key,
                        WriteType.WriteStringANSI);
                    catalogF.Set(area, $"TYPE_ITEMS_TAB2_{tj.PadZeros()}", item.Tab2 == TableTabTab.Empty ? "" : item.Tab2.Key,
                        WriteType.WriteStringANSI);
                }

                catalogF.Set(area, "COUNT_TYPE_VIEW_TAB", tcatalog.ViewTypeTabs.Count);
                for (var j = 0; j < tcatalog.ViewTypeTabs.Count; j++)
                {
                    var typetab = tcatalog.ViewTypeTabs[j];
                    var tj = j + 1;

                    catalogF.Set(area, $"TYPE_VIEW_TAB_KEY_{tj.PadZeros()}", typetab.ViewType.Key, WriteType.WriteStringANSI);
                    catalogF.Set(area, $"TYPE_VIEW_TAB_COUNT_LINES_RECORD_{tj.PadZeros()}", typetab.CountLinesRecord, WriteType.WriteStringANSI);
                    catalogF.Set(area, $"TYPE_VIEW_TAB_COUNT_TYPE_MODE_{tj.PadZeros()}", typetab.TypeModeItems.Count);
                    for (var k = 0; k < typetab.TypeModeItems.Count; k++)
                    {
                        var ttmi = typetab.TypeModeItems[k];
                        var tk = k + 1;

                        catalogF.Set(area, $"TYPE_MODE_KEY_{tj.PadZeros()}_{tk.PadZeros()}", ttmi.ViewMode.Key, WriteType.WriteStringANSI);
                        catalogF.Set(area, $"TYPE_MODE_COUNT_TYPE_ITEM_{tj.PadZeros()}_{tk.PadZeros()}", ttmi.ItemsKeys.Count);
                        for (var l = 0; l < ttmi.ItemsKeys.Count; l++)
                        {
                            var tl = l + 1;
                            catalogF.Set(area, $"TYPE_ITEM_{tj.PadZeros()}_{tk.PadZeros()}_{tl.PadZeros(2)}", ttmi.ItemsKeys[l],
                                WriteType.WriteStringANSI);
                        }
                    }
                }
            }
            catalogF.Save();

            //TPHYSIC
            var tphysicF = new TXTPropsAreasFields(fileTPhysical, true);
            tphysicF.Set("MAIN", "COUNT", Physicals.Count);
            for (var i = 0; i < Physicals.Count; i++)
            {
                var tphysical = Physicals[i];
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                tphysicF.SetComment(area, tphysical.Comment);

                tphysicF.Set(area, "KEY", tphysical.Key, WriteType.WriteStringANSI);
                tphysicF.Set(area, "NAME", tphysical.Name, WriteType.WriteStringANSI);
                tphysicF.Set(area, "ID", tphysical.ID);
                tphysicF.Set(area, "COMUNICATION_PORT", tphysical.CommunicationPort);
                tphysicF.Set(area, "REC_COUNT", tphysical.RecCount);
                tphysicF.Set(area, "REM", tphysical.Rem, WriteType.WriteStringANSINullable);
                tphysicF.Set(area, "REVERSE_ARROWS", tphysical.ReverseArrows, WriteType.WriteStringANSINullable);
                tphysicF.Set(area, "SAVE_XML", tphysical.SaveXML, WriteType.WriteStringANSI);
                tphysicF.Set(area, "CATALOG_KEY", tphysical.TableCatalog.Key, WriteType.WriteStringANSI);
            }
            tphysicF.Save();

            //TLOGICAL
            var tlogicF = new TXTPropsAreasFields(fileTLogical, true);
            tlogicF.Set("MAIN", "COUNT", Logicals.Count);
            for (var i = 0; i < Logicals.Count; i++)
            {
                var tlLogical = Logicals[i];
                var ti = i + 1;

                var area = $"TABLE_{ti.PadZeros()}";
                tlogicF.SetComment(area, tlLogical.Comment);

                tlogicF.Set(area, "KEY", tlLogical.Key, WriteType.WriteStringANSI);
                tlogicF.Set(area, "NAME", tlLogical.Name, WriteType.WriteStringANSI);
                tlogicF.Set(area, "TYPE_VIEW", tlLogical.ViewType.Key, WriteType.WriteStringANSI);
                tlogicF.Set(area, "TYPE_VIEW_FLAGS", tlLogical.TypeViewFlags, WriteType.WriteStringANSINullable);
                tlogicF.Set(area, "COUNT_REC", tlLogical.Records.Count);

                for (var j = 0; j < tlLogical.Records.Count; j++)
                {
                    var trecord = tlLogical.Records[j];
                    var tj = j + 1;

                    tlogicF.Set(area, $"COUNT_POS_{tj.PadZeros()}", trecord.Positions.Count);
                    for (var k = 0; k < trecord.Positions.Count; k++)
                    {
                        var tposition = trecord.Positions[k];
                        var tk = k + 1;

                        tlogicF.Set(area, $"POSITION_{tj.PadZeros()}_{tk.PadZeros()}", tposition.Position);
                        tlogicF.Set(area, $"TYPE_VIEW_KEY_{tj.PadZeros()}_{tk.PadZeros()}", tposition.TypeView.Key,
                            WriteType.WriteStringANSI);
                        tlogicF.Set(area, $"PHYSICAL_KEY_{tj.PadZeros()}_{tk.PadZeros()}", tposition.Table.Key,
                            WriteType.WriteStringANSI);
                    }
                }
            }
            tlogicF.Save();
        }

        #endregion

        #region TTEXTS

        /// <summary>
        ///     Nainicializuje texty do tabul
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="trains">vlaky</param>
        public static List<TableText> ReadTTexts(string path, IList<Train> trains)
        {
            var fileTTexts = CombinePath(path, FILE_TTEXTS);

            var ttexts = new List<TableText>();

            var ttextsF = new TXTPropsAreasFields(fileTTexts);

            var countt = int.Parse(ttextsF.Get("MAIN", "COUNT"));

            for (var i = 0; i < countt; i++)
            {
                var ttext = new TableText();
                var ti = i + 1;

                var area = $"TEXT_{ti.PadZeros()}";
                ttext.Comment = ttextsF.GetComment(area);

                ttext.Key = ttextsF.Get(area, "KEY").ANSItoUTF();
                ttext.Name = ttextsF.Get(area, "NAME").ANSItoUTF();
                for (var j = 0; j < ParseIntOrDefault(ttextsF.Get(area, "REALIZE_COUNT", false)); j++)
                {
                    var realization = new TableTextRealization();
                    var tj = j + 1;

                    var catname = ttextsF.Get(area, $"REALIZE_{tj.PadZeros()}_CATALOG_KEY").ANSItoUTF();
                    foreach (var catalog in GlobData.TableCatalogs)
                        if (catalog.Key == catname)
                        {
                            realization.Table = catalog;
                            break;
                        }

                    if (realization.Table == null)
                    {
                        throw new FormatException($"Súbor s textami pre tabule {ttext.Key} obsahuje neexistujúcu katalógovú tabuľu {catname}.");
                    }

                    var realname = ttextsF.Get(area, $"REALIZE_{tj.PadZeros()}_TYPEITEM_KEY").ANSItoUTF();
                    foreach (var item in realization.Table.Items)
                        if (item.Key == realname)
                        {
                            realization.Item = item;
                            break;
                        }

                    if (realization.Item == null)
                        throw new FormatException(
                            $"Súbor s textami pre tabule {ttext.Key} obsahuje neexistujúci stĺpec pre realizáciu {realname}, ktorý by mal patriť katalógovej tabuli {realization.Table.Key}.");

                    ttext.Realizations.Add(realization);
                }

                for (var j = 0; j < ParseIntOrDefault(ttextsF.Get(area, "COUNT", false)); j++)
                {
                    var ttrain = new TableTrain();
                    var tj = j + 1;

                    var id = ParseIntOrDefault(ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_ID", false));
                    foreach (var train in trains)
                        if (train.ID == id)
                            ttrain.Train = train;
                    if (ttrain.Train == null)
                        throw new FormatException($"Súbor s textami pre tabule obsahuje ID neexistujúceho vlaku ({id}).");

                    ttrain.FontID = ParseIntOrDefault(ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_IDX_FONT", false));
                    ttrain.Text = ttextsF.Get(area, $"TRAIN_{tj.PadZeros()}_TEXT").ANSItoUTF();

                    ttext.Trains.Add(ttrain);
                }

                ttexts.Add(ttext);
            }

            return ttexts;
        }

        /// <summary>
        ///     Zapise texty do tabul do suboru
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="ttexts">texty do tabul</param>
        public static void WriteTTexts(string path, List<TableText> ttexts)
        {
            var fileTTexts = CombinePath(path, FILE_TTEXTS);

            var ttextsF = new TXTPropsAreasFields(fileTTexts, true);

            ttextsF.Set("MAIN", "COUNT", ttexts.Count);

            for (var i = 0; i < ttexts.Count; i++)
            {
                var ttext = ttexts[i];
                var ti = i + 1;

                var area = $"TEXT_{ti.PadZeros()}";

                ttextsF.SetComment(area, ttext.Comment);

                ttextsF.Set(area, "KEY", ttext.Key, WriteType.WriteStringANSI);
                ttextsF.Set(area, "NAME", ttext.Name, WriteType.WriteStringANSI);

                ttextsF.Set(area, "REALIZE_COUNT", ttext.Realizations.Count);
                for (var j = 0; j < ttext.Realizations.Count; j++)
                {
                    var realization = ttext.Realizations[j];
                    var tj = j + 1;

                    ttextsF.Set(area, $"REALIZE_{tj.PadZeros()}_CATALOG_KEY", realization.Table.Key, WriteType.WriteStringANSI);
                    ttextsF.Set(area, $"REALIZE_{tj.PadZeros()}_TYPEITEM_KEY", realization.Item.Key, WriteType.WriteStringANSI);
                }

                ttextsF.Set(area, "COUNT", ttext.Trains.Count);
                for (var j = 0; j < ttext.Trains.Count; j++)
                {
                    var ttrain = ttext.Trains[j];
                    var tj = j + 1;

                    ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_ID", ttrain.Train.ID);
                    ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_IDX_FONT", ttrain.FontID);
                    ttextsF.Set(area, $"TRAIN_{tj.PadZeros()}_TEXT", ttrain.Text, WriteType.WriteStringANSI);
                }
            }

            ttextsF.Save();
        }

        #endregion

        #region MODETABS

        /// <summary>
        ///     Vrati typy fontov pre tabule
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        public static List<TableFont> ReadTableFonts(string path)
        {
            var fileModeTabs = CombinePath(path, FILE_MODETABS);

            var fonts = new List<TableFont>();

            var modetabsF = new TXTPropsAreasFields(fileModeTabs);

            const string area = "FONT";

            var count = int.Parse(modetabsF.Get(area, "COUNT"));
            GlobData.TableFontDir = ParseStringOrDefault(modetabsF.Get(area, "PATH", false));

            for (var i = 0; i < count; i++)
            {
                var ti = i + 1;
                var pad = ti.PadZeros();
                var font = new TableFont
                {
                    FontID = ParseIntOrDefault(modetabsF.Get(area, $"IDX_{pad}", false)),
                    Name = modetabsF.Get(area, $"NAME_{pad}").ANSItoUTF(),
                    Size = ParseIntOrDefault(modetabsF.Get(area, $"SIZE_{pad}", false)),
                    Width = ParseIntOrDefault(modetabsF.Get(area, $"WIDTH_{pad}", false)),
                    IsProportional = ParseIntOrDefault(modetabsF.Get(area, $"PROPORTIONAL_{pad}", false)).ToBool(),
                    FileName = ParseStringOrDefault(modetabsF.Get(area, $"FILE_NAME_{pad}", false)).ANSItoUTF(),
                    IsDia = ParseIntOrDefault(modetabsF.Get(area, $"IS_DIA_{pad}", false)).ToBool(),
                    IsLower = ParseIntOrDefault(modetabsF.Get(area, $"IS_LOWER_{pad}", false)).ToBool(),
                    IsUpper = ParseIntOrDefault(modetabsF.Get(area, $"IS_UPPER_{pad}", false)).ToBool(),
                    IsNumber = ParseIntOrDefault(modetabsF.Get(area, $"IS_NUMBER_{pad}", false)).ToBool(),
                    IsSpecChars = ParseIntOrDefault(modetabsF.Get(area, $"IS_SPEC_CHAR_{pad}", false)).ToBool(),
                    IsSpecAssigment = ParseIntOrDefault(modetabsF.Get(area, $"IS_SPECIAL_ASSIGNMENT_{pad}", false)).ToBool()
                };

                var type = ParseStringOrDefault(modetabsF.Get(area, $"BOLD_FACE_{pad}", false)).ANSItoUTF();
                font.Type = TableFontType.Parse(type);
                if (font.Type == null)
                {
                    throw new FormatException($"Písmo tabule {font.Name} má neplatný typ (BOLD_FACE_{pad}): {type}.");
                }

                fonts.Add(font);
            }

            return fonts;
        }

        /// <summary>
        ///     Zapise mody tabuli, typy tabuli, typy obsahov a typy fontov pre tabule do suboru
        /// </summary>
        /// <param name="path">cesta do priecinka s datami</param>
        /// <param name="fonts">fonty pre tabule</param>
        /// <param name="fontdir">priecinok v ktorom sa nachadzaju binarne subory s fontami</param>
        public static void WriteModeTabs(string path, List<TableFont> fonts, string fontdir)
        {
            var fileModeTabs = CombinePath(path, FILE_MODETABS);

            var modetabsF = new TXTPropsAreasFields(fileModeTabs, true);

            const string areaMode = "VIEW_MODE";
            const string areatType = "VIEW_TYPE";
            const string areaFillSection = "FILL_SECTION";
            const string areaManufacturer = "MANUFACTURER";
            const string areaFont = "FONT";
            const string areaAlign = "ALIGN";

            modetabsF.Set("MAIN", "BREAK_CHAR", "#", WriteType.WriteStringANSI);

            var modes = TableViewMode.GetValues();
            modetabsF.Set(areaMode, "COUNT", modes.Count);
            for (var i = 0; i < modes.Count; i++)
            {
                var mode = modes[i];
                var ti = i + 1;

                modetabsF.Set(areaMode, $"KEY_{ti.PadZeros()}", mode.Key, WriteType.WriteStringANSI);
                modetabsF.Set(areaMode, $"NAME_{ti.PadZeros()}", mode.Name, WriteType.WriteStringANSI);
            }

            var views = TableViewType.GetValues();
            modetabsF.Set(areatType, "COUNT", views.Count);
            for (var i = 0; i < views.Count; i++)
            {
                var type = views[i];
                var ti = i + 1;

                modetabsF.Set(areatType, $"KEY_{ti.PadZeros()}", type.Key, WriteType.WriteStringANSI);
                modetabsF.Set(areatType, $"NAME_{ti.PadZeros()}", type.Name, WriteType.WriteStringANSI);
            }

            var sections = TableFillSection.GetValues();
            modetabsF.Set(areaFillSection, "COUNT", sections.Count);
            for (var i = 0; i < sections.Count; i++)
            {
                var fill = sections[i];
                var ti = i + 1;

                modetabsF.Set(areaFillSection, $"IDX_{ti.PadZeros()}", fill.ID);
                modetabsF.Set(areaFillSection, $"NAME_{ti.PadZeros()}", fill.Name, WriteType.WriteStringANSI);
            }

            var manufactures = TableManufacturer.GetValues();
            modetabsF.Set(areaManufacturer, "COUNT", manufactures.Count);
            for (var i = 0; i < manufactures.Count; i++)
            {
                var manufacturer = manufactures[i];
                var ti = i + 1;

                modetabsF.Set(areaManufacturer, $"KEY_{ti.PadZeros()}", manufacturer.Name, WriteType.WriteStringANSI);
                modetabsF.Set(areaManufacturer, $"NAME_{ti.PadZeros()}", manufacturer.Name, WriteType.WriteStringANSI);
            }

            modetabsF.Set(areaFont, "COUNT", fonts.Count);
            if (!string.IsNullOrEmpty(fontdir)) 
                modetabsF.Set(areaFont, "PATH", fontdir, WriteType.WriteStringANSI);
            for (var i = 0; i < fonts.Count; i++)
            {
                var font = fonts[i];
                var ti = i + 1;

                modetabsF.Set(areaFont, $"IDX_{ti.PadZeros()}", font.FontID);
                modetabsF.Set(areaFont, $"NAME_{ti.PadZeros()}", font.Name, WriteType.WriteStringANSI);
                if (!string.IsNullOrEmpty(font.FileName))
                    modetabsF.Set(areaFont, $"FILE_NAME_{ti.PadZeros()}", font.Name, WriteType.WriteStringANSI);

                if (font.Type != null) modetabsF.Set(areaFont, $"BOLD_FACE_{ti.PadZeros()}", font.Type.Key, WriteType.WriteStringANSI);

                if (font.Size != 0) modetabsF.Set(areaFont, $"SIZE_{ti.PadZeros()}", font.Size);

                if (font.Width != 0) modetabsF.Set(areaFont, $"WIDTH_{ti.PadZeros()}", font.Width);

                modetabsF.Set(areaFont, $"PROPORTIONAL_{ti.PadZeros()}", font.IsProportional.ToNumber());
                modetabsF.Set(areaFont, $"IS_DIA_{ti.PadZeros()}", font.IsDia.ToNumber());
                modetabsF.Set(areaFont, $"IS_LOWER{ti.PadZeros()}", font.IsLower.ToNumber());
                modetabsF.Set(areaFont, $"IS_UPPER_{ti.PadZeros()}", font.IsUpper.ToNumber());
                modetabsF.Set(areaFont, $"IS_NUMBER_{ti.PadZeros()}", font.IsNumber.ToNumber());
                modetabsF.Set(areaFont, $"IS_SPEC_CHAR_{ti.PadZeros()}", font.IsSpecChars.ToNumber());
                modetabsF.Set(areaFont, $"IS_SPECIAL_ASSIGNMENT_{(i + 1).PadZeros()}", font.IsSpecAssigment.ToNumber());
            }

            var aligns = TableAlign.GetValues();
            modetabsF.Set(areaAlign, "COUNT", aligns.Count);
            for (var i = 0; i < aligns.Count; i++)
            {
                var align = aligns[i];
                var ti = i + 1;

                modetabsF.Set(areaAlign, $"IDX_{ti.PadZeros()}", align.ID);
                modetabsF.Set(areaAlign, $"NAME_{ti.PadZeros()}", align.Name, WriteType.WriteStringANSI);
            }

            modetabsF.Save();
        }

        #endregion
    }
}
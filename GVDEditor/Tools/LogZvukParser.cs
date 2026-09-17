using System.Globalization;
using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Citac binarneho suboru RAWBANK\LogZvuk.usr - pouzivatelskej polovice logickej zvukovej banky,
///     v ktorej INISS uklada texty vyluk, odklonov a dodatkov zalozene obsluhou.
///     Subor je MFC CArchive bez hlavicky: int32 pocet poloziek a potom objekty SLogTextConstReport.
/// </summary>
internal static class LogZvukParser
{
    private const string CLASS_CONST_REPORT = "SLogTextConstReport";
    private const string CLASS_STATION_LIST = "SDataFace_SEZNAM_STANIC";

    private const ushort TAG_NEW_CLASS = 0xFFFF;
    private const ushort TAG_NULL = 0x0000;
    private const ushort TAG_BIG = 0x7FFF;
    private const ushort TAG_CLASS_REF = 0x8000;

    /// <summary>
    ///     Precita subor LogZvuk.usr zo zvukovej banky. Ak subor neexistuje, vrati prazdny zoznam (obsluha zatial nic nezalozila).
    ///     Pri poskodenom subore vrati polozky precitane po chybu a chybu zaloguje.
    /// </summary>
    public static List<LogZvukText> ReadLogZvukUsr(string pathToBank)
    {
        var file = Utils.CombinePath(pathToBank, FileConsts.FILE_LOGZVUK_USR)!;
        var texts = new List<LogZvukText>();

        if (!File.Exists(file))
            return texts;

        try
        {
            using var reader = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            var classes = new List<string>();

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var className = ReadObjectHeader(reader, classes);
                if (className != CLASS_CONST_REPORT)
                    throw new InvalidDataException($"Neočakávaný objekt {className ?? "(null)"} na pozícii {i}.");

                // spolocna cast SLogText
                var key = ReadCString(reader);
                var name = ReadCString(reader);
                var description = ReadCString(reader);
                var type = reader.ReadInt32();
                reader.ReadInt32(); // vzdy -1

                // vlastna cast SLogTextConstReport
                var template = ReadCString(reader);
                var stations = new List<string>();

                var dataClass = ReadObjectHeader(reader, classes);
                if (dataClass is not null)
                {
                    if (dataClass != CLASS_STATION_LIST)
                        throw new InvalidDataException($"Neznámy objekt dát {dataClass} pri položke {key}.");

                    var countStations = reader.ReadInt32();
                    for (var j = 0; j < countStations; j++)
                    {
                        var stationId = reader.ReadInt32();
                        reader.ReadInt16(); // vzdy -1
                        reader.ReadInt16(); // poradove cislo z chvile zalozenia
                        stations.Add(stationId.ToString(CultureInfo.InvariantCulture));
                    }
                }

                texts.Add(new LogZvukText(key, name, description, type, template, stations));
            }
        }
        catch (Exception e) when (e is IOException or InvalidDataException or ArgumentException)
        {
            Log.Warning($"Súbor {file} sa nepodarilo prečítať celý ({texts.Count} položiek načítaných): {e.Message}");
        }

        return texts;
    }

    /// <summary>
    ///     Precita znacku objektu MFC CArchive. Vrati meno triedy objektu alebo null, ak je objekt prazdny (00 00).
    ///     Prvy vyskyt triedy: FF FF, schema, dlzka mena, meno. Dalsi vyskyt: 2-bajtovy odkaz s najvyssim bitom.
    /// </summary>
    private static string? ReadObjectHeader(BinaryReader reader, List<string> classes)
    {
        var tag = reader.ReadUInt16();

        if (tag == TAG_NULL)
            return null;

        if (tag == TAG_BIG)
        {
            // 32-bitova znacka - pri malych suboroch sa nevyskytuje
            var bigTag = reader.ReadUInt32();
            if ((bigTag & 0x80000000) == 0)
                throw new InvalidDataException("Odkaz na už načítaný objekt nie je podporovaný.");
            return ClassByIndex(classes, (int)(bigTag & 0x7FFFFFFF));
        }

        if (tag == TAG_NEW_CLASS)
        {
            reader.ReadUInt16(); // schema
            var nameLength = reader.ReadUInt16();
            var className = Encodings.Win1250.GetString(reader.ReadBytes(nameLength));
            // MFC cisluje triedy aj objekty v jednom rade: trieda dostane index, hned za nou jej prvy objekt
            classes.Add(className);
            classes.Add(className);
            return className;
        }

        if ((tag & TAG_CLASS_REF) != 0)
        {
            var className = ClassByIndex(classes, tag & 0x7FFF);
            classes.Add(className); // dalsi objekt tejto triedy
            return className;
        }

        throw new InvalidDataException("Odkaz na už načítaný objekt nie je podporovaný.");
    }

    private static string ClassByIndex(List<string> classes, int index)
    {
        // indexy MFC su od 1
        if (index < 1 || index > classes.Count)
            throw new InvalidDataException($"Neplatný odkaz na triedu {index}.");
        return classes[index - 1];
    }

    /// <summary>
    ///     Precita MFC CString: 1 bajt dlzka; ak je FF, nasleduje 2-bajtova dlzka; ak je ta FFFF, nasleduje 4-bajtova.
    ///     Znacka FF FE FF uvadza text v UTF-16.
    /// </summary>
    private static string ReadCString(BinaryReader reader)
    {
        var unicode = false;
        var length = (int)reader.ReadByte();

        if (length == 0xFF)
        {
            var w = reader.ReadUInt16();
            if (w == 0xFEFF)
            {
                unicode = true;
                length = reader.ReadByte();
                if (length == 0xFF)
                    w = reader.ReadUInt16();
                else
                    return ReadChars(reader, length, unicode);
            }

            length = w == 0xFFFF ? reader.ReadInt32() : w;
        }

        return ReadChars(reader, length, unicode);
    }

    private static string ReadChars(BinaryReader reader, int length, bool unicode)
    {
        if (length < 0)
            throw new InvalidDataException("Záporná dĺžka textu.");

        var bytes = reader.ReadBytes(unicode ? length * 2 : length);
        return unicode ? System.Text.Encoding.Unicode.GetString(bytes) : Encodings.Win1250.GetString(bytes);
    }
}

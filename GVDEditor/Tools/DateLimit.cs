using System.Collections;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Local

namespace GVDEditor.Tools;

/// <summary>
///     Trieda pre zpracovavanie bitoveho pola do poznamky a naopak.
/// </summary>
internal class DateLimit
{
    /// <summary>
    ///     Pevne kody dni tak, ako sa zapisuju v poznamke. Poradie znakov zodpoveda poradiu bitov
    ///     v <see cref="DayType"/>, teda '1' je pondelok, 'X' pracovny den a '+' sviatok.
    /// </summary>
    private const string WeekDaySigns = "1234567";

    private const string DayTypeSigns = WeekDaySigns + "X+";

    /// <summary>Pocet poloziek <see cref="DayIndex"/> (pondelok az sviatok).</summary>
    private const int DayIndexCount = (int)DayIndex.Holiday + 1;

    /// <summary>Minimalna dlzka useku, s ktorou zacina delenie celeho grafikonu.</summary>
    private const int InitialMinRunLength = 80;

    /// <summary>Minimalna dlzka suvisleho useku na zaciatku/konci intervalu, ktory sa oddeli samostatne.</summary>
    private const int MinEdgeRunLength = 14;

    /// <summary>Minimalna dlzka suvisleho useku vnutri intervalu, okolo ktoreho sa interval rozdeli.</summary>
    private const int MinInnerRunLength = 30;

    /// <summary>Najvyssi pocet dni jedneho typu, ktore smu vybocovat z tyzdenneho vzoru.</summary>
    private const int MaxBadDays = 8;

    /// <summary>Najvyssi pocet vynimiek z tyzdenneho vzoru.</summary>
    private const int MaxExceptions = 13;

    /// <summary>Najvyssi pocet vynimiek z tyzdenneho vzoru pre obdobie dlhsie ako <see cref="LongPeriodDays"/>.</summary>
    private const int MaxExceptionsLongPeriod = 19;

    private const int LongPeriodDays = 350;

    /// <summary>Najvacsia medzera medzi dvoma useky, ktore este mozno spojit do jedneho.</summary>
    private const int MaxMergeGap = 60;

    private readonly StringBuilder _builder;
    private readonly List<ParseData> _parsedData;

    private readonly bool _allowRunsDaily;
    private readonly bool _fromToday;
    private readonly int _maxDays;
    private readonly bool _monthRoman;
    private readonly bool _skipDateRangeCheck;
    private readonly bool _specDays;

    private BitArray _bits;

    /// <summary>Dlzka znaciek {}, ktore sa nepocitaju do dlzky vyslednej poznamky.</summary>
    private int _marksLength;

    /// <summary>Naposledy vypisany mesiac - sluzi na potlacenie jeho opakovania.</summary>
    private string _lastMonth;

    private int _position;
    private string _text;

    /// <summary>
    ///     Jazyk generovaných datumových obmedzeni.
    /// </summary>
    public enum Locale
    {
        Cz,
        Sk
    }

    /// <summary>
    ///     Kluce k textom
    /// </summary>
    public enum Message
    {
        Error,
        Empty,

        RunsDaily,
        RunsNever,
        RunsNeverAlt,
        RunsNext,
        RunsNot,
        RunsNotAlt,
        Runs,
        RunsAlt,

        From,
        To,
        And,
        On,

        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,

        Workday,
        Holiday,

        Jan,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }

    private static readonly string[] MessagesCz =
    {
        // Error, Empty
        "chyba", "",
        // RunsDaily .. RunsAlt
        "jede denně", "t.č. nejede", "nikdy", "zatím nejede", "nejede ", "kromě ", "jede ", "včetně ",
        // From, To, And, On
        "od ", "do ", " a ", "v ",
        // Monday .. Holiday
        "1", "2", "3", "4", "5", "6", "7", "X", "+",
        // Jan .. Dec
        "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII"
    };

    private static readonly string[] MessagesSk =
    {
        // Error, Empty
        "chyba", "",
        // RunsDaily .. RunsAlt
        "ide denne", "t.č. nejde", "nikdy", "zatiaľ nejde", "nejde ", "okrem ", "ide ", "vrátane ",
        // From, To, And, On
        "od ", "do ", " a ", "v ",
        // Monday .. Holiday
        "1", "2", "3", "4", "5", "6", "7", "X", "+",
        // Jan .. Dec
        "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII"
    };

    /// <summary>
    ///     Vzory, ktorymi sa pri parsovani rozpoznavaju sprAvy aj v skratenom alebo inojazycnom tvare.
    ///     Spravy, ktore v zozname nie su, sa porovnavaju len na presnu zhodu.
    /// </summary>
    private static readonly Dictionary<Message, string> MessagePatterns = new()
    {
        { Message.RunsDaily, "^jede denně|^ide denne" },
        { Message.RunsNever, @"^t\.č\. ne" },
        { Message.RunsNext, "^zat" },
        { Message.RunsNot, "^n" },
        { Message.RunsNotAlt, "^kr|^ok" },
        { Message.Runs, "^j|^i" },
        { Message.RunsAlt, "^vč|^vr" },
        { Message.From, "^o" },
        { Message.To, "^d" }
    };

    /// <summary>Statne sviatky s pevnym datumom v tvare MMDD.</summary>
    private static readonly int[] FixedHolidaysCz =
    {
        101, 501, 508, 705, 706, 928, 1028, 1117, 1224, 1225, 1226
    };

    /// <summary>Statne sviatky s pevnym datumom v tvare MMDD.</summary>
    private static readonly int[] FixedHolidaysSk =
    {
        101, 106, 501, 508, 705, 829, 901, 915, 1101, 1117, 1224, 1225, 1226
    };

    /// <summary>
    ///     Vytvori novu instanciu triedy <see cref="DateLimit"/>.
    /// </summary>
    /// <param name="from">Pociatocny datum platnosti GVD.</param>
    /// <param name="to">Koncovy datum platnosti GVD.</param>
    /// <param name="specDays">Pouzivat sviatky a pracovne dni.</param>
    /// <param name="allowRunsDaily">Vracat tiez - ide denne.</param>
    /// <param name="fromToday">Zohladnit az vzhladom ku dnesku.</param>
    /// <param name="insertMarks">Vkladat znacky {}.</param>
    /// <param name="maxDays">Pocet dni do buducnosti (maximalny poradovy index dna).</param>
    /// <param name="monthRoman">Cisla mesiacov rimskymi cislicami.</param>
    /// <param name="skipDateRangeCheck">Preskakovat u datumu chyby pre datum mimo grafikonu.</param>
    /// <param name="altForm">Pouzit zkrateny tvar poznamky.</param>
    /// <param name="today">Ktory datum pouzit ako dnesok (ak sa neuvedie, pouzije sa skutocny dnesok).</param>
    public DateLimit(DateTime from, DateTime to,
        bool specDays = true, bool allowRunsDaily = false,
        bool fromToday = false, bool insertMarks = true,
        int maxDays = 0, bool monthRoman = true,
        bool skipDateRangeCheck = false, bool altForm = false,
        DateTime? today = null)
    {
        if (to < from)
            throw new ArgumentException($"Dátum do {to} je menší ako dátum od {from}.");

        DateFrom = from;
        DateTo = to;
        AltForm = altForm;
        Today = today ?? DateTime.Today;

        _builder = new StringBuilder();
        _parsedData = new List<ParseData>();

        _specDays = specDays;
        _allowRunsDaily = allowRunsDaily;
        _fromToday = fromToday;
        InsertMarks = insertMarks;
        _maxDays = maxDays;
        _monthRoman = monthRoman;
        _skipDateRangeCheck = skipDateRangeCheck;
    }

    /// <summary>
    ///     Jazyk generovanych poznamok.
    /// </summary>
    public static Locale Loc { get; set; } = Locale.Sk;

    /// <summary>
    ///     Urcuje, ci sa nazvy typov dni obalia znackami {}.
    /// </summary>
    public bool InsertMarks { get; set; }

    /// <summary>
    ///     Vrati celkovy pocet dni grafikonu.
    /// </summary>
    public int TotalDays => MaxDay + 1;

    /// <summary>
    ///     Vrati maximalny poradovy index dna.
    /// </summary>
    public int MaxDay => DateDiff(DateFrom, DateTo);

    /// <summary>
    ///     Vrati pociatocny datum.
    /// </summary>
    public DateTime DateFrom { get; private set; }

    /// <summary>
    ///     Vrati koncovy datum.
    /// </summary>
    public DateTime DateTo { get; private set; }

    /// <summary>
    ///     Obdobie platnosti ako text.
    /// </summary>
    public string TextFromTo => $"{FormatDate(DateFrom)} - {FormatDate(DateTo)}";

    /// <summary>
    ///     Vrati priznak alternativneho tvaru textu.
    /// </summary>
    public bool AltForm { get; }

    /// <summary>
    ///     Vrati datum pouzity ako "dnes".
    /// </summary>
    public DateTime Today { get; }

    /// <summary>
    ///     Vytvori bitove pole pre priznaky ide/nejde.
    /// </summary>
    public BitArray CreateBitArray() => new(TotalDays);

    /// <summary>
    ///     Formatuje datum do formatu "dd:MM:yyyy".
    /// </summary>
    public static string FormatDate(DateTime date) => date.ToString("dd.MM.yyyy");

    /// <summary>
    ///     Vrati vzdialenost medzi datumami ako <see cref="int"/>.
    /// </summary>
    public static int DateDiff(DateTime from, DateTime to) => (int)Math.Round((to - from).TotalDays);

    /// <summary>
    ///     Konvertuje bitove pole do textovej poznamky.
    /// </summary>
    /// <param name="bits">Bitove pole.</param>
    /// <param name="cycle">Posunutie vzhladom k bitovemu polu.</param>
    /// <param name="validBits">Dni, ktore ma zmysel v poznamke uvadzat.</param>
    /// <returns>
    ///     Text poznamky.
    /// </returns>
    public string BitArrayToText(BitArray bits, int cycle = 0, BitArray validBits = null)
    {
        if (bits == null || bits.Length != TotalDays)
            throw new ArgumentException(@"Bitové pole na vstupe chýba alebo neodpovedá jeho dĺžka.", nameof(bits));

        var originalFrom = DateFrom;
        var originalTo = DateTo;

        try
        {
            DateFrom = DateFrom.AddDays(cycle);
            DateTo = DateTo.AddDays(cycle);

            var minIndex = 0;
            var maxIndex = MaxDay;

            if (_maxDays > 0 && DateTo > Today.AddDays(_maxDays))
            {
                DateTo = Today.AddDays(_maxDays);

                if (DateTo < DateFrom)
                    return MsgText(Message.RunsNext);

                maxIndex = DateDiff(DateFrom, DateTo);
            }

            if (_fromToday)
            {
                if (Today > DateTo)
                    return AltMsgText(Message.RunsNever, Message.RunsNeverAlt);

                if (Today > DateFrom)
                {
                    minIndex = DateDiff(DateFrom, Today);
                    DateFrom = Today;
                }
            }

            if (!HasMixedBits(bits, minIndex, maxIndex))
                return bits[minIndex]
                    ? MsgText(_allowRunsDaily ? Message.RunsDaily : Message.Empty)
                    : AltMsgText(Message.RunsNever, Message.RunsNeverAlt);

            if (minIndex > 0 || maxIndex != bits.Length - 1)
            {
                if (validBits != null && validBits.Count == bits.Count)
                    validBits = Slice(validBits, minIndex, maxIndex);

                bits = Slice(bits, minIndex, maxIndex);
            }

            _bits = bits;

            var positive = FormatBits(false, out var positiveCount, out var positiveLength, validBits);
            var negative = FormatBits(true, out var negativeCount, out var negativeLength, validBits);

            // negativny tvar sa uprednostni len ak je vyrazne kratsi, alebo kratsi a zaroven jednoduchsi
            var negativeIsBetter =
                negativeLength + (positiveLength > 40 ? 20 : 25) < positiveLength ||
                negativeLength < positiveLength && negativeCount < positiveCount;

            return negativeIsBetter ? negative : positive;
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            DateFrom = originalFrom;
            DateTo = originalTo;
            _bits = null;
        }

        return "";
    }

    /// <summary>
    ///     Konvertuje textovu poznamku do bitoveho pole.
    /// </summary>
    /// <param name="text">Text poznamky.</param>
    /// <returns>Text poznamky.</returns>
    public BitArray TextToBitArray(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text) || TokenIsMsg(text.Trim(), Message.RunsDaily, Message.Runs, Message.RunsAlt))
                return new BitArray(TotalDays, true);

            if (TokenIsMsg(text.Trim(), Message.RunsNever, Message.RunsNeverAlt, Message.RunsNot, Message.RunsNotAlt))
                return CreateBitArray();

            _bits = CreateBitArray();
            _text = text;
            _position = 0;
            _parsedData.Clear();

            ParseText();

            return _bits;
        }
        catch (ParseException)
        {
            throw;
        }
        catch (Exception)
        {
            return new BitArray(0);
        }
    }

    /// <summary>
    ///     Vrati, ci sa datumove obmedzenia ako texty prekryvaju.
    /// </summary>
    /// <param name="dl1">Text datumoveho obmedenia.</param>
    /// <param name="dl2">Text datumoveho obmedenia.</param>
    public bool Overlap(string dl1, string dl2)
    {
        var limit = TextAnd(dl1, dl2);
        return limit != "" && limit != MsgText(Message.RunsNever) && limit != MsgText(Message.RunsNeverAlt);
    }

    /// <summary>
    ///     Logicka operacia AND medzi textami.
    /// </summary>
    public string TextAnd(params string[] texts) => Combine((first, second) => first.And(second), texts);

    /// <summary>
    ///     Logicka operacia OR medzi textami.
    /// </summary>
    public string TextOr(params string[] texts) => Combine((first, second) => first.Or(second), texts);

    /// <summary>
    ///     Logicka operacia XOR medzi textami.
    /// </summary>
    public string TextXor(params string[] texts) => Combine((first, second) => first.Xor(second), texts);

    /// <summary>
    ///     Logicka operacia NOT podla textu.
    /// </summary>
    public string TextNot(string text) => BitArrayToText(TextToBitArray(text).Not());

    /// <summary>
    ///     Vrati text správy.
    /// </summary>
    public static string MsgText(Message message)
    {
        return Loc switch
        {
            Locale.Cz => MessagesCz[(int)message],
            Locale.Sk => MessagesSk[(int)message],
            _ => "?"
        };
    }

    /// <summary>
    ///     Text správy so zohlednenim alternativnej formulacie.
    /// </summary>
    public string AltMsgText(Message msg1, Message msg2) => MsgText(AltForm ? msg2 : msg1);

    /// <summary>
    ///     Vrati, ci je zadany datum sviatok alebo nedela.
    /// </summary>
    public static bool IsHoliday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        var fixedHolidays = Loc switch
        {
            Locale.Cz => FixedHolidaysCz,
            Locale.Sk => FixedHolidaysSk,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (Array.IndexOf(fixedHolidays, date.Month * 100 + date.Day) >= 0)
            return true;

        // pohyblive sviatky (Velky piatok a Velkonocny pondelok) mozu pripadnut len na marec alebo april
        if (date.Month is not (3 or 4))
            return false;

        var easterMonday = GetEasterMonday(date.Year);

        return date.DayOfWeek switch
        {
            DayOfWeek.Friday => date.AddDays(3).Date == easterMonday,
            DayOfWeek.Monday => date.Date == easterMonday,
            _ => false
        };
    }

    /// <summary>
    ///     Vykona logicku operaciu <paramref name="operation"/> nad bitovymi polami vsetkych textov
    ///     a vysledok prevedie spat na text.
    /// </summary>
    private string Combine(Func<BitArray, BitArray, BitArray> operation, IReadOnlyList<string> texts)
    {
        switch (texts.Count)
        {
            case 0:
                return "";
            case 1:
                return texts[0];
        }

        var bits = TextToBitArray(texts[0]);

        for (var i = 1; i < texts.Count; i++)
            bits = operation(bits, TextToBitArray(texts[i]));

        return BitArrayToText(bits);
    }

    /// <summary>
    ///     Vrati, ci sa v rozsahu <paramref name="from"/>-<paramref name="to"/> vyskytuju obe hodnoty bitov.
    /// </summary>
    private static bool HasMixedBits(BitArray bits, int from, int to)
    {
        for (var i = from + 1; i <= to; i++)
            if (bits[i] != bits[from])
                return true;

        return false;
    }

    /// <summary>
    ///     Vrati novu kopiu bitoveho pola orezanu na rozsah <paramref name="from"/>-<paramref name="to"/>.
    /// </summary>
    private static BitArray Slice(BitArray bits, int from, int to)
    {
        var result = new BitArray(to - from + 1);

        for (var i = from; i <= to; i++)
            result[i - from] = bits[i];

        return result;
    }

    /// <summary>
    ///     Odstrani z obmedzenia jednotlive dni, ktore nepatria medzi platne dni.
    /// </summary>
    private static void ReduceDates(IList<DateLimitInfo> limits, BitArray validBits)
    {
        for (var i = limits.Count - 1; i >= 0; i--)
        {
            var info = limits[i];

            if (info.ListRuns is { Count: > 0 })
                ReduceDates(info.ListRuns, validBits);

            if (info.ListRunsNot is { Count: > 0 })
                ReduceDates(info.ListRunsNot, validBits);

            if (info.Type == DayType.None && info.From > 0 && info.From == info.To && !validBits[info.From])
                limits.RemoveAt(i);
        }
    }

    /// <summary>
    ///     Vytvori text pre aktualne bitove pole <see cref="_bits"/>.
    /// </summary>
    /// <param name="isNot">Spracovat negovane bitove pole, teda vytvorit zapis v tvare "nejde ...".</param>
    /// <param name="infosCount">Pocet useku vysledneho obmedzenia - mensi pocet znamena jednoduchsi zapis.</param>
    /// <param name="length">Dlzka vysledneho textu bez znaciek {}.</param>
    /// <param name="validBits">Dni, ktore ma zmysel v poznamke uvadzat.</param>
    private string FormatBits(bool isNot, out int infosCount, out int length, BitArray validBits)
    {
        infosCount = 0;
        length = 0;

        if (isNot)
            _bits = _bits.Not();

        try
        {
            var limits = ProcessInterval(InitialMinRunLength, 0, MaxDay);

            if (limits == null)
                return null;

            if (validBits != null && validBits.Length == _bits.Length && limits.Count > 0)
                ReduceDates(limits, validBits);

            infosCount = limits.Count;
            _marksLength = 0;

            var formatted = Format(limits, isNot);

            if (string.IsNullOrEmpty(formatted))
                return formatted;

            if (AltForm)
            {
                // v skratenom tvare sa uvodne "ide" vypusta a pred zoznamom dni sa neopakuje predlozka
                if (formatted.StartsWith(MsgText(Message.RunsAlt)))
                    formatted = formatted.Substring(MsgText(Message.RunsAlt).Length);

                formatted = formatted.Replace(MsgText(Message.RunsAlt) + MsgText(Message.On), MsgText(Message.RunsAlt));
                formatted = formatted.Replace(MsgText(Message.RunsNotAlt) + MsgText(Message.On), MsgText(Message.RunsNotAlt));
            }

            length = formatted.Length - _marksLength;

            return formatted;
        }
        finally
        {
            if (isNot)
                _bits = _bits.Not();
        }
    }

    /// <summary>
    ///     Rozlozi interval <paramref name="from"/>-<paramref name="to"/> na useky datumoveho obmedzenia.
    /// </summary>
    /// <param name="minCount">Minimalna dlzka useku, ktory sa este oplati oddelit.</param>
    /// <param name="from">Zaciatok intervalu.</param>
    /// <param name="to">Koniec intervalu.</param>
    private List<DateLimitInfo> ProcessInterval(int minCount, int from, int to)
    {
        ReduceInterval(ref from, ref to);

        var limits = GetSingleDays(from, to);

        if (limits != null)
            return limits;

        var okCount = new DayCounter();
        var badCount = new DayCounter();
        var grouping = DayGrouping.None;

        // interval sa oplati skusit popisat tyzdennym vzorom len ak dni v tyzdni nie su vsetky rovnake
        var weekPattern = ScanDays(from, to, okCount, badCount, ref grouping) &&
                          !AllSet(okCount) && !badCount.AllZero && to - from > 6;

        if (!weekPattern)
        {
            limits = GetIntervals(from, to);

            if (limits != null)
                return limits;

            limits = SplitInterval(minCount, from, to);

            if (limits != null)
                return limits;
        }

        while (limits == null)
        {
            limits = ScanWeekDays(minCount, from, to);
            minCount -= Math.Max(minCount >> 1, 1);

            if (!weekPattern)
            {
                // pozn.: pripadny vysledok zo ScanWeekDays sa tu zahadzuje - povodne spravanie
                limits = SplitInterval(minCount, from, to);

                if (limits != null)
                    return limits;
            }
        }

        return limits;
    }

    /// <summary>
    ///     Postupne skusi vsetky sposoby rozdelenia intervalu na kratsie useky.
    /// </summary>
    private List<DateLimitInfo> SplitInterval(int minCount, int from, int to) =>
        SplitAtRunBlocks(minCount, from, to) ??
        SplitAtLongRun(minCount, from, to) ??
        SplitLeadingRun(minCount, from, to) ??
        SplitTrailingRun(minCount, from, to);

    /// <summary>
    ///     Rozdeli interval na useky ohranicene dnami, kedy vlak nejde.
    /// </summary>
    /// <remarks>
    ///     Povodny kod tu porovnaval dlzku jedineho dna s <paramref name="minCount"/>, takze usek
    ///     vznikne az vtedy, ked <paramref name="minCount"/> klesne na 1 alebo nizsie.
    /// </remarks>
    private List<DateLimitInfo> SplitAtRunBlocks(int minCount, int from, int to)
    {
        List<DateLimitInfo> limits = null;
        var blockFrom = -1;

        for (var day = from; day <= to; day++)
            if (Runs(day))
            {
                if (blockFrom < 0)
                    blockFrom = day;
            }
            else if (minCount <= 1 && blockFrom >= 0)
            {
                AddIntervals(ref limits, ProcessInterval(minCount, blockFrom, 0));
                blockFrom = -1;
            }

        if (blockFrom > from && limits != null)
            AddIntervals(ref limits, ProcessInterval(minCount, blockFrom, to));

        return limits;
    }

    /// <summary>
    ///     Najde vnutri intervalu dostatocne dlhy suvisly usek a rozdeli interval na cast pred nim,
    ///     samotny usek a cast za nim.
    /// </summary>
    private List<DateLimitInfo> SplitAtLongRun(int minCount, int from, int to)
    {
        List<DateLimitInfo> limits = null;
        var runFrom = -1;
        var runLength = 0;

        for (var day = from; day <= to; day++)
        {
            if (Runs(day))
            {
                runLength++;

                if (runFrom < 0)
                    runFrom = day;

                continue;
            }

            if (runLength >= MinInnerRunLength)
            {
                if (runFrom > from)
                    limits = ProcessInterval(minCount, from, runFrom - 1);

                AddInterval(ref limits, new DateLimitInfo(runFrom, day - 1));
                AddIntervals(ref limits, ProcessInterval(minCount, day, to));

                return limits;
            }

            runLength = 0;
            runFrom = -1;
        }

        return null;
    }

    /// <summary>
    ///     Oddeli dostatocne dlhy suvisly usek na zaciatku intervalu.
    /// </summary>
    private List<DateLimitInfo> SplitLeadingRun(int minCount, int from, int to)
    {
        var day = from;
        var runLength = 0;

        while (day <= to && Runs(day))
        {
            runLength++;
            day++;
        }

        if (runLength < MinEdgeRunLength)
            return null;

        List<DateLimitInfo> limits = null;

        AddInterval(ref limits, new DateLimitInfo(from, from + runLength - 1));
        AddIntervals(ref limits, ProcessInterval(minCount, from + runLength, to));

        return limits;
    }

    /// <summary>
    ///     Oddeli dostatocne dlhy suvisly usek na konci intervalu.
    /// </summary>
    private List<DateLimitInfo> SplitTrailingRun(int minCount, int from, int to)
    {
        var day = to;
        var runLength = 0;

        while (day >= from && Runs(day))
        {
            runLength++;
            day--;
        }

        if (runLength < MinEdgeRunLength)
            return null;

        var limits = ProcessInterval(minCount, from, to - runLength);
        AddInterval(ref limits, new DateLimitInfo(to - runLength + 1, to));

        return limits;
    }

    /// <summary>
    ///     Hlada najdlhsi usek, ktory sa da popisat tyzdennym vzorom (napr. "ide 1-5") spolu so zoznamom
    ///     vynimiek z neho. Zvysok intervalu spracuje rekurzivne.
    /// </summary>
    private List<DateLimitInfo> ScanWeekDays(int minCount, int from, int to)
    {
        List<DateLimitInfo> limits = null;
        var okCount = new DayCounter();
        var badCount = new DayCounter();

        for (var lastDay = to; lastDay >= from + 7; lastDay--)
        {
            if (lastDay < to && RunsNot(lastDay))
                continue;

            var grouping = DayGrouping.None;
            ScanDays(from, lastDay, okCount, badCount, ref grouping);

            // do vzoru sa dostanu len tie typy dni, ktore v useku vyrazne prevazuju
            var hasPattern = false;

            for (var index = DayIndex.Monday; index <= DayIndex.Holiday; index++)
                if (okCount[index] > badCount[index] * 2 && badCount[index] <= MaxBadDays)
                {
                    okCount[index] = 1;
                    hasPattern = true;
                }
                else
                    okCount[index] = 0;

            if (hasPattern)
            {
                // useky, ktore idu nad ramec vzoru
                var extraRuns = 0;

                for (var day = from; day <= lastDay; day++)
                    if (Runs(day) && okCount[GetDayIndex(day, grouping)] == 0 &&
                        (day == from || RunsNot(day - 1) || okCount[GetDayIndex(day - 1, grouping)] != 0))
                        extraRuns++;

                // useky dni vzoru, v ktorych vlak nejde
                var missingRuns = 0;
                var scan = from;

                while (scan <= lastDay)
                    if (okCount[GetDayIndex(scan, grouping)] != 0 && RunsNot(scan))
                    {
                        missingRuns++;

                        while (scan <= lastDay && RunsNot(scan))
                            scan++;
                    }
                    else
                        scan++;

                var exceptions = extraRuns + missingRuns;

                if (exceptions <= MaxExceptions ||
                    lastDay - from > LongPeriodDays && exceptions <= MaxExceptionsLongPeriod)
                {
                    if (missingRuns > 0)
                    {
                        // usek nesmie koncit dnom vzoru, v ktorom vlak nejde
                        var day = lastDay;

                        while (okCount[GetDayIndex(day, grouping)] == 0)
                            day--;

                        if (RunsNot(day))
                            continue;
                    }

                    // ak sa vzor opakuje aj v nasledujucich tyzdnoch, usek este nekonci tu
                    if (exceptions > 0 && lastDay < to - 21 && lastDay > 13 &&
                        (EqualPattern(lastDay - 6, lastDay + 1) && EqualPattern(lastDay - 6, lastDay + 8) &&
                         EqualPattern(lastDay - 6, lastDay + 15) ||
                         EqualPattern(lastDay - 13, lastDay + 1) && EqualPattern(lastDay - 13, lastDay + 8) &&
                         EqualPattern(lastDay - 13, lastDay + 15)))
                        continue;

                    if (exceptions > 3 && lastDay - from > 35)
                    {
                        var startOkCount = new DayCounter();
                        var startBadCount = new DayCounter();
                        var startGrouping = DayGrouping.None;

                        // ak ma zaciatok intervalu iny vzor, spracuje sa samostatne
                        if (ScanDays(from, from + 20, startOkCount, startBadCount, ref startGrouping) &&
                            GetDayType(okCount) != GetDayType(startOkCount))
                        {
                            var day = from + 20;

                            while (day <= lastDay - 1 && ScanDays(from, day, startOkCount, startBadCount, ref startGrouping))
                                day++;

                            lastDay = day - 1;
                            AddIntervals(ref limits, ProcessInterval(minCount, from, lastDay));

                            if (limits != null)
                            {
                                if (lastDay < to)
                                    AddIntervals(ref limits, ProcessInterval(minCount, lastDay + 1, to));

                                break;
                            }
                        }
                    }

                    var dayFrom = GetBetterDayFrom(from, okCount, grouping);
                    var dayTo = GetBetterDayTo(lastDay, okCount, grouping);

                    if (dayFrom > 0 && okCount[GetDayIndex(dayFrom, grouping)] == 0)
                    {
                        // usek zacina mimo vzoru - jeho zaciatok sa oddeli
                        var start = dayFrom;

                        while (Runs(dayFrom))
                            dayFrom++;

                        if (dayFrom > start)
                        {
                            AddIntervals(ref limits, ProcessInterval(minCount, start, dayFrom - 1));

                            while (RunsNot(dayFrom))
                                dayFrom++;

                            from = dayFrom;
                        }
                    }

                    var limit = new DateLimitInfo(dayFrom, dayTo);

                    if (dayFrom != dayTo)
                        limit.Type = CheckDayType(dayFrom, dayTo, GetDayType(okCount));

                    AddInterval(ref limits, limit);

                    if (extraRuns > 0)
                        AddExtraRuns(limits[limits.Count - 1], from, lastDay, okCount, grouping);

                    if (missingRuns > 0)
                        AddMissingRuns(limits[limits.Count - 1], from, lastDay, dayFrom, dayTo, okCount, grouping);
                }
            }

            if (limits != null)
            {
                if (lastDay < to)
                    AddIntervals(ref limits, ProcessInterval(minCount, lastDay + 1, to));

                break;
            }
        }

        return limits;
    }

    /// <summary>
    ///     Doplni do obmedzenia useky, v ktorych vlak ide nad ramec tyzdenneho vzoru.
    /// </summary>
    private void AddExtraRuns(DateLimitInfo limit, int from, int lastDay, DayCounter okCount, DayGrouping grouping)
    {
        var day = from;

        while (day <= lastDay)
            if (okCount[GetDayIndex(day, grouping)] == 0 && Runs(day))
            {
                var blockFrom = day;

                while (day <= lastDay && Runs(day))
                    day++;

                // dni, ktore uz pokryva vzor, sa na konci useku neuvadzaju
                var blockTo = day - 1;

                while (okCount[GetDayIndex(blockTo, grouping)] != 0)
                    blockTo--;

                AddPeriod(limit.ListRuns, blockFrom, blockTo);
            }
            else
                day++;
    }

    /// <summary>
    ///     Doplni do obmedzenia useky dni tyzdenneho vzoru, v ktorych vlak nejde.
    /// </summary>
    private void AddMissingRuns(DateLimitInfo limit, int from, int lastDay, int dayFrom, int dayTo,
        DayCounter okCount, DayGrouping grouping)
    {
        var day = from;

        while (day <= lastDay)
            if (okCount[GetDayIndex(day, grouping)] != 0 && RunsNot(day))
            {
                var gapFrom = day;
                var gapTo = day;
                var patternDayCount = 0;
                var patternDays = new int[2];

                while (day <= lastDay && RunsNot(day))
                {
                    if (okCount[GetDayIndex(day, grouping)] != 0)
                    {
                        gapTo = day;

                        if (patternDayCount < patternDays.Length)
                            patternDays[patternDayCount] = gapTo;

                        patternDayCount++;
                    }

                    day++;
                }

                if (patternDayCount < 3)
                {
                    // jeden alebo dva dni sa vypisu ako samostatne datumy
                    for (var i = 0; i < patternDayCount; i++)
                        AddDay(limit.ListRunsNot, patternDays[i]);
                }
                else
                {
                    // tri a viac dni sa zapisu ako obdobie roztiahnute na cele okolie bez jazdy
                    while (gapFrom >= dayFrom && RunsNot(gapFrom))
                        gapFrom--;

                    gapFrom++;

                    while (gapTo <= dayTo && RunsNot(gapTo))
                        gapTo++;

                    gapTo--;

                    AddPeriod(limit.ListRunsNot, gapFrom, gapTo);

                    if (gapTo > day)
                        day = gapTo;
                }
            }
            else
                day++;
    }

    /// <summary>
    ///     Pokusi sa zredukovat interval <paramref name="from"/>-<paramref name="to"/>.
    /// </summary>
    /// <param name="from">zaciatok intervalu</param>
    /// <param name="to">koniec intervalu</param>
    private void ReduceInterval(ref int from, ref int to)
    {
        while (from < to && RunsNot(from))
            from++;

        while (from < to && RunsNot(to))
            to--;
    }

    /// <summary>
    ///     Vrati jednotlive intervaly datumoveho obmedzenia.
    /// </summary>
    /// <param name="from">zaciatok intervalu</param>
    /// <param name="to">koniec intervalu</param>
    private List<DateLimitInfo> GetIntervals(int from, int to)
    {
        var runEnds = 0;
        var runStarts = 0;

        for (var day = from; day < to; day++)
            if (Runs(day) && RunsNot(day + 1))
                runEnds++;
            else if (Runs(day + 1) && RunsNot(day))
                runStarts++;

        if (Runs(to))
            runEnds++;
        else
            runStarts++;

        // vypis obdobi ma zmysel len ak je useku, kedy vlak ide, malo
        if (!(runEnds <= 2 && runEnds <= runStarts || runEnds == 1 && runStarts == 0))
            return null;

        var info = new DateLimitInfo();
        var blockFrom = -1;

        for (var day = from; day <= to; day++)
            if (Runs(day))
            {
                if (blockFrom < 0)
                    blockFrom = day;
            }
            else if (blockFrom >= 0)
            {
                AddPeriod(info.ListRuns, blockFrom, day - 1);
                blockFrom = -1;
            }

        if (blockFrom >= 0)
            AddPeriod(info.ListRuns, blockFrom, to);

        return [info];
    }

    /// <summary>
    ///     Vrati obmedzenie zapisane tyzdennym vzorom alebo vypisom jednotlivych dni, ak je takyto
    ///     zapis mozny. Inak vrati <see langword="null"/>.
    /// </summary>
    private List<DateLimitInfo> GetSingleDays(int from, int to)
    {
        var runDays = 0;

        for (var day = from; day <= to; day++)
            if (Runs(day))
                runDays++;

        if (runDays == 0)
            return null;

        var okCount = new DayCounter();
        var badCount = new DayCounter();
        var grouping = DayGrouping.None;

        var weekPattern = ScanDays(from, to, okCount, badCount, ref grouping);
        var totalDays = to - from + 1;

        // bez tyzdenneho vzoru sa jednotlive dni vypisu len ak ich je malo
        if (!weekPattern && !(runDays <= 6 && (runDays == totalDays || runDays <= totalDays + 1 - runDays)))
            return null;

        var info = new DateLimitInfo();

        if (weekPattern && to - from > 8)
        {
            info.From = GetBetterDayFrom(from, okCount, grouping);
            info.To = GetBetterDayTo(to, okCount, grouping);
            info.Type = GetDayType(okCount);
        }
        else
        {
            for (var day = from; day <= to; day++)
                if (Runs(day))
                    AddDay(info.ListRuns, day);
        }

        return [info];
    }

    /// <summary>
    ///     Prida prvky (intervaly) zoznamu <paramref name="appendIntervals"/> do zoznamu <paramref name="baseIntervals"/>.<br></br>
    ///     Ak <paramref name="baseIntervals"/> je <see langword="null"/>, priradi referenciu <paramref name="appendIntervals"/> do <paramref name="baseIntervals"/>.
    /// </summary>
    /// <param name="baseIntervals">zakladny zoznam</param>
    /// <param name="appendIntervals">zoznam na priradenie do zakladneho zoznamu</param>
    private static void AddIntervals(ref List<DateLimitInfo> baseIntervals, List<DateLimitInfo> appendIntervals)
    {
        if (baseIntervals == null)
        {
            baseIntervals = appendIntervals;
            return;
        }

        baseIntervals.AddRange(appendIntervals);
    }

    /// <summary>
    ///     Prida prvok <paramref name="interval"/> do zoznamu <paramref name="baseIntervals"/>.<br></br>
    ///     Ak <paramref name="baseIntervals"/> je <see langword="null"/>, vytvori sa nova instancia triedy <see cref="List{DateLimitInfo}"/>.
    /// </summary>
    /// <param name="baseIntervals">zakladny zoznam</param>
    /// <param name="interval">interval na pridanie</param>
    private static void AddInterval(ref List<DateLimitInfo> baseIntervals, DateLimitInfo interval)
    {
        baseIntervals ??= new List<DateLimitInfo>();
        baseIntervals.Add(interval);
    }

    /// <summary>
    ///     Prida jeden den ako periodu s rovnakym zaciatkom aj koncom.
    /// </summary>
    private static void AddDay(List<DateLimitInfo> runs, int day) => AddPeriod(runs, day, day);

    /// <summary>
    ///     Prida periodu. Ak nadvazuje na poslednu periodu v zozname, obe sa spoja do jednej.
    /// </summary>
    /// <param name="runs">intervaly</param>
    /// <param name="from">ide od</param>
    /// <param name="to">ide do</param>
    private static void AddPeriod(List<DateLimitInfo> runs, int from, int to)
    {
        if (from > 0 && runs.Count > 0 && runs[runs.Count - 1].To == from - 1)
        {
            runs[runs.Count - 1].To = to;
            return;
        }

        runs.Add(new DateLimitInfo(from, to));
    }

    /// <summary>
    ///     Vrati, ci v zadany den vlak IDE.
    /// </summary>
    /// <param name="day">Den na posudenie.</param>
    private bool Runs(int day) => _bits[day];

    /// <summary>
    ///     Vrati, ci v zadany den vlak NEJDE.
    /// </summary>
    /// <param name="day">Den na posudenie.</param>
    private bool RunsNot(int day) => !Runs(day);

    /// <summary>
    ///     Prida zadany den do <see cref="StringBuilder"/>a, ktory pred pridanim sformatuje.
    /// </summary>
    /// <param name="day">Den, ktory sa ma pridat na koniec buildera.</param>
    private void AppendDay(int day)
    {
        AppendComma();
        _builder.Append(FormatDay(day));
    }

    /// <summary>
    ///     Sformatuje zadany den. Ak ma rovnaky mesiac ako naposledy vypisany den, mesiac sa
    ///     z predchadzajuceho datumu v builderi odstrani (zapise sa teda len raz, napr. "1.,5.I.").
    /// </summary>
    /// <param name="day">Den, ktory sa ma sformatovat.</param>
    /// <returns>sformatovany den ako retazec.</returns>
    private string FormatDay(int day)
    {
        var date = DateFrom.AddDays(day);
        var month = MsgMonth(date.Month) + ".";

        if (!DateUnique(date))
            month += date.Year;

        if (!string.IsNullOrEmpty(_lastMonth) && _lastMonth == month)
        {
            var index = _builder.ToString().LastIndexOf(_lastMonth, StringComparison.Ordinal);
            _builder.Remove(index, _lastMonth.Length);
        }
        else
            _lastMonth = month;

        return $"{date.Day}.{_lastMonth}";
    }

    /// <summary>
    ///     Prida znak ciarky (,) na koniec <see cref="StringBuilder"/>a.
    /// </summary>
    private void AppendComma()
    {
        var last = _builder.Length - 1;

        if (last > 0 && _builder[last] != ' ' && _builder[last] != ',')
            _builder.Append(',');
    }

    /// <summary>
    ///     Prida znak medzery ( ) na koniec <see cref="StringBuilder"/>a.
    /// </summary>
    private void AppendSpace()
    {
        var last = _builder.Length - 1;

        if (last > 0 && _builder[last] > ' ')
            _builder.Append(' ');
    }

    /// <summary>
    ///     Spocita, kolkokrat vlak v intervale ide a nejde v jednotlivych typoch dni. Ak prevazuje
    ///     jazda podla pracovnych dni a sviatkov, prepne <paramref name="grouping"/> na toto zlucenie
    ///     a pocitadla dni v tyzdni vynuluje.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/>, ak sa jazda da uplne popisat typmi dni, teda ziadny typ dna nie je
    ///     zaroven jazdny aj nejazdny.
    /// </returns>
    private bool ScanDays(int from, int to, DayCounter okCount, DayCounter badCount, ref DayGrouping grouping)
    {
        okCount.Clear();
        badCount.Clear();

        var dayIndex = GetDayIndex(from, DayGrouping.None);
        var saturdays = 0;
        var totalBad = 0;

        for (var day = from; day <= to; day++)
        {
            var counter = Runs(day) ? okCount : badCount;

            if (_specDays)
            {
                var type = GetDayType(day);

                if ((type & DayType.Workday) != DayType.None)
                    counter[DayIndex.Workday]++;
                else if ((type & DayType.Holiday) != DayType.None)
                {
                    counter[DayIndex.Holiday]++;

                    if (dayIndex == DayIndex.Saturday && Runs(day))
                        saturdays++;
                }
            }

            counter[dayIndex]++;

            if (RunsNot(day))
                totalBad++;

            dayIndex = GetNextDayIndex(dayIndex);
        }

        if (totalBad == 0)
            return false;

        if (_specDays)
            ApplySpecDays(okCount, badCount, saturdays, ref grouping);

        for (var index = DayIndex.Monday; index <= DayIndex.Holiday; index++)
            if (okCount[index] != 0 && badCount[index] != 0)
                return false;

        return true;
    }

    /// <summary>
    ///     Rozhodne, ci sa jazda lepsie popise dnami v tyzdni, alebo pracovnymi dnami a sviatkami,
    ///     a pocitadla nepouziteho popisu vynuluje.
    /// </summary>
    private static void ApplySpecDays(DayCounter okCount, DayCounter badCount, int saturdays, ref DayGrouping grouping)
    {
        // sobota, ktora je zaroven sviatkom, sa nepocita do sviatkov, ak sa jazda riadi sobotami
        if (okCount[DayIndex.Saturday] > 2 * badCount[DayIndex.Saturday] &&
            okCount[DayIndex.Holiday] < 2 * badCount[DayIndex.Holiday])
            okCount[DayIndex.Holiday] -= saturdays;

        var weekDayScore = 0;
        var specDayScore = 0;

        for (var index = DayIndex.Monday; index <= DayIndex.Holiday; index++)
        {
            if (okCount[index] <= 0 || okCount[index] <= badCount[index])
                continue;

            if (index <= DayIndex.Sunday)
                weekDayScore += okCount[index] - badCount[index];

            if (index is DayIndex.Saturday or DayIndex.Workday or DayIndex.Holiday)
                specDayScore += okCount[index] - badCount[index];
        }

        if (specDayScore <= weekDayScore)
        {
            // jazda sa popise dnami v tyzdni - pracovne dni a sviatky sa nepouziju
            okCount[DayIndex.Workday] = 0;
            okCount[DayIndex.Holiday] = 0;
            badCount[DayIndex.Workday] = 0;
            badCount[DayIndex.Holiday] = 0;
            return;
        }

        grouping = DayGrouping.WorkdayHoliday;

        for (var index = DayIndex.Monday; index <= DayIndex.Sunday; index++)
            if (index != DayIndex.Saturday)
            {
                okCount[index] = 0;
                badCount[index] = 0;
            }

        if (okCount[DayIndex.Holiday] > 2 * badCount[DayIndex.Holiday] &&
            okCount[DayIndex.Saturday] < 2 * badCount[DayIndex.Saturday])
            okCount[DayIndex.Saturday] -= saturdays;
        else
            grouping |= DayGrouping.KeepSaturday;
    }

    /// <summary>
    ///     Posunie zaciatok useku na prvy den patriaci do tyzdenneho vzoru. Ak vzor siaha az na
    ///     zaciatok grafikonu, vrati 0 - zaciatok sa potom v poznamke neuvadza.
    /// </summary>
    private int GetBetterDayFrom(int from, DayCounter okCount, DayGrouping grouping)
    {
        if (from >= 7)
            return from;

        var day = from;

        while (day >= 0 && (Runs(day) || okCount[GetDayIndex(day, grouping)] == 0))
            day--;

        if (day < 0)
            return 0;

        day = from;

        while (okCount[GetDayIndex(day, grouping)] == 0)
            day++;

        return day;
    }

    /// <summary>
    ///     Posunie koniec useku na posledny den patriaci do tyzdenneho vzoru. Ak vzor siaha az na
    ///     koniec grafikonu, vrati <see cref="MaxDay"/> - koniec sa potom v poznamke neuvadza.
    /// </summary>
    private int GetBetterDayTo(int to, DayCounter okCount, DayGrouping grouping)
    {
        if (to <= MaxDay - 7)
            return to;

        var day = to;

        while (day <= MaxDay && (Runs(day) || okCount[GetDayIndex(day, grouping)] == 0))
            day++;

        if (day > MaxDay)
            return MaxDay;

        day = to;

        while (okCount[GetDayIndex(day, grouping)] == 0)
            day--;

        return day;
    }

    /// <summary>
    ///     Vrati, ci su nastavene vsetky dni v tyzdni, alebo vsetky pracovne dni spolu so sviatkami.
    /// </summary>
    private static bool AllSet(DayCounter count)
    {
        var dayType = GetDayType(count);
        return (dayType & DayType.All1) == DayType.All1 || (dayType & DayType.All2) == DayType.All2;
    }

    /// <summary>
    ///     Vrati, ci sa tyzdenny vzor od dna <paramref name="from"/> zhoduje so vzorom od dna <paramref name="to"/>.
    /// </summary>
    private bool EqualPattern(int from, int to)
    {
        for (var offset = 0; offset <= 6; offset++)
            if (Runs(from + offset) != Runs(to + offset))
                return false;

        return true;
    }

    /// <summary>
    ///     Sformatuje vsetky useky obmedzenia do vysledneho textu poznamky.
    /// </summary>
    private string Format(IList<DateLimitInfo> limits, bool isNot)
    {
        if (limits == null || limits.Count == 0)
            return MsgText(Message.Empty);

        Merge(limits);
        _builder.Length = 0;
        var level = Level.Undefined;

        for (var i = 0; i < limits.Count; i++)
            FormatInfo(limits[i], i + 1 < limits.Count ? limits[i + 1] : null, isNot, ref level);

        return _builder.ToString();
    }

    /// <summary>
    ///     Sformatuje jeden usek obmedzenia.
    /// </summary>
    /// <param name="info">Usek na sformatovanie.</param>
    /// <param name="next">Nasledujuci usek, alebo <see langword="null"/> pri poslednom useku.</param>
    /// <param name="isNot">Text sa tvori z negovaneho bitoveho pola.</param>
    /// <param name="level">Naposledy vypisana uvodna spojka - opakovane sa nevypisuje.</param>
    private void FormatInfo(DateLimitInfo info, DateLimitInfo next, bool isNot, ref Level level)
    {
        AppendComma();
        _lastMonth = null;

        if (info.AllIsSet && info.From == 0 && info.To == MaxDay && !info.RunsNot)
        {
            _builder.Append(MsgText(Message.RunsDaily));
            return;
        }

        // pri negovanom poli maju vyznamy "ide" a "nejde" opacny vyznam
        var mainLevel = isNot ? Level.RunsNot : Level.Runs;
        var mainText = isNot ? AltMsgText(Message.RunsNot, Message.RunsNotAlt) : AltMsgText(Message.Runs, Message.RunsAlt);
        var exceptionLevel = isNot ? Level.Runs : Level.RunsNot;
        var exceptionText = isNot ? AltMsgText(Message.Runs, Message.RunsAlt) : AltMsgText(Message.RunsNot, Message.RunsNotAlt);

        var baseLength = _builder.Length;

        if (info.HaveDays || info.From != 0 || info.To != 0)
        {
            AppendPeriod(info.From, info.To);

            if (info.HaveDays && next != null && next.Type == info.Type && !info.Runs && !info.RunsNot)
                _builder.Append(MsgText(Message.And));
            else if (info.HaveDays)
                AppendDays(info.Type);

            if (_builder.Length > baseLength)
            {
                if (level != mainLevel)
                {
                    _builder.Insert(baseLength, mainText);
                    level = mainLevel;
                }
                else if (next == null && baseLength > 0 && _builder[baseLength - 1] == ',' && !info.HaveDays)
                {
                    // posledny usek sa k predchadzajucemu pripoji spojkou namiesto ciarky
                    baseLength--;
                    _builder.Remove(baseLength, 1);
                    _builder.Insert(baseLength, MsgText(Message.And));
                }
            }
        }

        foreach (var run in info.ListRuns)
        {
            if (_builder.Length == baseLength && level != mainLevel)
            {
                _builder.Append(mainText);
                level = mainLevel;
            }

            AppendPeriod(run.From, run.To);
        }

        _lastMonth = null;

        for (var i = 0; i < info.ListRunsNot.Count; i++)
        {
            AppendComma();

            if (i == 0 && level != exceptionLevel)
            {
                _builder.Append(exceptionText);
                level = exceptionLevel;
            }

            AppendPeriod(info.ListRunsNot[i].From, info.ListRunsNot[i].To);
        }
    }

    /// <summary>
    ///     Spoji susedne useky obmedzenia, ktore sa daju zapisat spolocne.
    /// </summary>
    private static void Merge(IList<DateLimitInfo> limits)
    {
        var i = 0;

        while (i + 1 < limits.Count)
            if (limits[i].Merge(limits[i + 1]))
                limits.RemoveAt(i + 1);
            else
                i++;
    }

    /// <summary>
    ///     Pripoji zoznam typov dni, napr. "v 1-5,7". Tri a viac dni po sebe sa zapisu ako rozsah.
    /// </summary>
    private void AppendDays(DayType dayType)
    {
        AppendSpace();
        _builder.Append(MsgText(Message.On));

        if ((dayType & DayType.Workday) == DayType.Workday)
            _builder.Append(MsgDayType(Message.Workday));

        var day = 0;

        while (day <= 6)
        {
            if ((dayType & (DayType)(1 << day)) == DayType.None)
            {
                day++;
                continue;
            }

            var last = day;

            while (last < 6 && (dayType & (DayType)(1 << (last + 1))) != DayType.None)
                last++;

            if (last - day > 1)
            {
                AppendComma();
                _builder.Append(MsgDayType(Message.Monday + day)).Append("-").Append(MsgDayType(Message.Monday + last));
            }
            else
                while (day <= last)
                {
                    AppendComma();
                    _builder.Append(MsgDayType(Message.Monday + day));
                    day++;
                }

            day = last + 1;
        }

        if ((dayType & DayType.Holiday) != DayType.Holiday)
            return;

        AppendComma();
        _builder.Append(MsgDayType(Message.Holiday));
    }

    /// <summary>
    ///     Pripoji obdobie. Hranice zhodne s hranicami grafikonu sa neuvadzaju, kratke obdobia sa
    ///     vypisu ako jednotlive datumy.
    /// </summary>
    private void AppendPeriod(int dayFrom, int dayTo)
    {
        if (dayFrom == 0 && dayTo == MaxDay)
            return;

        if (dayTo - dayFrom <= 1)
        {
            for (var day = dayFrom; day <= dayTo; day++)
                AppendDay(day);

            return;
        }

        AppendComma();

        if (dayFrom > 0)
            _builder.Append(MsgText(Message.From)).Append(FormatDay(dayFrom));

        if (dayTo < MaxDay)
        {
            AppendSpace();
            _builder.Append(MsgText(Message.To)).Append(FormatDay(dayTo));
        }

        _lastMonth = null;
    }

    /// <summary>
    ///     Vrati, ci den a mesiac zadaneho datumu pripadnu do platnosti grafikonu najviac raz,
    ///     teda ci netreba k datumu uvadzat aj rok.
    /// </summary>
    private bool DateUnique(DateTime date)
    {
        var count = 0;

        for (var year = DateFrom.Year; year <= DateTo.Year; year++)
        {
            // 29.2. v nepriestupnom roku neexistuje
            if (date.Day > DateTime.DaysInMonth(year, date.Month))
                continue;

            var candidate = new DateTime(year, date.Month, date.Day);

            if (candidate >= DateFrom && candidate <= DateTo)
                count++;
        }

        return count <= 1;
    }

    /// <summary>
    ///     Rozlozi text poznamky na useky a vysledok zapise do bitoveho pola.
    /// </summary>
    private void ParseText()
    {
        var state = new ParseState();

        while (SkipWhiteSpace())
        {
            var token = ExtractToken();

            if (token != "," && TryReadToken(token, state))
            {
                _position += token.Length;
                continue;
            }

            // ciarka mimo zoznamu dni ukoncuje usek
            if (state.DateLevel != DateLevel.On)
            {
                FlushData(state, false);
                _position += token.Length;
                continue;
            }

            // v zozname dni sa usek ukonci az vtedy, ked dalsi token uz nie je pevny kod dna
            if (!NextTokenIsDayCode(token))
                FlushData(state, false);

            _position += token.Length;
        }

        FlushData(state, false);
        ApplyParsedData(state.Level);
    }

    /// <summary>
    ///     Spracuje jeden token textu poznamky.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/>, ak token len prepol parser do rezimu zoznamu dni a este sa ma
    ///     posudit v kontexte nasledujuceho tokenu.
    /// </returns>
    private bool TryReadToken(string token, ParseState state)
    {
        if (TokenIsMsg(token, Message.And))
            FlushData(state, true);
        else if (TokenIsMsg(token, Message.Runs, Message.RunsAlt))
        {
            FlushData(state, false);
            state.Level = Level.Runs;
            state.DateLevel = DateLevel.Date;
        }
        else if (TokenIsMsg(token, Message.RunsNot, Message.RunsNotAlt))
        {
            FlushData(state, false);
            state.Level = Level.RunsNot;
            state.DateLevel = DateLevel.Date;
        }
        else if (TokenIsMsg(token, Message.From))
            state.DateLevel = DateLevel.From;
        else if (TokenIsMsg(token, Message.To))
            state.DateLevel = DateLevel.To;
        else if (TokenIsMsg(token, Message.On))
        {
            state.DateLevel = DateLevel.On;
            state.Days = DayType.None;
        }
        else if (state.DateLevel == DateLevel.On)
            state.Days |= GetDayType(token);
        else if (IsDayType(token) || IsDayRange(token))
        {
            // pevny kod dna bez uvodnej predlozky
            state.DateLevel = DateLevel.On;
            state.Days = DayType.None;
            return false;
        }
        else
            ReadDate(token, state);

        return true;
    }

    /// <summary>
    ///     Zapise datum z tokenu do stavu parsera podla toho, ci ide o "od", "do", alebo o jeden den.
    /// </summary>
    private void ReadDate(string token, ParseState state)
    {
        var date = GetDate(token, state.DateLevel == DateLevel.To);

        switch (state.DateLevel)
        {
            case DateLevel.From:
                state.From = date;
                break;
            case DateLevel.To:
                state.To = date;
                break;
            default:
                state.From = date;
                state.To = date;
                break;
        }
    }

    /// <summary>
    ///     Nazrie za zadany token bez posunutia pozicie a vrati, ci nasledujuci token este patri do
    ///     zoznamu pevnych kodov dni (jednoznakovy kod alebo rozsah tvaru "1-5").
    /// </summary>
    private bool NextTokenIsDayCode(string token)
    {
        var position = _position;
        _position += token.Length;

        var next = SkipWhiteSpace() ? ExtractToken() : "??";
        _position = position;

        return next.Length <= 1 || next.IndexOf('-') >= 0 && next.Length == 3;
    }

    /// <summary>
    ///     Ulozi rozparsovany usek a pripravi stav parsera na dalsi usek.
    /// </summary>
    /// <param name="state">Stav parsera.</param>
    /// <param name="and">Usek je s nasledujucim usekom spojeny spojkou "a".</param>
    private void FlushData(ParseState state, bool and)
    {
        if (state.From == DateTime.MinValue && state.To == DateTime.MinValue && state.Days == DayType.None)
            return;

        var data = new ParseData
        {
            From = state.From,
            To = state.To,
            Level = state.Level,
            Days = state.Days,
            And = and
        };

        // pevne kody dni sa uvadzaju az za poslednym usekom, plati vsak pre vsetky useky spojene spojkou "a"
        if (state.Days != DayType.None)
            for (var i = _parsedData.Count - 1; i >= 0 && _parsedData[i].And && _parsedData[i].Days == DayType.None; i--)
                _parsedData[i].Days = state.Days;

        _parsedData.Add(data);

        state.From = DateTime.MinValue;
        state.To = DateTime.MinValue;
        state.Days = DayType.None;
        state.DateLevel = DateLevel.Date;
    }

    /// <summary>
    ///     Prenesie vsetky rozparsovane useky do bitoveho pola.
    /// </summary>
    private void ApplyParsedData(Level level)
    {
        if (_parsedData.Count == 0)
            FlushData(new ParseState { Level = level, From = DateFrom }, false);

        for (var i = 0; i < _parsedData.Count; i++)
        {
            var parseData = _parsedData[i];

            // poznamka zacinajuca "nejde" znamena, ze vlak inak ide kazdy den
            if (i == 0 && parseData.Level == Level.RunsNot)
                _bits.SetAll(true);

            if (parseData.From == DateTime.MinValue)
                parseData.From = DateFrom;

            if (parseData.To == DateTime.MinValue)
                parseData.To = DateTo;

            var lastDay = DateDiff(DateFrom, parseData.To);

            for (var day = DateDiff(DateFrom, parseData.From); day <= lastDay; day++)
                if (day >= 0 && day <= MaxDay &&
                    (parseData.Days == DayType.None || (GetDayType(day, true) & parseData.Days) != DayType.None))
                    _bits[day] = parseData.Level == Level.Runs;
        }
    }

    /// <summary>
    ///     Posunie poziciu na najblizsi neprazdny znak.
    /// </summary>
    /// <returns><see langword="true"/>, ak v texte este nejaky znak zostal.</returns>
    private bool SkipWhiteSpace()
    {
        while (_position < _text.Length && _text[_position] <= ' ')
            _position++;

        return _position < _text.Length;
    }

    /// <summary>
    ///     Vrati token na aktualnej pozicii bez toho, aby poziciu posunul. Tokenom je bud samotna
    ///     ciarka, alebo znaky az po najblizsiu medzeru ci ciarku.
    /// </summary>
    private string ExtractToken()
    {
        var pos = _position + 1;

        while (pos < _text.Length && _text[pos] > ' ' && _text[pos] != ',' && _text[_position] != ',')
            pos++;

        return _text.Substring(_position, pos - _position);
    }

    /// <summary>
    ///     Vrati, ci token zodpoveda niektorej zo zadanych sprav.
    /// </summary>
    private static bool TokenIsMsg(string token, params Message[] msgs)
    {
        foreach (var msg in msgs)
        {
            if (string.Equals(token, MsgText(msg).Trim(), StringComparison.OrdinalIgnoreCase))
                return true;

            if (!MessagePatterns.TryGetValue(msg, out var pattern))
                continue;

            // jednoslovny vzor nesmie zabrat viacslovny token
            if (token.Contains(" ") && !pattern.Contains(" "))
                continue;

            if (Regex.IsMatch(token, pattern))
                return true;
        }

        return false;
    }

    /// <summary>
    ///     Vrati, ci je token zlozeny len z pevnych kodov dni.
    /// </summary>
    private static bool IsDayType(string token) => token.ToUpper().All(c => DayTypeSigns.IndexOf(c) >= 0);

    /// <summary>
    ///     Vrati, ci je token rozsah dni v tvare "1-5".
    /// </summary>
    private static bool IsDayRange(string token) =>
        token.Length == 3 && token[1] == '-' && char.IsDigit(token[0]) && char.IsDigit(token[2]);

    /// <summary>
    ///     Prevedie pevny kod dni ("1", "X+", "1-5") na priznaky typov dni.
    /// </summary>
    private DayType GetDayType(string token)
    {
        token = token.ToUpper();

        if (IsDayType(token))
            return token.Aggregate(DayType.None, (current, c) => current | (DayType)(1 << DayTypeSigns.IndexOf(c)));

        var first = DayTypeSigns.IndexOf(token[0]);

        // rozsah dni - jeho zaciatok musi lezat pred nedelou
        if (token.Length == 3 && token[1] == '-' && first < 6)
        {
            var last = WeekDaySigns.IndexOf(token[2]);

            if (last > first)
            {
                var range = DayType.None;

                for (var i = first; i <= last; i++)
                    range |= (DayType)(1 << i);

                return range;
            }
        }

        throw new ParseException($"Chybný pevný kód dňa {token}.", _position);
    }

    /// <summary>
    ///     Prevedie token na datum. Rok sa v poznamke uvadzat nemusi - vtedy sa odvodi z platnosti grafikonu.
    /// </summary>
    /// <param name="token">Token s datumom.</param>
    /// <param name="checkLast">Token je koncovym datumom obdobia.</param>
    private DateTime GetDate(string token, bool checkLast)
    {
        if (token.IndexOf('-') >= 0)
            throw new ParseException("Pre interval dát použite od ... do ..., nie -.", _position);

        var dotIndex = token.IndexOf('.');

        if (dotIndex < 0 || !int.TryParse(token.Substring(0, dotIndex), out var day) || day is <= 0 or > 31)
            throw new ParseException($"Chybný dátum {token}.", _position);

        string rest;

        if (dotIndex + 1 < token.Length)
            rest = token.Substring(dotIndex + 1);
        else
        {
            // den je uvedeny samostatne (napr. "1.,5.I."), mesiac sa hlada v zvysku textu
            rest = _text.Substring(_position + token.Length);
            var match = Regex.Match(rest, "\\.[0-9IXV]+\\.");

            if (!match.Success)
                rest = "";
            else
                rest = match.Index + match.Value.Length + 4 > rest.Length
                    ? match.Value.Substring(1)
                    : rest.Substring(match.Index + 1, match.Value.Length + 3);
        }

        var monthEnd = rest.IndexOf('.');

        if (monthEnd < 0)
            throw new ParseException($"Chybný dátum {token}.", _position);

        var month = GetMonth(rest.Substring(0, monthEnd));
        var yearSet = int.TryParse(rest.Substring(monthEnd + 1), out var year) && year is >= 2000 and < 2100;

        if (!yearSet)
            year = month < DateFrom.Month || month == DateFrom.Month && day < DateFrom.Day
                ? DateFrom.Year + 1
                : DateFrom.Year;

        var date = CreateDate(year, month, day, token);

        if (date > DateTo && !yearSet)
        {
            if (checkLast)
            {
                if (_skipDateRangeCheck)
                    return DateTo;

                throw new ParseException($"Koncový dátum {FormatDate(date)} je mimo rozsahu platnosti grafikonu.", _position);
            }

            date = CreateDate(year - 1, month, day, token);
        }

        if (date >= DateFrom && date <= DateTo)
            return date;

        if (!_skipDateRangeCheck)
            throw new ParseException($"Dátum {FormatDate(date)} je mimo rozsahu platnosti grafikonu.", _position);

        // datum bez roku sa posunie o rok dopredu, ak tak lezi blizsie k platnosti grafikonu
        if (!yearSet && date < DateFrom &&
            DateFrom.Subtract(date).TotalDays > date.AddYears(1).Subtract(DateTo).TotalDays)
            date = date.AddYears(1);

        return date;
    }

    /// <summary>
    ///     Vytvori datum a neplatnu kombinaciu prevedie na <see cref="ParseException"/>.
    /// </summary>
    private DateTime CreateDate(int year, int month, int day, string token)
    {
        try
        {
            return new DateTime(year, month, day);
        }
        catch (Exception)
        {
            throw new ParseException($"Chybný dátum {token}.", _position);
        }
    }

    /// <summary>
    ///     Prevedie cislo mesiaca alebo jeho rimsku cislicu na cislo mesiaca.
    /// </summary>
    private int GetMonth(string month)
    {
        if (int.TryParse(month, out var number))
            return number;

        // rimske cislice mesiacov su na konci pola sprav, hlada sa az od nich (znak "X" je aj kodom pracovneho dna)
        var index = Array.IndexOf(MessagesCz, month.ToUpper(), (int)Message.Jan);

        if (index is < (int)Message.Jan or > (int)Message.Dec)
            throw new ParseException($"Neplatný mesiac {month}.", _position);

        return index - (int)Message.Jan + 1;
    }

    /// <summary>
    ///     Vrati cislo mesiaca ako rimsku cislicu alebo ako cislo podla nastavenia.
    /// </summary>
    private string MsgMonth(int month) => _monthRoman ? MsgText(Message.Jan + month - 1) : month.ToString();

    /// <summary>
    ///     Vrati nazov typu dna, pripadne obaleny znackami {}. Dlzka znaciek sa pripocita
    ///     k <see cref="_marksLength"/>, aby sa nezapocitala do dlzky poznamky.
    /// </summary>
    private string MsgDayType(Message message)
    {
        if (!InsertMarks)
            return MsgText(message);

        _marksLength += 2;
        return "{" + MsgText(message) + "}";
    }

    /// <summary>
    ///     Vrati typ zadaneho dna - den v tyzdni a pripadne aj priznak pracovneho dna alebo sviatku.
    /// </summary>
    private DayType GetDayType(DateTime date, bool forceSpecDays = false)
    {
        var dayType = (DayType)(1 << (int)GetDayIndex(date));

        if (!_specDays && !forceSpecDays)
            return dayType;

        if (IsHoliday(date))
            dayType |= DayType.Holiday;
        else if (dayType <= DayType.Friday)
            dayType |= DayType.Workday;

        return dayType;
    }

    /// <inheritdoc cref="GetDayType(DateTime,bool)"/>
    private DayType GetDayType(int day, bool forceSpecDays = false) => GetDayType(DateFrom.AddDays(day), forceSpecDays);

    /// <summary>
    ///     Vrati typy dni, ktore maju v pocitadle nenulovu hodnotu.
    /// </summary>
    private static DayType GetDayType(DayCounter okCount)
    {
        var dayType = DayType.None;

        for (var index = DayIndex.Monday; index <= DayIndex.Holiday; index++)
            if (okCount[index] != 0)
                dayType |= (DayType)(1 << (int)index);

        return dayType;
    }

    /// <summary>
    ///     Vrati <paramref name="dayType"/>, ak sa v obdobi vyskytuje aspon jeden den mimo tychto typov,
    ///     inak <see cref="DayType.None"/> - typy dni potom netreba v poznamke uvadzat.
    /// </summary>
    private DayType CheckDayType(int dayFrom, int dayTo, DayType dayType)
    {
        while (dayFrom <= dayTo)
        {
            if ((GetDayType(dayFrom) & dayType) == DayType.None)
                return dayType;

            dayFrom++;
        }

        return DayType.None;
    }

    /// <summary>
    ///     Prevedie datum na index dna v tyzdni, kde pondelok je 0.
    /// </summary>
    private static DayIndex GetDayIndex(DateTime date) => (DayIndex)date.AddDays(-1).DayOfWeek;

    /// <summary>
    ///     Vrati index dna pouzity pri porovnavani. Pri zluceni na pracovne dni a sviatky vrati
    ///     <see cref="DayIndex.Workday"/> alebo <see cref="DayIndex.Holiday"/> namiesto dna v tyzdni.
    /// </summary>
    private DayIndex GetDayIndex(int day, DayGrouping grouping)
    {
        var date = DateFrom.AddDays(day);
        var index = GetDayIndex(date);

        if (grouping == DayGrouping.None || !_specDays)
            return index;

        if (index == DayIndex.Saturday && (grouping & DayGrouping.KeepSaturday) != 0)
            return index;

        if (IsHoliday(date))
            return DayIndex.Holiday;

        return index == DayIndex.Saturday ? index : DayIndex.Workday;
    }

    /// <summary>
    ///     Vrati index nasledujuceho dna.
    ///     Ak je <paramref name="day"/> <see cref="DayIndex.Sunday"/>, vrati <see cref="DayIndex.Monday"/>.
    /// </summary>
    /// <param name="day">Index dna.</param>
    /// <returns>Index nasledujuceho dna.</returns>
    private static DayIndex GetNextDayIndex(DayIndex day) => day >= DayIndex.Sunday ? DayIndex.Monday : day + 1;

    /// <summary>
    ///     Vrati datum Velkonocneho pondelka v zadanom roku (Gaussov velkonocny algoritmus).
    /// </summary>
    private static DateTime GetEasterMonday(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var f = c / 4;
        var g = c % 4;
        var h = (8 * b + 13) / 25;
        var i = (19 * a + b - d - h + 15) % 30;
        var j = (a + 11 * i) / 319;
        var k = (2 * e + 2 * f - g - i + j + 32) % 7;

        var month = (i - j + k + 91) / 25;
        var day = (i - j + k + 20 + month) % 32;

        return new DateTime(year, month, day);
    }

    /// <summary>
    ///     Chyba pri parsovani textu poznamky.
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException(string message, int pos) : base(message) => Position = pos;

        public int Position { get; }
    }

    private enum Level
    {
        /// <summary>
        ///     Nedefinovane.
        /// </summary>
        Undefined,

        /// <summary>
        ///     Vlak ide.
        /// </summary>
        Runs,

        /// <summary>
        ///     Vlak nejde.
        /// </summary>
        RunsNot
    }

    /// <summary>
    ///     Cast poznamky, ku ktorej sa vztahuje prave spracovany token.
    /// </summary>
    private enum DateLevel
    {
        Date,
        From,
        To,
        On
    }

    /// <summary>
    ///     Sposob, akym sa dni zlucuju pri hladani tyzdenneho vzoru.
    /// </summary>
    [Flags]
    private enum DayGrouping
    {
        /// <summary>
        ///     Dni sa posudzuju podla dna v tyzdni.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Dni sa zlucuju na pracovne dni a sviatky.
        /// </summary>
        WorkdayHoliday = 1,

        /// <summary>
        ///     Sobota sa aj napriek zluceniu posudzuje samostatne.
        /// </summary>
        KeepSaturday = 32
    }

    /// <summary>
    ///     Pocitadlo dni pre kazdu polozku <see cref="DayIndex"/>.
    /// </summary>
    private sealed class DayCounter
    {
        private readonly int[] _counts = new int[DayIndexCount];

        public int this[DayIndex index]
        {
            get => _counts[(int)index];
            set => _counts[(int)index] = value;
        }

        /// <summary>
        ///     Vrati, ci su vsetky pocitadla nulove.
        /// </summary>
        public bool AllZero => _counts.All(count => count == 0);

        public void Clear() => Array.Clear(_counts, 0, _counts.Length);
    }

    /// <summary>
    ///     Jeden usek datumoveho obmedzenia - obdobie, typy dni a vynimky z nich.
    /// </summary>
    private class DateLimitInfo
    {
        /// <summary>Zoznam obdobi, kedy vlak navyse ide.</summary>
        public List<DateLimitInfo> ListRuns;

        /// <summary>Zoznam obdobi, kedy vlak nejde.</summary>
        public List<DateLimitInfo> ListRunsNot;

        public int From;
        public int To;
        public DayType Type;

        public DateLimitInfo()
        {
            ListRuns = new List<DateLimitInfo>();
            ListRunsNot = new List<DateLimitInfo>();
        }

        public DateLimitInfo(int dayFrom, int dayTo) : this()
        {
            From = dayFrom;
            To = dayTo;
        }

        /// <summary>Su nastavene vsetky dni v tyzdni, alebo pracovne dni spolu so sviatkami.</summary>
        public bool AllIsSet => (Type & DayType.All1) == DayType.All1 || (Type & DayType.All2) == DayType.All2;

        /// <summary>Typy dni treba v poznamke uviest.</summary>
        public bool HaveDays => !AllIsSet && Type > DayType.None;

        public bool Runs => ListRuns.Count > 0;

        public bool RunsNot => ListRunsNot.Count > 0;

        /// <summary>
        ///     Pokusi sa pripojit nasledujuci usek k tomuto useku.
        /// </summary>
        /// <returns><see langword="true"/>, ak sa useky podarilo spojit.</returns>
        public bool Merge(DateLimitInfo info)
        {
            // useky sa spoja bud ak maju rovnake typy dni a lezia dostatocne blizko pri sebe,
            // alebo ak pripojeny usek neurcuje ziadne vlastne obdobie
            var compatible = HaveDays && Type == info.Type && To + MaxMergeGap > info.From &&
                             (Runs || info.Runs || RunsNot || info.RunsNot);
            var isEmpty = !info.HaveDays && info.From == 0 && info.To == 0;

            if (!compatible && !isEmpty)
                return false;

            if (info.To != 0 || info.From != 0)
            {
                // medzera medzi usekmi sa zapise ako obdobie, kedy vlak nejde
                if (Runs && ListRuns.Any(run => run.From > To && run.From < info.From || run.To > To && run.To < info.From))
                    return false;

                ListRunsNot.Add(new DateLimitInfo(To + 1, info.From - 1));
                To = info.To;
            }

            if (info.Runs)
            {
                if (Runs)
                    ListRuns.AddRange(info.ListRuns);
                else
                    ListRuns = info.ListRuns;
            }

            if (info.RunsNot)
            {
                if (RunsNot)
                    ListRunsNot.AddRange(info.ListRunsNot);
                else
                    ListRunsNot = info.ListRunsNot;
            }

            return true;
        }
    }

    /// <summary>
    ///     Priebezny stav parsera textovej poznamky.
    /// </summary>
    private sealed class ParseState
    {
        public DayType Days;
        public DateLevel DateLevel = DateLevel.Date;
        public DateTime From;
        public Level Level = Level.Runs;
        public DateTime To;
    }

    /// <summary>
    ///     Jeden rozparsovany usek poznamky.
    /// </summary>
    private class ParseData
    {
        public bool And;
        public DayType Days;
        public DateTime From;
        public Level Level = Level.Runs;
        public DateTime To;
    }

    [Flags]
    private enum DayType
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        Workday = 128,
        Holiday = 256,

        /// <summary>Vsetky dni v tyzdni.</summary>
        All1 = 127,

        /// <summary>Sobota, pracovne dni a sviatky.</summary>
        All2 = 416
    }

    private enum DayIndex
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
        Workday,
        Holiday
    }
}

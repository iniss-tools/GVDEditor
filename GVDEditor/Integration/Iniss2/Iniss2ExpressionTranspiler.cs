using System.Globalization;
using ToolsCore.Iniss.Expressions;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Preklad vyrazov INISS 3.39 (podmienky TabTab, dynamicke hodnoty StateDgm) do VB.NET servera INISS2. Vysledok je vyraz nad
/// clenmi <c>VlakS</c> v bloku <c>With VlakS</c>, preto cleny vlaku zacinaju bodkou. INISS 3.39 pocita len s celymi cislami
/// a pravda je nenulova hodnota; preklad drzi pri uzle logicky aj ciselny tvar a prevadza medzi nimi explicitne, aby VB.NET
/// (bez Option Strict) nepremenil True na -1. Spojky sa vyhodnocuju bez skratenia (<c>And</c>, <c>Or</c>) ako v INISSe.
/// Druh vlaku bez kluca v TrTypes a kolaj, ktora v grafikone nie je, sa prelozia na <c>False</c> - vlak ich mat nemoze.
/// Zatvorky su len tam, kde ich vyzaduje priorita operatorov VB.NET, a pri miesani spojok <c>And</c>/<c>Or</c>/<c>Xor</c>,
/// operatorov <c>\</c> a <c>Mod</c> s inymi aritmetickymi a pri <c>Not</c> nad porovnanim, kde by priorita citatela mylila.
/// </summary>
internal sealed class Iniss2ExpressionTranspiler
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    // bity slova priznakov vlaku 3.39 (Prizn_*) a ich obdoba vo VlakS; prestup je v INISS2 priznak bodu trasy
    private static readonly (int Bit, Code Code)[] FlagBits =
    [
        (0x100, Atom(".VylukaPrij")),
        (0x200, Atom(".VylukaOdj")),
        (0x1000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.M")),
        (0x2000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.O")),
        (0x4000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.D")),
        (0x8000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.X")),
        (0x10000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.R")),
        (0x20000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.L")),
        (0x40000, FlagSet(".PriznakyVlaku", "VlajkyVarianty.N")),
        (0x80000, FlagSet(".PriznakyStanice", "VlajkyStanice.PR"))
    ];

    // stavove bity 3.39 (Stav_*) a priznaky stavu INISS2
    private static readonly (int Bit, Code Code)[] StateBits =
    [
        (0x20, FlagSet(".PriznakyStavu", "NODEFLAGS.ODBAVEN")),
        (0x80, FlagSet(".PriznakyStavu", "NODEFLAGS.STOJI"))
    ];

    // udalosti GTN v hornom bajte hodnoty Wait a priznaky GTN zastavenia v INISS2
    private static readonly (uint Bit, string Name)[] WaitEvents =
    [
        (0x80000000u, "VVC"),
        (0x40000000u, "OVC"),
        (0x20000000u, "ZCV"),
        (0x10000000u, "Vj"),
        (0x08000000u, "Odj")
    ];

    private readonly IReadOnlyDictionary<string, int> _trainTypeKeys;
    private readonly ILookup<int, string> _keysByIndex;
    private readonly Func<string, string?> _trackKey;
    private readonly ExprEvalSite _site;
    private readonly List<Iniss2ExprProblem> _problems = [];

    /// <summary>
    /// Vytvori prekladac.
    /// </summary>
    /// <param name="trainTypeKeys">kluce druhov vlakov z TrTypes s indexom zabudovaneho druhu INISSu</param>
    /// <param name="trackKey">kluc kolaje INISS2 podla nazvu kolaje 3.39; null = kolaj nie je</param>
    /// <param name="site">miesto vyhodnotenia - urcuje vyznam <c>VYLUKAZDE</c> a <c>ZPOZDENI</c></param>
    public Iniss2ExpressionTranspiler(IReadOnlyDictionary<string, int> trainTypeKeys, Func<string, string?> trackKey,
        ExprEvalSite site = ExprEvalSite.Default)
    {
        _trainTypeKeys = trainTypeKeys;
        _keysByIndex = trainTypeKeys.OrderBy(pair => pair.Key, StringComparer.Ordinal).ToLookup(pair => pair.Value, pair => pair.Key);
        _trackKey = trackKey;
        _site = site;
    }

    /// <summary>
    /// Co sa doteraz nedalo preniest presne.
    /// </summary>
    public IReadOnlyList<Iniss2ExprProblem> Problems => _problems;

    /// <summary>
    /// Prelozi podmienku (TabTab, AutoCondition) na vyraz typu Boolean; null = vyraz sa neda prelozit.
    /// </summary>
    public string? Condition(string text) => Parse(text, ExprContext.Condition) is { } root ? Condition(root) : null;

    /// <summary>
    /// Prelozi podmienku, ktora sa moze spojit s dalsou spojkou <c>AndAlso</c>: vrati vyraz a jeho tvar ako operand spojky
    /// (podmienka so spojkou <c>Or</c> alebo <c>Xor</c> v zatvorkach); null = vyraz sa neda prelozit.
    /// </summary>
    public (string Code, string AndAlsoOperand)? JoinableCondition(string text)
    {
        if (Parse(text, ExprContext.Condition) is not { } root)
            return null;

        var code = AsBool(Emit(root));
        return (code.Text, Operand(code, Level.And, false));
    }

    /// <summary>
    /// Prelozi ciselnu hodnotu (IndCat, AutoMode, AutoTimePointAdd…) na vyraz typu Integer; null = vyraz sa neda prelozit.
    /// </summary>
    public string? Number(string text) => Parse(text, ExprContext.Condition) is { } root ? Number(root) : null;

    /// <summary>
    /// Prelozi porovnanie ciselnej hodnoty s cislom (napr. casovy bod automatiky); null = vyraz sa neda prelozit.
    /// </summary>
    public string? NumberEquals(string text, int value) =>
        Parse(text, ExprContext.Condition) is { } root ? Binary(AsInt(Emit(root)), "=", Literal(value), Level.Comparison).Text : null;

    /// <summary>
    /// Prelozi uzol na vyraz typu Boolean.
    /// </summary>
    public string Condition(ExprNode node) => AsBool(Emit(node)).Text;

    /// <summary>
    /// Prelozi uzol na vyraz typu Integer.
    /// </summary>
    public string Number(ExprNode node) => AsInt(Emit(node)).Text;

    /// <summary>
    /// Prelozi hodnotu kluca <c>Wait</c> na podmienku, ze prisli udalosti GTN; null = automatika na nic necaka. Viac udalosti
    /// spaja <c>And</c>, takze podmienka moze byt operandom spojky <c>AndAlso</c> bez zatvoriek.
    /// </summary>
    public string? Wait(string text)
    {
        if (Parse(text, ExprContext.StateDgmWait) is not { } root)
            return null;

        if (!ExprEvaluator.TryFoldConstant(root, out var value))
        {
            _problems.Add(new Iniss2ExprProblem(Iniss2ExprIssue.WaitNotConstant, text));
            return null;
        }

        return WaitCondition(unchecked((uint)value))?.Text;
    }

    /// <summary>
    /// Podmienka kluca <c>WaitPath</c> - nenulova hodnota caka na vchodovu vlakovu cestu.
    /// </summary>
    public static string WaitPath => WaitCondition(0x80000000u)!.Value.Text;

    /// <summary>
    /// Telo funkcie pre server: server doplni <c>Return</c>, len ked kod slovo <c>return</c> neobsahuje - napriklad v retazci.
    /// </summary>
    public static string Body(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return expression.Contains("return ", StringComparison.OrdinalIgnoreCase) ? "Return " + expression : expression;
    }

    private static Code? WaitCondition(uint value)
    {
        var events = WaitEvents.Where(e => (value & e.Bit) != 0).Select(e => FlagSet(".PriznakyGTN", "StaniceVlaku.FLAGS." + e.Name)).ToList();
        return events.Count == 0 ? null : Chain(events, "And", Level.And);
    }

    private ExprNode? Parse(string text, ExprContext context)
    {
        var result = ExprParser.Parse(text, context, new Symbols(_trainTypeKeys));
        if (result.Root is { } root && result.Error is null)
            return root;

        _problems.Add(new Iniss2ExprProblem(Iniss2ExprIssue.ParseError, result.Error?.Message ?? text));
        return null;
    }

    // priorita operatorov VB.NET od najnizsej (operatory, ktore preklad nepouziva, chybaju)
    private enum Level
    {
        Xor,
        Or,
        And,
        Not,
        Comparison,
        Additive,
        Modulus,
        IntegerDivision,
        Multiplicative,
        Unary,
        Primary
    }

    // kod a priorita jeho najvyssieho operatora; literal, clen, volanie a If(...) su Primary
    private readonly record struct Code(string Text, Level Level);

    // tvar uzla: logicky, ciselny alebo oba (maska PRIZNAK - logicky tvar plati, ked je niektory bit nastaveny)
    private sealed record Vb(Code? Bool, Code? Int, bool ZeroOne, int? Constant = null)
    {
        public static Vb Logical(Code code) => new(code, null, true);

        public static Vb Integer(Code code) => new(null, code, false);

        public static Vb Number(int value) => new(null, Literal(value), value is 0 or 1, value);
    }

    private static Code Atom(string text) => new(text, Level.Primary);

    private static bool IsLogical(Level level) => level is Level.Xor or Level.Or or Level.And;

    private static bool IsArithmetic(Level level) => level is >= Level.Additive and <= Level.Multiplicative;

    // operand binarneho operatora: zatvorky, ked ma nizsiu prioritu (pravy aj rovnaku - operatory su zlava; spojky And, Or
    // a Xor nie, su asociativne a bez skratenia sa vyhodnotia vsetky operandy v rovnakom poradi), alebo ked by bez nich
    // citatel prioritu lahko pomylil (rozne spojky, \ a Mod s inou aritmetikou, zaporne cislo za + a -)
    private static string Operand(Code operand, Level parent, bool right)
    {
        var level = operand.Level;
        var wrap = level < parent || (right && level == parent && !IsLogical(parent))
                   || (IsLogical(parent) && IsLogical(level) && level != parent)
                   || (IsArithmetic(parent) && IsArithmetic(level) && level != parent
                       && (parent is Level.Modulus or Level.IntegerDivision || level is Level.Modulus or Level.IntegerDivision))
                   || (parent == Level.Additive && right && level == Level.Unary);
        return wrap ? "(" + operand.Text + ")" : operand.Text;
    }

    private static Code Binary(Code left, string op, Code right, Level level) =>
        new($"{Operand(left, level, false)} {op} {Operand(right, level, true)}", level);

    // viac operandov tej istej spojky zlava (a Or b Or c = (a Or b) Or c)
    private static Code Chain(IEnumerable<Code> operands, string op, Level level) => operands.Aggregate((left, right) => Binary(left, op, right, level));

    // Not nad porovnanim alebo aritmetikou ma zatvorky - VB.NET by ho vyhodnotil az po nich
    private static Code Not(Code operand) =>
        new("Not " + (operand.Level is Level.Not or Level.Unary or Level.Primary ? operand.Text : "(" + operand.Text + ")"), Level.Not);

    private static Code Negate(Code operand) => new("-" + (operand.Level == Level.Primary ? operand.Text : "(" + operand.Text + ")"), Level.Unary);

    // nastaveny priznak: (slovo And priznak) <> 0
    private static Code FlagSet(string word, string flag) => Binary(Binary(Atom(word), "And", Atom(flag), Level.And), "<>", Atom("0"), Level.Comparison);

    private static Code If(Code condition, Code whenTrue, Code whenFalse) => Atom($"If({condition.Text}, {whenTrue.Text}, {whenFalse.Text})");

    private static Code EqualTo(Code left, Code right) => Binary(left, "=", right, Level.Comparison);

    private static Code AsBool(Vb value) =>
        value.Constant is { } constant ? Atom(constant != 0 ? "True" : "False") : value.Bool ?? Binary(value.Int!.Value, "<>", Atom("0"), Level.Comparison);

    private static Code AsInt(Vb value) => value.Int ?? If(value.Bool!.Value, Atom("1"), Atom("0"));

    private static Code Literal(int value) => value switch
    {
        int.MinValue => new Code("-2147483647 - 1", Level.Additive),
        < 0 => new Code(value.ToString(Inv), Level.Unary),
        _ => Atom(value.ToString(Inv))
    };

    private static Code Text(string value) => Atom("\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"");

    private Vb Emit(ExprNode node)
    {
        if (node is not ExprNumberNode && ExprEvaluator.TryFoldConstant(node, out var folded))
            return Vb.Number(folded);

        return node switch
        {
            ExprNumberNode number => Vb.Number(number.Value),
            ExprParenNode paren => Emit(paren.Inner),
            ExprUnaryNode unary => Unary(unary),
            ExprBinaryNode binary => Binary(binary),
            ExprConditionalNode conditional => Conditional(conditional),
            ExprFunctionNode function => Function(function),
            _ => throw new InvalidOperationException("Retazec mimo argumentu funkcie.")
        };
    }

    private Vb Unary(ExprUnaryNode node)
    {
        var operand = Emit(node.Operand);
        return node.Operator switch
        {
            ExprTokenKind.Not or ExprTokenKind.NotWord => Vb.Logical(Not(AsBool(operand))),
            ExprTokenKind.BitNot => Vb.Integer(Not(AsInt(operand))),
            ExprTokenKind.Minus => Vb.Integer(Negate(AsInt(operand))),
            ExprTokenKind.Plus => operand,
            ExprTokenKind.Odd => Vb.Integer(Binary(AsInt(operand), "Mod", Atom("2"), Level.Modulus)),
            _ => throw new InvalidOperationException("Neznamy unarny operator.")
        };
    }

    private Vb Binary(ExprBinaryNode node)
    {
        var left = Emit(node.Left);
        var right = Emit(node.Right);
        switch (node.Operator)
        {
            case ExprTokenKind.And or ExprTokenKind.AndWord:
                return Vb.Logical(Binary(AsBool(left), "And", AsBool(right), Level.And));
            case ExprTokenKind.Or or ExprTokenKind.OrWord:
                return Vb.Logical(Binary(AsBool(left), "Or", AsBool(right), Level.Or));
            case ExprTokenKind.BitAnd or ExprTokenKind.BitOr or ExprTokenKind.BitXor:
            {
                var (op, level) = node.Operator switch
                {
                    ExprTokenKind.BitAnd => ("And", Level.And),
                    ExprTokenKind.BitOr => ("Or", Level.Or),
                    _ => ("Xor", Level.Xor)
                };
                // bitove spojky dvoch hodnot 0/1 su logicke, inak sa pocita s cislami (napr. maska PRIZNAK)
                return left.ZeroOne && right.ZeroOne
                    ? Vb.Logical(Binary(AsBool(left), op, AsBool(right), level))
                    : Vb.Integer(Binary(AsInt(left), op, AsInt(right), level));
            }
            case ExprTokenKind.Equal or ExprTokenKind.NotEqual:
                return LogicalEquality(left, right, node.Operator == ExprTokenKind.Equal)
                       ?? LogicalEquality(right, left, node.Operator == ExprTokenKind.Equal)
                       ?? Vb.Logical(Binary(AsInt(left), node.Operator == ExprTokenKind.Equal ? "=" : "<>", AsInt(right), Level.Comparison));
            case ExprTokenKind.LessOrEqual:
                return Vb.Logical(Binary(AsInt(left), "<=", AsInt(right), Level.Comparison));
            case ExprTokenKind.Less:
                return Vb.Logical(Binary(AsInt(left), "<", AsInt(right), Level.Comparison));
            case ExprTokenKind.GreaterOrEqual:
                return Vb.Logical(Binary(AsInt(left), ">=", AsInt(right), Level.Comparison));
            case ExprTokenKind.Greater:
                return Vb.Logical(Binary(AsInt(left), ">", AsInt(right), Level.Comparison));
            case ExprTokenKind.Plus:
                return Vb.Integer(Binary(AsInt(left), "+", AsInt(right), Level.Additive));
            case ExprTokenKind.Minus:
                return Vb.Integer(Binary(AsInt(left), "-", AsInt(right), Level.Additive));
            case ExprTokenKind.Star:
                return Vb.Integer(Binary(AsInt(left), "*", AsInt(right), Level.Multiplicative));
            case ExprTokenKind.Slash:
                return Vb.Integer(Binary(AsInt(left), "\\", AsInt(right), Level.IntegerDivision));
            case ExprTokenKind.Percent:
                return Vb.Integer(Binary(AsInt(left), "Mod", AsInt(right), Level.Modulus));
            default:
                throw new InvalidOperationException("Neznamy binarny operator.");
        }
    }

    // logicka hodnota porovnana s 0 alebo 1 ostane logicka: x == 1 je x, x == 0 je Not x
    private static Vb? LogicalEquality(Vb logical, Vb constant, bool equal)
    {
        if (!logical.ZeroOne || logical.Constant is not null || constant.Constant is not (0 or 1))
            return null;

        return equal == (constant.Constant == 1) ? Vb.Logical(AsBool(logical)) : Vb.Logical(Not(AsBool(logical)));
    }

    private Vb Conditional(ExprConditionalNode node)
    {
        var condition = AsBool(Emit(node.Condition));
        var whenTrue = Emit(node.WhenTrue);
        var whenFalse = Emit(node.WhenFalse);
        return whenTrue.ZeroOne && whenFalse.ZeroOne
            ? Vb.Logical(If(condition, AsBool(whenTrue), AsBool(whenFalse)))
            : Vb.Integer(If(condition, AsInt(whenTrue), AsInt(whenFalse)));
    }

    private Vb Function(ExprFunctionNode node)
    {
        var arg = node.Argument;
        switch (node.Canonical)
        {
            case ExprFunction.CVLAKU:
                // cislo vlaku ako cislo - INISS cita veduce cislice (strtol), VB.NET Val
                if (arg is null)
                    return Vb.Integer(Atom("CInt(Val(.CVlaku))"));
                return arg is ExprStringNode number
                    ? Vb.Logical(EqualTo(Atom(".CVlaku"), Text(number.Value)))
                    : Vb.Logical(EqualTo(Atom("CInt(Val(.CVlaku))"), AsInt(Emit(arg))));

            case ExprFunction.POZICE:
                return Position(arg);

            case ExprFunction.TYP:
                if (arg is null)
                    return Vb.Integer(TypeIndex());
                return ExprEvaluator.TryFoldConstant(arg, out var index) ? TypeIs(index) : Vb.Logical(EqualTo(TypeIndex(), AsInt(Emit(arg))));

            case ExprFunction.PRIZNAK:
                if (arg is null)
                    return Masked(FlagBits, unchecked((int)0xFFFFFF00));
                return ExprEvaluator.TryFoldConstant(arg, out var mask)
                    ? Masked(FlagBits, mask)
                    : Vb.Integer(Binary(AsInt(Masked(FlagBits, unchecked((int)0xFFFFFF00))), "And", AsInt(Emit(arg)), Level.And));

            case ExprFunction.STAV:
                return State(arg);

            case ExprFunction.NAKLTYP:
                return TypeIn(_trainTypeKeys.Where(pair => ExprTrainTypes.IsCargo(pair.Value)).Select(pair => pair.Key).Order(StringComparer.Ordinal).ToList());

            case ExprFunction.VYLUKAZDE:
                return Vb.Logical(_site switch
                {
                    ExprEvalSite.ArrivalTable => Atom(".VylukaPrij"),
                    ExprEvalSite.DepartureTable => Atom(".VylukaOdj"),
                    _ => Binary(Atom(".VylukaPrij"), "Or", Atom(".VylukaOdj"), Level.Or)
                });

            case ExprFunction.ODKLON:
                return Vb.Logical(Atom(".JeOdklonen"));

            // INISS 3.39 meskanie drzi v sekundach a vracia cele minuty
            case ExprFunction.ZPOZDENIPRIJ:
                return Vb.Integer(Minutes(".ZpozdeniSkutecnePrijezd"));

            case ExprFunction.ZPOZDENIODJ:
                return Vb.Integer(Minutes(".ZpozdeniSkutecneOdjezd"));

            case ExprFunction.ZPOZDENI:
                return Vb.Integer(_site switch
                {
                    ExprEvalSite.ArrivalTable => Minutes(".ZpozdeniSkutecnePrijezd"),
                    ExprEvalSite.DepartureTable => Minutes(".ZpozdeniSkutecneOdjezd"),
                    _ => Atom($"Math.Max({Minutes(".ZpozdeniSkutecnePrijezd").Text}, {Minutes(".ZpozdeniSkutecneOdjezd").Text})")
                });

            case ExprFunction.DOBAPOBYTU:
                return Vb.Integer(Atom(".DobaPobytu"));

            case ExprFunction.PLANDOBAPOBYTU:
                return Vb.Integer(Atom(".PlanDobaPobytu"));

            case ExprFunction.ZAJMSTANICE:
                return ValueOrEquals(".Stanice.iSR70", arg);

            case ExprFunction.VYCHSTANICE:
                return ValueOrEquals(".VychStanice", arg);

            case ExprFunction.CILSTANICE:
                return ValueOrEquals(".CilStanice", arg);

            // miestny vlak v stanici zacina alebo konci
            case ExprFunction.MISTNI:
                return Vb.Logical(Local());

            case ExprFunction.CIZI:
                return Vb.Logical(Not(Local()));

            case ExprFunction.ZESMERU:
                return Vb.Logical(Atom($".ZeSmeru({AsInt(Emit(arg!)).Text})"));

            case ExprFunction.DOSMERU:
                return Vb.Logical(Atom($".DoSmeru({AsInt(Emit(arg!)).Text})"));

            case ExprFunction.KOLEJPRIJ:
                return Track(".KlicPozicePrijSkut", StringArgument(node));

            case ExprFunction.KOLEJODJ:
                return Track(".KlicPoziceOdjSkut", StringArgument(node));

            case ExprFunction.DATUMPRIJ:
                return Vb.Integer(Atom("CInt(.DatumPrij.ToOADate())"));

            case ExprFunction.DATUMODJ:
                return Vb.Integer(Atom("CInt(.DatumOdj.ToOADate())"));

            // INISS 3.39 v sekundach od polnoci, vlak bez casu ma cas druhej strany; INISS2 v minutach, bez casu -1
            case ExprFunction.CASPRIJ:
                return Vb.Integer(Binary(Atom("If(.CasPrij < 0, .CasOdj, .CasPrij)"), "*", Atom("60"), Level.Multiplicative));

            case ExprFunction.CASODJ:
                return Vb.Integer(Binary(Atom("If(.CasOdj < 0, .CasPrij, .CasOdj)"), "*", Atom("60"), Level.Multiplicative));

            case ExprFunction.INDCAT6:
                return Vb.Integer(Atom(".IndCat6()"));

            case ExprFunction.INDCAT8:
                return Vb.Integer(Atom(".IndCat8()"));

            case ExprFunction.DATE:
                return Vb.Integer(Atom("CInt(Now.Date.ToOADate())"));

            case ExprFunction.TIME:
                return Vb.Integer(new Code("Now.Hour * 3600 + Now.Minute * 60 + Now.Second", Level.Additive));

            // dopravca a nazov vlaku bez ohladu na velkost pismen (INISS ceske triedenie)
            case ExprFunction.OPERATOR:
                return Vb.Logical(EqualTo(Atom($"StrComp(.Dopravce, {Text(StringArgument(node)).Text}, CompareMethod.Text)"), Atom("0")));

            case ExprFunction.NAZVLAKU:
                return Vb.Logical(EqualTo(Atom($"StrComp(.NazVlaku, {Text(StringArgument(node)).Text}, CompareMethod.Text)"), Atom("0")));

            // linka sa v INISS2 zapisuje ako linka IDS bodu trasy; prvy a posledny bod ma jednu linku, preto vychadzajuci vlak
            // ma linku prichodu rovnaku ako odchodu a konciaci naopak - v 3.39 moze byt druha strana prazdna
            case ExprFunction.LINKAPRIJ:
                return Vb.Logical(EqualTo(Atom(".LinkaIDS(True)"), Text(StringArgument(node))));

            case ExprFunction.LINKAODJ:
                return Vb.Logical(EqualTo(Atom(".LinkaIDS(False)"), Text(StringArgument(node))));

            case ExprFunction.ZMENALINKY:
                return Vb.Logical(Binary(Atom(".LinkaIDS(True)"), "<>", Atom(".LinkaIDS(False)"), Level.Comparison));

            default:
                throw new InvalidOperationException("Neznama funkcia " + node.Function.Name + ".");
        }
    }

    private static Code Minutes(string delay) => Binary(Atom(delay), "\\", Atom("60"), Level.IntegerDivision);

    private static Code Local() => Binary(Atom(".JeVychozi"), "Or", Atom(".JeKoncici"), Level.Or);

    private static string StringArgument(ExprFunctionNode node) =>
        node.Argument is ExprStringNode text ? text.Value : throw new InvalidOperationException("Funkcia " + node.Function.Name + " potrebuje retazec.");

    private Vb ValueOrEquals(string value, ExprNode? arg) =>
        arg is null ? Vb.Integer(Atom(value)) : Vb.Logical(EqualTo(Atom(value), AsInt(Emit(arg))));

    private Vb Position(ExprNode? arg)
    {
        var value = Atom("If(.JeVychozi, 1, If(.JeKoncici, 3, 2))");
        if (arg is null)
            return Vb.Integer(value);

        if (!ExprEvaluator.TryFoldConstant(arg, out var position))
            return Vb.Logical(EqualTo(value, AsInt(Emit(arg))));

        return position switch
        {
            1 => Vb.Logical(Atom(".JeVychozi")),
            2 => Vb.Logical(Not(Local())),
            3 => Vb.Logical(Binary(Atom(".JeKoncici"), "And", Not(Atom(".JeVychozi")), Level.And)),
            _ => Vb.Number(0)
        };
    }

    // index zabudovaneho druhu INISSu podla kluca druhu vlaku; druh mimo TrTypes ma 0 ako v INISSe
    private Code TypeIndex()
    {
        var code = Atom("0");
        foreach (var group in _keysByIndex.OrderByDescending(group => group.Key))
            code = If(AsBool(TypeIn([.. group])), Literal(group.Key), code);
        return code;
    }

    private Vb TypeIs(int index) => TypeIn([.. _keysByIndex[index]]);

    private static Vb TypeIn(List<string> keys) =>
        keys.Count == 0 ? Vb.Number(0) : Vb.Logical(Chain(keys.Select(key => EqualTo(Atom(".Druh"), Text(key))), "Or", Level.Or));

    // vybrane bity slova priznakov: logicky tvar = niektory je nastaveny, ciselny = maska ako v INISSe
    private static Vb Masked((int Bit, Code Code)[] bits, int mask)
    {
        var selected = bits.Where(bit => (bit.Bit & mask) != 0).ToList();
        if (selected.Count == 0)
            return Vb.Number(0);

        var logical = Chain(selected.Select(bit => bit.Code), "Or", Level.Or);
        var number = Chain(selected.Select(bit => If(bit.Code, Literal(bit.Bit), Atom("0"))), "Or", Level.Or);
        return new Vb(logical, number, false);
    }

    private Vb State(ExprNode? arg)
    {
        var all = StateBits.Aggregate(0, (mask, bit) => mask | bit.Bit);
        if (arg is null)
            return Vb.Integer(AsInt(Masked(StateBits, all)));

        if (!ExprEvaluator.TryFoldConstant(arg, out var mask))
        {
            var word = AsInt(Masked(StateBits, all));
            var value = AsInt(Emit(arg));
            return Vb.Logical(EqualTo(Binary(word, "And", value, Level.And), value));
        }

        // STAV(x) plati, ked su nastavene vsetky bity x; bit, ktory INISS2 nepozna, nie je nastaveny nikdy
        if ((mask & ~all) != 0)
        {
            _problems.Add(new Iniss2ExprProblem(Iniss2ExprIssue.UnknownState, "0x" + mask.ToString("X", Inv)));
            return Vb.Number(0);
        }

        var required = StateBits.Where(bit => (bit.Bit & mask) != 0).Select(bit => bit.Code).ToList();
        return required.Count == 0 ? Vb.Number(1) : Vb.Logical(Chain(required, "And", Level.And));
    }

    private Vb Track(string member, string name)
    {
        if (name.Length == 0)
            return Vb.Logical(EqualTo(Atom(member), Text("")));

        return _trackKey(name) is { } key ? Vb.Logical(EqualTo(Atom(member), Text(key))) : Vb.Number(0);
    }

    private sealed class Symbols(IReadOnlyDictionary<string, int> trainTypeKeys) : IExprSymbolProvider
    {
        public IReadOnlyDictionary<string, int>? TrainTypeKeys => trainTypeKeys;
    }
}

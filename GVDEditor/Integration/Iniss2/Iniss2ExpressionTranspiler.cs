using System.Globalization;
using ToolsCore.Iniss.Expressions;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Preklad vyrazov INISS 3.39 (podmienky TabTab, dynamicke hodnoty StateDgm) do VB.NET servera INISS2. Vysledok je vyraz nad
/// clenmi <c>VlakS</c> v bloku <c>With VlakS</c>, preto cleny vlaku zacinaju bodkou. INISS 3.39 pocita len s celymi cislami
/// a pravda je nenulova hodnota; preklad drzi pri uzle logicky aj ciselny tvar a prevadza medzi nimi explicitne, aby VB.NET
/// (bez Option Strict) nepremenil True na -1. Spojky sa vyhodnocuju bez skratenia (<c>And</c>, <c>Or</c>) ako v INISSe.
/// Druh vlaku bez kluca v TrTypes a kolaj, ktora v grafikone nie je, sa prelozia na <c>False</c> - vlak ich mat nemoze.
/// </summary>
internal sealed class Iniss2ExpressionTranspiler
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    // bity slova priznakov vlaku 3.39 (Prizn_*) a ich obdoba vo VlakS; prestup je v INISS2 priznak bodu trasy
    private static readonly (int Bit, string Code)[] FlagBits =
    [
        (0x100, ".VylukaPrij"),
        (0x200, ".VylukaOdj"),
        (0x1000, "((.PriznakyVlaku And VlajkyVarianty.M) <> 0)"),
        (0x2000, "((.PriznakyVlaku And VlajkyVarianty.O) <> 0)"),
        (0x4000, "((.PriznakyVlaku And VlajkyVarianty.D) <> 0)"),
        (0x8000, "((.PriznakyVlaku And VlajkyVarianty.X) <> 0)"),
        (0x10000, "((.PriznakyVlaku And VlajkyVarianty.R) <> 0)"),
        (0x20000, "((.PriznakyVlaku And VlajkyVarianty.L) <> 0)"),
        (0x40000, "((.PriznakyVlaku And VlajkyVarianty.N) <> 0)"),
        (0x80000, "((.PriznakyStanice And VlajkyStanice.PR) <> 0)")
    ];

    // stavove bity 3.39 (Stav_*) a priznaky stavu INISS2
    private static readonly (int Bit, string Code)[] StateBits =
    [
        (0x20, "((.PriznakyStavu And NODEFLAGS.ODBAVEN) <> 0)"),
        (0x80, "((.PriznakyStavu And NODEFLAGS.STOJI) <> 0)")
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
    /// Prelozi ciselnu hodnotu (IndCat, AutoMode, AutoTimePointAdd…) na vyraz typu Integer; null = vyraz sa neda prelozit.
    /// </summary>
    public string? Number(string text) => Parse(text, ExprContext.Condition) is { } root ? Number(root) : null;

    /// <summary>
    /// Prelozi uzol na vyraz typu Boolean.
    /// </summary>
    public string Condition(ExprNode node) => AsBool(Emit(node));

    /// <summary>
    /// Prelozi uzol na vyraz typu Integer.
    /// </summary>
    public string Number(ExprNode node) => AsInt(Emit(node));

    /// <summary>
    /// Prelozi hodnotu kluca <c>Wait</c> na podmienku, ze prisli udalosti GTN; null = automatika na nic necaka.
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

        return WaitCondition(unchecked((uint)value));
    }

    /// <summary>
    /// Podmienka kluca <c>WaitPath</c> - nenulova hodnota caka na vchodovu vlakovu cestu.
    /// </summary>
    public static string WaitPath => WaitCondition(0x80000000u)!;

    /// <summary>
    /// Telo funkcie pre server: server doplni <c>Return</c>, len ked kod slovo <c>return</c> neobsahuje - napriklad v retazci.
    /// </summary>
    public static string Body(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return expression.Contains("return ", StringComparison.OrdinalIgnoreCase) ? "Return " + expression : expression;
    }

    private static string? WaitCondition(uint value)
    {
        var events = WaitEvents.Where(e => (value & e.Bit) != 0).Select(e => $"((.PriznakyGTN And StaniceVlaku.FLAGS.{e.Name}) <> 0)").ToList();
        return events.Count switch
        {
            0 => null,
            1 => events[0],
            _ => "(" + string.Join(" And ", events) + ")"
        };
    }

    private ExprNode? Parse(string text, ExprContext context)
    {
        var result = ExprParser.Parse(text, context, new Symbols(_trainTypeKeys));
        if (result.Root is { } root && result.Error is null)
            return root;

        _problems.Add(new Iniss2ExprProblem(Iniss2ExprIssue.ParseError, result.Error?.Message ?? text));
        return null;
    }

    // tvar uzla: logicky, ciselny alebo oba (maska PRIZNAK - logicky tvar plati, ked je niektory bit nastaveny)
    private sealed record Vb(string? Bool, string? Int, bool ZeroOne, int? Constant = null)
    {
        public static Vb Logical(string code) => new(code, null, true);

        public static Vb Integer(string code) => new(null, code, false);

        public static Vb Number(int value) => new(null, Literal(value), value is 0 or 1, value);
    }

    private static string AsBool(Vb value) =>
        value.Constant is { } constant ? constant != 0 ? "True" : "False" : value.Bool ?? $"({value.Int} <> 0)";

    private static string AsInt(Vb value) => value.Int ?? $"If({value.Bool}, 1, 0)";

    private static string Literal(int value) => value switch
    {
        int.MinValue => "(-2147483647 - 1)",
        < 0 => "(" + value.ToString(Inv) + ")",
        _ => value.ToString(Inv)
    };

    private static string Text(string value) => "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

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
            ExprTokenKind.Not or ExprTokenKind.NotWord => Vb.Logical($"(Not {AsBool(operand)})"),
            ExprTokenKind.BitNot => Vb.Integer($"(Not {AsInt(operand)})"),
            ExprTokenKind.Minus => Vb.Integer($"(-{AsInt(operand)})"),
            ExprTokenKind.Plus => operand,
            ExprTokenKind.Odd => Vb.Integer($"({AsInt(operand)} Mod 2)"),
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
                return Vb.Logical($"({AsBool(left)} And {AsBool(right)})");
            case ExprTokenKind.Or or ExprTokenKind.OrWord:
                return Vb.Logical($"({AsBool(left)} Or {AsBool(right)})");
            case ExprTokenKind.BitAnd or ExprTokenKind.BitOr or ExprTokenKind.BitXor:
            {
                var op = node.Operator == ExprTokenKind.BitAnd ? "And" : node.Operator == ExprTokenKind.BitOr ? "Or" : "Xor";
                // bitove spojky dvoch hodnot 0/1 su logicke, inak sa pocita s cislami (napr. maska PRIZNAK)
                return left.ZeroOne && right.ZeroOne
                    ? Vb.Logical($"({AsBool(left)} {op} {AsBool(right)})")
                    : Vb.Integer($"({AsInt(left)} {op} {AsInt(right)})");
            }
            case ExprTokenKind.Equal or ExprTokenKind.NotEqual:
                return LogicalEquality(left, right, node.Operator == ExprTokenKind.Equal)
                       ?? LogicalEquality(right, left, node.Operator == ExprTokenKind.Equal)
                       ?? Vb.Logical($"({AsInt(left)} {(node.Operator == ExprTokenKind.Equal ? "=" : "<>")} {AsInt(right)})");
            case ExprTokenKind.LessOrEqual:
                return Vb.Logical($"({AsInt(left)} <= {AsInt(right)})");
            case ExprTokenKind.Less:
                return Vb.Logical($"({AsInt(left)} < {AsInt(right)})");
            case ExprTokenKind.GreaterOrEqual:
                return Vb.Logical($"({AsInt(left)} >= {AsInt(right)})");
            case ExprTokenKind.Greater:
                return Vb.Logical($"({AsInt(left)} > {AsInt(right)})");
            case ExprTokenKind.Plus:
                return Vb.Integer($"({AsInt(left)} + {AsInt(right)})");
            case ExprTokenKind.Minus:
                return Vb.Integer($"({AsInt(left)} - {AsInt(right)})");
            case ExprTokenKind.Star:
                return Vb.Integer($"({AsInt(left)} * {AsInt(right)})");
            case ExprTokenKind.Slash:
                return Vb.Integer($"({AsInt(left)} \\ {AsInt(right)})");
            case ExprTokenKind.Percent:
                return Vb.Integer($"({AsInt(left)} Mod {AsInt(right)})");
            default:
                throw new InvalidOperationException("Neznamy binarny operator.");
        }
    }

    // logicka hodnota porovnana s 0 alebo 1 ostane logicka: x == 1 je x, x == 0 je Not x
    private static Vb? LogicalEquality(Vb logical, Vb constant, bool equal)
    {
        if (!logical.ZeroOne || logical.Constant is not null || constant.Constant is not (0 or 1))
            return null;

        return equal == (constant.Constant == 1) ? Vb.Logical(AsBool(logical)) : Vb.Logical($"(Not {AsBool(logical)})");
    }

    private Vb Conditional(ExprConditionalNode node)
    {
        var condition = AsBool(Emit(node.Condition));
        var whenTrue = Emit(node.WhenTrue);
        var whenFalse = Emit(node.WhenFalse);
        return whenTrue.ZeroOne && whenFalse.ZeroOne
            ? Vb.Logical($"If({condition}, {AsBool(whenTrue)}, {AsBool(whenFalse)})")
            : Vb.Integer($"If({condition}, {AsInt(whenTrue)}, {AsInt(whenFalse)})");
    }

    private Vb Function(ExprFunctionNode node)
    {
        var arg = node.Argument;
        switch (node.Canonical)
        {
            case ExprFunction.CVLAKU:
                // cislo vlaku ako cislo - INISS cita veduce cislice (strtol), VB.NET Val
                if (arg is null)
                    return Vb.Integer("CInt(Val(.CVlaku))");
                return arg is ExprStringNode number
                    ? Vb.Logical($"(.CVlaku = {Text(number.Value)})")
                    : Vb.Logical($"(CInt(Val(.CVlaku)) = {AsInt(Emit(arg))})");

            case ExprFunction.POZICE:
                return Position(arg);

            case ExprFunction.TYP:
                if (arg is null)
                    return Vb.Integer(TypeIndex());
                return ExprEvaluator.TryFoldConstant(arg, out var index) ? TypeIs(index) : Vb.Logical($"({TypeIndex()} = {AsInt(Emit(arg))})");

            case ExprFunction.PRIZNAK:
                if (arg is null)
                    return Masked(FlagBits, unchecked((int)0xFFFFFF00));
                return ExprEvaluator.TryFoldConstant(arg, out var mask)
                    ? Masked(FlagBits, mask)
                    : Vb.Integer($"({AsInt(Masked(FlagBits, unchecked((int)0xFFFFFF00)))} And {AsInt(Emit(arg))})");

            case ExprFunction.STAV:
                return State(arg);

            case ExprFunction.NAKLTYP:
                return TypeIn(_trainTypeKeys.Where(pair => ExprTrainTypes.IsCargo(pair.Value)).Select(pair => pair.Key).Order(StringComparer.Ordinal).ToList());

            case ExprFunction.VYLUKAZDE:
                return Vb.Logical(_site switch
                {
                    ExprEvalSite.ArrivalTable => ".VylukaPrij",
                    ExprEvalSite.DepartureTable => ".VylukaOdj",
                    _ => "(.VylukaPrij Or .VylukaOdj)"
                });

            case ExprFunction.ODKLON:
                return Vb.Logical(".JeOdklonen");

            // INISS 3.39 meskanie drzi v sekundach a vracia cele minuty
            case ExprFunction.ZPOZDENIPRIJ:
                return Vb.Integer("(.ZpozdeniSkutecnePrijezd \\ 60)");

            case ExprFunction.ZPOZDENIODJ:
                return Vb.Integer("(.ZpozdeniSkutecneOdjezd \\ 60)");

            case ExprFunction.ZPOZDENI:
                return Vb.Integer(_site switch
                {
                    ExprEvalSite.ArrivalTable => "(.ZpozdeniSkutecnePrijezd \\ 60)",
                    ExprEvalSite.DepartureTable => "(.ZpozdeniSkutecneOdjezd \\ 60)",
                    _ => "Math.Max(.ZpozdeniSkutecnePrijezd \\ 60, .ZpozdeniSkutecneOdjezd \\ 60)"
                });

            case ExprFunction.DOBAPOBYTU:
                return Vb.Integer(".DobaPobytu");

            case ExprFunction.PLANDOBAPOBYTU:
                return Vb.Integer(".PlanDobaPobytu");

            case ExprFunction.ZAJMSTANICE:
                return ValueOrEquals(".Stanice.iSR70", arg);

            case ExprFunction.VYCHSTANICE:
                return ValueOrEquals(".VychStanice", arg);

            case ExprFunction.CILSTANICE:
                return ValueOrEquals(".CilStanice", arg);

            // miestny vlak v stanici zacina alebo konci
            case ExprFunction.MISTNI:
                return Vb.Logical("(.JeVychozi Or .JeKoncici)");

            case ExprFunction.CIZI:
                return Vb.Logical("(Not (.JeVychozi Or .JeKoncici))");

            case ExprFunction.ZESMERU:
                return Vb.Logical($".ZeSmeru({AsInt(Emit(arg!))})");

            case ExprFunction.DOSMERU:
                return Vb.Logical($".DoSmeru({AsInt(Emit(arg!))})");

            case ExprFunction.KOLEJPRIJ:
                return Track(".KlicPozicePrijSkut", StringArgument(node));

            case ExprFunction.KOLEJODJ:
                return Track(".KlicPoziceOdjSkut", StringArgument(node));

            case ExprFunction.DATUMPRIJ:
                return Vb.Integer("CInt(.DatumPrij.ToOADate())");

            case ExprFunction.DATUMODJ:
                return Vb.Integer("CInt(.DatumOdj.ToOADate())");

            // INISS 3.39 v sekundach od polnoci, vlak bez casu ma cas druhej strany; INISS2 v minutach, bez casu -1
            case ExprFunction.CASPRIJ:
                return Vb.Integer("(If(.CasPrij < 0, .CasOdj, .CasPrij) * 60)");

            case ExprFunction.CASODJ:
                return Vb.Integer("(If(.CasOdj < 0, .CasPrij, .CasOdj) * 60)");

            case ExprFunction.INDCAT6:
                return Vb.Integer(".IndCat6()");

            case ExprFunction.INDCAT8:
                return Vb.Integer(".IndCat8()");

            case ExprFunction.DATE:
                return Vb.Integer("CInt(Now.Date.ToOADate())");

            case ExprFunction.TIME:
                return Vb.Integer("(Now.Hour * 3600 + Now.Minute * 60 + Now.Second)");

            // dopravca a nazov vlaku bez ohladu na velkost pismen (INISS ceske triedenie)
            case ExprFunction.OPERATOR:
                return Vb.Logical($"(StrComp(.Dopravce, {Text(StringArgument(node))}, CompareMethod.Text) = 0)");

            case ExprFunction.NAZVLAKU:
                return Vb.Logical($"(StrComp(.NazVlaku, {Text(StringArgument(node))}, CompareMethod.Text) = 0)");

            // linka sa v INISS2 zapisuje ako linka IDS bodu trasy; prvy a posledny bod ma jednu linku, preto vychadzajuci vlak
            // ma linku prichodu rovnaku ako odchodu a konciaci naopak - v 3.39 moze byt druha strana prazdna
            case ExprFunction.LINKAPRIJ:
                return Vb.Logical($"(.LinkaIDS(True) = {Text(StringArgument(node))})");

            case ExprFunction.LINKAODJ:
                return Vb.Logical($"(.LinkaIDS(False) = {Text(StringArgument(node))})");

            case ExprFunction.ZMENALINKY:
                return Vb.Logical("(.LinkaIDS(True) <> .LinkaIDS(False))");

            default:
                throw new InvalidOperationException("Neznama funkcia " + node.Function.Name + ".");
        }
    }

    private static string StringArgument(ExprFunctionNode node) =>
        node.Argument is ExprStringNode text ? text.Value : throw new InvalidOperationException("Funkcia " + node.Function.Name + " potrebuje retazec.");

    private Vb ValueOrEquals(string value, ExprNode? arg) =>
        arg is null ? Vb.Integer(value) : Vb.Logical($"({value} = {AsInt(Emit(arg))})");

    private Vb Position(ExprNode? arg)
    {
        const string value = "If(.JeVychozi, 1, If(.JeKoncici, 3, 2))";
        if (arg is null)
            return Vb.Integer(value);

        if (!ExprEvaluator.TryFoldConstant(arg, out var position))
            return Vb.Logical($"({value} = {AsInt(Emit(arg))})");

        return position switch
        {
            1 => Vb.Logical(".JeVychozi"),
            2 => Vb.Logical("(Not (.JeVychozi Or .JeKoncici))"),
            3 => Vb.Logical("(.JeKoncici And Not .JeVychozi)"),
            _ => Vb.Number(0)
        };
    }

    // index zabudovaneho druhu INISSu podla kluca druhu vlaku; druh mimo TrTypes ma 0 ako v INISSe
    private string TypeIndex()
    {
        var code = "0";
        foreach (var group in _keysByIndex.OrderByDescending(group => group.Key))
            code = $"If({AsBool(TypeIn([.. group]))}, {Literal(group.Key)}, {code})";
        return code;
    }

    private Vb TypeIs(int index) => TypeIn([.. _keysByIndex[index]]);

    private static Vb TypeIn(List<string> keys) => keys.Count switch
    {
        0 => Vb.Number(0),
        1 => Vb.Logical($"(.Druh = {Text(keys[0])})"),
        _ => Vb.Logical("(" + string.Join(" Or ", keys.Select(key => $".Druh = {Text(key)}")) + ")")
    };

    // vybrane bity slova priznakov: logicky tvar = niektory je nastaveny, ciselny = maska ako v INISSe
    private static Vb Masked((int Bit, string Code)[] bits, int mask)
    {
        var selected = bits.Where(bit => (bit.Bit & mask) != 0).ToList();
        if (selected.Count == 0)
            return Vb.Number(0);

        var logical = selected.Count == 1 ? selected[0].Code : "(" + string.Join(" Or ", selected.Select(bit => bit.Code)) + ")";
        var number = selected.Count == 1
            ? $"If({selected[0].Code}, {Literal(selected[0].Bit)}, 0)"
            : "(" + string.Join(" Or ", selected.Select(bit => $"If({bit.Code}, {Literal(bit.Bit)}, 0)")) + ")";
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
            return Vb.Logical($"(({word} And {value}) = {value})");
        }

        // STAV(x) plati, ked su nastavene vsetky bity x; bit, ktory INISS2 nepozna, nie je nastaveny nikdy
        if ((mask & ~all) != 0)
        {
            _problems.Add(new Iniss2ExprProblem(Iniss2ExprIssue.UnknownState, "0x" + mask.ToString("X", Inv)));
            return Vb.Number(0);
        }

        var required = StateBits.Where(bit => (bit.Bit & mask) != 0).Select(bit => bit.Code).ToList();
        return required.Count switch
        {
            0 => Vb.Number(1),
            1 => Vb.Logical(required[0]),
            _ => Vb.Logical("(" + string.Join(" And ", required) + ")")
        };
    }

    private Vb Track(string member, string name)
    {
        if (name.Length == 0)
            return Vb.Logical($"({member} = \"\")");

        return _trackKey(name) is { } key ? Vb.Logical($"({member} = {Text(key)})") : Vb.Number(0);
    }

    private sealed class Symbols(IReadOnlyDictionary<string, int> trainTypeKeys) : IExprSymbolProvider
    {
        public IReadOnlyDictionary<string, int>? TrainTypeKeys => trainTypeKeys;
    }
}

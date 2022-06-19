using System.Reflection;
using AutocompleteMenuNS;
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.Tools;

internal class FunctionItem : AutocompleteItem
{
    public FunctionItem(string fname, string text, string desc = "", FunReturnType returns = FunReturnType.Bool,
        params FunParameter[] parameters) : base(text)
    {
        ImageIndex = 0;

        var titleB = new StringBuilder();
        titleB.Append(fname);
        if (parameters.Length != 0)
        {
            titleB.Append('(');
            foreach (var par in parameters)
            {
                titleB.Append(par.Type.ToString() + ' ');
                titleB.Append(par.Name);
            }

            titleB.Append(')');
        }

        base.ToolTipTitle = $"(funkcia) {returns} {titleB}";
        base.MenuText = titleB.ToString();

        base.ToolTipText = desc;
        FunctionName = fname;
    }

    public string FunctionName { get; }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpper()))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpper()))
            return CompareResult.Visible;
        return CompareResult.Hidden;
    }
}

internal class ConstantItem : AutocompleteItem
{
    public ConstantItem(string cname, string text, string desc = "") : base(text)
    {
        ImageIndex = 1;
        base.ToolTipTitle = "(konštanta) " + cname;
        base.MenuText = cname;
        base.ToolTipText = desc;
        ConstName = cname;
    }

    public string ConstName { get; }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpper()))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpper()))
            return CompareResult.Visible;

        return CompareResult.Hidden;
    }
}

internal class EventItem : AutocompleteItem
{
    public EventItem(string ename, string text, string desc = "") : base(text)
    {
        ImageIndex = 2;
        base.ToolTipTitle = "(udalosť) " + ename;
        base.MenuText = ename;
        base.ToolTipText = desc;
    }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpper()))
            return CompareResult.VisibleAndSelected;
        return CompareResult.Hidden;
    }
}

internal static class TabTabACItems
{
    private static readonly FunParameter pOperator = new(FunParameterType.String, "názov");
    private static readonly FunParameter pTypVlaku = new(FunParameterType.TypVlaku, "typ");
    private static readonly FunParameter pStavVlaku = new(FunParameterType.StavVlaku, "stav");
    private static readonly FunParameter pPriznak = new(FunParameterType.Priznak, "príznak");
    private static readonly FunParameter pStID = new(FunParameterType.Int, "id_stanice");
    public static readonly FunctionItem Operator = new("OPERATOR", "OPERATOR(\"\")", "Zistí, či sa dopravca daného vlaku rovná zadanému názvu dopravcu.", FunReturnType.Bool, pOperator);
    public static readonly FunctionItem CVlaku = new("CVLAKU", "CVLAKU", "Vracia číslo daného vlaku.", FunReturnType.Int);
    public static readonly FunctionItem Pozice = new("POZICE", "POZICE", "Vracia pozíciu vlaku v stanici (vychádza, prechádza, končí).");
    public static readonly FunctionItem Typ = new("TYP", "TYP()", "Zistí, či sa typ vlaku rovná zadanému typu vlaku.", FunReturnType.Bool, pTypVlaku);
    public static readonly FunctionItem Priznak = new("PRIZNAK", "PRIZNAK()", "Zistí, či má daný vlak príznak rovný zadanému príznaku.", FunReturnType.Bool, pPriznak);
    public static readonly FunctionItem Stav = new("STAV", "STAV()", "Zistí, či sa stav vlaku rovná zadanému stavu vlaku.", FunReturnType.Bool, pStavVlaku);
    public static readonly FunctionItem NaklTyp = new("NAKLTYP", "NAKLTYP", "Vracia, či je vlak nákladného typu.");
    public static readonly FunctionItem VylukaTu = new("VYLUKAZDE", "VYLUKAZDE", "Vracia, či má vlak v tejto stanici výluku.");
    public static readonly FunctionItem Odklon = new("ODKLON", "ODKLON", "Vracia, či má vlak odklonenú trasu.");
    public static readonly FunctionItem MeskaniePrichod = new("ZPOZDENIPRIJ", "ZPOZDENIPRIJ");
    public static readonly FunctionItem MeskanieOdchod = new("ZPOZDENIODJ", "ZPOZDENIODJ");
    public static readonly FunctionItem Meskanie = new("ZPOZDENI", "ZPOZDENI", "Vracia, či má vlak nejaké meškanie.");
    public static readonly FunctionItem DobaPobytu = new("DOBAPOBYTU", "DOBAPOBYTU");
    public static readonly FunctionItem PlanDobaPobytu = new("PLANDOBAPOBYTU", "PLANDOBAPOBYTU");
    public static readonly FunctionItem ZaujmovaStanica = new("ZAJMOSTANICE", "ZAJMOSTANICE", "Zistí, či sa záujmová stanica rovná zadanému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem VychodziaStanica = new("VYCHSTANICE", "VYCHSTANICE", "Zistí, či sa východzia stanica vlaku rovná zadanenému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem KonecnaStanica = new("CILSTANICE", "CILSTANICE", "Zistí, či sa konečná stanica vlaku rovná zadanému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem Miestne = new("MISTNI", "MISTNI");
    public static readonly FunctionItem Cudzie = new("CIZI", "CIZI");
    public static readonly FunctionItem ZoSmeru = new("ZESMERU", "ZESMERU()", "", FunReturnType.Bool, pStID);
    public static readonly FunctionItem DoSmeru = new("DOSMERU", "DOSMERU()", "", FunReturnType.Bool, pStID);
    public static readonly FunctionItem HomeStation = new("HOMESTATION", "HOMESTATION()", "Zistí, či sa záujmová stanica rovná zadanenému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem BaseStation = new("BASESTATION", "BASESTATION()", "Zistí, či sa východzia stanica vlaku rovná zadanenému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem EndStation = new("ENDSTATION", "ENDSTATION()", "Zistí, či sa konečná stanica vlaku rovná zadanenému ID stanice.", FunReturnType.Bool, pStID);
    public static readonly FunctionItem KolajPrichod = new("KOLEJPRIJ", "KOLEJPRIJ(\"\")");
    public static readonly FunctionItem KolajOdchod = new("KOLEJODJ", "KOLEJODJ(\"\")");
    public static readonly FunctionItem DatumPrichod = new("DATUMPRIJ", "DATUMPRIJ");
    public static readonly FunctionItem DatumOdchod = new("DATUMODJ", "DATUMODJ");
    public static readonly FunctionItem CasPrichod = new("CASPRIJ", "CASPRIJ");
    public static readonly FunctionItem CasOdchod = new("CASODJ", "CASODJ");
    public static readonly FunctionItem IndCat6 = new("INDCAT6", "INDCAT6");
    public static readonly FunctionItem IndCat8 = new("INDCAT8", "INDCAT8");
    public static readonly FunctionItem Date = new("DATE", "DATE");
    public static readonly FunctionItem Time = new("TIME", "TIME");

    public static readonly ConstantItem PozV = new("Poz_V", "Poz_V", "Pozícia Vychádzajúci");
    public static readonly ConstantItem PozP = new("Poz_P", "Poz_P", "Pozícia Prechádzajúci");
    public static readonly ConstantItem PozK = new("Poz_K", "Poz_K", "Pozícia Končiaci");
    public static readonly ConstantItem PriznVyl = new("Prizn_Vyl", "Prizn_Vyl", "Príznak Vlak vo výluke");
    public static readonly ConstantItem PriznVylP = new("Prizn_VylP", "Prizn_VylP", "Príznak Vlak vo výluke na príchode");
    public static readonly ConstantItem PriznVylO = new("Prizn_VylO", "Prizn_VylO", "Príznak Vlak vo výluke na odchode");
    public static readonly ConstantItem PriznM = new("Prizn_M", "Prizn_M", "Vlak má príznak Medzištátny");
    public static readonly ConstantItem PriznO = new("Prizn_O", "Prizn_O", "Vlak má príznak XXX");
    public static readonly ConstantItem PriznD = new("Prizn_D", "Prizn_D", "Vlak má príznak Diaľkový");
    public static readonly ConstantItem PriznX = new("Prizn_X", "Prizn_X", "Vlak má príznak Mimoriadny");
    public static readonly ConstantItem PriznR = new("Prizn_R", "Prizn_R", "Vlak má príznak Miestenkový");
    public static readonly ConstantItem PriznL = new("Prizn_L", "Prizn_L", "Vlak má príznak Iba lôžkový");
    public static readonly ConstantItem PriznN = new("Prizn_N", "Prizn_N", "Vlak má príznak Nízkopodlažný");
    public static readonly ConstantItem PriznPre = new("Prizn_Pre", "Prizn_Pre");
    public static readonly ConstantItem StavOdbaveny = new("Stav_Odbaven", "Stav_Odbaven");
    public static readonly ConstantItem StavStoji = new("Stav_Stoji", "Stav_Stoji");

    public static readonly ConstantItem TypOs = new("Typ_Os", "Typ_Os", "Typ osobný vlak");
    public static readonly ConstantItem TypZr = new("Typ_Zr", "Typ_Zr", "Typ zrýchlený vlak");
    public static readonly ConstantItem TypBus = new("Typ_Bus", "Typ_Bus", "Typ autobus");
    public static readonly ConstantItem TypR = new("Typ_R", "Typ_R", "Typ rýchlik");
    public static readonly ConstantItem TypEx = new("Typ_Ex", "Typ_Ex", "Typ expres");
    public static readonly ConstantItem TypREX = new("Typ_REX", "Typ_REX", "Typ regionálny expres");
    public static readonly ConstantItem TypER = new("Typ_ER", "Typ_ER", "Typ EuroRegional");
    public static readonly ConstantItem TypRJ = new("Typ_RJ", "Typ_RJ", "Typ RegioJet");
    public static readonly ConstantItem TypRR = new("Typ_RR", "Typ_RR", "Typ regionálny rýchlik");
    public static readonly ConstantItem TypEC = new("Typ_EC", "Typ_EC", "Typ EuroCity");
    public static readonly ConstantItem TypIC = new("Typ_IC", "Typ_IC", "Typ InterCity");
    public static readonly ConstantItem TypSC = new("Typ_SC", "Typ_SC", "Typ SuperCity");
    public static readonly ConstantItem TypEN = new("Typ_EN", "Typ_EN", "Typ EuroNight");

    public static readonly ConstantItem TypX1 = new("Typ_X1", "Typ_X1", "Používateľský typ vlaku X1");
    public static readonly ConstantItem TypX2 = new("Typ_X2", "Typ_X2", "Používateľský typ vlaku X2");
    public static readonly ConstantItem TypX3 = new("Typ_X3", "Typ_X3", "Používateľský typ vlaku X3");
    public static readonly ConstantItem TypX4 = new("Typ_X4", "Typ_X4", "Používateľský typ vlaku X4");
    public static readonly ConstantItem TypX5 = new("Typ_X5", "Typ_X5", "Používateľský typ vlaku X5");
    public static readonly ConstantItem TypX6 = new("Typ_X6", "Typ_X6", "Používateľský typ vlaku X6");
    public static readonly ConstantItem TypX7 = new("Typ_X7", "Typ_X7", "Používateľský typ vlaku X7");
    public static readonly ConstantItem TypX8 = new("Typ_X8", "Typ_X8", "Používateľský typ vlaku X8");
    public static readonly ConstantItem TypX9 = new("Typ_X9", "Typ_X9", "Používateľský typ vlaku X9");
    public static readonly ConstantItem TypR1 = new("Typ_R1", "Typ_R1", "Používateľský typ vlaku R1 (typu rýchlik)");
    public static readonly ConstantItem TypR2 = new("Typ_R2", "Typ_R2", "Používateľský typ vlaku R2 (typu rýchlik)");
    public static readonly ConstantItem TypR3 = new("Typ_R3", "Typ_R3", "Používateľský typ vlaku R3 (typu rýchlik)");
    public static readonly ConstantItem TypR4 = new("Typ_R4", "Typ_R4", "Používateľský typ vlaku R4 (typu rýchlik)");
    public static readonly ConstantItem TypR5 = new("Typ_R5", "Typ_R5", "Používateľský typ vlaku R5 (typu rýchlik)");
    public static readonly ConstantItem TypR6 = new("Typ_R6", "Typ_R6", "Používateľský typ vlaku R6 (typu rýchlik)");
    public static readonly ConstantItem TypR7 = new("Typ_R7", "Typ_R7", "Používateľský typ vlaku R7 (typu rýchlik)");
    public static readonly ConstantItem TypR8 = new("Typ_R8", "Typ_R8", "Používateľský typ vlaku R8 (typu rýchlik)");
    public static readonly ConstantItem TypR9 = new("Typ_R9", "Typ_R9", "Používateľský typ vlaku R9 (typu rýchlik)");
    public static readonly ConstantItem TypOs1 = new("Typ_Os1", "Typ_Os1", "Používateľský typ vlaku Os1 (typu Os)");
    public static readonly ConstantItem TypOs2 = new("Typ_Os2", "Typ_Os2", "Používateľský typ vlaku Os2 (typu Os)");
    public static readonly ConstantItem TypOs3 = new("Typ_Os3", "Typ_Os3", "Používateľský typ vlaku Os3 (typu Os)");
    public static readonly ConstantItem TypOs4 = new("Typ_Os4", "Typ_Os4", "Používateľský typ vlaku Os4 (typu Os)");
    public static readonly ConstantItem TypOs5 = new("Typ_Os5", "Typ_Os5", "Používateľský typ vlaku Os5 (typu Os)");
    public static readonly ConstantItem TypOs6 = new("Typ_Os6", "Typ_Os6", "Používateľský typ vlaku Os6 (typu Os)");
    public static readonly ConstantItem TypOs7 = new("Typ_Os7", "Typ_Os7", "Používateľský typ vlaku Os7 (typu Os)");
    public static readonly ConstantItem TypOs8 = new("Typ_Os8", "Typ_Os8", "Používateľský typ vlaku Os8 (typu Os)");
    public static readonly ConstantItem TypOs9 = new("Typ_Os9", "Typ_Os9", "Používateľský typ vlaku Os9 (typu Os)");
    public static readonly ConstantItem TypSl1 = new("Typ_Sl1", "Typ_Sl1", "Používateľský typ vlaku Sl1 (typu služobný vlak)");
    public static readonly ConstantItem TypSl2 = new("Typ_Sl2", "Typ_Sl2", "Používateľský typ vlaku Sl2 (typu služobný vlak)");
    public static readonly ConstantItem TypSl3 = new("Typ_Sl3", "Typ_Sl3", "Používateľský typ vlaku Sl3 (typu služobný vlak)");
    public static readonly ConstantItem TypSl4 = new("Typ_Sl4", "Typ_Sl4", "Používateľský typ vlaku Sl4 (typu služobný vlak)");
    public static readonly ConstantItem TypSl5 = new("Typ_Sl5", "Typ_Sl5", "Používateľský typ vlaku Sl5 (typu služobný vlak)");
    public static readonly ConstantItem TypSl6 = new("Typ_Sl6", "Typ_Sl6", "Používateľský typ vlaku Sl6 (typu služobný vlak)");
    public static readonly ConstantItem TypSl7 = new("Typ_Sl7", "Typ_Sl7", "Používateľský typ vlaku Sl7 (typu služobný vlak)");
    public static readonly ConstantItem TypSl8 = new("Typ_Sl8", "Typ_Sl8", "Používateľský typ vlaku Sl8 (typu služobný vlak)");
    public static readonly ConstantItem TypSl9 = new("Typ_Sl9", "Typ_Sl9", "Používateľský typ vlaku Sl9 (typu služobný vlak)");

    public static readonly EventItem SWITCH = new("#SWITCH", "#SWITCH", "Text sa bude preblikávať. Ak text obsahuje '#', budú sa časti textu rezdelené týmto znakom preblikávať navzájom.");

    public static readonly EventItem MERGE = new("#MERGE", "#MERGE");
    public static readonly EventItem VYLUKA = new("#VYLUKA", "#VYLUKA");
    public static readonly EventItem ODKLON = new("#ODKLON", "#ODKLON");

    public static IEnumerable<AutocompleteItem> GetItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Select(field => field.GetValue(null) as AutocompleteItem).ToList();
    }

    public static IEnumerable<FunctionItem> GetFunctionItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is FunctionItem).Select(field => field.GetValue(null) as FunctionItem).ToList();
    }

    public static IEnumerable<ConstantItem> GetConstantItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is ConstantItem).Select(field => field.GetValue(null) as ConstantItem).ToList();
    }

    public static IEnumerable<EventItem> GetEventItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is EventItem).Select(field => field.GetValue(null) as EventItem).ToList();
    }
}

internal enum FunReturnType
{
    Bool,
    Int
}

internal enum FunParameterType
{
    Bool,
    Int,
    String,
    TypVlaku,
    StavVlaku,
    Priznak
}

internal readonly struct FunParameter
{
    public readonly FunParameterType Type;
    public readonly string Name;

    public FunParameter(FunParameterType type, string name)
    {
        Type = type;
        Name = name;
    }
}
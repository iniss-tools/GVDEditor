using System.Globalization;
using System.Reflection;
using AutocompleteMenuNS;
using GVDEditor.Properties;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.TabTabEditor;

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
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpperInvariant()))
            return CompareResult.Visible;
        return CompareResult.Hidden;
    }
}

internal class ConstantItem : AutocompleteItem
{
    public ConstantItem(string cname, string text, string desc = "") : base(text)
    {
        ImageIndex = 1;
        base.ToolTipTitle = Resources.TabTabAC_Constant + cname;
        base.MenuText = cname;
        base.ToolTipText = desc;
        ConstName = cname;
    }

    public string ConstName { get; }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpperInvariant()))
            return CompareResult.Visible;

        return CompareResult.Hidden;
    }
}

internal class EventItem : AutocompleteItem
{
    public EventItem(string ename, string text, string desc = "") : base(text)
    {
        ImageIndex = 2;
        base.ToolTipTitle = Resources.TabTabAC_Event + ename;
        base.MenuText = ename;
        base.ToolTipText = desc;
    }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        return CompareResult.Hidden;
    }
}

internal static class TabTabACItems
{
    private static readonly FunParameter pOperator = new(FunParameterType.String, Resources.TabTabAC_ParamName);
    private static readonly FunParameter pTypVlaku = new(FunParameterType.TypVlaku, "typ");
    private static readonly FunParameter pStavVlaku = new(FunParameterType.StavVlaku, "stav");
    private static readonly FunParameter pPriznak = new(FunParameterType.Priznak, Resources.TabTabAC_ParamFlag);
    private static readonly FunParameter pStID = new(FunParameterType.Int, "id_stanice");
    public static readonly FunctionItem Operator = new("OPERATOR", "OPERATOR(\"\")", Resources.TabTabAC_Operator, FunReturnType.Bool, pOperator);
    public static readonly FunctionItem CVlaku = new("CVLAKU", "CVLAKU", Resources.TabTabAC_CVlaku, FunReturnType.Int);
    public static readonly FunctionItem Pozice = new("POZICE", "POZICE", Resources.TabTabAC_Pozice);
    public static readonly FunctionItem Typ = new("TYP", "TYP()", Resources.TabTabAC_Typ, FunReturnType.Bool, pTypVlaku);
    public static readonly FunctionItem Priznak = new("PRIZNAK", "PRIZNAK()", Resources.TabTabAC_Priznak, FunReturnType.Bool, pPriznak);
    public static readonly FunctionItem Stav = new("STAV", "STAV()", Resources.TabTabAC_Stav, FunReturnType.Bool, pStavVlaku);
    public static readonly FunctionItem NaklTyp = new("NAKLTYP", "NAKLTYP", Resources.TabTabAC_NaklTyp);
    public static readonly FunctionItem VylukaTu = new("VYLUKAZDE", "VYLUKAZDE", Resources.TabTabAC_VylukaZde);
    public static readonly FunctionItem Odklon = new("ODKLON", "ODKLON", Resources.TabTabAC_Odklon);
    public static readonly FunctionItem MeskaniePrichod = new("ZPOZDENIPRIJ", "ZPOZDENIPRIJ");
    public static readonly FunctionItem MeskanieOdchod = new("ZPOZDENIODJ", "ZPOZDENIODJ");
    public static readonly FunctionItem Meskanie = new("ZPOZDENI", "ZPOZDENI", Resources.TabTabAC_Zpozdeni);
    public static readonly FunctionItem DobaPobytu = new("DOBAPOBYTU", "DOBAPOBYTU");
    public static readonly FunctionItem PlanDobaPobytu = new("PLANDOBAPOBYTU", "PLANDOBAPOBYTU");
    public static readonly FunctionItem ZaujmovaStanica = new("ZAJMOSTANICE", "ZAJMOSTANICE", Resources.TabTabAC_HomeStation, FunReturnType.Bool, pStID);
    public static readonly FunctionItem VychodziaStanica = new("VYCHSTANICE", "VYCHSTANICE", Resources.TabTabAC_BaseStation, FunReturnType.Bool, pStID);
    public static readonly FunctionItem KonecnaStanica = new("CILSTANICE", "CILSTANICE", Resources.TabTabAC_EndStation, FunReturnType.Bool, pStID);
    public static readonly FunctionItem Miestne = new("MISTNI", "MISTNI");
    public static readonly FunctionItem Cudzie = new("CIZI", "CIZI");
    public static readonly FunctionItem ZoSmeru = new("ZESMERU", "ZESMERU()", "", FunReturnType.Bool, pStID);
    public static readonly FunctionItem DoSmeru = new("DOSMERU", "DOSMERU()", "", FunReturnType.Bool, pStID);
    public static readonly FunctionItem HomeStation = new("HOMESTATION", "HOMESTATION()", Resources.TabTabAC_HomeStation, FunReturnType.Bool, pStID);
    public static readonly FunctionItem BaseStation = new("BASESTATION", "BASESTATION()", Resources.TabTabAC_BaseStation, FunReturnType.Bool, pStID);
    public static readonly FunctionItem EndStation = new("ENDSTATION", "ENDSTATION()", Resources.TabTabAC_EndStation, FunReturnType.Bool, pStID);
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

    public static readonly ConstantItem PozV = new("Poz_V", "Poz_V", Resources.TabTabAC_PozV);
    public static readonly ConstantItem PozP = new("Poz_P", "Poz_P", Resources.TabTabAC_PozP);
    public static readonly ConstantItem PozK = new("Poz_K", "Poz_K", Resources.TabTabAC_PozK);
    public static readonly ConstantItem PriznVyl = new("Prizn_Vyl", "Prizn_Vyl", Resources.TabTabAC_PriznVyl);
    public static readonly ConstantItem PriznVylP = new("Prizn_VylP", "Prizn_VylP", Resources.TabTabAC_PriznVylP);
    public static readonly ConstantItem PriznVylO = new("Prizn_VylO", "Prizn_VylO", Resources.TabTabAC_PriznVylO);
    public static readonly ConstantItem PriznM = new("Prizn_M", "Prizn_M", Resources.TabTabAC_PriznM);
    public static readonly ConstantItem PriznO = new("Prizn_O", "Prizn_O", Resources.TabTabAC_PriznO);
    public static readonly ConstantItem PriznD = new("Prizn_D", "Prizn_D", Resources.TabTabAC_PriznD);
    public static readonly ConstantItem PriznX = new("Prizn_X", "Prizn_X", Resources.TabTabAC_PriznX);
    public static readonly ConstantItem PriznR = new("Prizn_R", "Prizn_R", Resources.TabTabAC_PriznR);
    public static readonly ConstantItem PriznL = new("Prizn_L", "Prizn_L", Resources.TabTabAC_PriznL);
    public static readonly ConstantItem PriznN = new("Prizn_N", "Prizn_N", Resources.TabTabAC_PriznN);
    public static readonly ConstantItem PriznPre = new("Prizn_Pre", "Prizn_Pre");
    public static readonly ConstantItem StavOdbaveny = new("Stav_Odbaven", "Stav_Odbaven");
    public static readonly ConstantItem StavStoji = new("Stav_Stoji", "Stav_Stoji");

    public static readonly ConstantItem TypOs = new("Typ_Os", "Typ_Os", Resources.TabTabAC_TypOs);
    public static readonly ConstantItem TypZr = new("Typ_Zr", "Typ_Zr", Resources.TabTabAC_TypZr);
    public static readonly ConstantItem TypBus = new("Typ_Bus", "Typ_Bus", Resources.TabTabAC_TypBus);
    public static readonly ConstantItem TypR = new("Typ_R", "Typ_R", Resources.TabTabAC_TypR);
    public static readonly ConstantItem TypEx = new("Typ_Ex", "Typ_Ex", Resources.TabTabAC_TypEx);
    public static readonly ConstantItem TypREX = new("Typ_REX", "Typ_REX", Resources.TabTabAC_TypREX);
    public static readonly ConstantItem TypER = new("Typ_ER", "Typ_ER", "Typ EuroRegional");
    public static readonly ConstantItem TypRJ = new("Typ_RJ", "Typ_RJ", "Typ RegioJet");
    public static readonly ConstantItem TypRR = new("Typ_RR", "Typ_RR", Resources.TabTabAC_TypRR);
    public static readonly ConstantItem TypEC = new("Typ_EC", "Typ_EC", "Typ EuroCity");
    public static readonly ConstantItem TypIC = new("Typ_IC", "Typ_IC", "Typ InterCity");
    public static readonly ConstantItem TypSC = new("Typ_SC", "Typ_SC", "Typ SuperCity");
    public static readonly ConstantItem TypEN = new("Typ_EN", "Typ_EN", "Typ EuroNight");

    public static readonly ConstantItem TypX1 = new("Typ_X1", "Typ_X1", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X1"));
    public static readonly ConstantItem TypX2 = new("Typ_X2", "Typ_X2", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X2"));
    public static readonly ConstantItem TypX3 = new("Typ_X3", "Typ_X3", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X3"));
    public static readonly ConstantItem TypX4 = new("Typ_X4", "Typ_X4", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X4"));
    public static readonly ConstantItem TypX5 = new("Typ_X5", "Typ_X5", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X5"));
    public static readonly ConstantItem TypX6 = new("Typ_X6", "Typ_X6", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X6"));
    public static readonly ConstantItem TypX7 = new("Typ_X7", "Typ_X7", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X7"));
    public static readonly ConstantItem TypX8 = new("Typ_X8", "Typ_X8", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X8"));
    public static readonly ConstantItem TypX9 = new("Typ_X9", "Typ_X9", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserType, "X9"));
    public static readonly ConstantItem TypR1 = new("Typ_R1", "Typ_R1", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R1"));
    public static readonly ConstantItem TypR2 = new("Typ_R2", "Typ_R2", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R2"));
    public static readonly ConstantItem TypR3 = new("Typ_R3", "Typ_R3", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R3"));
    public static readonly ConstantItem TypR4 = new("Typ_R4", "Typ_R4", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R4"));
    public static readonly ConstantItem TypR5 = new("Typ_R5", "Typ_R5", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R5"));
    public static readonly ConstantItem TypR6 = new("Typ_R6", "Typ_R6", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R6"));
    public static readonly ConstantItem TypR7 = new("Typ_R7", "Typ_R7", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R7"));
    public static readonly ConstantItem TypR8 = new("Typ_R8", "Typ_R8", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R8"));
    public static readonly ConstantItem TypR9 = new("Typ_R9", "Typ_R9", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeR, "R9"));
    public static readonly ConstantItem TypOs1 = new("Typ_Os1", "Typ_Os1", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os1"));
    public static readonly ConstantItem TypOs2 = new("Typ_Os2", "Typ_Os2", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os2"));
    public static readonly ConstantItem TypOs3 = new("Typ_Os3", "Typ_Os3", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os3"));
    public static readonly ConstantItem TypOs4 = new("Typ_Os4", "Typ_Os4", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os4"));
    public static readonly ConstantItem TypOs5 = new("Typ_Os5", "Typ_Os5", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os5"));
    public static readonly ConstantItem TypOs6 = new("Typ_Os6", "Typ_Os6", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os6"));
    public static readonly ConstantItem TypOs7 = new("Typ_Os7", "Typ_Os7", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os7"));
    public static readonly ConstantItem TypOs8 = new("Typ_Os8", "Typ_Os8", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os8"));
    public static readonly ConstantItem TypOs9 = new("Typ_Os9", "Typ_Os9", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeOs, "Os9"));
    public static readonly ConstantItem TypSl1 = new("Typ_Sl1", "Typ_Sl1", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl1"));
    public static readonly ConstantItem TypSl2 = new("Typ_Sl2", "Typ_Sl2", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl2"));
    public static readonly ConstantItem TypSl3 = new("Typ_Sl3", "Typ_Sl3", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl3"));
    public static readonly ConstantItem TypSl4 = new("Typ_Sl4", "Typ_Sl4", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl4"));
    public static readonly ConstantItem TypSl5 = new("Typ_Sl5", "Typ_Sl5", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl5"));
    public static readonly ConstantItem TypSl6 = new("Typ_Sl6", "Typ_Sl6", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl6"));
    public static readonly ConstantItem TypSl7 = new("Typ_Sl7", "Typ_Sl7", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl7"));
    public static readonly ConstantItem TypSl8 = new("Typ_Sl8", "Typ_Sl8", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl8"));
    public static readonly ConstantItem TypSl9 = new("Typ_Sl9", "Typ_Sl9", string.Format(CultureInfo.CurrentCulture, Resources.TabTabAC_UserTypeSl, "Sl9"));

    public static readonly EventItem SWITCH = new("#SWITCH", "#SWITCH", Resources.TabTabAC_Switch);

    public static readonly EventItem MERGE = new("#MERGE", "#MERGE");
    public static readonly EventItem VYLUKA = new("#VYLUKA", "#VYLUKA");
    public static readonly EventItem ODKLON = new("#ODKLON", "#ODKLON");

    public static IEnumerable<AutocompleteItem> GetItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Select(field => (AutocompleteItem)field.GetValue(null)!).ToList();
    }

    public static IEnumerable<FunctionItem> GetFunctionItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is FunctionItem).Select(field => (FunctionItem)field.GetValue(null)!).ToList();
    }

    public static IEnumerable<ConstantItem> GetConstantItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is ConstantItem).Select(field => (ConstantItem)field.GetValue(null)!).ToList();
    }

    public static IEnumerable<EventItem> GetEventItems()
    {
        var fields = typeof(TabTabACItems).GetFields(BindingFlags.Static | BindingFlags.Public);

        return fields.Where(field => field.GetValue(null) is EventItem).Select(field => (EventItem)field.GetValue(null)!).ToList();
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
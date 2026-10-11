using System.Text.RegularExpressions;
using GVDEditor.Domain.Entities;
using Iniss.Elis;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Poznamky vlakov z ELIS: ktore platia v stanici grafikonu a co z nich import nastavi.
/// </summary>
/// <remarks>
/// ELIS pise usek, v ktorom poznamka plati, na koniec textu: <c>náhradná autobusová doprava (Púchov-&gt;Vsetín)</c>,
/// poznamku k jednej stanici ako <c>nie je prípoj medzi vlakmi 3322 a 140 (Žilina)</c>. Kluc poznamky je text bez
/// useku - rovnaka poznamka na inom useku sa tak priraduje len raz.
/// </remarks>
internal static partial class ElisRemarks
{
    private const string SectionArrow = "->";

    /// <summary>
    /// Poznamky vlaku, ktore platia v stanici grafikonu, ako kluce bez useku. Poznamka s usekom, ktory sa na trase
    /// vlaku neda najst, sa vynecha.
    /// </summary>
    /// <param name="train">Vlak z ELIS.</param>
    /// <param name="stationName">Nazov stanice grafikonu tak, ako ho pise ELIS.</param>
    public static List<string> AtStation(ElisTrain train, string stationName)
    {
        var route = train.StationsBefore.Select(stop => stop.Name)
            .Append(stationName)
            .Concat(train.StationsAfter.Select(stop => stop.Name))
            .Select(Normalize)
            .ToList();
        var home = train.StationsBefore.Count;

        var result = new List<string>();
        foreach (var remark in train.Remarks)
            if (!IsInformation(remark) && AppliesAt(remark, route, home, out var key) && key.Length != 0 && !result.Contains(key))
                result.Add(key);

        return result;
    }

    /// <summary>
    /// Ci poznamka plati v stanici na pozicii <paramref name="home" /> trasy a jej kluc (text bez useku).
    /// </summary>
    /// <param name="remark">Text poznamky z ELIS.</param>
    /// <param name="route">Normalizovane nazvy stanic trasy (<see cref="Normalize" />) vratane stanice grafikonu.</param>
    /// <param name="home">Pozicia stanice grafikonu v trase.</param>
    /// <param name="key">Text poznamky bez useku.</param>
    internal static bool AppliesAt(string remark, IReadOnlyList<string> route, int home, out string key)
    {
        key = remark.Trim();
        if (!TrySplitTail(key, out var text, out var tail))
            return true;

        var arrow = tail.IndexOf(SectionArrow, StringComparison.Ordinal);
        if (arrow >= 0)
        {
            var from = IndexOf(route, tail[..arrow]);
            var to = IndexOf(route, tail[(arrow + SectionArrow.Length)..]);
            key = text;
            return from >= 0 && to >= from && from <= home && home <= to;
        }

        // zatvorka bez sipky je stanica, len ak taka na trase je - inak patri k textu ("dámske kupé (kupé pre ...)")
        var at = IndexOf(route, tail);
        if (at < 0)
            return true;

        key = text;
        return at == home;
    }

    /// <summary>
    /// Ci je poznamka len informacia o spojeni (prestupy, prechod hranice, zmena druhu vlaku na useku) - na hlasenie vlaku
    /// sa neda pouzit a takych je vela s roznymi cislami vlakov, preto sa do kontroly ani nedostane.
    /// </summary>
    internal static bool IsInformation(string remark) => InformationText().IsMatch(StringUtils.RemoveDiacritics(remark).ToLowerInvariant());

    /// <summary>
    /// Navrh pre poznamku, ktoru grafikon este nepozna: priznak vlaku podla znamych textov ELIS (slovenskych aj
    /// ceskych). Doplnok ani vyluku nenavrhuje - tie vybera obsluha.
    /// </summary>
    public static ElisRemarkAction Suggest(string remark)
    {
        var text = StringUtils.RemoveDiacritics(remark).ToLowerInvariant();
        if (ReservationText().IsMatch(text))
            return ElisRemarkAction.Nothing with { Flag = ElisTrainFlag.Reservation };
        if (LowFloorText().IsMatch(text))
            return ElisRemarkAction.Nothing with { Flag = ElisTrainFlag.LowFloor };
        if (SleeperOnlyText().IsMatch(text))
            return ElisRemarkAction.Nothing with { Flag = ElisTrainFlag.SleeperOnly };
        return ElisRemarkAction.Nothing;
    }

    /// <summary>
    /// Nastavi vlaku priznaky, doplnky a vyluku podla jeho poznamok. Doplnok, ktory vlak uz ma, sa neprida znova;
    /// vyluka sa priradi, len ak vlak ziadnu nema.
    /// </summary>
    /// <param name="train">Importovany vlak.</param>
    /// <param name="remarks">Poznamky vlaku platne v stanici (kluce).</param>
    /// <param name="actions">Co urobit s poznamkou.</param>
    /// <param name="createAddition">Vytvori doplnok podla kodu pre dany vlak; <see langword="null" />, ak taky v banke nie je.</param>
    public static void Apply(Train train, IEnumerable<string> remarks, IReadOnlyDictionary<string, ElisRemarkAction> actions,
        Func<Train, string, Dodatok?> createAddition)
    {
        foreach (var remark in remarks)
        {
            if (!actions.TryGetValue(remark, out var action))
                continue;

            switch (action.Flag)
            {
                case ElisTrainFlag.Reservation:
                    train.IsMiestenkovy = true;
                    break;
                case ElisTrainFlag.LowFloor:
                    train.IsNizkopodlazny = true;
                    break;
                case ElisTrainFlag.SleeperOnly:
                    train.IsIbaLozkovy = true;
                    break;
            }

            if (action.Addition.Length != 0 && train.Doplnky.All(dodatok => dodatok.Name != action.Addition) &&
                createAddition(train, action.Addition) is { } addition)
                train.Doplnky.Add(addition);

            if (action.Lockout != 0 && train.LockoutNumber == 0)
                train.LockoutNumber = action.Lockout;
        }
    }

    /// <summary>
    /// Nazov stanice na porovnanie s nazvami v poznamkach - bez diakritiky, bodiek, pomlciek a medzier.
    /// </summary>
    internal static string Normalize(string name) =>
        StringUtils.RemoveDiacritics(name.Replace(".", "").Replace("-", "").Replace(" ", "")).ToLowerInvariant();

    private static int IndexOf(IReadOnlyList<string> route, string name)
    {
        var wanted = Normalize(name);
        for (var i = 0; i < route.Count; i++)
            if (route[i] == wanted)
                return i;

        return -1;
    }

    /// <summary>
    /// Oddeli poslednu zatvorku na konci textu; zatvorky v nej (dvojjazycne nazvy) sa preskakuju.
    /// </summary>
    private static bool TrySplitTail(string remark, out string text, out string tail)
    {
        text = remark;
        tail = "";
        if (!remark.EndsWith(')'))
            return false;

        var depth = 0;
        for (var i = remark.Length - 1; i >= 0; i--)
        {
            if (remark[i] == ')')
                depth++;
            else if (remark[i] == '(' && --depth == 0)
            {
                text = remark[..i].TrimEnd();
                tail = remark[(i + 1)..^1].Trim();
                return text.Length != 0 && tail.Length != 0;
            }
        }

        return false;
    }

    [GeneratedRegex(@"^(nie je|neni) prip|^z[oe] stanice .*\((odch|odj)\.|^pohranicn[yi] prechodov[yi] bod|^v useku .*\bvlak veden")]
    private static partial Regex InformationText();

    [GeneratedRegex(@"\bpovinne m(?:ie|i)stenkov[yi] vlak|\bpovinna rezervace mist\b")]
    private static partial Regex ReservationText();

    [GeneratedRegex(@"[zs]nizenou podlahou|nizkopodlazn")]
    private static partial Regex LowFloorText();

    [GeneratedRegex(@"\b(len|iba|pouze|jen) (lozkov|luzkov)")]
    private static partial Regex SleeperOnlyText();
}

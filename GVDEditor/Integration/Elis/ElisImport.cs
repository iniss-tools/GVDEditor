using GVDEditor.Domain.Entities;
using Iniss.Elis.Data;
using Iniss.PlanRadenia.Data;

namespace GVDEditor.Integration.Elis;

/// <summary>
/// Medzivysledok importu z ELIS - nacitane data a stanice, ktore treba priradit rucne.
/// </summary>
internal sealed class ElisImport
{
    public required ElisBridgeClient Client { get; init; }
    public required ElisResult Data { get; init; }
    public required List<string> Unresolved { get; init; }
    public required string GVDPath { get; init; }
    public required GVDInfo GVDInfo { get; init; }

    /// <summary>
    /// Co sa stane s povodnymi vlakmi. Nesie sa spolu s vysledkom - stav okolo asynchronneho behu sa lahko strati
    /// a chyba by sa prejavila az tichym nenahradenim vlakov.
    /// </summary>
    public required ElisImportMode Mode { get; init; }

    /// <summary>
    /// Ci sa maju naimportovanym vlakom vygenerovat texty na tabuliach.
    /// </summary>
    public required bool GenerateTableTexts { get; init; }

    /// <summary>
    /// Vlaky, z ktorych sa preberie kratke hlasenie a hlasenia doplnkov; prazdne = nepreberat.
    /// </summary>
    public required IReadOnlyList<Train> SourceTrains { get; init; }

    /// <summary>
    /// Volba kratkeho hlasenia pre stanice z predchadzajuceho importu (ID stanice -> hlasi sa).
    /// </summary>
    public required Dictionary<string, bool> SavedShort { get; init; }

    /// <summary>
    /// Priradenie poznamok vlakov z predchadzajuceho importu.
    /// </summary>
    public required Dictionary<string, ElisRemarkAction> SavedRemarks { get; init; }

    /// <summary>Vlaky stanice v plane radenia; <see langword="null" />, ak sa plan necital alebo sa necital uspesne.</summary>
    public PlanResult? Plan { get; init; }

    /// <summary>Preco sa plan radenia nepodarilo precitat (sprava pre pouzivatela), inak <see langword="null" />.</summary>
    public string? PlanError { get; init; }

    /// <summary>Nazvy stanic pre poznamky planu a stanice grafikonu, ku ktorym patria.</summary>
    public Dictionary<string, Station> PlanStations { get; init; } = [];
}

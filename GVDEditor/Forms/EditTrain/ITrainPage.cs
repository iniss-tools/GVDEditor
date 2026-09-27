using GVDEditor.Tools;

namespace GVDEditor.Forms.EditTrain;

/// <summary>
///     Stranka okna vlaku. Meni priamo koncept vlaku (<see cref="TrainDraft" />) a oznami to udalostou
///     <see cref="Changed" />; okno potom cely koncept skontroluje a chyby vrati strankam cez <see cref="ShowProblems" />.
/// </summary>
internal interface ITrainPage
{
    /// <summary>
    ///     Stranka zmenila koncept vlaku.
    /// </summary>
    event EventHandler? Changed;

    /// <summary>
    ///     Pole, ktore stranka zobrazuje.
    /// </summary>
    bool Handles(TrainRules.Field field);

    /// <summary>
    ///     Oznaci chyby a upozornenia svojich poli (dostane vsetky, vyberie si svoje).
    /// </summary>
    void ShowProblems(IReadOnlyList<TrainRules.Problem> problems);

    /// <summary>
    ///     Presunie fokus na pole s chybou (pri zozname vyberie aj polozku <see cref="TrainRules.Problem.Row" />).
    /// </summary>
    void FocusField(TrainRules.Problem problem);
}

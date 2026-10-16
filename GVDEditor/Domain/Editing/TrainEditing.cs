using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Domain.Editing;

/// <summary>
/// Ulozenie vlaku z konceptu a jeho odstranenie z grafikonu - spolocne pre okno Upravit vlak a zmeny cez agenta.
/// </summary>
internal static class TrainEditing
{
    /// <summary>
    /// Radenia patria cislu vlaku: koncept zobrazi radenia ineho vlaku s rovnakym cislom (okrem riadku
    /// <paramref name="row" />), inak vlastne. Upravy zobrazenych radeni ostanu.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="trains">vlaky grafikonu</param>
    /// <param name="row">riadok upravovaneho vlaku; pri novom vlaku a kopii pocet vlakov</param>
    /// <param name="other">vlak s rovnakym cislom a radenim; <see langword="null" /> = koncept ma vlastne radenia</param>
    /// <returns>Koncept prave prevzal radenia vlaku <paramref name="other" />.</returns>
    public static bool SyncRadenia(TrainDraft draft, IReadOnlyList<Train> trains, int row, out Train? other)
    {
        other = null;
        var number = draft.Number;
        if (string.IsNullOrEmpty(number))
            return false;

        // pri kopii je row novy riadok, takze sem patri aj zdrojovy vlak - Shows ho vsak vynecha
        other = trains.Where((train, i) => train.Number == number && train.Radenia.Count != 0 && row != i).FirstOrDefault();
        if (other is null)
        {
            draft.Radenia.RestoreOwn();
            return false;
        }

        // koncept uz zobrazuje radenia tohto cisla (varianta, zdroj kopie) - netreba ich preberat a zahodit upravy
        if (draft.Radenia.Shows(other.Radenia))
            return false;

        draft.Radenia.Load(other.Radenia);
        return true;
    }

    /// <summary>
    /// Zapise koncept bez chyb (<see cref="Rules.TrainRules.Check" />) do vlaku <paramref name="train" />, alebo do noveho
    /// vlaku (pridat a kopirovat) - ako tlacidlo Ulozit okna vlaku. Novy vlak do zoznamu neprida; cisla variant prideli
    /// az <see cref="TrainVariants.Normalize" />.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="train">upravovany vlak; <see langword="null" /> = novy vlak</param>
    /// <param name="row">riadok upravovaneho vlaku; pri novom vlaku pocet vlakov</param>
    /// <param name="document">grafikon</param>
    /// <returns>Ulozeny vlak.</returns>
    public static Train Save(TrainDraft draft, Train? train, int row, GrafikonDocument document)
    {
        // cislo sa mohlo zmenit - radenie patri cislu, s ktorym sa vlak ulozi
        SyncRadenia(draft, document.Trains, row, out _);

        var saved = train ?? new Train();
        // vlak, ktory odide do inej skupiny variant, dostane v nej nove cislo (TrainVariants.Normalize po ulozeni) -
        // vlakom, ktore uz v skupine su, cisla ostanu
        if (train is not null && draft.KeyChanged && !draft.RenamesSiblings)
            saved.Variant = -1;

        draft.ApplyTo(saved);
        draft.ApplyToSiblings();
        draft.ApplyVariantLimits();
        draft.Radenia.Commit(saved, document.Radenia, document.Trains);
        return saved;
    }

    /// <summary>
    /// Odstrani vlak z grafikonu aj jeho texty na tabuliach - text bez vlaku by sa nedal zapisat.
    /// </summary>
    public static void Remove(Train train, GrafikonDocument document)
    {
        foreach (var text in document.TableTexts)
            text.Trains.RemoveAll(t => ReferenceEquals(t.Train, train));
        document.Trains.Remove(train);
    }

    /// <summary>
    /// Cisla vlakov podla poradia v zozname od 1 - v hlavnom okne ich prideluje tabulka vlakov.
    /// </summary>
    public static void Renumber(IReadOnlyList<Train> trains)
    {
        for (var i = 0; i < trains.Count; i++)
            trains[i].ID = i + 1;
    }
}

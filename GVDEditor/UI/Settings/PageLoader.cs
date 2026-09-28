using ExControls;

namespace GVDEditor.UI.Settings;

/// <summary>
///     Naplna stranky okna nastaveni postupne, aby sa okno otvorilo hned: pred zobrazenim len stranku, ktorou sa
///     otvara, ostatne po zobrazeni okna jednu po druhej (medzi nimi okno reaguje) alebo hned, ked ich pouzivatel
///     vyberie. Pred OK treba naplnit vsetky (<see cref="LoadAll" />) - kontrola chyb musi vidiet kazdu stranku.
/// </summary>
internal sealed class PageLoader
{
    private readonly Form _form;
    private readonly List<(ExOptionsPanel Panel, Action Load)> _pending = [];
    private bool _running;

    /// <summary>
    ///     Vytvori plnenie stranok okna <paramref name="form" /> so stromom stranok <paramref name="view" />.
    /// </summary>
    public PageLoader(Form form, ExOptionsView view)
    {
        _form = form;
        view.SelectedPanelChanged += (_, _) => Load(view.SelectedPanel);
        form.Shown += (_, _) => StartIdleLoading();
    }

    /// <summary>
    ///     Prida stranku na panele <paramref name="panel" />; stranky sa po zobrazeni okna plnia v poradi pridania.
    /// </summary>
    public void Add(ExOptionsPanel panel, Action load) => _pending.Add((panel, load));

    /// <summary>
    ///     Naplni stranku na panele <paramref name="panel" />, ak este naplnena nie je.
    /// </summary>
    public void Load(ExOptionsPanel? panel)
    {
        var index = _pending.FindIndex(p => ReferenceEquals(p.Panel, panel));
        if (index < 0)
            return;

        var load = _pending[index].Load;
        _pending.RemoveAt(index);
        load();
    }

    /// <summary>
    ///     Naplni vsetky este nenaplnene stranky.
    /// </summary>
    public void LoadAll()
    {
        while (_pending.Count > 0)
            Load(_pending[0].Panel);
    }

    // stranka sa plni az ked je okno necinne - po vykresleni, medzi stranky sa dostanu kliky a klavesy pouzivatela
    private void StartIdleLoading()
    {
        if (_running || _pending.Count == 0)
            return;

        _running = true;
        Application.Idle += OnIdle;
        _form.FormClosed += (_, _) => StopIdleLoading();
    }

    private void StopIdleLoading()
    {
        if (!_running)
            return;

        _running = false;
        Application.Idle -= OnIdle;
    }

    private void OnIdle(object? sender, EventArgs e)
    {
        if (_form.IsDisposed || _pending.Count == 0)
        {
            StopIdleLoading();
            return;
        }

        Load(_pending[0].Panel);

        // Idle pride znova, az ked do fronty okna nieco dojde - prazdna sprava ho zobudi
        if (_pending.Count > 0)
            _form.BeginInvoke(() => { });
        else
            StopIdleLoading();
    }
}

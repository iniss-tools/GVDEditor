using System.Drawing.Drawing2D;
using ExControls;
using GVDEditor.Properties;
using ToolsCore.StateDgm;
using ToolsCore.XML;

namespace GVDEditor.Controls;

/// <summary>
///     Graf jednej kategorie stavoveho diagramu: uzly su stavy v poradi zo suboru (zhora nadol), hrany su akcie
///     s <c>NextState</c> (dopredne vpravo, spatne vlavo, slucky na uzle). Uzol nesie znacku automatiky a starterov;
///     nedosiahnutelne stavy su sive, slepe maju oranzovy okraj, chybajuci NextState je cervena ciarkovana hrana.
///     Klik vyberie stav, tahanie z uzla na uzol ziada novy prechod. Pozicie sa nikam neukladaju.
/// </summary>
internal sealed class StateDgmGraphPanel : Control
{
    private const int NODE_W = 230;
    private const int NODE_H = 48;
    private const int GAP = 34;
    private const int MARGIN = 24;
    private const int ARC_STEP = 22;

    private StateDgmCategory? _category;
    private StateDgmDiagram? _diagram;
    private StateDgmState? _selected;
    private readonly List<Node> _nodes = [];
    private readonly List<Edge> _edges = [];
    private Node? _dragFrom;
    private Point _dragPos;
    private bool _dragging;
    private Node? _hover;
    private readonly ToolTip _tip = new() { InitialDelay = 300 };
    private string _tipText = "";
    private readonly VScrollBar _vScroll = new() { Dock = DockStyle.Right };
    private readonly HScrollBar _hScroll = new() { Dock = DockStyle.Bottom };

    public StateDgmGraphPanel()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        Controls.Add(_vScroll);
        Controls.Add(_hScroll);
        _vScroll.Scroll += (_, _) => Invalidate();
        _hScroll.Scroll += (_, _) => Invalidate();
        Cursor = Cursors.Default;
    }

    /// <summary>Pouzivatel klikol na stav.</summary>
    public event EventHandler<StateDgmState>? StateSelected;

    /// <summary>Pouzivatel potiahol z jedneho stavu na druhy - chce novy prechod.</summary>
    public event EventHandler<(StateDgmState From, StateDgmState To)>? TransitionRequested;

    /// <summary>Pouzivatel dvojklikol na hranu (akciu).</summary>
    public event EventHandler<StateDgmEvent>? EventActivated;

    /// <summary>Zobrazit vsetky prechody, alebo len prechody vybraneho stavu (a prechody don).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowAllEdges { get; set; } = true;

    /// <summary>Farby podla temy.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ControlsColorScheme? Scheme { get; set; }
    
    /// <summary>Dark theme for scroll bars.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool DarkScrollBar { get; set; }

    /// <summary>Nastavi zobrazenu kategoriu (null = prazdny graf) a vybrany stav.</summary>
    public void ShowCategory(StateDgmDiagram? diagram, StateDgmCategory? category, StateDgmState? selected)
    {
        _diagram = diagram;
        _category = category;
        _selected = selected;
        Rebuild();
        EnsureVisible();
        Invalidate();
    }

    /// <summary>Zmeni vybrany stav bez prestavby grafu.</summary>
    public void Select(StateDgmState? state)
    {
        _selected = state;
        EnsureVisible();
        Invalidate();
    }

    /// <summary>Odroluje tak, aby bol vybrany uzol vidiet.</summary>
    private void EnsureVisible()
    {
        var n = _nodes.FirstOrDefault(x => x.State == _selected);
        if (n == null || !_vScroll.Visible) return;
        var top = n.Bounds.Top - GAP;
        var bottom = n.Bounds.Bottom + GAP;
        var view = _vScroll.LargeChange;
        var max = Math.Max(0, _vScroll.Maximum - _vScroll.LargeChange + 1);
        if (top < _vScroll.Value) _vScroll.Value = Math.Clamp(top, 0, max);
        else if (bottom > _vScroll.Value + view) _vScroll.Value = Math.Clamp(bottom - view, 0, max);
    }

    /// <summary>Prestavi graf po zmene modelu.</summary>
    public void Rebuild(StateDgmState? selected)
    {
        _selected = selected;
        Rebuild();
        EnsureVisible();
        Invalidate();
    }

    private sealed class Node
    {
        public required StateDgmState State;
        public required int Index;
        public Rectangle Bounds;
        public bool Reachable = true;
        public bool DeadEnd;
    }

    private sealed class Edge
    {
        public required StateDgmEvent Event;
        public required Node From;
        public Node? To;
        public string Label = "";
        public GraphicsPath? Path;
        public PointF LabelPos;
    }

    private void Rebuild()
    {
        _nodes.Clear();
        _edges.Clear();
        if (_category == null) return;

        for (var i = 0; i < _category.States.Count; i++)
            _nodes.Add(new Node { State = _category.States[i], Index = i });

        var byKey = new Dictionary<string, Node>(StringComparer.Ordinal);
        foreach (var n in _nodes) byKey.TryAdd(n.State.Key, n);

        foreach (var n in _nodes)
        {
            var controls = n.State.Controls.ToLookup(c => c.EventKey, StringComparer.Ordinal);
            foreach (var e in n.State.Events)
            {
                if (!e.ChangesState) continue;
                byKey.TryGetValue(e.NextState!, out var to);
                var btn = controls[e.Key].Select(c => c.DesignKey).FirstOrDefault(d => d.Length > 0);
                var label = btn ?? e.Key;
                if (!string.IsNullOrEmpty(e.ReportKey)) label += " ♪";
                _edges.Add(new Edge { Event = e, From = n, To = to, Label = label });
            }
        }

        // dosiahnutelnost z prveho stavu, slepe stavy
        if (_nodes.Count > 0)
        {
            foreach (var n in _nodes) n.Reachable = false;
            var queue = new Queue<Node>();
            _nodes[0].Reachable = true;
            queue.Enqueue(_nodes[0]);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                foreach (var e in _edges.Where(e => e.From == n && e.To is { Reachable: false }))
                {
                    e.To!.Reachable = true;
                    queue.Enqueue(e.To);
                }
            }

            foreach (var n in _nodes)
                n.DeadEnd = !n.State.Events.Any(e => e.ChangesState) && !n.State.Attr.HasFlag(StateDgmAttr.Shadow);
        }

        DoLayout();
    }

    private int ContentWidth => NODE_W + 2 * MARGIN + 2 * ARC_STEP * Math.Max(1, _nodes.Count);
    private int ContentHeight => MARGIN * 2 + Math.Max(1, _nodes.Count) * (NODE_H + GAP);

    private void DoLayout()
    {
        var x = MARGIN + ARC_STEP * Math.Max(1, _nodes.Count);
        for (var i = 0; i < _nodes.Count; i++)
            _nodes[i].Bounds = new Rectangle(x, MARGIN + i * (NODE_H + GAP), NODE_W, NODE_H);

        // hrany: dopredne po pravej strane, spatne po lavej; vzdialenejsie dalej od uzlov
        var rightUsed = new Dictionary<(int, int), int>();
        foreach (var e in _edges)
        {
            e.Path?.Dispose();
            e.Path = new GraphicsPath();
            var a = e.From.Bounds;
            if (e.To == null)
            {
                // chybajuci stav - kratka ciarkovana sipka doprava
                var p1 = new Point(a.Right, a.Top + NODE_H / 2);
                var p2 = p1 with { X = a.Right + 60 };
                e.Path.AddLine(p1, p2);
                e.LabelPos = new PointF(p2.X + 4, p2.Y - 7);
                continue;
            }

            var b = e.To.Bounds;
            if (e.To == e.From)
            {
                // slucka na pravom hornom rohu
                var r = new Rectangle(a.Right - 14, a.Top - 18, 36, 36);
                e.Path.AddArc(r, 100, 320);
                e.LabelPos = new PointF(r.Right + 2, r.Top);
                continue;
            }

            var forward = e.To.Index > e.From.Index;
            var dist = Math.Abs(e.To.Index - e.From.Index);
            var key = forward 
                ? (Math.Min(e.From.Index, e.To.Index), Math.Max(e.From.Index, e.To.Index)) 
                : (-1 - Math.Min(e.From.Index, e.To.Index), Math.Max(e.From.Index, e.To.Index));
            rightUsed.TryGetValue(key, out var n);
            rightUsed[key] = n + 1;
            var offset = ARC_STEP * dist + n * 6;
            if (forward)
            {
                var p1 = new PointF(a.Right, a.Top + NODE_H * 0.5f + 4);
                var p2 = new PointF(b.Right, b.Top + NODE_H * 0.5f - 4);
                var cx = a.Right + offset;
                e.Path.AddBezier(p1, p1 with { X = cx }, p2 with { X = cx }, p2);
                e.LabelPos = new PointF(cx - 2, (p1.Y + p2.Y) / 2 - 7);
            }
            else
            {
                var p1 = new PointF(a.Left, a.Top + NODE_H * 0.5f - 4);
                var p2 = new PointF(b.Left, b.Top + NODE_H * 0.5f + 4);
                var cx = a.Left - offset;
                e.Path.AddBezier(p1, p1 with { X = cx }, p2 with { X = cx }, p2);
                e.LabelPos = new PointF(cx + 2, (p1.Y + p2.Y) / 2 - 7);
            }
        }

        UpdateScroll();
    }

    private void UpdateScroll()
    {
        var w = ContentWidth;
        var h = ContentHeight;
        _vScroll.Visible = h > ClientSize.Height;
        _hScroll.Visible = w > ClientSize.Width;
        _vScroll.Maximum = Math.Max(0, h);
        _vScroll.LargeChange = Math.Max(1, ClientSize.Height - (_hScroll.Visible ? _hScroll.Height : 0));
        _vScroll.SmallChange = NODE_H;
        _hScroll.Maximum = Math.Max(0, w);
        _hScroll.LargeChange = Math.Max(1, ClientSize.Width - (_vScroll.Visible ? _vScroll.Width : 0));
        _hScroll.SmallChange = 20;
        _vScroll.Value = Math.Min(_vScroll.Value, Math.Max(0, _vScroll.Maximum - _vScroll.LargeChange + 1));
        _hScroll.Value = Math.Min(_hScroll.Value, Math.Max(0, _hScroll.Maximum - _hScroll.LargeChange + 1));
    }

    private Point Offset => new(_hScroll.Visible ? -_hScroll.Value : Math.Max(0, (ClientSize.Width - ContentWidth) / 2), _vScroll.Visible ? -_vScroll.Value : 0);

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateScroll();
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (!_vScroll.Visible) return;
        _vScroll.Value = Math.Clamp(_vScroll.Value - Math.Sign(e.Delta) * NODE_H, 0, Math.Max(0, _vScroll.Maximum - _vScroll.LargeChange + 1));
        Invalidate();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (DarkScrollBar)
        {
            _vScroll.SetTheme(WindowsTheme.DarkExplorer);
            _hScroll.SetTheme(WindowsTheme.DarkExplorer);
        }
    }

    #region Kreslenie

    protected override void OnPaint(PaintEventArgs e)
    {
        var scheme = Scheme;
        var back = scheme?.Box.BackColor ?? SystemColors.Window;
        var fore = scheme?.Box.ForeColor ?? SystemColors.WindowText;
        var accent = scheme?.Highlight.BackColor ?? SystemColors.Highlight;
        var accentFore = scheme?.Highlight.ForeColor ?? SystemColors.HighlightText;
        var border = scheme?.Border.ForeColor ?? SystemColors.ControlDark;
        var nodeBack = scheme?.Button.BackColor ?? SystemColors.Control;
        var grey = Color.FromArgb(140, fore);
        var g = e.Graphics;
        g.Clear(back);
        if (_category == null || _nodes.Count == 0)
        {
            TextRenderer.DrawText(g, Resources.FStateDgm_GrafPrazdny, Font, ClientRectangle, grey, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        g.SmoothingMode = SmoothingMode.AntiAlias;
        var off = Offset;
        g.TranslateTransform(off.X, off.Y);
        // TextRenderer transformaciu Graphics ignoruje - obdlzniky textu treba posunut rucne
        Rectangle T(Rectangle r) => r with { X = r.X + off.X, Y = r.Y + off.Y };
        var small = new Font(Font.FontFamily, Font.Size - 1);
        var bold = new Font(Font, FontStyle.Bold);

        // hrany - najprv nezvyraznene, potom hrany vybraneho stavu
        foreach (var pass in new[] { 0, 1 })
        {
            foreach (var edge in _edges)
            {
                var involved = _selected != null && (edge.From.State == _selected || edge.To?.State == _selected);
                if ((pass == 1) != involved) continue;
                if (!ShowAllEdges && !involved) continue;
                var missing = edge.To == null;
                var color = missing ? Color.IndianRed : involved ? accent : Color.FromArgb(ShowAllEdges ? 70 : 0, fore);
                using var pen = new Pen(color, involved ? 2f : 1.2f);
                pen.CustomEndCap = new AdjustableArrowCap(4, 6);
                if (missing) pen.DashStyle = DashStyle.Dash;
                if (edge.Path != null) g.DrawPath(pen, edge.Path);
                if (involved || missing || !ShowAllEdges || _selected == null)
                {
                    var text = missing ? $"{edge.Label} → {edge.Event.NextState}?" : edge.Label;
                    var size = TextRenderer.MeasureText(g, text, small);
                    var lp = edge.LabelPos;
                    var forwardSide = edge.To == null || edge.To == edge.From || edge.To.Index > edge.From.Index;
                    var rect = forwardSide ? new Rectangle((int)lp.X, (int)lp.Y, size.Width, size.Height) : new Rectangle((int)lp.X - size.Width, (int)lp.Y, size.Width, size.Height);
                    using var lb = new SolidBrush(Color.FromArgb(220, back));
                    g.FillRectangle(lb, rect);
                    TextRenderer.DrawText(g, text, small, T(rect), missing ? Color.IndianRed : involved ? accent : Color.FromArgb(160, fore), TextFormatFlags.NoPadding);
                }
            }
        }

        // uzly
        foreach (var n in _nodes)
        {
            var r = n.Bounds;
            using var path = RoundedRect(r, 8);
            var selected = n.State == _selected;
            using var fill = new SolidBrush(selected ? accent : nodeBack);
            g.FillPath(fill, path);
            using var pen = new Pen(n.DeadEnd ? Color.DarkOrange : selected ? accent : border, n.DeadEnd ? 2f : 1.2f);
            if (n.DeadEnd) pen.DashStyle = DashStyle.Dash;
            g.DrawPath(pen, path);

            var textColor = selected ? accentFore : n.Reachable ? fore : grey;
            var title = n.State.Key + (n.State.Name.Length > 0 && n.State.Name != n.State.Key ? $"  ({n.State.Name})" : "");
            TextRenderer.DrawText(g, title, bold, T(new Rectangle(r.X + 8, r.Y + 5, r.Width - 16, 18)), textColor, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            var sub = SubText(n);
            TextRenderer.DrawText(g, sub, small, T(new Rectangle(r.X + 8, r.Y + 25, r.Width - 16, 18)), selected ? accentFore : Color.FromArgb(170, textColor), TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            if (n.Index == 0)
            {
                // pociatocny stav - znacka vlavo
                using var sb = new SolidBrush(accent);
                g.FillPolygon(sb, [new Point(r.Left - 14, r.Top + NODE_H / 2 - 6), new Point(r.Left - 4, r.Top + NODE_H / 2), new Point(r.Left - 14, r.Top + NODE_H / 2 + 6)]);
            }
        }

        if (_dragging && _dragFrom != null)
        {
            using var pen = new Pen(accent, 2f);
            pen.DashStyle = DashStyle.Dot;
            pen.CustomEndCap = new AdjustableArrowCap(4, 6);
            var a = _dragFrom.Bounds;
            g.DrawLine(pen, new Point(a.Right, a.Top + NODE_H / 2), new Point(_dragPos.X - Offset.X, _dragPos.Y - Offset.Y));
        }

        small.Dispose();
        bold.Dispose();
    }

    private static string SubText(Node n)
    {
        var s = n.State;
        var parts = new List<string>();
        if (s.HasAutomation)
        {
            var mode = s.AutoMode!.IsExpression ? "ƒ" : s.AutoMode.Number == 2 
                ? Resources.FStateDgm_Graf_Automat 
                : Resources.FStateDgm_Graf_Poloautomat;
            var tp = s.AutoTimePoint == null ? "" : s.AutoTimePoint.IsExpression 
                ? "ƒ" 
                : s.AutoTimePoint.Number == 1 
                    ? Resources.FStateDgm_Graf_Prichod 
                    : Resources.FStateDgm_Graf_Odchod;
            var add = s.AutoTimePointAdd == null 
                ? "" 
                : s.AutoTimePointAdd.IsExpression 
                    ? " ƒ" 
                    : s.AutoTimePointAdd.Number == 0 
                        ? "" 
                        : " " + (s.AutoTimePointAdd.Number < 0 ? "−" : "+") + SdEditorContext.Seconds(s.AutoTimePointAdd.Number!.Value);
            var wait = s.Wait != null ? " ⏳" + s.Wait.Text : "";
            parts.Add($"⏱ {mode} {tp}{add}{wait}".Trim());
        }

        if (s.Starters.Count > 0) parts.Add($"↻ {s.Starters.Count}");
        if (parts.Count == 0) parts.Add($"{s.Events.Count(e => e.ChangesState)} → · {s.Controls.Count} ⌨");
        return string.Join("   ", parts);
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        var p = new GraphicsPath();
        var d = radius * 2;
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    #endregion

    #region Mys

    private Node? HitNode(Point p)
    {
        var q = new Point(p.X - Offset.X, p.Y - Offset.Y);
        return _nodes.FirstOrDefault(n => n.Bounds.Contains(q));
    }

    private Edge? HitEdge(Point p)
    {
        var q = new PointF(p.X - Offset.X, p.Y - Offset.Y);
        using var pen = new Pen(Color.Black, 6f);
        return _edges.FirstOrDefault(e => e.Path != null && e.Path.IsOutlineVisible(q, pen));
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Focus();
        if (e.Button != MouseButtons.Left) return;
        var n = HitNode(e.Location);
        if (n != null)
        {
            _dragFrom = n;
            _dragPos = e.Location;
            _dragging = false;
            if (_selected != n.State)
            {
                _selected = n.State;
                Invalidate();
                StateSelected?.Invoke(this, n.State);
            }
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_dragFrom != null && e.Button == MouseButtons.Left)
        {
            if (!_dragging && (Math.Abs(e.X - _dragPos.X) > 6 || Math.Abs(e.Y - _dragPos.Y) > 6)) _dragging = true;
            if (_dragging)
            {
                _dragPos = e.Location;
                Cursor = HitNode(e.Location) is { } t && t != _dragFrom ? Cursors.Hand : Cursors.Cross;
                Invalidate();
            }

            return;
        }

        var n = HitNode(e.Location);
        var edge = n == null ? HitEdge(e.Location) : null;
        Cursor = n != null ? Cursors.Hand : Cursors.Default;
        var tip = n != null ? NodeTip(n) : edge != null ? EdgeTip(edge) : "";
        if (tip != _tipText)
        {
            _tipText = tip;
            _tip.SetToolTip(this, tip);
        }

        if (n != _hover)
        {
            _hover = n;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (_dragFrom != null && _dragging)
        {
            var target = HitNode(e.Location);
            if (target != null && target != _dragFrom)
                TransitionRequested?.Invoke(this, (_dragFrom.State, target.State));
        }

        _dragFrom = null;
        _dragging = false;
        Cursor = Cursors.Default;
        Invalidate();
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
        base.OnMouseDoubleClick(e);
        if (HitNode(e.Location) != null) return;
        if (HitEdge(e.Location) is { } edge) EventActivated?.Invoke(this, edge.Event);
    }

    private static string NodeTip(Node n)
    {
        var s = n.State;
        var lines = new List<string> { s.Key + (s.Name.Length > 0 ? $" – {s.Name}" : "") };
        if (!n.Reachable) lines.Add(Resources.FStateDgm_Graf_Nedosiahnutelny);
        if (n.DeadEnd) lines.Add(Resources.FStateDgm_Graf_Slepy);
        if (s.Attr != StateDgmAttr.None) lines.Add("Attr: " + StateDgmWriter.AttrNames(s.Attr));
        foreach (var e in s.Events.Where(e => e.ChangesState))
            lines.Add($"→ {e.NextState}   [{e.Key}]{(string.IsNullOrEmpty(e.ReportKey) ? "" : " ♪ " + e.ReportKey)}");
        foreach (var st in s.Starters)
            lines.Add("↻ " + SdStarterEditor.Sentence(st));
        return string.Join(Environment.NewLine, lines);
    }

    private static string EdgeTip(Edge e) =>
        $"{e.Event.Key}: {e.From.State.Key} → {e.Event.NextState}" + (string.IsNullOrEmpty(e.Event.ReportKey) ? "" : $"{Environment.NewLine}♪ {e.Event.ReportKey}") + Environment.NewLine + e.Event.Class;

    #endregion
}

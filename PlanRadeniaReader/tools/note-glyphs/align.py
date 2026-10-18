# Zarovnanie prepisov riadkov poznamok k obrazku a zber oznacenych znakov
#  kolo 0: vzory z pisma Arial (vykreslene 8x a zmensene), bunka znaku = jeho posun
#  kolo 1+: vzory zo skutocnych znakov (zhluky podla sirky)
# Vynutene zarovnanie: DP cez znaky prepisu, medzi znakmi 0..GAP prazdnych stlpcov.
import os, sys, json, pickle
import numpy as np
from PIL import Image, ImageDraw, ImageFont

# pracovny priecinok: pages/pageN.png a notes.tsv z PlanRadeniaReader (--debug, --dump-notes), sem idu aj vysledky
D = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (pages/, notes.tsv) - vysledky nepatria do repozitara.')
HERE = os.path.dirname(os.path.abspath(__file__))
TOP, BOTTOM = 10, 4          # riadky nad a pod uplatnou ciarou (vratane)
ROWS = TOP + BOTTOM
FONT = 'C:/Windows/Fonts/arial.ttf'
SIZE = 10.1
SS = 8
GAP = 7
INNER = int(os.environ.get('INNER', '1'))
REGISTER = os.environ.get('REGISTER', '1') == '1'

groups = json.load(open(os.path.join(D, 'notegroups.json')))
trans = {}
for l in open(os.path.join(HERE, 'trans.txt'), encoding='utf-8'):
    l = l.rstrip('\n')
    if not l.strip() or l.startswith('#'):
        continue
    k, t = l.split('\t', 1)
    trans.setdefault(int(k), []).append(t)

pages = {}
def page(p):
    if p not in pages:
        pages[p] = 255 - np.asarray(Image.open(os.path.join(D, 'pages', f'page{p}.png')).convert('L')).astype(np.float32)
    return pages[p]

def baseline(band):
    prof = (band > 90).sum(1)
    m = prof.max()
    rows = [y for y in range(len(prof)) if prof[y] >= 0.35 * m]
    return max(rows)

def rows_at(a, b, dy, x0, x1):
    """Riadky ROWS od uplatnej ciary b posunute o zlomok dy (linearna interpolacia)."""
    y = b - TOP + 1 + dy + np.arange(ROWS)
    y0 = np.floor(y).astype(int)
    t = (y - y0)[:, None]
    return (a[y0, x0:x1] * (1 - t) + a[y0 + 1, x0:x1] * t) / 255.0

REF = None

def reference():
    """Priemerny zvisly profil riadkov (podiel atramentu v riadkoch) - podla neho sa riadky zarovnaju zvisle."""
    global REF
    if REF is None:
        profs = []
        for g in groups:
            for f in g:
                a = page(f['page'])
                band = a[f['y'] + 3:f['y'] + f['h'] - 3, f['x']:f['x'] + f['w']]
                b = baseline(band) + f['y'] + 3
                p = rows_at(a, b, 0, f['x'], f['x'] + f['w']).sum(1)
                profs.append(p / p.sum())
        REF = np.mean(profs, 0)
        for _ in range(2):
            regs = []
            for g in groups:
                for f in g:
                    a = page(f['page'])
                    band = a[f['y'] + 3:f['y'] + f['h'] - 3, f['x']:f['x'] + f['w']]
                    b = baseline(band) + f['y'] + 3
                    dy = best_shift(a, b, f['x'], f['x'] + f['w'], REF)
                    p = rows_at(a, b, dy, f['x'], f['x'] + f['w']).sum(1)
                    regs.append(p / p.sum())
            REF = np.mean(regs, 0)
    return REF

def best_shift(a, b, x0, x1, ref):
    best = (1e9, 0.0)
    for dy in np.arange(-1.5, 1.51, 0.125):
        p = rows_at(a, b, dy, x0, x1).sum(1)
        d = ((p / p.sum() - ref) ** 2).sum()
        best = min(best, (d, float(dy)))
    return best[1]

def line_image(f, pad=4):
    a = page(f['page'])
    x0, x1 = f['x'] - pad, f['x'] + f['w'] + pad
    band = a[f['y'] + 3:f['y'] + f['h'] - 3, x0:x1]
    b = baseline(band) + f['y'] + 3
    dy = best_shift(a, b, f['x'], f['x'] + f['w'], reference()) if REGISTER else 0.0
    return rows_at(a, b, dy, x0, x1), x0, b, dy

# ---------------- vzory z pisma
def synth_templates(chars):
    font = ImageFont.truetype(FONT, SIZE * SS)
    sub = {'@': '@', '%': '%', **{chr(0x2460 + i): '@' for i in range(7)}}
    out = {}
    for c in chars:
        g = sub.get(c, c)
        adv = font.getlength(g) / SS
        vs = []
        for ph in range(4):
            off = ph * SS // 4
            w = int(np.ceil(adv + ph / 4))
            im = Image.new('L', (w * SS + SS, ROWS * SS), 0)
            ImageDraw.Draw(im).text((off, TOP * SS), g, font=font, fill=255, anchor='ls')
            a = np.asarray(im).astype(np.float32)[:, :w * SS]
            a = a.reshape(ROWS, SS, w, SS).mean((1, 3)) / 255.0
            vs.append(a)
        out[c] = vs
    return out

# ---------------- vynutene zarovnanie
def align(img, text, templates):
    """Vrati [(znak, zaciatok, sirka, vzor)] alebo None. Medzery v prepise sa vynechaju (prazdne stlpce su volne)."""
    chars = [c for c in text if c != ' ']
    # pred znakom za medzerou v prepise moze byt dlha medzera (aj tabulator), inde len INNER
    spaced = []
    for i, c in enumerate(text):
        if c != ' ':
            spaced.append(i > 0 and text[i - 1] == ' ')
    W = img.shape[1]
    colsq = (img ** 2).sum(0)
    blank = np.concatenate([[0], np.cumsum(colsq)])      # cena prazdnych stlpcov [a, b) = blank[b]-blank[a]
    INF = 1e18
    # dp[x] = najlepsia cena, ak predosle znaky koncia pred stlpcom x (x = zaciatok volnej casti)
    start = np.full(W + 1, INF)
    # zaciatok: lubovolne prazdne stlpce na zaciatku
    for x in range(min(W, 30) + 1):
        start[x] = blank[x]
    back = []
    cur = start
    for c, sp in zip(chars, spaced):
        # najprv medzera
        gapped = np.full(W + 1, INF)
        garg = np.zeros(W + 1, dtype=np.int32)
        for g in range((40 if sp else INNER) + 1):
            prev = np.full(W + 1, INF)
            prev[g:] = cur[:W + 1 - g] + (blank[g:] - blank[:W + 1 - g])
            better = prev < gapped
            gapped[better] = prev[better]
            garg[better] = g
        nxt = np.full(W + 1, INF)
        narg = np.full((W + 1, 2), -1, dtype=np.int32)
        for vi, t in enumerate(templates[c]):
            w = t.shape[1]
            if w > W:
                continue
            # cena vzoru na kazdom zaciatku x: sum (img[:, x:x+w] - t)^2
            win = np.lib.stride_tricks.sliding_window_view(img, w, axis=1)   # ROWS x (W-w+1) x w
            cost = ((win - t[:, None, :]) ** 2).sum((0, 2))
            cand = np.full(W + 1, INF)
            cand[w:] = gapped[:W + 1 - w] + cost
            better = cand < nxt
            nxt[better] = cand[better]
            narg[better, 0] = vi
            narg[better, 1] = w
        back.append((garg, narg))
        cur = nxt
    # koniec: zvysok prazdny
    total = cur + (blank[W] - blank)
    end = int(np.argmin(total))
    if total[end] >= INF / 2:
        return None, None
    res = []
    x = end
    for c, (garg, narg) in zip(reversed(chars), reversed(back)):
        vi, w = narg[x]
        s = x - w
        g = garg[s]
        res.append((c, int(s), int(w), int(vi)))
        x = s - g
    res.reverse()
    return res, float(total[end])

def run(templates, sample_all=True):
    out = []
    for k, alts in sorted(trans.items()):
        for f in (groups[k] if sample_all else groups[k][:1]):
            img, x0, b, dy = line_image(f)
            # skupina moze mat viac prepisov (rovnaky vlak a sirka, iny text) - plati ten s najnizsou cenou
            best = (None, None, None)
            for t in (alts if sample_all else alts[:1]):
                r, c = align(img, t, templates)
                if r is not None and (best[1] is None or c / len(r) < best[1] / len(best[0])):
                    best = (r, c, t)
            res, cost, text = best
            if res is None:
                print('nezarovnane', k, f['page'])
                continue
            n = len(res)
            out.append(dict(group=k, page=f['page'], y=f['y'], x0=x0, base=b, dy=dy, text=text, cells=res, cost=cost / max(1, n)))
    return out

def line_of(a, extra=4):
    """Obrazok zarovnaneho riadku (rovnaky ako pri zarovnani)."""
    x1 = a['x0'] + a['cells'][-1][1] + a['cells'][-1][2] + extra
    return rows_at(page(a['page']), a['base'], a.get('dy', 0.0), a['x0'], x1)

def harvest(aligned):
    """Bunky znakov zo skutocnych obrazkov."""
    cells = {}
    for a in aligned:
        img = line_of(a)
        for c, s, w, vi in a['cells']:
            cells.setdefault(c, []).append(img[:, s:s + w].copy())
    return cells

PHASES = int(os.environ.get('PHASES', '4'))

def build(cells, maxk=12):
    """
    Vzory zo skutocnych buniek: podla sirky bunky a polohy tazisku znaku v nej (na 1/PHASES bodu), v skupine
    median. Rovnaky znak posunuty o zlomok bodu vyzera inak - spolocny priemer by bol rozmazany.
    """
    out = {}
    for c, lst in cells.items():
        groups_ = {}
        for a in lst:
            ink = a.sum(0)
            cx = (ink * np.arange(a.shape[1])).sum() / max(ink.sum(), 1e-6)
            key = (a.shape[1], int(round(cx * PHASES)))
            groups_.setdefault(key, []).append(a)
        vs = []
        n = len(lst)
        for key, items in sorted(groups_.items(), key=lambda kv: -len(kv[1])):
            if vs and len(items) < max(3, n // 40):
                continue
            vs.append(np.median(np.stack(items), axis=0))
        out[c] = vs[:maxk]
    return out

if __name__ == '__main__':
    alphabet = sorted({c for ts in trans.values() for t in ts for c in t if c != ' '})
    print('abeceda', ''.join(alphabet))
    tm = synth_templates(alphabet)
    al = run(tm, sample_all=False)
    print('kolo 0: riadkov', len(al), 'cena', np.mean([a['cost'] for a in al]))
    for it in range(3):
        tm = build(harvest(al))
        al = run(tm, sample_all=True)
        costs = np.array([a['cost'] for a in al])
        print(f'kolo {it + 1}: riadkov', len(al), 'cena', costs.mean(), 'max', costs.max())
    pickle.dump(dict(aligned=al, templates=tm), open(os.path.join(D, 'aligned.pkl'), 'wb'))

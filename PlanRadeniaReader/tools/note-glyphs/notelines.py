# Vyrezy riadkov poznamok: zoskupenie podla vlaku a sirky textu, mozaiky na prepis
import sys, os, json
import numpy as np
from PIL import Image, ImageDraw

# pracovny priecinok: pages/pageN.png a notes.tsv z PlanRadeniaReader (--debug, --dump-notes), sem idu aj vysledky
D = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (pages/, notes.tsv) - vysledky nepatria do repozitara.')
HERE = os.path.dirname(os.path.abspath(__file__))
rows = [l.rstrip('\n').split('\t') for l in open(os.path.join(D, 'notes.tsv'), encoding='utf-8')]
pages = {}

def page(p):
    if p not in pages:
        pages[p] = np.asarray(Image.open(os.path.join(D, 'pages', f'page{p}.png')).convert('L')).astype(np.uint8)
    return pages[p]

def split_band(a, x, y, w, h):
    """Pas s viacerymi riadkami textu rozdeli v najsvetlejsich riadkoch (riadok poznamky ma 9-10 bodov)."""
    if h <= 13:
        return [(y, h)]
    prof = (a[y:y + h, x:x + w] < 200).sum(1)
    n = round(h / 12)
    cuts = [0]
    for k in range(1, n):
        c = round(k * h / n)
        best = min(range(c - 3, c + 4), key=lambda r: prof[r])
        cuts.append(best)
    cuts.append(h)
    return [(y + cuts[i], cuts[i + 1] - cuts[i]) for i in range(n)]

lines = []
for p, train, box in rows:
    x, y, w, h = map(int, box.split(','))
    a = page(int(p))
    # pas kolies pod vozidlami: podvozky = siroke tmave behy (pismena su tenke)
    dark = a[y:y + h, x:x + w] < 100
    longest = 0
    for r in dark:
        run = 0
        for v in r:
            run = run + 1 if v else 0
            longest = max(longest, run)
    if longest > 12:
        continue
    for ly, lh in split_band(a, x, y, w, h):
        band = a[ly:ly + lh, x:x + w]
        ink = np.nonzero((band < 235).sum(0))[0]
        if len(ink) == 0:
            continue
        x0, x1 = x + ink.min(), x + ink.max() + 1
        lines.append(dict(page=int(p), train=train, x=int(x0), y=ly - 3, w=int(x1 - x0), h=lh + 6))

# skupiny: rovnaky vlak a sirka do 2 bodov = rovnaky text
groups = []
for l in lines:
    for g in groups:
        f = g[0]
        if f['train'] == l['train'] and abs(f['w'] - l['w']) <= 2 and abs(f['h'] - l['h']) <= 1:
            g.append(l)
            break
    else:
        groups.append([l])
print('riadkov', len(lines), 'skupin', len(groups))
json.dump(groups, open(os.path.join(D, 'notegroups.json'), 'w'), indent=0)

# mozaiky: useky riadkov po 640 bodoch (s presahom), 3x
K = 3
SEG = 640
os.makedirs(os.path.join(D, 'mos3'), exist_ok=True)
items = []
for i, g in enumerate(groups):
    f = g[0]
    for j, sx in enumerate(range(0, f['w'], SEG)):
        items.append((f"{i}{'abcdefgh'[j]}", f, sx))
for m in range(0, len(items), 36):
    chunk = items[m:m + 36]
    W = SEG * K + 80
    H = sum(f['h'] * K + 4 for _, f, _ in chunk)
    img = Image.new('L', (W, H), 255)
    d = ImageDraw.Draw(img)
    yy = 0
    for label, f, sx in chunk:
        a = page(f['page'])
        x0 = f['x'] + sx - (12 if sx else 2)
        crop = Image.fromarray(a[f['y']:f['y'] + f['h'], x0:min(f['x'] + f['w'] + 2, f['x'] + sx + SEG + 2)])
        crop = crop.resize((crop.width * K, crop.height * K), Image.LANCZOS)
        img.paste(crop, (76, yy))
        d.text((2, yy + 6), label, fill=0)
        yy += f['h'] * K + 4
    img.save(os.path.join(D, 'mos3', f'm{m:04d}.png'))
print('usekov', len(items))

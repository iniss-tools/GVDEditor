# Zhluky tvarov cislic cisel vozni (glyphs.tsv z PlanRadeniaReader --dump-glyphs) a obrazok na oznacenie
#   python cluster.py [prah podobnosti, predvolene 0.75 - s nim vznikli zhluky v labels.txt]
# Vystup v PLAN_WORK: clusters.json (vzor zhluku a jeho tvary s poctom), clusters.png (cislo zhluku cervene, pocet modre)
import collections, json, os, sys
from PIL import Image, ImageDraw

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (glyphs.tsv) - vysledky nepatria do repozitara.')

rows = [l.rstrip('\n').split('\t') for l in open(os.path.join(W, 'glyphs.tsv'), encoding='utf-8')]


def parse(p):
    """Tvar zo zapisu 'riadok/riadok/...' (tmavost 0-9)."""
    r = p.split('/')
    w = max(len(x) for x in r)
    return w, len(r), [[int(c) for c in x.ljust(w, '0')] for x in r]


def at(g, x, y):
    w, h, b = g
    return b[y][x] if 0 <= x < w and 0 <= y < h else 0


def sim(a, b):
    """Podobnost tvarov zarovnanych na spodok (posun o bod v kazdom smere), 1 = rovnake."""
    W_ = max(a[0], b[0]) + 1
    H = max(a[1], b[1])
    best = 0
    for dx in (-1, 0, 1):
        for dy in (-1, 0, 1):
            diff = ink = 0
            for y in range(H):
                for x in range(-1, W_):
                    pa = at(a, x, a[1] - H + y)
                    pb = at(b, x + dx, b[1] - H + y + dy)
                    if pa == 0 and pb == 0:
                        continue
                    ink += max(pa, pb)
                    diff += abs(pa - pb)
            if ink:
                best = max(best, 1 - diff / ink)
    return best


counts = collections.Counter(r[1] for r in rows)
thr = float(sys.argv[1]) if len(sys.argv) > 1 else 0.75
clusters = []
# najcastejsie tvary prve - stanu sa vzormi zhlukov
for pat, n in counts.most_common():
    g = parse(pat)
    best, bi = 0, -1
    for i, c in enumerate(clusters):
        mg = c[1]
        if abs(mg[0] - g[0]) > 2 or abs(mg[1] - g[1]) > 2:
            continue
        s = sim(g, mg)
        if s > best:
            best, bi = s, i
    if best >= thr:
        clusters[bi][2][pat] += n
    else:
        clusters.append([pat, g, collections.Counter({pat: n})])
clusters.sort(key=lambda c: -sum(c[2].values()))
print('zhlukov', len(clusters), [sum(c[2].values()) for c in clusters])
json.dump([[c[0], dict(c[2])] for c in clusters], open(os.path.join(W, 'clusters.json'), 'w'))

scale, cols = 7, 12
cell_w, cell_h = 10 * scale + 26, 11 * scale + 28
n = len(clusters)
img = Image.new('RGB', (cols * cell_w, ((n + cols - 1) // cols) * cell_h), 'white')
d = ImageDraw.Draw(img)
for i, (pat, g, mem) in enumerate(clusters):
    cx, cy = (i % cols) * cell_w, (i // cols) * cell_h
    w, h, b = g
    for y in range(h):
        for x in range(w):
            v = 255 - b[y][x] * 28
            d.rectangle([cx + 20 + x * scale, cy + 14 + y * scale, cx + 20 + x * scale + scale - 1, cy + 14 + y * scale + scale - 1], fill=(v, v, v))
    d.text((cx + 2, cy + 2), str(i), fill='red')
    d.text((cx + 40, cy + 2), str(sum(mem.values())), fill='blue')
img.save(os.path.join(W, 'clusters.png'))
print('obrazok', img.size)

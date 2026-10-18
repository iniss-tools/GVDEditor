# Navrh oznaceni novych zhlukov (clusters.json) podla doterajsich vzorov Digits.txt - prepise labels.txt.
#   python prelabel.py [najmensia podobnost, predvolene 0.8]
# Zhluk s podobnostou pod hranicou dostane ? - doplnte ho rucne podla clusters.png. Zhluk s menej ako 3 znakmi
# dostane - (do vzorov nejde).
import json, os, sys

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (clusters.json).')
HERE = os.path.dirname(os.path.abspath(__file__))


def parse(p):
    r = p.split('/')
    w = max(len(x) for x in r)
    return w, len(r), [[int(c) for c in x.ljust(w, '0')] for x in r]


def at(g, x, y):
    w, h, b = g
    return b[y][x] if 0 <= x < w and 0 <= y < h else 0


def sim(a, b):
    """Rovnaka podobnost ako pri zhlukovani (cluster.py)."""
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


limit = float(sys.argv[1]) if len(sys.argv) > 1 else 0.8
templates = []
for l in open(os.path.join(HERE, '..', '..', 'Digits.txt'), encoding='utf-8'):
    l = l.strip()
    if l and not l.startswith('#'):
        templates.append((l[0], parse(l[2:].strip())))

clusters = json.load(open(os.path.join(W, 'clusters.json')))
lines = ['# zhluk<TAB>cislica podla clusters.png; - = zhluk sa do vzorov nedava (zlepene alebo neuplne znaky)',
         '# navrh prelabel.py podla doterajsich vzorov; ? = treba doplnit rucne']
unsure = 0
for i, (med, members) in enumerate(clusters):
    g = parse(med)
    score, label = max(((sim(g, t), d) for d, t in templates), default=(0, '?'))
    if sum(members.values()) < 3:
        label = '-'
    elif score < limit:
        label = '?'
        unsure += 1
    lines.append(f'{i}\t{label}\t# {score:.2f}')
open(os.path.join(HERE, 'labels.txt'), 'w', encoding='utf-8').write('\n'.join(lines) + '\n')
print(len(clusters), 'zhlukov, na rucne doplnenie', unsure)

# Zhluky piktogramov z legiend stran planu (icons.tsv z PlanRadeniaReader --dump-icons) a obrazok na oznacenie
# Vystup v PLAN_WORK: iconclusters.json (stred zhluku = priemerne farby v mriezke 12x12, najcastejsie texty legendy
# z OCR), iconclusters.png (prvy vyskyt piktogramu s textom legendy vedla neho; cislo zhluku cervene, pocet modre).
# Obrazok potrebuje strany planu v PLAN_WORK/pages (PlanRadeniaReader --debug).
import collections, json, os, sys
from PIL import Image, ImageDraw

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (icons.tsv, pages/) - vysledky nepatria do repozitara.')

rows = []
for l in open(os.path.join(W, 'icons.tsv'), encoding='utf-8'):
    p, box, label, sample = l.rstrip('\n').split('\t')
    v = [int(x) for x in sample.split(',')]
    rows.append((int(p), box, label, v))


def diff(a, b):
    return sum(abs(x - y) for x, y in zip(a, b)) / len(a)


# piktogram patri k najblizsiemu zhluku, ak sa od jeho stredu lisi menej nez 18 na zlozku farby
clusters = []
for r in rows:
    best, bi = 1e9, -1
    for i, c in enumerate(clusters):
        d = diff(r[3], c['center'])
        if d < best:
            best, bi = d, i
    if best < 18:
        clusters[bi]['members'].append(r)
    else:
        clusters.append({'center': r[3], 'members': [r]})
clusters.sort(key=lambda c: -len(c['members']))
print('zhlukov', len(clusters), [len(c['members']) for c in clusters])
json.dump([{'center': c['center'], 'labels': collections.Counter(m[2] for m in c['members']).most_common(4),
            'first': [c['members'][0][0], c['members'][0][1]]} for c in clusters],
          open(os.path.join(W, 'iconclusters.json'), 'w', encoding='utf-8'), ensure_ascii=False)

cols = 2
cw, ch = 760, 90
img = Image.new('RGB', (cols * cw, ((len(clusters) + 1) // cols) * ch), 'white')
d = ImageDraw.Draw(img)
pages = {}
for i, c in enumerate(clusters):
    p, box, label, v = c['members'][0]
    if p not in pages:
        pages[p] = Image.open(os.path.join(W, 'pages', f'page{p}.png')).convert('RGB')
    pos, size = box.split(' ')
    x, y = map(int, pos.split(','))
    w, h = map(int, size.split('x'))
    # piktogram a text legendy napravo od neho, 2x
    crop = pages[p].crop((x - 2, y - 2, x + w + 330, y + h + 2))
    crop = crop.resize((crop.width * 2, crop.height * 2))
    cx, cy = (i % cols) * cw, (i // cols) * ch
    img.paste(crop.crop((0, 0, cw - 40, ch - 16)), (cx + 36, cy + 4))
    d.text((cx + 2, cy + 4), str(i), fill='red')
    d.text((cx + 2, cy + 20), str(len(c['members'])), fill='blue')
img.save(os.path.join(W, 'iconclusters.png'))
print('obrazok', img.size)

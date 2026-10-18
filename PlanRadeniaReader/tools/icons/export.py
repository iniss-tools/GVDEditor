# Zapise Icons.txt (vlozeny do PlanRadeniaReader): popis piktogramu z labels.txt a stred zhluku.
import json, os, sys

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (iconclusters.json).')
HERE = os.path.dirname(os.path.abspath(__file__))

labels = {}
for l in open(os.path.join(HERE, 'labels.txt'), encoding='utf-8'):
    if l.strip() and not l.startswith('#'):
        k, v = l.rstrip('\n').split('\t')[:2]
        labels[int(k)] = v.strip()

if '?' in labels.values():
    sys.exit('V labels.txt su este neoznacene zhluky (?).')
clusters = json.load(open(os.path.join(W, 'iconclusters.json'), encoding='utf-8'))
missing = [i for i in range(len(clusters)) if i not in labels]
if missing:
    sys.exit(f'Zhluky bez popisu v labels.txt: {missing}')
out = ['# katalog piktogramov planu radenia ZSSK: popis z legendy, tabulator, priemerne farby piktogramu v mriezke 12x12 (R,G,B za sebou)',
       '# zostavene z legiend vsetkych stran planu 1.9.-12.12.2026; viac riadkov s rovnakym popisom su varianty kreslenia']
for i, c in enumerate(clusters):
    out.append(labels[i] + '\t' + ','.join(str(int(v)) for v in c['center']))
path = os.path.join(HERE, '..', '..', 'Icons.txt')
open(path, 'w', encoding='utf-8', newline='\r\n').write('\n'.join(out) + '\n')
print(len(out) - 2, 'piktogramov ->', os.path.normpath(path))

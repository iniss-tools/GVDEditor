# Zapise Digits.txt (vlozeny do PlanRadeniaReader): pre kazdy oznaceny zhluk cislicu a vzor zhluku.
# Zhluky s - v labels.txt a zhluky s menej ako 3 znakmi sa vynechaju.
import json, os, sys

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (clusters.json).')
HERE = os.path.dirname(os.path.abspath(__file__))

labels = {}
for l in open(os.path.join(HERE, 'labels.txt'), encoding='utf-8'):
    if l.strip() and not l.startswith('#'):
        k, v = l.rstrip('\n').split('\t')[:2]
        labels[int(k)] = v.strip()

if '?' in labels.values():
    sys.exit('V labels.txt su este neoznacene zhluky (?).')
clusters = json.load(open(os.path.join(W, 'clusters.json')))
out = ['# vzory cislic cisel vozni z planu radenia ZSSK pri 200 dpi: cislica, medzera, tmavost 0-9 po riadkoch oddelenych lomkou',
       '# cislice su orezane strechou vozna (spodne 1-2 riadky), vzory su stredy zhlukov tvarov z planu 1.9.-12.12.2026']
for i, (med, members) in enumerate(clusters):
    if labels.get(i, '-') == '-' or sum(members.values()) < 3:
        continue
    out.append(f'{labels[i]} {med}')
path = os.path.join(HERE, '..', '..', 'Digits.txt')
open(path, 'w', encoding='utf-8', newline='\r\n').write('\n'.join(out) + '\n')
print(len(out) - 2, 'vzorov ->', os.path.normpath(path))

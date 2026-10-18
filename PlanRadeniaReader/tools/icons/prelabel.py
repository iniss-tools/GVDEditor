# Navrh popisov novych zhlukov (iconclusters.json) podla doterajsieho katalogu Icons.txt - prepise labels.txt.
# Zhluk, ktory sa od kazdeho piktogramu katalogu lisi aspon o 18 na zlozku farby (ako pri zhlukovani), dostane ?
# a v poznamke text legendy z OCR - doplnte ho rucne podla iconclusters.png presne podla legendy ZSSK.
import json, os, sys

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (iconclusters.json).')
HERE = os.path.dirname(os.path.abspath(__file__))

catalog = []
for l in open(os.path.join(HERE, '..', '..', 'Icons.txt'), encoding='utf-8'):
    if l.strip() and not l.startswith('#'):
        label, values = l.rstrip('\r\n').split('\t')
        catalog.append((label, [int(v) for v in values.split(',')]))


def diff(a, b):
    return sum(abs(x - y) for x, y in zip(a, b)) / len(a)


clusters = json.load(open(os.path.join(W, 'iconclusters.json'), encoding='utf-8'))
lines = ['# zhluk<TAB>popis piktogramu z legendy (podla iconclusters.png); rovnaky popis pri viacerych zhlukoch = varianty kreslenia',
         '# popisy su texty ZSSK - PlanRadeniaConversion v GVDEditore podla nich urcuje druh vozna (napr. „vozeň 1. triedy“)',
         '# navrh prelabel.py podla doterajsieho katalogu; ? = treba doplnit rucne']
unsure = 0
for i, c in enumerate(clusters):
    d, label = min(((diff(c['center'], v), lab) for lab, v in catalog), default=(1e9, '?'))
    ocr = c['labels'][0][0] if c['labels'] else ''
    if d >= 18:
        label = '?'
        unsure += 1
    lines.append(f'{i}\t{label}\t# {d:.1f} OCR: {ocr}')
open(os.path.join(HERE, 'labels.txt'), 'w', encoding='utf-8').write('\n'.join(lines) + '\n')
print(len(clusters), 'zhlukov, na rucne doplnenie', unsure)

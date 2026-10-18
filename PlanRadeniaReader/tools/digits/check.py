# Kontrola oznaceni zhlukov podla susednych cisel vozni: cisla vozni za sebou sa vacsinou lisia o 1, takze cislo
# zo zhluku s nejasnym oznacenim (napr. 6 a 9 su si podobne) by malo mat suseda +-1.
#   python check.py [zhluky na kontrolu oddelene ciarkou, predvolene zhluky cislic 0, 3, 6, 8 a 9 - zamienaju sa]
# Vypise podozrive cisla a pre kazdy kontrolovany zhluk, kolkokrat jeho cislo malo / nemalo suseda.
import collections, json, os, sys

W = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (glyphs.tsv, clusters.json).')
HERE = os.path.dirname(os.path.abspath(__file__))

labels = {}
for l in open(os.path.join(HERE, 'labels.txt'), encoding='utf-8'):
    if l.strip() and not l.startswith('#'):
        k, v = l.rstrip('\n').split('\t')[:2]
        labels[int(k)] = v.strip()
clusters = json.load(open(os.path.join(W, 'clusters.json')))
pat2c = {p: i for i, (med, members) in enumerate(clusters) for p in members}
amb = {int(x) for x in sys.argv[1].split(',')} if len(sys.argv) > 1 else {i for i, d in labels.items() if d in '03689'}

rows = [l.rstrip('\n').split('\t') for l in open(os.path.join(W, 'glyphs.tsv'), encoding='utf-8')]
veh = collections.defaultdict(list)
for where, pat in rows:
    page, train, bx, gx = where.split(':')
    veh[(page, train)].append((int(bx), int(gx), pat2c[pat]))

stats = collections.Counter()
for (page, train), items in veh.items():
    byv = collections.defaultdict(list)
    for bx, gx, c in items:
        byv[bx].append((gx, c))
    # cisla vozni v poradi zlava
    nums = []
    for bx in sorted(byv):
        cs = [c for gx, c in sorted(byv[bx])]
        nums.append((''.join(labels.get(c, '?') for c in cs), cs))
    seq = [int(n) if n.isdigit() else -99 for n, _ in nums]
    for i, (n, cs) in enumerate(nums):
        if not amb & set(cs):
            continue
        nb = [seq[j] for j in (i - 1, i + 1) if 0 <= j < len(seq)]
        ok = n.isdigit() and any(abs(int(n) - x) == 1 for x in nb)
        for c in cs:
            if c in amb:
                stats[(c, ok)] += 1
        if not ok and stats[(cs[0], False)] < 3:
            print(page, train, 'cislo', n, 'susedia', nb, 'zhluky', cs)
print(sorted(stats.items()))

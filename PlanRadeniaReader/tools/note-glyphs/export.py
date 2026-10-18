# Vzory pisma poznamok pre PlanRadeniaReader (NoteGlyphs.txt):
#  - naucene vzory (median buniek podla sirky a fazy)
#  - nahradne vzory z pisma Arial pre znaky, ktore v plane neboli (rozmazanie a kontrast nastavene podla naucenych)
#  - zvisly profil riadku na zarovnanie
import os, sys, pickle, unicodedata
import numpy as np
from PIL import Image, ImageDraw, ImageFont, ImageFilter
# pracovny priecinok: pages/pageN.png a notes.tsv z PlanRadeniaReader (--debug, --dump-notes), sem idu aj vysledky
D = os.environ.get('PLAN_WORK') or sys.exit('Nastavte PLAN_WORK na pracovny priecinok (pages/, notes.tsv) - vysledky nepatria do repozitara.')
HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)
import align as A

data = pickle.load(open(os.path.join(D, 'aligned.pkl'), 'rb'))
real = data['templates']
FONT = 'C:/Windows/Fonts/arial.ttf'
SS = 8

def synth(c, size, ph, sigma, gain, gamma, w=None):
    font = ImageFont.truetype(FONT, size * SS)
    adv = font.getlength(c) / SS
    if w is None:
        w = max(2, int(round(adv)))
    im = Image.new('L', ((w + 2) * SS, A.ROWS * SS), 0)
    ImageDraw.Draw(im).text((ph * SS / 4 + 0.0, A.TOP * SS), c, font=font, fill=255, anchor='ls')
    a = np.asarray(im).astype(np.float32)[:, :w * SS]
    a = a.reshape(A.ROWS, SS, w, SS).mean((1, 3)) / 255.0
    if sigma > 0:
        im2 = Image.fromarray((a * 255).astype(np.uint8)).filter(ImageFilter.GaussianBlur(sigma))
        a = np.asarray(im2).astype(np.float32) / 255.0
    return np.clip(gain * a ** gamma, 0, 1)

def fit_error(size, sigma, gain, gamma):
    err = 0; n = 0
    for c in 'abdeghinoprstuvz0123456789':
        if c not in real:
            continue
        for t in real[c][:3]:
            w = t.shape[1]
            best = min(((synth(c, size, ph, sigma, gain, gamma, w) - t) ** 2).sum() for ph in range(4))
            err += best; n += 1
    return err / n

if __name__ == '__main__':
    best = None
    for size in (10.4, 10.7, 11.0):
        for sigma in (0.8, 1.0, 1.2):
            for gain in (0.8, 1.0, 1.2):
                for gamma in (0.8, 1.0, 1.3):
                    e = fit_error(size, sigma, gain, gamma)
                    if best is None or e < best[0]:
                        best = (e, size, sigma, gain, gamma)
    print('nahradne pismo', best)
    _, size, sigma, gain, gamma = best
    # zname znaky z pisma - slovenske, ceske, polske, madarske, nemecke pismena a bezne znaky
    extra = set('ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz')
    extra |= set('ÁÄČĎÉĚÍĹĽŇÓÔŔŘŠŤÚŮÝŽáäčďéěíĺľňóôŕřšťúůýžĄĆĘŁŃŚŹŻąćęłńśźżÖŐÜŰöőüűß')
    extra |= set('()/+&:;\'')
    lines = ['# Vzory pisma poznamok planu radenia (generuje tools/note-glyphs/export.py)',
             '# riadok: znak <TAB> sirka <TAB> zdroj (r = naucene, s = nahradne) <TAB> ROWS*sirka hodnot 0-255 po riadkoch',
             f'# ROWS {A.ROWS} TOP {A.TOP}']
    ref = A.reference()
    lines.append('profil\t' + ' '.join(f'{v:.6f}' for v in ref))
    count = {'r': 0, 's': 0}
    for c in sorted(real):
        for t in real[c]:
            lines.append(f'{c}\t{t.shape[1]}\tr\t' + ' '.join(str(int(round(v * 255))) for v in t.ravel()))
            count['r'] += 1
    for c in sorted(extra - set(real)):
        font = ImageFont.truetype(FONT, size * SS)
        adv = font.getlength(c) / SS
        for w in sorted({max(2, int(np.floor(adv))), max(2, int(np.ceil(adv)))}):
            for ph in (0, 2):
                t = synth(c, size, ph, sigma, gain, gamma, w)
                lines.append(f'{c}\t{w}\ts\t' + ' '.join(str(int(round(v * 255))) for v in t.ravel()))
                count['s'] += 1
    out = os.path.join(HERE, '..', '..', 'NoteGlyphs.txt')
    open(out, 'w', encoding='utf-8', newline='\n').write('\n'.join(lines) + '\n')
    print('vzorov', count, 'subor', os.path.getsize(out))

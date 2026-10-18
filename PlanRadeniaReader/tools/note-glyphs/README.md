# Vzory písma poznámok plánu radenia

`NoteGlyphs.txt` (vložený do PlanRadeniaReader) sú vzory znakov drobného písma poznámok pod radením
(asi 9 bodov na výšku pri 200 dpi). Rovnaký znak vyzerá inak podľa posunu o zlomok bodu, preto má znak
viac vzorov podľa šírky bunky a polohy znaku v nej. Vzory sa naučia z prepisov riadkov poznámok:

1. Strany plánu a polohy riadkov poznámok do pracovného priečinka `W` mimo repozitára (skripty ho čítajú
   z premennej `PLAN_WORK` a zapisujú doň všetky medzivýsledky; bez nej skončia):

   ```
   PlanRadeniaReader --pdf plan.pdf --pages 1-999 --debug W\pages --dump-notes W\notes.tsv
   ```

   (`--debug` uloží strany ako `pageN.png`; ostatné ladiace obrázky netreba.)
2. `python notelines.py` – zoskupí rovnaké riadky (vlak + šírka) do `notegroups.json` a uloží mozaiky
   `mos3/*.png` na prepis.
3. Prepis do `trans.txt`: `skupina<TAB>text`; čísla dní v krúžku ako ①–⑦, kladivká (pracovné dni) ako `%`,
   odrážka `•`. Skupina s viacerými rôznymi riadkami môže mať viac prepisov – platí ten, ktorý sedí najlepšie.
   Prepis patrí k číslovaniu skupín z kroku 2, teda k jednému vydaniu plánu.
4. `python align.py` – zarovná prepisy k obrázkom (najprv vzormi z písma Arial, potom zo skutočných znakov)
   a uloží `aligned.pkl`.
5. `python export.py` – zapíše `NoteGlyphs.txt` (naučené vzory, náhradné vzory písma Arial pre znaky,
   ktoré v pláne neboli, a zvislý profil riadku).

Potrebný je Python s `numpy` a `Pillow` (a písmo Arial vo Windows).
Trans.txt je prepis plánu radenia platného od 1.9.2026 (`Radenie_vlakov_web_5okt2026.pdf`).

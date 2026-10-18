# Vzory číslic čísel vozňov

`Digits.txt` (vložený do PlanRadeniaReader) sú vzory číslic čísel vozňov. Číslo stojí na streche vozňa a spodok
číslic s ňou splýva, preto sú vzory orezané strechou. Vzor je najčastejší tvar zhluku podobných tvarov.

Všetky skripty pracujú v pracovnom priečinku mimo repozitára z premennej `PLAN_WORK` (bez nej skončia).

1. Tvary číslic všetkých vozňov plánu:

   ```
   PlanRadeniaReader --pdf plan.pdf --pages 1-999 --dump-glyphs W\glyphs.tsv
   ```

2. `python cluster.py` – zhluky podobných tvarov (`clusters.json`) a obrázok `clusters.png` (číslo zhluku červené,
   počet tvarov modré). Prah podobnosti 0,75 je predvolený, s ním vznikli zhluky v `labels.txt`.
3. `python prelabel.py` – navrhne číslicu každého zhluku podľa doterajších vzorov `Digits.txt` a prepíše
   `labels.txt`. Zhluky, ktoré sa nepodobajú žiadnemu vzoru, dostanú `?` – doplňte ich podľa `clusters.png`;
   `-` = zhluk do vzorov nejde (zlepené alebo neúplné znaky).
4. `python check.py` – kontrola podľa susedných vozňov: čísla vozňov za sebou sa líšia o 1, takže číslo zo
   zhluku s nesprávnou číslicou (zámena 6/9, 3/8) suseda nemá. Vypíše podozrivé čísla a počty.
5. `python export.py` – zapíše `Digits.txt` (neoznačené zhluky `?` nepustí).

`labels.txt` v repozitári patrí k výpisu plánu platného od 1.9.2026 (`Radenie_vlakov_web_5okt2026.pdf`) staršou
verziou čítačky – čerstvý výpis dá iné číslovanie zhlukov, preto vždy začnite krokom 3.

Potrebný je Python s `Pillow`.

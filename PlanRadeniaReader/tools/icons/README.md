# Katalóg piktogramov vozňov

`Icons.txt` (vložený do PlanRadeniaReader) je katalóg piktogramov vozňov: popis z legendy ZSSK a priemerné farby
piktogramu v mriežke 12×12. Piktogram vo voze sa porovná s katalógom a vozeň dostane jeho popis. Podľa popisov
GVDEditor určuje druh vozňa (`PlanRadeniaConversion.KindOf`: *vozeň 1. triedy*, *lôžkový vozeň*…) – preto musia
byť presne ako v legende.

Všetky skripty pracujú v pracovnom priečinku mimo repozitára z premennej `PLAN_WORK` (bez nej skončia).

1. Piktogramy z legiend všetkých strán a strany ako obrázky (na kontrolu):

   ```
   PlanRadeniaReader --pdf plan.pdf --pages 1-999 --dump-icons W\icons.tsv --debug W\pages
   ```

2. `python cluster.py` – zhluky rovnakých piktogramov (`iconclusters.json`) a obrázok `iconclusters.png`
   (piktogram s textom legendy vedľa neho, číslo zhluku červené, počet modré).
3. `python prelabel.py` – navrhne popis každého zhluku podľa doterajšieho katalógu `Icons.txt` a prepíše
   `labels.txt`. Nový piktogram dostane `?` a v poznámke text legendy z OCR – popis doplňte presne podľa legendy
   na obrázku. Ten istý piktogram nakreslený inak tvorí vlastný zhluk s rovnakým popisom.
4. `python export.py` – zapíše `Icons.txt` (neoznačené zhluky `?` nepustí).

Nový druh vozňa, ktorý sa má hlásiť v radení, treba doplniť aj do `PlanRadeniaConversion.KindOf` v GVDEditore.

`labels.txt` v repozitári patrí k výpisu plánu platného od 1.9.2026 (`Radenie_vlakov_web_5okt2026.pdf`).

Potrebný je Python s `Pillow`.

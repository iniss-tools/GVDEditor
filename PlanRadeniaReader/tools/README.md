# Nástroje na vzory PlanRadeniaReader

PlanRadeniaReader číta plán radenia vlakov ZSSK podľa vzorov naučených z plánu. Vzory sú vložené do programu,
import radenia ich len číta. Nanovo ich treba naučiť, len ak ZSSK zmení vzhľad plánu – prejaví sa to veľa
neistými poznámkami a vozňami s neznámym druhom v kontrole importu.

| Súbor            | Čo obsahuje                           | Nástroj                              |
|------------------|---------------------------------------|--------------------------------------|
| `Digits.txt`     | číslice čísel vozňov                  | [digits](digits/README.md)           |
| `Icons.txt`      | piktogramy vozňov s popisom z legendy | [icons](icons/README.md)             |
| `NoteGlyphs.txt` | písmo poznámok pod radením            | [note-glyphs](note-glyphs/README.md) |

Všetky nástroje zdieľajú pracovný priečinok mimo repozitára v premennej `PLAN_WORK`, kam PlanRadeniaReader
zapíše výpisy (`--dump-glyphs`, `--dump-icons`, `--dump-notes`, `--debug` so stranami ako obrázkami) a nástroje
medzivýsledky. Potrebný je Python s `numpy` a `Pillow`.

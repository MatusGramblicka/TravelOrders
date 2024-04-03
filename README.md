![](https://github.com/MatusGramblicka/TravelOrders/raw/master/Sample.png)

# Všeobecný popis
Cieľom úlohy je vytvorenie jednoduchej aplikácie pre správu cestovných príkazov vo veľmi 
obmedzenom rozsahu s dôrazom na správny návrh dátových štruktúr a prácu s dátovou vrstvou. Pre 
formu užívateľského rozhrania nie sú definované žiadne požiadavky a nebude predmetom 
hodnotenia.
# Základné entity
## Zamestnanec
Každý zamestnanec je jednoznačne identifikovaný osobným číslom vo forme reťazca (nemusí 
obsahovať iba číslice) o max. dĺžke 10 znakov. Pri zamestnancovi je potrebné tiež evidovať krstné 
meno, priezvisko, dátum narodenia a rodné číslo.
## Mesto
Entita predstavuje geografické umiestnenie, pri ktorom je potrebné evidovať názov mesta 
(obce), štát a zemepisné súradnice (šírka, dĺžka).
## Cestovný príkaz (CP)
Ide o primárnu entitu definujúcu pracovnú cestu, ktorá musí poskytovať nasledovné 
informácie:
 - Jednoznačný identifikátor CP automaticky generovaný systémom (celé číslo)
 - Dátum vytvorenia CP automaticky generovaný systémom
 - Účastník pracovnej cesty – záznam z evidencie zamestnancov
 - Miesto začiatku pracovnej cesty – záznam z evidencie miest
 - Miesto konca pracovnej cesty – záznam z evidencie miest
 - Dátum a čas začiatku pracovnej cesty
 - Dátum a čas konca pracovnej cesty
 - Doprava – 1 až N položiek z nasledovnej skupiny možností:
    1. Služobné auto
    2. Autobus
    3. MHD
    4. Pešo 
    5. Vlak
    6. Taxi
    7. Lietadlo
 - Stav – vždy jedna z nasledovných možností
    1. Vytvorený
    2. Schválený
    3. Vyúčtovaný
    4. Zrušený
# Funkčné požiadavky
Aplikácia zobrazí na úvodnej stránke zoznam evidovaných cestovných príkazov s možnosťou 
filtrovania podľa účastníka pracovnej cesty a ponúkne UI pre nasledovné funkcie:
 - Vytvorenie nového CP
 - Editácia existujúceho CP
 - Vymazanie existujúceho CP

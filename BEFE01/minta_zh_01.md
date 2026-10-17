# Minta ZH 01

## Bevezető
A repository-hoz hozzáadásra került egy MiningService. Ez egy olyan szolgáltatás, amely egy megadott stringhez keres egy egész számot (nonce), hogy a string és a szám összefűzéséből számított SHA-256 hash adott számú nullával kezdődjön. Pl: Hello world és 6 bemenet esetén 40-50mp-es futásidő várható a nonce eggyel nagyobbra állítása kb. 16x-osára növeli a futásidőt

## 1. MiningController (8p)
Készíts el egy olyan Controllert és API végpontot, ahol egy új szövegbányászási feladatot tudunk indítani. Készítsd el a szükséges DTO-t! Amikor egy szövegbányászási feladat elkészül, akkor a MiningResult-ot mentsük el egy adattáblába! Tehát a Controllernek egy elindító végponttal és egy minden resultot lekérő végponttal kell rendelkeznie! Ennél a feladatnál még kis difficulty esetén megvárhatod az eredményt és visszaadhatot a végpontról a MiningResult-ot!

## 2. API kliens (2p)
Generáld le a kliens nswag-gel! Lehessen onnan is indítani a bányászást, ne csak swaggerből!

## 3. Aszinkron job (4p)
A bányászás mostmár nagyobb difficulty-val HangFire-el történjen aszinkron jobként! Az indító végpont csak egy OK-t vagy ID-t kapjon arról, hogy elindult a bányászási folyamat!

## 4. Aszinkron progress értesítések (4p)
Nem tudjuk előre megjósolni, hogy hány iteráció múlva találunk eredményt. Mindenesetre pl. 1000 iterációnként küldjünk le értesítést SignalR-el a klienseknek, hogy halad a bányászási folyamat!

## 5. Aszinkron végeredmény értesítés (2p)
Amikor megtaláljuk a kívánt eredményt, akkor egy másik SignalR csatornán jelezzük a kliensek felé, hogy készen vagyunk! A feladatban mehet minden kliensnek az értesítés, nem kell elkülöníteni, hogy melyik feladatot, melyik kliens indította el!

Zárthelyi összesen: 20 pont \
Végrehajtási idő: 120 perc
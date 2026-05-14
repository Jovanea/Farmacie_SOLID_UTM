# Laborator 2 - Design Patterns Creaționale

Acest document descrie implementarea teoretică și practică a celor 5 patternuri creaționale: **Singleton, Factory Method, Abstract Factory, Builder** și **Prototype** în aplicația `Farmacie_SOLID_UTM`.

---

## 1. Singleton (Instanță Unică)

**Definiție:** Asigură existența unei singure instanțe a unei clase și oferă un punct global de acces la ea.

**Ce problemă rezolvă?**
* **La general:** Previne crearea multiplă a resurselor care trebuie să fie unice (baze de date, setări, loguri).
* **În aplicația mea:** Automatizează și securizează gestiunea stocului central.
* **Exemplu clar:** Indiferent dacă adaugi un produs de la casa de marcat sau faci recepție în depozit, vrei ca ambele acțiuni să se reflecte în *același* inventar central. Singleton garantează că nimeni nu poate crea un al doilea inventar fantomă din greșeală prin comanda `new StocManager()`.

**Cod scurt:**
```csharp
// Variabilele din module diferite acceseaza strict acelasi obiect din memorie
var inventarGhișeu = StocManager.Instance;
var inventarDepozit = StocManager.Instance;

inventarGhișeu.AdaugaProdus(aspirina);
// inventarDepozit va vedea și el aspirina adăugată instant
```

**Explicație Cod:** Constructorul este declarat `private`, blocând din fașă orice încercare de a da `new`. Când se cere instanța prin proprietatea publică statică `Instance`, clasa verifică în culise dacă obiectul a fost deja creat anterior. Dacă da, îl întoarce pe cel existent, dacă nu, îl construiește o singură dată (Lazy Initialization).

**Diagramă Desenată Visual (ASCII):**
```text
       +-------------------------+
       |       StocManager       |
       +-------------------------+
       | - static _instance      |
       +-------------------------+
       | - StocManager()         |
       | + static GetInstance()  |
       +-------------------------+
```

**Explicație diagramă:** Clasa `StocManager` deține o referință statică privată către ea însăși (linia cu minus). Publicul și restul aplicației au acces la obiect exclusiv prin funcția statică `GetInstance()`.

---

## 2. Factory Method (Metoda Fabrică)

**Definiție:** Definește o interfață abstractă pentru crearea unui obiect, lăsând subclasele să decidă tipul exact de obiect ce va fi instanțiat.

**Ce problemă rezolvă?**
* **La general:** Decuplează logica generală a aplicației de crearea fizică directă a claselor concrete (elimină invaziile de if-uri cu `new ClasaConcreta()`).
* **În aplicația mea:** Crearea dinamică de produse farmaceutice în funcție de selecția făcută din frontend (UI).
* **Exemplu clar:** Utilizatorul apasă pe dropdown "Medicament" și apoi pe "Adaugă". În loc să scriem în panoul vizual zeci de if-uri greoaie, pasăm string-ul către o "Fabrică". Fabrica știe cum să construiască acel tip de date și ne returnează obiectul proaspăt, eliberând interfața de detaliile construirii.

**Cod scurt:**
```csharp
// Aplicatia nu stie cum se construieste un Nurofen. Deleaga unei fabrici.
ProdusFactory fabrica = new MedicamentFactory();

// Fabrica intoarce un Produs abstractizat
Produs nurofen = fabrica.CreazaProdus("Nurofen", 25.5m); 
```

**Explicație Cod:** Clientul lucrează doar cu interfața generală `ProdusFactory` și așteaptă să primească un `Produs`. Implementarea concretă (`MedicamentFactory`) știe rețeta pentru a instanția corect un `Medicament` și a seta câmpurile necesare înainte de a-l returna clientului.

**Diagramă Desenată Visual (ASCII):**
```text
   +-------------------+              +-------------------+
   |  ProdusFactory    |              |      Produs       |
   |   <<abstract>>    |              |   <<abstract>>    |
   +-------------------+              +-------------------+
   | + CreazaProdus()  |              |                   |
   +--------+----------+              +---------+---------+
            | (moștenește)                      | (moștenește)
            V                                   V
   +-------------------+              +-------------------+
   | MedicamentFactory | -- creează-> |    Medicament     |
   +-------------------+              +-------------------+
```

**Explicație diagramă:** Fabrica abstractă delegă responsabilitatea creării efective către subclasa sa `MedicamentFactory`, care cunoaște dependențele și instanțiază produsul concret `Medicament` printr-o relație direcțională de creare (săgeata dreapta).

---

## 3. Abstract Factory (Fabrica Abstractă)

**Definiție:** Oferă o interfață pentru crearea unor familii întregi de obiecte înrudite sau dependente, fără a le specifica direct clasele concrete.

**Ce problemă rezolvă?**
* **La general:** Previne amestecarea accidentală a obiectelor incompatibile din familii diferite (ex: asortarea UI-ului modern vs. clasic).
* **În aplicația mea:** Crearea truselor medicale cu produse 100% asortate și compatibile (Trusa Adulți vs. Trusa Copii).
* **Exemplu clar:** Vrei să faci o trusă pentru copii. Folosești `TrusaCopiiFactory`. Când îi ceri un sirop, ea îți dă `SiropCopii` (dozaj mic). Când îi ceri un plasture, ea îți dă `PlastureColorat`. Fabrica garantează că nu va scăpa niciodată din greșeală un medicament forte pentru adulți în trusa de copii, produsele fiind mereu din aceeași familie.

**Cod scurt:**
```csharp
// Instantiem fabrica potrivita familiei de produse de copii
ITrusaFactory fabricaCopii = new TrusaCopiiFactory();

// Ne asiguram matematic ca elementele generate se asorteaza
var sirop = fabricaCopii.CreareMedicamentDurere();
var bandaj = fabricaCopii.CreareBandaj();
```

**Explicație Cod:** Nu dăm `new Sirop()` și `new Bandaj()` manual, riscând greșeli. Folosind un obiect central de creare, el dictează compatibilitatea. Indiferent ce fabrică bagi în stânga (`ITrusaFactory`), codul tău va obține cele 2 produse esențiale aferente acelei nișe.

**Diagramă Desenată Visual (ASCII):**
```text
          +-----------------------+
          |    ITrusaFactory      |
          |     <<interface>>     |
          +-----------------------+
          | + CreareMedicament()  |
          | + CreareBandaj()      |
          +----------+------------+
                     |
            +--------+--------+
            |                 | (implementează)
            V                 V
 +--------------------+ +--------------------+
 | TrusaCopiiFactory  | | TrusaAdultiFactory |
 +--------------------+ +--------------------+
```

**Explicație diagramă:** Contractul de bază (`ITrusaFactory`) dictează obligativitatea fabricării setului complet de produse complementare. Fabricile concrete livrează implementările stricte ale familiilor lor.

---

## 4. Builder (Constructorul)

**Definiție:** Separă total construcția unui obiect complex de reprezentarea sa internă, permițând asamblarea aceluiași tip de obiect pas cu pas, prin procese personalizabile.

**Ce problemă rezolvă?**
* **La general:** Elimină constructorii gigantici, plini de variabile opționale urâte (Telescoping Constructor Anti-Pattern).
* **În aplicația mea:** Asamblarea truselor medicale modulare și personalizate (ex: Trusa de Vacanță, Trusa de Prim-Ajutor).
* **Exemplu clar:** O trusă poate conține dezinfectant, pastile, seringi, etc. În loc să forțăm crearea ei dintr-o singură linie greoaie, `Builder`-ul ne dă posibilitatea să adăugăm treptat: punem aspirina, apoi adăugăm plasturii. Totul este dirijat de clasa `Director` care ține minte pașii rețetei.

**Cod scurt:**
```csharp
var builder = new TrusaBuilder();
var director = new TrusaDirector(builder);

// Directorul dicteaza ordinea si pasii pe care sa-i aplice builder-ul
TrusaMedicala trusaVacanta = director.ConstructTrusaVacanta();
```

**Explicație Cod:** Directorul cunoaște planul arhitectural al trusei de vacanță. El apelează pe rând, secvențial, funcțiile din `builder` (ex: `AdaugaDezinfectant()`, `AdaugaPastile()`). Abia la terminarea asamblării, builder-ul livrează produsul finit curat.

**Diagramă Desenată Visual (ASCII):**
```text
   +----------------+             +-----------------+
   | TrusaDirector  | -- comandă->|  TrusaBuilder   |
   +----------------+             +-----------------+
   | + Construct()  |             | + AdaugaPastile()|
   +----------------+             | + AdaugaBandaj()|
                                  | + GetTrusa()    |
                                  +-----------------+
```

**Explicație diagramă:** Săgeata arată clar cine conduce. `TrusaDirector` deține referința instrumentului de construire `Builder` și îl orchestrează pas cu pas, lăsând clientul principal complet descărcat de sarcina asamblării.

---

## 5. Prototype (Prototipul)

**Definiție:** Specifică tipul de obiect pe care vrei să-l creezi apelând la o instanță-prototip gata făcută, și generează obiecte complet noi clonând acest prototip.

**Ce problemă rezolvă?**
* **La general:** Salvează cantități imense de timp și procesor atunci când reconstrucția unui obiect de la zero necesită interogări grele de rețea sau preluări din baze de date.
* **În aplicația mea:** Duplicarea instantanee a medicamentelor perfect configurate din tabela de gestiune.
* **Exemplu clar:** Avem "Aspirină" care a fost completată de farmacist cu 10 setări grele: TVA, preț de bază, cod producător, marjă, unitate măsură. Când trebuie să adăugăm un produs similar pe alt lot, nu repornim procesul manual! Apelăm clonarea și obținem un obiect nou, gata completat cu valorile inițiale.

**Cod scurt:**
```csharp
// Luam un obiect original direct din sistemul existent
Produs aspirinaOriginala = stoc.GetProdus("Aspirina");

// Cream un produs nou in memorie, copiind garantat 100% din atribute
Produs aspirinaClona = aspirinaOriginala.Cloneaza();
```

**Explicație Cod:** Contractul pattern-ului se bazează pe metoda polimorfică `Cloneaza()`. În spate, aceasta execută copierea superficială sau adâncă (`MemberwiseClone()`) generând o instanță nouă de ram perfect desprinsă, scutindu-ne total de reconfigurarea atributelor.

**Diagramă Desenată Visual (ASCII):**
```text
          +-----------------------+
          |       Produs          |
          |    <<abstract>>       |
          +-----------------------+
          | + Cloneaza()          |
          +----------+------------+
                     | (moștenește)
             +-------+-------+
             |               |
             V               V
   +------------------+ +------------------+
   |   Medicament     | | EchipamentMedical|
   +------------------+ +------------------+
   | + Cloneaza()     | | + Cloneaza()     |
   +------------------+ +------------------+
```

**Explicație diagramă:** Fiecare subclasă inferioară din sistem implementează funcția obligatorie a Părintelui `Cloneaza()`. Apelantul pur și simplu prinde obiectul de interfața superioară `Produs` și cere dublura. Nu îl interesează dacă sub capotă a clonat un sirop sau o seringă.

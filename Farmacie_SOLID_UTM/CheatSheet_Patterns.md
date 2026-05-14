# Ghid Complet: Cele 22 Design Pattern-uri în Farmacie_SOLID_UTM

Acest document descrie implementarea teoretică și practică a celor 22 de design pattern-uri GoF (exceptând Interpreter) în aplicația `Farmacie_SOLID_UTM`, structurate exact pentru prezentare, **cu diagramele desenate direct din text** pentru a fi vizibile în absolut orice program.

---

## 1. Singleton (Instanță Unică)

**Definiție:** Asigură existența unei singure instanțe a unei clase și oferă un punct global de acces la ea.

**Ce problemă rezolvă?**
* **La general:** Previne crearea multiplă a resurselor care trebuie să fie unice (baze de date, setări).
* **În aplicația mea:** Asigură un singur inventar centralizat pentru toată farmacia.
* **Exemplu clar:** Indiferent de fereastra din care adaugi un produs, toți folosesc același `StocManager` central, evitând existența a două stocuri paralele cu cantități diferite.

**Cod scurt:**
```csharp
// Ambele variabile vor indica spre exact aceeași zonă din memorie
var inventarGhișeu = StocManager.Instance;
var inventarDepozit = StocManager.Instance;

inventarGhișeu.AdaugaProdus(aspirina);
// inventarDepozit va vedea și el aspirina adăugată instant
```

**Explicație Cod:** Apelând proprietatea statică `Instance`, clasa verifică dacă inventarul a fost deja creat. Dacă da, îl dă pe cel existent. Astfel `new StocManager()` nu este apelat niciodată din exterior.

**Diagramă desenată:**
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

**Explicație diagramă:** Clasa `StocManager` deține o referință statică privată către ea însăși (cu semnul minus) și expune metoda publică statică (cu semnul plus) pentru a accesa această instanță, ascunzând complet constructorul.

---

## 2. Factory Method (Metoda Fabrică)

**Definiție:** Definește o interfață pentru crearea unui obiect, lăsând subclasele să decidă tipul exact ce va fi instanțiat.

**Ce problemă rezolvă?**
* **La general:** Decuplează logica de afaceri de crearea directă a claselor concrete (elimină `new ClasaConcreta()`).
* **În aplicația mea:** Crearea dinamică de produse farmaceutice în funcție de selecția din UI.
* **Exemplu clar:** Utilizatorul apasă "Adaugă" din meniul derulant. În loc să facem if-uri gigantice cu `new Medicament()` sau `new Echipament()`, trimitem cererea unei Fabrici care decide ea ce obiect naște.

**Cod scurt:**
```csharp
ProdusFactory fabrica = new MedicamentFactory();
// Fabrica ascunde logica de 'new Medicament()'. Returneaza tipul de baza 'Produs'
Produs nurofen = fabrica.CreazaProdus("Nurofen", 25.5m); 
```

**Explicație Cod:** Clientul lucrează doar cu interfața abstractă `ProdusFactory`. Când cere crearea, implementarea concretă a fabricii decide să returneze un obiect instanțiat gata de folosire.

**Diagramă desenată:**
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

**Explicație diagramă:** Fabrica abstractă delegă responsabilitatea creării către subclasa `MedicamentFactory`, singura care cunoaște și are voie să instanțieze produsul concret `Medicament`.

---

## 3. Abstract Factory (Fabrica Abstractă)

**Definiție:** Oferă o interfață pentru crearea unor familii de obiecte înrudite sau dependente fără a le specifica clasele concrete.

**Ce problemă rezolvă?**
* **La general:** Previne amestecarea obiectelor incompatibile din familii diferite (ex: stil modern vs. clasic).
* **În aplicația mea:** Crearea truselor cu produse 100% compatibile (Adulți vs. Copii).
* **Exemplu clar:** Când creăm o trusă pentru copii, fabrica ne va genera mereu un sirop de copii și un plasture colorat. Nu va scăpa niciodată din greșeală un medicament cu concentrație forte pentru adulți în trusa de copii.

**Cod scurt:**
```csharp
// Instantiem fabrica potrivita familiei de produse
ITrusaFactory fabricaCopii = new TrusaCopiiFactory();

// Ne asiguram ca elementele generate sunt din aceeasi familie (Copii)
var sirop = fabricaCopii.CreareMedicamentDurere();
var bandaj = fabricaCopii.CreareBandaj();
```

**Explicație Cod:** Folosind o singură fabrică (`TrusaCopiiFactory`), obținem garantat un pachet de obiecte care sunt proiectate să fie utilizate împreună, fără să verificăm tipul fiecăruia.

**Diagramă desenată:**
```text
          +-----------------------+
          |    ITrusaFactory      |
          |     <<interface>>     |
          +-----------------------+
          | + CreareMedicament()  |
          | + CreareBandaj()      |
          +----------+------------+
                     | (implementează)
                     V
          +-----------------------+
          |  TrusaCopiiFactory    |
          +-----------------------+
```

**Explicație diagramă:** Interfața dictează că orice fabrică de truse trebuie să scoată cel puțin 2 tipuri de produse complementare. Fabrica concretă livrează implementările asortate ale acelei familii.

---

## 4. Builder (Constructorul)

**Definiție:** Separă construcția unui obiect complex de reprezentarea sa, permițând crearea aceluiași tip de obiect prin procese diferite, pas cu pas.

**Ce problemă rezolvă?**
* **La general:** Evită un constructor gigantic cu 15 parametri opționali.
* **În aplicația mea:** Asamblarea truselor medicale personalizate.
* **Exemplu clar:** Nu forțăm crearea trusei dintr-o bucată. Builder-ul permite asamblarea pe pași: pune aspirina, pune plasturi, controlate de un `Director` care știe ordinea standard de asamblare.

**Cod scurt:**
```csharp
var builder = new TrusaBuilder();
var director = new TrusaDirector(builder);

// Directorul dicteaza ordinea pasilor in builder
TrusaMedicala trusaVacanta = director.ConstructTrusaVacanta();
```

**Explicație Cod:** Directorul cunoaște "rețeta" trusei de vacanță. El apelează pe rând funcțiile din `builder` (`AdaugaDezinfectant()`, `AdaugaPastile()`). La final, builder-ul returnează obiectul complex asamblat.

**Diagramă desenată:**
```text
   +----------------+             +-----------------+
   | TrusaDirector  | -- comandă->|  TrusaBuilder   |
   +----------------+             +-----------------+
   | + Construct()  |             | + AdaugaX()     |
   +----------------+             | + AdaugaY()     |
                                  | + GetTrusa()    |
                                  +-----------------+
```

**Explicație diagramă:** `TrusaDirector` orchestrează procesul. El deține referința spre un `Builder` căruia îi spune ce pași să execute, eliberând clientul de cunoașterea detaliilor de asamblare.

---

## 5. Prototype (Prototipul)

**Definiție:** Specifică tipurile de obiecte de creat folosind o instanță-prototip și creează obiecte noi prin clonarea (copierea) acesteia.

**Ce problemă rezolvă?**
* **La general:** Salvează timp și resurse când instanțierea de la zero e costisitoare.
* **În aplicația mea:** Duplicarea rapidă a medicamentelor direct din tabelul de gestiune.
* **Exemplu clar:** Dacă avem un produs configurat perfect cu TVA, adaos, descriere și cod producător, în loc să dăm `new` și să reculegem datele, apelăm metoda `Cloneaza()` pe rândul respectiv.

**Cod scurt:**
```csharp
Produs aspirinaOriginala = stoc.GetProdus("Aspirina");

// Cream un produs nou copiind 100% proprietatile celui vechi
Produs aspirinaClona = aspirinaOriginala.Cloneaza();
```

**Explicație Cod:** Metoda `Cloneaza()` execută sub capotă o copiere directă în memorie a obiectului existent, scutindu-ne de reconfigurarea manuală a atributelor.

**Diagramă desenată:**
```text
          +-----------------------+
          |       Produs          |
          |    <<abstract>>       |
          +-----------------------+
          | + Cloneaza()          |
          +----------+------------+
                     | (moștenește)
                     V
          +-----------------------+
          |     Medicament        |
          +-----------------------+
          | + Cloneaza()          |
          +-----------------------+
```

**Explicație diagramă:** Interfața superioară `Produs` expune contractul de clonare. Apelantului nu îi pasă ce tip concret de produs a clonat, deoarece primește înapoi un duplicat perfect compatibil.

---

## 6. Decorator (Decoratorul)

**Definiție:** Atașează dinamic responsabilități adiționale unui obiect, oferind o alternativă flexibilă la crearea de subclase.

**Ce problemă rezolvă?**
* **La general:** Previne "explozia de subclase" atunci când încerci să combini atribute.
* **În aplicația mea:** Adaugă ambalaje sau taxe speciale prețului medicamentelor.
* **Exemplu clar:** Avem un `Medicament`. Dacă clientul îl vrea pentru cadou, îl "îmbrăcăm" într-un `AmbalajCadouDecorator` care adaugă automat taxa de ambalare la preț, fără să modificăm codul medicamentului.

**Cod scurt:**
```csharp
Produs cadou = new Medicament("Sirop", 50m);

// Imbracam obiectul original in decorativ (Wrapper)
cadou = new AmbalajCadouDecorator(cadou); 

Console.WriteLine(cadou.GetPret()); // Afiseaza 50 + taxa ambalaj
```

**Explicație Cod:** Obiectul original este trimis în constructorul Decoratorului. Decoratorul interceptează metoda `GetPret()`, extrage prețul de 50 de la obiectul intern și adună manual taxa sa extra.

**Diagramă desenată:**
```text
     +-------------------+
     |      Produs       |
     |   <<abstract>>    |<-------------------------+
     +-------------------+                          |
     | + GetPret()       |                          |
     +---------+---------+                          | (îmbracă)
               | (moștenește)                       |
               V                                    |
     +-------------------+                          |
     | ProdusDecorator   | <>-----------------------+
     |   <<abstract>>    |
     +-------------------+
               | (moștenește)
               V
     +------------------------+
     | AmbalajCadouDecorator  |
     +------------------------+
```

**Explicație diagramă:** `ProdusDecorator` atât moștenește cât și conține un `Produs`. Asta îi permite să respecte interfața de bază și să poată fi înlănțuit la infinit ca o matrioșcă.

---

## 7. Adapter (Adaptorul / Wrapper)

**Definiție:** Convertește interfața unei clase în interfața așteptată de clienți, permițând obiectelor cu interfețe incompatibile să colaboreze.

**Ce problemă rezolvă?**
* **La general:** Permite folosirea codului vechi (legacy) alături de noile structuri moderne.
* **În aplicația mea:** Traduce rețetele vechi CNAS în obiecte interne ale farmaciei.
* **Exemplu clar:** Sistemul național trimite rețete într-un format XML arhaic. `AdaptorReteteCNAS` interceptează fișierul, îi desface etichetele și ne livrează proprietățile formatate fix cum le cerem noi.

**Cod scurt:**
```csharp
// Sistemul nostru modern se asteapta la o IRetetaNoua
IRetetaNoua adaptor = new AdaptorReteteCNAS();

// Apelam metoda noastra, dar sub capota se trage din sistemul vechi XML
string dateFarmacie = adaptor.PreluareDate(); 
```

**Explicație Cod:** Clientul strigă standardul modern `PreluareDate()`. Adaptorul, mascat ca un obiect modern, se întoarce în secret spre sistemul vechi și trage metoda urâtă `DescarcaXmlArhaic()`, servind datele traduse.

**Diagramă desenată:**
```text
   +-------------------+         +-------------------+
   |   IRetetaNoua     |         | SistemVechiCNAS   |
   |  <<interface>>    |         |                   |
   +-------------------+         +-------------------+
   | + PreluareDate()  |         | + DescarcaXml()   |
   +---------+---------+         +---------+---------+
             | (implementează)             ^
             V                             | (apelează)
   +-------------------+                   |
   | AdaptorReteteCNAS |-------------------+
   +-------------------+
```

**Explicație diagramă:** Adaptorul se integrează curat implementând interfața `IRetetaNoua`, dar deține o referință direcțională către `SistemVechiCNAS` pentru a apela logica veche izolată.

---

## 8. Bridge (Puntea)

**Definiție:** Decuplează o abstractizare de implementarea ei, permițând ambelor părți să varieze independent.

**Ce problemă rezolvă?**
* **La general:** Evită moștenirea masivă atunci când clasa se împarte logic pe 2 axe.
* **În aplicația mea:** Separarea formei farmaceutice de modul de administrare.
* **Exemplu clar:** Un medicament variază după formă (Sirop/Pastilă) și administrare (Oral/Injectabil). Puntea le lasă să fie combinate liber la rulare, fără să creăm clase inutile de tip `SiropOral`.

**Cod scurt:**
```csharp
// Construim o combinatie dinamic din cele 2 axe separate
FormaMedicament siropOral = new Sirop(new AdministrareOrala());

// Cand apelam siropul, el deleaga administrarea clasei din paranteze
siropOral.Aplica();
```

**Explicație Cod:** În loc să moștenim ambele caracteristici, trecem obiectul `AdministrareOrala` în interiorul formei `Sirop` prin compoziție. Executarea se face folosind "puntea" de legătură.

**Diagramă desenată:**
```text
    +-------------------+               +--------------------+
    | FormaMedicament   | <>----------> | IModAdministrare   |
    |   <<abstract>>    |  (puntea)     |    <<interface>>   |
    +-------------------+               +--------------------+
    | + Aplica()        |               | + Executa()        |
    +---------+---------+               +---------+----------+
              |                                   |
              V                                   V
    +-------------------+               +--------------------+
    |      Sirop        |               | AdministrareOrala  |
    +-------------------+               +--------------------+
```

**Explicație diagramă:** Puntea fizică e săgeata de agregare (cu romb) dintre Abstracție și Implementare. Ierarhiile din stânga și dreapta pot crește independent cu zeci de clase noi.

---

## 9. Composite (Compozit)

**Definiție:** Compune obiecte într-o structură arborescentă ("întreg-parte") permițând tratarea uniformă a elementelor simple și a grupurilor.

**Ce problemă rezolvă?**
* **La general:** Gestiunea structurilor tip arbore (ca fișierele și folderele) cu aceleași funcții.
* **În aplicația mea:** Gestiunea pachetelor promoționale mari ce conțin cutii și produse individuale.
* **Exemplu clar:** O `CutiePromotionala` conține 3 medicamente. Apelăm simplu metoda `.GetPret()` pe cutie, care buclează invizibil și adună prețul frunzelor din interior, returnând prețul total.

**Cod scurt:**
```csharp
var cutiePromo = new CutiePromotionala();
cutiePromo.Adauga(new Medicament("Nurofen", 20m));
cutiePromo.Adauga(new Medicament("Aspirina", 15m));

// Cutia e tratata ca un produs normal, isi aduna sub-elementele automat
Console.WriteLine(cutiePromo.GetPret()); // Afiseaza 35
```

**Explicație Cod:** Atât `Medicament` (frunză) cât și `CutiePromotionala` (nod) moștenesc din aceeași interfață care definește `GetPret()`. Cutia pur și simplu cheamă această metodă pe copiii săi și le adună valorile.

**Diagramă desenată:**
```text
     +-------------------+
     | ComponentaCatalog |<-------------------------+
     |   <<abstract>>    |                          |
     +-------------------+                          |
     | + GetPret()       |                          |
     +---------+---------+                          |
               |                                    | (conține listă)
       +-------+-------+                            |
       |               |                            |
       V               V                            |
 +------------+  +--------------------+             |
 |   Produs   |  | CutiePromotionala  | <>----------+
 |  (Frunză)  |  +--------------------+
 +------------+  | - elemente (List)  |
                 +--------------------+
```

**Explicație diagramă:** Cutia compusă deține o listă de elemente care sunt fix de tipul părintelui abstract. Astfel, cutia poate conține alte cutii recursive la nesfârșit.

---

## 10. Facade (Fațada)

**Definiție:** Oferă o interfață unificată, de nivel înalt, care face ca un subsistem complex să fie ușor de folosit.

**Ce problemă rezolvă?**
* **La general:** Simplifică utilizarea sistemelor uriașe mascând clasele grele din spate.
* **În aplicația mea:** Procesul de vânzare și facturare complet ascuns de ochii farmacistului.
* **Exemplu clar:** Farmacistul apasă doar `Panou.Vinde()`. Fațada interceptează și strigă tăcut către sistemul GestiuneStoc (să scadă numărul) și CasaMarcat (pentru încasare).

**Cod scurt:**
```csharp
var panouVanzare = new PanouVanzareFacade();

// Sub capota se apeleaza stocul, plata si chitanta
panouVanzare.VindeProdus(medicamentSelectat); 
```

**Explicație Cod:** Toate liniile dureroase cu inițializări și coordonări sunt internalizate în clasa Facade. Utilizatorul apelează o singură metodă curată care orchestrează tot haosul intern.

**Diagramă desenată:**
```text
         +-----------------------+
Client ->|  PanouVanzareFacade   |
         +-----------------------+
           |       |       |  (apelează ordonat subsistemele)
           V       V       V
      +------+ +-------+ +--------+
      | Casa | | Stoc  | | Fiscal |
      +------+ +-------+ +--------+
```

**Explicație diagramă:** Fațada (Facade) acționează ca un recepționer de hotel; tu îi ceri o cameră, iar el coordonează cameristele, facturarea și cheile (subsistemele) pentru a rezolva cererea.

---

## 11. Flyweight (Categoria Pană)

**Definiție:** Utilizează partajarea datelor (sharing) pentru a sprijini eficient un număr enorm de obiecte care au o parte semnificativă din stare comună.

**Ce problemă rezolvă?**
* **La general:** Prăbușirea memoriei RAM din cauza zecilor de mii de obiecte mari și identice.
* **În aplicația mea:** Stocarea a mii de cutii fizice pe raft în sistemul de evidență.
* **Exemplu clar:** Prospectul Aspirinei ocupă 3MB text. În loc să salvăm acei 3MB pentru *fiecare* din cele 10.000 de cutii, salvăm prospectul o singură dată (Flyweight), și-l pasăm la afișare alături de Lot.

**Cod scurt:**
```csharp
var fabrica = new FlyweightFactory();
var prospectAspirina = fabrica.GetProspect("Aspirina"); 

// Prospectul partajat primeste datele volatile per cutie
prospectAspirina.AfiseazaCutie(lot: "RO1234", expirare: "2025");
```

**Explicație Cod:** Obiectul `prospectAspirina` este extras dintr-un Cache. Nu e recreat niciodată. Funcția `AfiseazaCutie` acceptă ca parametri stările volatile unice per cutie.

**Diagramă desenată:**
```text
    +------------------+         +--------------------+
    | FlyweightFactory | ------> | ProspectPartajat   |
    +------------------+         +--------------------+
    | + GetProspect()  |         | + AfiseazaCutie()  |
    +------------------+         +--------------------+
```

**Explicație diagramă:** Fabrica primește cererea. Dacă obiectul greu există în dicționar, îl returnează instant. Altfel creează unul nou, garantând că se salvează RAM.

---

## 12. Proxy (Intermediarul)

**Definiție:** Furnizează un surogat (înlocuitor) pentru un alt obiect pentru a-i controla strict accesul.

**Ce problemă rezolvă?**
* **La general:** Interceptează cererile pentru securitate sau amână instanțierea claselor uriașe (Lazy Load).
* **În aplicația mea:** Blocarea accesului farmaciștilor fără nivel de permisiune la narcotice.
* **Exemplu clar:** Proxy-ul `AccesMorfina` deține aceeași interfață ca baza de date. Când ceri lista, el îți cere parola, și doar dacă ești autorizat deschide canalul de comunicare real.

**Cod scurt:**
```csharp
IListaMedicamente listaSecurizata = new AccesMorfinaProxy();

// Proxy intercepteaza apelul, cere parola si decide incotro o ia
listaSecurizata.GetLista("ParolaGresita"); // Arunca Exceptie
listaSecurizata.GetLista("Admin123"); // Returneaza datele reale
```

**Explicație Cod:** Implementând identic interfața listei, Proxy-ul interceptează execuția. Prin decizii de filtrare, poate nega accesul, protejând datele vitale.

**Diagramă desenată:**
```text
   +----------------------+
   | IListaMedicamente    |
   |    <<interface>>     |
   +----------+-----------+
              |
      +-------+-------+
      |               |
      V               V
+-------------+  +------------------+
| AccesProxy  |->| BazaDateReală    |
| (Verifică)  |  +------------------+
+-------------+  
```

**Explicație diagramă:** Clientul apelează interfața crezând că vorbește cu DB-ul, dar de fapt vorbește cu Proxy-ul, singurul responsabil cu validarea parolei și pasarea cererii către DB.

---

## 13. Chain of Responsibility (Lanțul de Aprobare)

**Definiție:** Permite transmiterea unei cereri de-a lungul unui lanț de handleri (manipulatori).

**Ce problemă rezolvă?**
* **La general:** Decuplează expeditorul unei cereri de receptorul ei fizic.
* **În aplicația mea:** Automatizează sistemul de aprobări de discount.
* **Exemplu clar:** Clientul cere o reducere mare. Farmacistul nu are gradul, așa că trimite cererea în sus la Manager. Managerul o trimite la Director. Farmacistul a chemat un singur obiect.

**Cod scurt:**
```csharp
// Legam lantul de decizie ierarhic
farmacist.SetNext(manager).SetNext(director);

// Farmacistul cedeaza cererea automat in sus daca e prea mare
farmacist.GestioneazaCererea(15); 
```

**Explicație Cod:** Farmacistul analizează cererea. Deoarece limita lui e mică, nu o refuză, ci execută direct `_nextHandler.GestioneazaCererea(15)`. Responsabilitatea sare de la un om la altul pe lanț.

**Diagramă desenată:**
```text
   +--------------------+
   |  IHandlerAprobare  | <---------------------+
   |   <<interface>>    |                       | (referință următorul)
   +--------------------+                       |
   | + SetNext()        |                       |
   | + Gestioneaza()    |                       |
   +---------+----------+                       |
             |                                  |
             V                                  |
   +--------------------+                       |
   |  FarmacistHandler  | o---------------------+
   +--------------------+
```

**Explicație diagramă:** Secretul e agregarea recursivă. Un handler reține mereu adresa următorului obiect omolog superior la care să arunce pasul mai departe.

---

## 14. Command (Comanda / Capsula)

**Definiție:** Încapsulează o cerere sub formă de obiect, permițând salvarea în istoric, parametrizarea și operații de Undo.

**Ce problemă rezolvă?**
* **La general:** Permite acțiunea de Ctrl+Z (Undo) sau stocarea tranzacțiilor.
* **În aplicația mea:** Evidența fiscală la Casa de Marcat cu funcția de anulare a vânzării.
* **Exemplu clar:** Nu tăiem stocul imediat. Creăm un obiect `ComandaVanzare`. Dacă clientul a fugit fără să plătească, apasăm `Undo()` din capsulă, inversând stocul.

**Cod scurt:**
```csharp
ICommand vanzare = new ComandaVanzare(stoc, cantitate);

// Capsula memoreaza tranzactia. O rulam acum
casaDeMarcat.Execute(vanzare);

// Daca ceva pica, casa (invoker-ul) apeleaza rollback
casaDeMarcat.UndoUltima();
```

**Explicație Cod:** `ComandaVanzare` deține informații salvate local. `Execute()` scade datele, iar `Undo()` execută matematic procesul invers pe aceleași variabile protejate.

**Diagramă desenată:**
```text
 +---------------+       +--------------+       +--------------+
 | CasaDeMarcat  |       |   ICommand   |       | DepozitStoc  |
 |  (Invoker)    | ----> | <<interface>>| ----> |  (Receiver)  |
 +---------------+       +--------------+       +--------------+
 | + Execute()   |       | + Execute()  |       | + Scade()    |
 | + Undo()      |       | + Undo()     |       | + Adauga()   |
 +---------------+       +-------+------+       +--------------+
                                 |
                                 V
                         +--------------+
                         | ComandaVanzare|
                         +--------------+
```

**Explicație diagramă:** Invoker-ul (Casa) trage oarbește pe o interfață. Doar clasa `ComandaVanzare` știe că la `Execute` trebuie să transmită semnalul către Stoc.

---

## 15. Iterator (Cursorul)

**Definiție:** Oferă o modalitate de a accesa secvențial elementele unei colecții fără a expune structura sa internă.

**Ce problemă rezolvă?**
* **La general:** Trecerea prin structuri complexe fără a te bloca de tipul de listă folosit (Array, Graph).
* **În aplicația mea:** Parcurgerea rafturilor farmaceutice la inventar.
* **Exemplu clar:** Vrei să numeri cutiile. Farmacistul nu trebuie să afle că dulapul a fost stocat intern sub formă de Dicționar. El dă `iterator.GetNext()` până se termină colecția.

**Cod scurt:**
```csharp
var iterator = dulap.CreateIterator();

// Parcurgem oarbește fara sa stim structura de date interna
while(iterator.HasMore()) {
    Console.WriteLine(iterator.GetNext().Nume);
}
```

**Explicație Cod:** Logica de buclă se află în Iteratorul delegat. Interfața utilizator rămâne total independentă de schimbările colecției din backend.

**Diagramă desenată:**
```text
 +--------------+        +---------------+
 | IIterator    | <----- | IteratorDulap |
 | <<interface>>|        +---------------+
 +--------------+        | + GetNext()   |
 | + HasMore()  |        +---------------+
 | + GetNext()  |                ^
 +--------------+                | (parcurge pas cu pas)
                                 V
                         +---------------+
                         | DulapAgregat  |
                         +---------------+
```

**Explicație diagramă:** Structura agregată livrează mecanismul propriu de iterare garantat compatibil 100% cu interiorul ei, respectând independența (Decuplarea).

---

## 16. Mediator (Turnul de Control)

**Definiție:** Definește un obiect care încapsulează modul în care un grup de obiecte interacționează, eliminând cuplajul direct.

**Ce problemă rezolvă?**
* **La general:** Previne "Spaghetti Code" unde toate clasele se cunosc și se strigă unele pe altele.
* **În aplicația mea:** Comunicarea curată între Vânzări și Depozit.
* **Exemplu clar:** Vânzările nu taie stoc în Depozit. Vânzările aruncă un mesaj către `CentralaFarmacie`. Centrala, ca turn de control, analizează și emite decizia către Depozit.

**Cod scurt:**
```csharp
centrala.SeteazaVanzari(departamentVanzari);
centrala.SeteazaDepozit(departamentDepozit);

// Vanzarea declanseaza automat o scadere in depozit prin hub-ul central
departamentVanzari.EfectueazaVanzare(); 
```

**Explicație Cod:** Sub capotă, departamentul strigă `_mediator.Notifica(this, "VANZARE")`. Singurul capabil de reacție la acest string este Mediatorul, care apelează în siguranță Depozitul.

**Diagramă desenată:**
```text
 +--------------+      +------------------+      +--------------+
 | Dep. Vanzari | <--> | CentralaFarmacie | <--> | Dep. Depozit |
 |  (Coleg A)   |      |   (Mediatorul)   |      |  (Coleg B)   |
 +--------------+      +------------------+      +--------------+
```

**Explicație diagramă:** Clasele Colege comunică doar cu interfața abstractă a Mediatorului. Astfel, modulele devin independente unele de altele.

---

## 17. Memento (Istoricul)

**Definiție:** Capturează și exteriorizează starea internă a unui obiect pentru a putea fi restaurat mai târziu, fără a rupe încapsularea.

**Ce problemă rezolvă?**
* **La general:** Crearea salvărilor (Save/Load) care nu sparg securitatea variabilelor private.
* **În aplicația mea:** Salvarea coșului de cumpărături pentru evitarea ștergerilor accidentale.
* **Exemplu clar:** Farmacistul pune produse în coș și face `Caretaker.Salveaza()`. Aplicația face o fotografie. Dacă golește coșul greșit, apasă Restore și poza renaște coșul exact la fel.

**Cod scurt:**
```csharp
// Facem o fotografie imuabila a cosului si o depunem in seif
var fotografieCos = cosVirtual.Salveaza();
istoricCaretaker.AdaugaSalvare(fotografieCos);

// In caz de eroare, reconstruim starea pe baza pozei
cosVirtual.Restaureaza(istoricCaretaker.GetUltimaSalvare());
```

**Explicație Cod:** `CosMemento` nu oferă acces public la variabile. E doar un seif. Instanța originară e singura care își poate reatribui valorile folosind `Restaureaza()`.

**Diagramă desenată:**
```text
 +---------------+       creează poză       +-----------------+
 | Originator    | -----------------------> |    Memento      |
 | (CosProduse)  |                          +-----------------+
 +---------------+                          | - stare_privata |
        ^                                   +-----------------+
        | restaurează poza                           ^
        |                                            | păstrează 
 +---------------+                                   | (în seif)
 | Caretaker     | ----------------------------------+
 | (IstoricCos)  |
 +---------------+
```

**Explicație diagramă:** Originatorul creează Mementoul. `Caretaker`-ul e doar un "manager de fișiere" de salvări orb, depozitându-le fără a le putea citi conținutul interior.

---

## 18. Observer (Observatorul / Abonatul)

**Definiție:** Definește o dependență unu-la-mai-mulți, astfel încât toți abonații sunt notificați automat la schimbări.

**Ce problemă rezolvă?**
* **La general:** Construirea sistemelor Publish-Subscribe (notificări tip push).
* **În aplicația mea:** Alarma de avertizare la scăderea stocului critic.
* **Exemplu clar:** Când Aspirina (`Publisher`) ajunge la stoc=0, ea își parcurge lista de abonați (`Farmaciști`) și le apelează funcția de notificare aruncându-le alerta pe monitor.

**Cod scurt:**
```csharp
produsAspirina.Subscribe(farmacistAbonat);

// Actiunea centrala declanseaza alarma catre toti abonatii inscrisi
produsAspirina.ModificaStoc(0); 
```

**Explicație Cod:** Obiectul deține intern o listă de abonați. La actualizare, folosește un ciclu `foreach` prin abonați, chemându-le funcția comună de update.

**Diagramă desenată:**
```text
 +-----------------+       notifică abonații    +-----------------+
 | ProdusPublisher | -------------------------> | IObserver       |
 |  (Subiectul)    |                            | <<interface>>   |
 +-----------------+                            +-----------------+
 | + Subscribe()   |                            | + Actualizeaza()|
 | + ModificaStoc()|                            +--------+--------+
 +-----------------+                                     |
                                                         V
                                                +-----------------+
                                                | FarmacistAbonat |
                                                +-----------------+
```

**Explicație diagramă:** Subiectul central comunică doar cu interfața de abonat. Orice componentă hardware sau software poate primi notificarea dacă implementează contractul necesar.

---

## 19. State (Starea)

**Definiție:** Permite unui obiect să își modifice comportamentul atunci când starea sa internă suferă o schimbare, simulând schimbarea clasei.

**Ce problemă rezolvă?**
* **La general:** Eliminarea uriașei cantități de instrucțiuni de tip `if-else` / `switch` bazate pe status.
* **În aplicația mea:** Traseul comenzii de aprovizionare.
* **Exemplu clar:** Butonul `Anulare` merge strună în faza `StareNoua`. Dar când comanda a ajuns la curier (în `StareLivrata`), comportamentul codului se mută automat pe interzicerea anulării.

**Cod scurt:**
```csharp
comanda.Proceseaza(); // Face tranzitia la faza urmatoare
comanda.Livreaza();   // Seteaza obiectul intern de stare pe StareLivrata

// Refuza actiunea aruncand exceptie din interiorul clasei noi de stare
comanda.Anuleaza();   
```

**Explicație Cod:** `ComandaAprovizionare` are un pointer la starea ei curentă. Când fazele trec, pointerul e schimbat cu noile clase experte pe etapa respectivă care preiau total frâiele.

**Diagramă desenată:**
```text
   +-------------------+               +-------------------+
   | ComandaContext    | <>----------> | StareComanda      |
   +-------------------+  (pointer)    |   <<abstract>>    |
   | - stare_curenta   |               +-------------------+
   | + Anuleaza()      |               | + Anuleaza()      |
   +-------------------+               +---------+---------+
                                                 |
                                         +-------+-------+
                                         |               |
                                         V               V
                                 +------------+  +-------------+
                                 | StareNoua  |  | StareLivrata|
                                 +------------+  +-------------+
```

**Explicație diagramă:** Contextul nu evaluează IF-uri. La o cerere externă de anulare, Comanda efectuează orb `_stareCurenta.Anuleaza()`, lăsând starea conectată la acel moment să rezolve decizia.

---

## 20. Strategy (Strategia)

**Definiție:** Definește o familie de algoritmi puternici și îi face complet interschimbabili direct la execuție (runtime).

**Ce problemă rezolvă?**
* **La general:** Capacitatea de a schimba o funcționalitate logică grea ca pe un simplu "cartuș".
* **În aplicația mea:** Formula de discount la casa de marcat.
* **Exemplu clar:** VIP-ului îi aplicăm pe loc algoritmul `-10%`. Clientului normal îi aplicăm algoritmul standard. Alegerea calculului se face inserând pur și simplu strategia dorită în obiectul final.

**Cod scurt:**
```csharp
// Incarcam un cartus de logica noua (fidelitate)
calculator.SetStrategie(new PretFidelitate());

// Casa calculeaza totalul folosind noul cartus (cu -10%)
decimal totalRedus = calculator.Total(100m); 
```

**Explicație Cod:** Funcția centrală `Total` delegă calculele efective clasei inserate la setare. Algoritmul `PretFidelitate` este chemat polimorfic și taie din preț complet ascuns.

**Diagramă desenată:**
```text
   +-------------------+               +-------------------+
   | CheckoutFarmacie  | <>----------> | ICalculPret       |
   +-------------------+   (aleasă)    |   <<interface>>   |
   | - _strategie      |               +-------------------+
   | + Total()         |               | + Calculeaza()    |
   +-------------------+               +---------+---------+
                                                 |
                                         +-------+-------+
                                         |               |
                                         V               V
                                 +------------+  +-------------+
                                 | PretNormal |  | PretFidelit |
                                 +------------+  +-------------+
```

**Explicație diagramă:** Clasa de bază (Contextul) integrează logica fără a cunoaște deloc dacă la momentul respectiv este folosită o reducere sau o triplare de preț. Totul se alege din frontend.

---

## 21. Template Method (Metoda Șablon)

**Definiție:** Definește scheletul rigid al unui algoritm într-o clasă superioară, lăsând subclasele să completeze anumiți pași.

**Ce problemă rezolvă?**
* **La general:** Prevenirea copy-paste-ului la clasele care au 90% din logica de execuție la fel, dar 10% unică.
* **În aplicația mea:** Generarea rapoartelor de vânzări cu formate unice de fișier.
* **Exemplu clar:** Clasa tată dictează regula fixă de execuție: 1. `Iei Date`. 2. `Le formatezi`. 3. `Printezi`. Subclasa PDF moștenește rețeta, dar îi este dat voie să scrie cod doar la pasul 2 de "Formatezi", făcând fișierul PDF.

**Cod scurt:**
```csharp
// Instantiem clasa de raport in format CSV
RaportTemplate raport = new RaportCSVVanzari();

// Apelam metoda fixa de schelet care forteaza ordinea
raport.GenereazaRaport(); 
```

**Explicație Cod:** `GenereazaRaport()` e metoda finală ce nu poate fi spartă. În spatele ei se apelează metoda abstractă `Formateaza()`, care prin natură face ca fluxul să ajungă direct în codul personalizat din subclasă.

**Diagramă desenată:**
```text
          +-----------------------+
          |   RaportTemplate      |
          |    <<abstract>>       |
          +-----------------------+
          | + GenereazaRaport()   | <-- scheletul fix (ordinea)
          | # Formateaza()*       | <-- delegare către copil
          +----------+------------+
                     |
             +-------+-------+
             |               |
             V               V
     +--------------+  +--------------+
     |  RaportCSV   |  |  RaportPDF   |
     +--------------+  +--------------+
```

**Explicație diagramă:** Părintele deține inițiativa acțiunilor prin funcția publică. Prin Inversiunea de Control (Hollywood Principle: *Nu ne suna tu, te sunăm noi*), părintele declanșează apelurile către copiii săi.

---

## 22. Visitor (Vizitatorul)

**Definiție:** Permite aplicarea de operațiuni noi și logici specifice fără a altera deloc clasele originale peste care activează.

**Ce problemă rezolvă?**
* **La general:** Extragerea logicilor externe străine (Exporturi, Impozite, Audituri) din modelele curate de date.
* **În aplicația mea:** Extragerea de XML din Facturile de evidență națională E-Factura.
* **Exemplu clar:** Nu scriem cod mizerabil de export în clasa de domeniu `Factura`. Creăm `ExportXmlVisitor`. Vizitatorul se duce la factura curată, ea îi deblochează ușa cu metoda `Accept()`, iar Vizitatorul adună cifrele asamblând el însuși structura XML de la distanță.

**Cod scurt:**
```csharp
var inspectorXML = new ExportXmlVisitor();

// Factura se lasa investigata chemand preluarea prin Double Dispatch
facturaExistenta.Accept(inspectorXML); 
```

**Explicație Cod:** Intern, funcția `Accept()` a facturii rulează codul `visitor.ViziteazaFactura(this)`. Trimite propria adresă memorie către Vizitator, dându-i acces total la date fără să-i preia din funcții.

**Diagramă desenată:**
```text
   +-------------------+          +-------------------+
   | IAcceptaVisitor   |          | IVisitor          |
   |   <<interface>>   |          |   <<interface>>   |
   +-------------------+          +-------------------+
   | + Accept(IVisitor)|          | + ViziteazaFactura|
   +---------+---------+          +---------+---------+
             |                              |
             V                              V
   +-------------------+          +-------------------+
   |   Factura         | ------>  | ExportXmlVisitor  |
   +-------------------+          +-------------------+
```

**Explicație diagramă:** Datele pure expun exclusiv funcția minoră publică `Accept`. Design-ul permite aducerea de zeci de alte noi inspectoare analitice în viitor fără ca Factura să fie măcar o dată rescrisă, respectând Principiul OCP.

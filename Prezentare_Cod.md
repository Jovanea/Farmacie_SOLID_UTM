# Prezentare Laborator 1 - OOP și SOLID

## 1. Structura Aplicației (OOP)

Aplicația gestionează produse farmaceutice folosind o structură orientată pe obiecte:

*   **Încapsulare**: Proprietățile clasei `Produs` (`Nume`, `Pret`) au setteri protejați (`protected set`) pentru a preveni modificarea lor directă din afara ierarhiei de clase.
*   **Moștenire**: Clasele `Medicament` și `EchipamentMedical` moștenesc clasa de bază `Produs`, reutilizând codul comun.
*   **Polimorfism**: Metoda `ObtineDetalii()` este abstractă în `Produs` și suprascrisă (`override`) în clasele derivate pentru a oferi comportament specific.

## 2. Aplicarea Principiilor SOLID

### SRP - Single Responsibility Principle
*   **Concept**: O clasă trebuie să aibă un singur motiv de a se modifica.
*   **Implementare**: Am separat logica de stocare de logica de afișare. `ConsolaStorage` se ocupă doar de salvarea datelor, în timp ce `Form1` se ocupă doar de interacțiunea cu utilizatorul.

### OCP - Open/Closed Principle
*   **Concept**: Entitățile software trebuie să fie deschise pentru extindere, dar închise pentru modificare.
*   **Implementare**: Putem adăuga noi tipuri de produse (ex: `SuplimentAlimentar`) prin crearea unei noi clase care moștenește `Produs`, fără a modifica clasa `Produs` existentă.

### LSP - Liskov Substitution Principle
*   **Concept**: Obiectele unei superclase trebuie să poată fi înlocuite cu obiecte ale subclaselor fără a afecta corectitudinea programului.
*   **Implementare**: Orice obiect de tip `Medicament` sau `EchipamentMedical` poate fi tratat ca un `Produs` generic. Interfața `IStocare` acceptă orice `Produs`, indiferent de tipul specific.

### ISP - Interface Segregation Principle
*   **Concept**: Nu forța clienții să depindă de interfețe pe care nu le folosesc.
*   **Implementare**: Interfața `IStocare` conține doar metoda strict necesară (`Salveaza`). Dacă am fi avut nevoie de ștergere doar pentru admini, am fi creat o altă interfață `IStergere`, nu am fi aglomerat `IStocare`.

### DIP - Dependency Inversion Principle
*   **Concept**: Modulele de nivel înalt nu trebuie să depindă de module de nivel jos. Ambele trebuie să depindă de abstractizări.
*   **Implementare**: Clasa `Form1` nu depinde direct de `ConsolaStorage` (o clasă concretă), ci de interfața `IStocare`. Dependența este "injectată" prin constructorul `Form1(IStocare stocare)` în `Program.cs`. Acest lucru ne permite să schimbăm ușor `ConsolaStorage` cu `DatabaseStorage` în viitor, fără a modifica `Form1`.

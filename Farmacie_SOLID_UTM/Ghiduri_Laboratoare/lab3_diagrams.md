# Laborator 3 - Pattern-uri Creaționale (Diagrame UML)

Aceste diagrame ilustrează structura pattern-urilor implementate: Singleton, Builder și Prototype.

## 1. Singleton Pattern (`StocManager`)

Clasa `StocManager` asigură un singur punct de acces la inventarul de produse din întreaga aplicație.

```mermaid
classDiagram
    class StocManager {
        <<singleton>>
        - static StocManager _instance
        - List~Produs~ _produse
        - StocManager()
        + static StocManager Instance
        + void AdaugaProdus(Produs p)
        + List~Produs~ GetProduse()
    }

    class Form1 {
        + ...
    }

    Form1 ..> StocManager : folosește (Instance)
```

## 2. Builder Pattern (`TrusaBuilder`)

Separă construcția obiectului complex [TrusaMedicala](file:///c:/Users/John/Desktop/TMPPP/Farmacie_SOLID_UTM/Farmacie_SOLID_UTM/Models/TrusaMedicala.cs#9-40) de reprezentarea sa finală, permițând crearea pas-cu-pas.

```mermaid
classDiagram
    %% Produsul Complex
    class TrusaMedicala {
        - List~Produs~ _continut
        + string Nume
        + void AdaugaProdus(Produs p)
        + string ListeazaContinut()
    }

    %% Constructorul (Builder)
    class TrusaBuilder {
        - TrusaMedicala _trusa
        + TrusaBuilder StartTrusa(string nume)
        + TrusaBuilder AdaugaMedicament(...)
        + TrusaBuilder AdaugaBandaj(...)
        + TrusaMedicala Build()
    }

    %% Directorul (Cerință Strictă)
    class TrusaDirector {
        - TrusaBuilder _builder
        + TrusaDirector(builder)
        + ConstructTrusaVacanta() TrusaMedicala
        + ConstructTrusaAuto() TrusaMedicala
    }

    %% Relații
    TrusaBuilder ..> TrusaMedicala : construiește (builds)
    TrusaDirector o-- TrusaBuilder : are (aggregation)
    
    %% Clientul folosește Directorul
    class Form1 {
        + BtnBuilder_Click()
    }
    Form1 ..> TrusaDirector : folosește
```

## 3. Prototype Pattern (`IPrototip`)

Permite clonarea obiectelor [Produs](file:///c:/Users/John/Desktop/TMPPP/Farmacie_SOLID_UTM/Farmacie_SOLID_UTM/Models/TrusaMedicala.cs#14-18) (Medicament, EchipamentMedical) pentru a crea duplicate rapid.

```mermaid
classDiagram
    %% Interfața pentru Clonare
    class IPrototip {
        <<interface>>
        + Cloneaza() Produs
    }

    %% Clasa de Bază Abstractă implementează Interfața
    class Produs {
        <<abstract>>
        + ...
        + abstract Cloneaza() Produs
    }

    %% Implementare Concretă 1
    class Medicament {
        + ...
        + Cloneaza() Produs
    }

    %% Implementare Concretă 2
    class EchipamentMedical {
        + ...
        + Cloneaza() Produs
    }

    %% Relații
    IPrototip <|.. Produs
    Produs <|-- Medicament
    Produs <|-- EchipamentMedical

    %% Clientul folosește clonarea
    class Form1 {
        + BtnClone_Click()
    }
    Form1 ..> Produs : cloneaza()
```

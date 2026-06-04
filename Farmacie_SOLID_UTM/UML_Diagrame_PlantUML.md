# Diagrame UML PlantUML — FarmSys (toate 22 patternuri)
# Copiaza fiecare bloc pe plantuml.com si apasa Submit

---

## 1. Singleton — StocManager

```plantuml
@startuml Singleton
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #2E86C1
  HeaderFontColor white
  HeaderFontStyle bold
}

class StocManager {
  - {static} _instance : StocManager
  - {static} _lock : object
  - _produse : List<Produs>
  --
  - StocManager()
  + {static} Instance : StocManager
  + AdaugaProdus(p : Produs) : void
  + GetProduse() : List<Produs>
  + GetTotalProduse() : int
  + ScadeStoc(nume : string, cant : int) : bool
}

note right of StocManager
  Double-Check Locking:
  if (_instance == null)
    lock (_lock)
      if (_instance == null)
        _instance = new StocManager()
end note
@enduml
```

---

## 2. Factory Method — ProdusFactory

```plantuml
@startuml FactoryMethod
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

abstract class ProdusFactory {
  + {abstract} CreazaProdus(n:string, p:decimal, e:string) : Produs
}

class MedicamentFactory {
  + CreazaProdus(n, p, e) : Medicament
}

class EchipamentFactory {
  + CreazaProdus(n, p, e) : EchipamentMedical
}

abstract class Produs {
  + Nume : string
  + Pret : decimal
  + {abstract} ObtineDetalii() : string
}

class Medicament {
  + Producator : string
  + ObtineDetalii() : string
}

class EchipamentMedical {
  + Tip : string
  + ObtineDetalii() : string
}

ProdusFactory <|-- MedicamentFactory
ProdusFactory <|-- EchipamentFactory
MedicamentFactory ..> Medicament : <<creates>>
EchipamentFactory ..> EchipamentMedical : <<creates>>
Produs <|-- Medicament
Produs <|-- EchipamentMedical
@enduml
```

---

## 3. Abstract Factory — ITrusaFactory

```plantuml
@startuml AbstractFactory
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface ITrusaFactory {
  + CreareMedicamentDurere() : Produs
  + CreareBandaj() : Produs
}

class TrusaAdultiFactory {
  + CreareMedicamentDurere() : Ibuprofen400mg
  + CreareBandaj() : BandajElastic
}

class TrusaCopiiFactory {
  + CreareMedicamentDurere() : SiropDurere
  + CreareBandaj() : PlastureColorat
}

abstract class Produs {
  + Nume : string
  + Pret : decimal
}

class Ibuprofen400mg
class BandajElastic
class SiropDurere
class PlastureColorat

ITrusaFactory <|.. TrusaAdultiFactory
ITrusaFactory <|.. TrusaCopiiFactory
TrusaAdultiFactory ..> Ibuprofen400mg : <<creates>>
TrusaAdultiFactory ..> BandajElastic : <<creates>>
TrusaCopiiFactory ..> SiropDurere : <<creates>>
TrusaCopiiFactory ..> PlastureColorat : <<creates>>
Produs <|-- Ibuprofen400mg
Produs <|-- BandajElastic
Produs <|-- SiropDurere
Produs <|-- PlastureColorat

note bottom of TrusaAdultiFactory
  Familia Adulti: produse
  garantat compatibile
end note
@enduml
```

---

## 4. Builder + Director — TrusaBuilder

```plantuml
@startuml Builder
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

class TrusaDirector {
  - _builder : TrusaBuilder
  --
  + TrusaDirector(b : TrusaBuilder)
  + ConstructTrusaVacanta() : TrusaMedicala
  + ConstructTrusaAuto() : TrusaMedicala
}

class TrusaBuilder {
  - _trusa : TrusaMedicala
  --
  + AdaugaMedicament(n:string, p:decimal) : void
  + AdaugaEchipament(n:string, p:decimal) : void
  + AdaugaBandaj(n:string, p:decimal) : void
  + GetTrusa() : TrusaMedicala
}

class TrusaMedicala {
  - _componente : List<string>
  - _pretTotal : decimal
  --
  + AdaugaComponenta(n:string, p:decimal) : void
  + CalculeazaPretTotal() : decimal
  + ListeazaContinut() : string
}

TrusaDirector --> TrusaBuilder : uses
TrusaBuilder --> TrusaMedicala : builds
@enduml
```

---

## 5. Prototype — Produs.Cloneaza()

```plantuml
@startuml Prototype
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IPrototip {
  + Cloneaza() : Produs
}

abstract class Produs {
  + Nume : string
  + Pret : decimal
  + Cantitate : int
  --
  + {abstract} Cloneaza() : Produs
  + {abstract} ObtineDetalii() : string
}

class Medicament {
  - _producator : string
  --
  + Cloneaza() : Produs
  + ObtineDetalii() : string
}

class EchipamentMedical {
  - _tip : string
  --
  + Cloneaza() : Produs
  + ObtineDetalii() : string
}

class PachetProduse {
  - _produse : List<Produs>
  --
  + Cloneaza() : Produs
  + ObtineDetalii() : string
}

IPrototip <|.. Produs
Produs <|-- Medicament
Produs <|-- EchipamentMedical
Produs <|-- PachetProduse

note right of Medicament
  return new Medicament(
    Nume + " (copie)",
    Pret, Producator)
end note
@enduml
```

---

## 6. Decorator — AmbalajCadouDecorator

```plantuml
@startuml Decorator
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

abstract class Produs {
  + Nume : string
  + {virtual} Pret : decimal
  --
  + {abstract} ObtineDetalii() : string
  + {abstract} Cloneaza() : Produs
}

abstract class ProdusDecorator {
  # _produsDecorat : Produs
  --
  + ProdusDecorator(p : Produs)
  + {override} ObtineDetalii() : string
}

class AmbalajCadouDecorator {
  - COST_AMBALAJ : decimal = 5m
  --
  + {override} Pret : decimal
  + {override} ObtineDetalii() : string
}

class Medicament {
  + Pret : decimal
  + ObtineDetalii() : string
}

Produs <|-- ProdusDecorator
Produs <|-- Medicament
ProdusDecorator <|-- AmbalajCadouDecorator
ProdusDecorator o--> Produs : wraps

note right of AmbalajCadouDecorator
  Pret = _produsDecorat.Pret + 5m
end note
@enduml
```

---

## 7. Adapter — ProdusAdapter

```plantuml
@startuml Adapter
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

abstract class Produs {
  + Nume : string
  + Pret : decimal
  --
  + {abstract} ObtineDetalii() : string
  + {abstract} Cloneaza() : Produs
}

class FurnizorExternProdus {
  + GetProductName() : string
  + GetPrice() : decimal
  + GetCategory() : string
  + GetExpiryDate() : DateTime
}

class ProdusAdapter {
  - _furnizor : FurnizorExternProdus
  --
  + ProdusAdapter(f : FurnizorExternProdus)
  + {override} ObtineDetalii() : string
  + {override} Cloneaza() : Produs
}

Produs <|-- ProdusAdapter
ProdusAdapter --> FurnizorExternProdus : adapts

note bottom of ProdusAdapter
  Constructor:
  base(f.GetProductName(), f.GetPrice())
  Traducere interfata incompatibila
end note
@enduml
```

---

## 8. Bridge — Notificator + IPlatformaTrimitere

```plantuml
@startuml Bridge
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IPlatformaTrimitere {
  + Trimite(mesaj : string) : void
}

class TrimitereSms {
  + Trimite(mesaj : string) : void
}

class TrimitereEmail {
  + Trimite(mesaj : string) : void
}

class Notificator {
  # _platforma : IPlatformaTrimitere
  --
  + Notificator(p : IPlatformaTrimitere)
  + {virtual} ExpediazaAlerta(mesaj : string) : void
}

class NotificatorUrgent {
  + {override} ExpediazaAlerta(mesaj : string) : void
}

IPlatformaTrimitere <|.. TrimitereSms
IPlatformaTrimitere <|.. TrimitereEmail
Notificator <|-- NotificatorUrgent
Notificator o--> IPlatformaTrimitere : has

note bottom of Notificator
  Abstractie si implementare
  variaza independent
end note
@enduml
```

---

## 9. Composite — PachetProduse

```plantuml
@startuml Composite
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

abstract class Produs {
  + Nume : string
  + {virtual} Pret : decimal
  + Cantitate : int
  --
  + {abstract} ObtineDetalii() : string
  + {abstract} Cloneaza() : Produs
}

class Medicament {
  + {override} Pret : decimal
  + ObtineDetalii() : string
}

class EchipamentMedical {
  + {override} Pret : decimal
  + ObtineDetalii() : string
}

class PachetProduse {
  - _produse : List<Produs>
  --
  + {override} Pret : decimal <<sum>>
  + AdaugaInPachet(p : Produs) : void
  + ScoateDinPachet(p : Produs) : void
  + ObtineDetalii() : string
  + Cloneaza() : Produs <<deep copy>>
}

Produs <|-- Medicament
Produs <|-- EchipamentMedical
Produs <|-- PachetProduse
PachetProduse "1" o--> "*" Produs : contains
@enduml
```

---

## 10. Facade — FarmacieFacade

```plantuml
@startuml Facade
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

class FarmacieFacade {
  - _stoc : SistemStoc
  - _plata : SistemPlata
  - _facturare : SistemFacturare
  --
  + EfectueazaVanzare(produs:string, suma:decimal) : void
}

class SistemStoc {
  + ScadeStoc(produs : string) : void
  + VerificaDisponibilitate(p : string) : bool
}

class SistemPlata {
  + ProceseazaPlata(suma : decimal) : void
  + EmiteChitanta() : void
}

class SistemFacturare {
  + EmiteFactura(produs:string, suma:decimal) : void
  + TrimiteFactura() : void
}

FarmacieFacade --> SistemStoc : uses
FarmacieFacade --> SistemPlata : uses
FarmacieFacade --> SistemFacturare : uses

note top of FarmacieFacade
  Client apeleaza doar:
  EfectueazaVanzare()
  fara a cunoaste subsistemele
end note
@enduml
```

---

## 11. Flyweight — CategorieFactory

```plantuml
@startuml Flyweight
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

class CategorieFactory {
  - _cache : Dictionary<string, CategorieFlyweight>
  --
  + GetCategorie(tip:string, desc:string) : CategorieFlyweight
  + NumarCategoriiCreate : int
}

class CategorieFlyweight {
  + Tip : string <<intrinsic>>
  + Descriere : string <<intrinsic>>
  --
  + AfiseazaDetalii(produs : string) : void
}

CategorieFactory --> CategorieFlyweight : creates / caches

note right of CategorieFactory
  5 cereri, 4 obiecte create:
  "Analgezice" cerut de 2 ori
  -> returneaza aceeasi instanta
end note
@enduml
```

---

## 12. Proxy — ProxyBazaDate

```plantuml
@startuml Proxy
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IAccesBazaDate {
  + StergeProdus(nume : string) : void
  + ModificaPret(n:string, p:decimal) : void
}

class RealBazaDate {
  + StergeProdus(nume : string) : void
  + ModificaPret(n:string, p:decimal) : void
}

class ProxyBazaDate {
  - _rol : string
  - _real : RealBazaDate
  --
  + ProxyBazaDate(rol : string)
  + StergeProdus(nume : string) : void
  + ModificaPret(n:string, p:decimal) : void
}

IAccesBazaDate <|.. RealBazaDate
IAccesBazaDate <|.. ProxyBazaDate
ProxyBazaDate --> RealBazaDate : delegates if Manager

note right of ProxyBazaDate
  if (_rol == "Manager")
    -> permite accesul
  else
    -> REFUZA, RealBazaDate
       nu e instantiata
end note
@enduml
```

---

## 13. Chain of Responsibility — AprobareDiscount

```plantuml
@startuml ChainOfResponsibility
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IAprobareHandler {
  + GestioneazaCererea(procent : decimal) : void
  + SetNext(h : IAprobareHandler) : IAprobareHandler
}

abstract class BaseHandler {
  # _urmator : IAprobareHandler
  --
  + SetNext(h : IAprobareHandler) : IAprobareHandler
}

class FarmacistHandler {
  + GestioneazaCererea(procent : decimal) : void
}

class ManagerHandler {
  + GestioneazaCererea(procent : decimal) : void
}

class DirectorHandler {
  + GestioneazaCererea(procent : decimal) : void
}

IAprobareHandler <|.. BaseHandler
BaseHandler <|-- FarmacistHandler
BaseHandler <|-- ManagerHandler
BaseHandler <|-- DirectorHandler
FarmacistHandler --> ManagerHandler : next (if > 5%)
ManagerHandler --> DirectorHandler : next (if > 15%)

note bottom
  farmacist.SetNext(manager).SetNext(director)
  Cerere 5%  -> Farmacist aproba
  Cerere 15% -> Manager aproba
  Cerere 20% -> Director aproba
end note
@enduml
```

---

## 14. Command — ComandaVanzare

```plantuml
@startuml Command
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface ICommand {
  + Execute() : void
  + Undo() : void
}

class ComandaVanzare {
  - _sistem : SistemGestiune
  - _produs : string
  - _cantitate : int
  --
  + Execute() : void
  + Undo() : void
}

class CasaDeMarcat {
  - _command : ICommand
  --
  + SetCommand(c : ICommand) : void
  + ExecuteCommand() : void
}

class SistemGestiune {
  + VindeProdus(p:string, c:int) : void
  + ReturneazaProdus(p:string, c:int) : void
}

ICommand <|.. ComandaVanzare
CasaDeMarcat --> ICommand : invokes
ComandaVanzare --> SistemGestiune : calls
@enduml
```

---

## 15. Iterator — DulapMedicamente

```plantuml
@startuml Iterator
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IIterator {
  + HasMore() : bool
  + GetNext() : string
}

interface IIterableCollection {
  + CreateIterator() : IIterator
}

class DulapMedicamente {
  - _medicamente : List<string>
  --
  + Adauga(med : string) : void
  + CreateIterator() : IIterator
}

class IteratorDulap {
  - _colectie : List<string>
  - _index : int
  --
  + HasMore() : bool
  + GetNext() : string
}

IIterableCollection <|.. DulapMedicamente
IIterator <|.. IteratorDulap
DulapMedicamente ..> IteratorDulap : <<creates>>
@enduml
```

---

## 16. Mediator — CentralaFarmacie

```plantuml
@startuml Mediator
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IMediator {
  + Notifica(sender:object, eveniment:string) : void
}

class CentralaFarmacie {
  - _vanzari : DepartamentVanzari
  - _depozit : DepartamentDepozit
  --
  + SeteazaVanzari(d : DepartamentVanzari) : void
  + SeteazaDepozit(d : DepartamentDepozit) : void
  + Notifica(sender:object, eveniment:string) : void
}

class DepartamentVanzari {
  - _mediator : IMediator
  --
  + EfectueazaVanzare() : void
}

class DepartamentDepozit {
  - _mediator : IMediator
  --
  + ActualizeazaStoc() : void
  + CereReaprovizionare() : void
}

IMediator <|.. CentralaFarmacie
CentralaFarmacie --> DepartamentVanzari : notifies
CentralaFarmacie --> DepartamentDepozit : notifies
DepartamentVanzari --> IMediator : uses
DepartamentDepozit --> IMediator : uses
@enduml
```

---

## 17. Memento — IstoricCosCaretaker

```plantuml
@startuml Memento
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

class CosOriginator {
  - _produse : List<string>
  --
  + AdaugaProdus(p : string) : void
  + AfiseazaContinut() : string
  + Salveaza() : CosMemento
  + Restaureaza(m : CosMemento) : void
}

class CosMemento {
  - _produse : List<string>
  --
  + CosMemento(produse : List<string>)
  + GetProduse() : List<string>
}

class IstoricCosCaretaker {
  - _cos : CosOriginator
  - _istoric : Stack<CosMemento>
  --
  + IstoricCosCaretaker(cos : CosOriginator)
  + SalveazaStarea() : void
  + Undo() : void
}

CosOriginator ..> CosMemento : <<creates>>
IstoricCosCaretaker --> CosOriginator : manages
IstoricCosCaretaker o--> CosMemento : stores stack
@enduml
```

---

## 18. Observer — ProdusPublisher

```plantuml
@startuml Observer
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface ISubscriber {
  + Update(publisher : ProdusPublisher) : void
}

class ProdusPublisher {
  - _subscribers : List<ISubscriber>
  - _stocPrincipal : int
  + NumeProdus : string
  --
  + Subscribe(s : ISubscriber) : void
  + Unsubscribe(s : ISubscriber) : void
  + NotifySubscribers() : void
  + ModificaStoc(cantitate : int) : void
  + GetStoc() : int
}

class SistemAprovizionare {
  + Update(p : ProdusPublisher) : void
}

class FarmacistAbonat {
  - _nume : string
  --
  + FarmacistAbonat(nume : string)
  + Update(p : ProdusPublisher) : void
}

ISubscriber <|.. SistemAprovizionare
ISubscriber <|.. FarmacistAbonat
ProdusPublisher o--> "*" ISubscriber : notifies

note right of ProdusPublisher
  if (_stocPrincipal < 10)
    NotifySubscribers()
end note
@enduml
```

---

## 19. State — ComandaAprovizionare

```plantuml
@startuml State
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IStareComanda {
  + Proceseaza(c : ComandaAprovizionare) : void
  + Livreaza(c : ComandaAprovizionare) : void
  + Anuleaza(c : ComandaAprovizionare) : void
}

class ComandaAprovizionare {
  - _stare : IStareComanda
  --
  + ComandaAprovizionare(s : IStareComanda)
  + Proceseaza() : void
  + Livreaza() : void
  + Anuleaza() : void
  + SetStare(s : IStareComanda) : void
}

class StareNoua {
  + Proceseaza(c) : void
  + Livreaza(c) : void
  + Anuleaza(c) : void
}

class StareInProcesare {
  + Proceseaza(c) : void
  + Livreaza(c) : void
  + Anuleaza(c) : void
}

class StareLivrata {
  + Proceseaza(c) : void
  + Livreaza(c) : void
  + Anuleaza(c) : void
}

IStareComanda <|.. StareNoua
IStareComanda <|.. StareInProcesare
IStareComanda <|.. StareLivrata
ComandaAprovizionare --> IStareComanda : has state

StareNoua --> StareInProcesare : Proceseaza()
StareInProcesare --> StareLivrata : Livreaza()
@enduml
```

---

## 20. Strategy — CalculatorPretFinal

```plantuml
@startuml Strategy
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IStrategieDiscount {
  + AplicaDiscount(total : decimal) : decimal
}

class CalculatorPretFinal {
  - _strategie : IStrategieDiscount
  --
  + SetStrategie(s : IStrategieDiscount) : void
  + CalculeazaPretul(total : decimal) : decimal
}

class FaraDiscount {
  + AplicaDiscount(total : decimal) : decimal
}

class DiscountFidelitate {
  + AplicaDiscount(total : decimal) : decimal
}

class DiscountPensionar {
  + AplicaDiscount(total : decimal) : decimal
}

IStrategieDiscount <|.. FaraDiscount
IStrategieDiscount <|.. DiscountFidelitate
IStrategieDiscount <|.. DiscountPensionar
CalculatorPretFinal --> IStrategieDiscount : uses

note right of FaraDiscount : return total
note right of DiscountFidelitate : return total * 0.90m
note right of DiscountPensionar : return total * 0.80m
@enduml
```

---

## 21. Template Method — RaportTemplate

```plantuml
@startuml TemplateMethod
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

abstract class RaportTemplate {
  # _continut : string
  --
  + GenereazaRaport() : string <<template>>
  # CulegeDate() : void <<common>>
  # {abstract} FormateazaRaport() : void
  # {abstract} SalveazaFisier() : string
  # PrinteazaRaport(cale : string) : void <<common>>
}

class RaportZilnicVanzari {
  # FormateazaRaport() : void
  # SalveazaFisier() : string
}

class RaportStocCritic {
  # FormateazaRaport() : void
  # SalveazaFisier() : string
}

RaportTemplate <|-- RaportZilnicVanzari
RaportTemplate <|-- RaportStocCritic

note right of RaportTemplate
  GenereazaRaport() {
    CulegeDate()       // fix
    FormateazaRaport() // abstract
    SalveazaFisier()   // abstract
    PrinteazaRaport()  // fix
  }
end note

note bottom of RaportZilnicVanzari : Formateaza CSV\nSalveaza .csv pe Desktop
note bottom of RaportStocCritic : Formateaza TXT\nSalveaza .txt pe Desktop
@enduml
```

---

## 22. Visitor — ExportXmlVisitor

```plantuml
@startuml Visitor
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

interface IVisitorExport {
  + Visit(r : RetetaCompensata) : void
  + Visit(f : FacturaFirma) : void
}

interface IDocumentFarmacie {
  + Accept(v : IVisitorExport) : void
}

class ExportXmlVisitor {
  - _xml : StringBuilder
  --
  + Visit(r : RetetaCompensata) : void
  + Visit(f : FacturaFirma) : void
  + Salveaza() : (cale, xml)
}

class RetetaCompensata {
  + NumePacient : string
  + Diagnostic : string
  --
  + Accept(v : IVisitorExport) : void
}

class FacturaFirma {
  + NumeFirma : string
  + TotalDePlata : decimal
  --
  + Accept(v : IVisitorExport) : void
}

IVisitorExport <|.. ExportXmlVisitor
IDocumentFarmacie <|.. RetetaCompensata
IDocumentFarmacie <|.. FacturaFirma
ExportXmlVisitor ..> RetetaCompensata : visits
ExportXmlVisitor ..> FacturaFirma : visits

note bottom of RetetaCompensata
  Accept(v) {
    v.Visit(this) // Double Dispatch
  }
end note
@enduml
```

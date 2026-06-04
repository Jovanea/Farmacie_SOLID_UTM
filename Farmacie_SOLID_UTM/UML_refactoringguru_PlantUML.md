# Diagrame UML — structura refactoring.guru
# Copiaza fiecare bloc pe plantuml.com -> Submit

---

## 1. Singleton

```plantuml
@startuml Singleton
skinparam classAttributeIconSize 0
skinparam defaultFontName Arial
skinparam defaultFontSize 13
skinparam class {
  BackgroundColor #EBF5FB
  BorderColor #2E86C1
  HeaderBackgroundColor #1A5276
  HeaderFontColor white
}

class Singleton {
  - {static} instance : Singleton
  - data : string
  --
  - Singleton()
  + {static} getInstance() : Singleton
  + someBusinessLogic() : void
}

Singleton --> Singleton : instance

note right of Singleton
  In proiect: StocManager
  getInstance() = Instance (property)
  Thread-safe cu Double-Check Locking
end note
@enduml
```

---

## 2. Factory Method

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

abstract class Creator {
  + {abstract} createProduct() : Product
  + someOperation() : void
}

class ConcreteCreatorA {
  + createProduct() : Product
}

class ConcreteCreatorB {
  + createProduct() : Product
}

interface Product {
  + doStuff() : void
}

class ConcreteProductA {
  + doStuff() : void
}

class ConcreteProductB {
  + doStuff() : void
}

Creator <|-- ConcreteCreatorA
Creator <|-- ConcreteCreatorB
Product <|.. ConcreteProductA
Product <|.. ConcreteProductB
ConcreteCreatorA ..> ConcreteProductA : <<creates>>
ConcreteCreatorB ..> ConcreteProductB : <<creates>>

note right of Creator
  In proiect:
  Creator = ProdusFactory
  ConcreteCreatorA = MedicamentFactory
  ConcreteCreatorB = EchipamentFactory
  Product = Produs
end note
@enduml
```

---

## 3. Abstract Factory

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

interface AbstractFactory {
  + createProductA() : AbstractProductA
  + createProductB() : AbstractProductB
}

class ConcreteFactory1 {
  + createProductA() : AbstractProductA
  + createProductB() : AbstractProductB
}

class ConcreteFactory2 {
  + createProductA() : AbstractProductA
  + createProductB() : AbstractProductB
}

interface AbstractProductA {
  + usefulFunctionA() : void
}

interface AbstractProductB {
  + usefulFunctionB() : void
  + anotherUsefulFunctionB(a : AbstractProductA) : void
}

class ProductA1 {
  + usefulFunctionA() : void
}
class ProductA2 {
  + usefulFunctionA() : void
}
class ProductB1 {
  + usefulFunctionB() : void
}
class ProductB2 {
  + usefulFunctionB() : void
}

class Client {
  + Client(f : AbstractFactory)
  + someOperation() : void
}

AbstractFactory <|.. ConcreteFactory1
AbstractFactory <|.. ConcreteFactory2
AbstractProductA <|.. ProductA1
AbstractProductA <|.. ProductA2
AbstractProductB <|.. ProductB1
AbstractProductB <|.. ProductB2
ConcreteFactory1 ..> ProductA1 : <<creates>>
ConcreteFactory1 ..> ProductB1 : <<creates>>
ConcreteFactory2 ..> ProductA2 : <<creates>>
ConcreteFactory2 ..> ProductB2 : <<creates>>
Client --> AbstractFactory : uses

note right of AbstractFactory
  In proiect:
  AbstractFactory = ITrusaFactory
  ConcreteFactory1 = TrusaAdultiFactory
  ConcreteFactory2 = TrusaCopiiFactory
  ProductA = Medicament, ProductB = Bandaj
end note
@enduml
```

---

## 4. Builder

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

interface Builder {
  + reset() : void
  + buildStepA() : void
  + buildStepB() : void
  + buildStepZ() : void
}

class ConcreteBuilder1 {
  - result : Product1
  --
  + reset() : void
  + buildStepA() : void
  + buildStepB() : void
  + buildStepZ() : void
  + getResult() : Product1
}

class ConcreteBuilder2 {
  - result : Product2
  --
  + reset() : void
  + buildStepA() : void
  + buildStepB() : void
  + buildStepZ() : void
  + getResult() : Product2
}

class Director {
  - builder : Builder
  --
  + Director(b : Builder)
  + changeBuilder(b : Builder) : void
  + makeSimpleProduct() : void
  + makeFullFeaturedProduct() : void
}

class Product1 {
  - parts : List<string>
  + listParts() : void
}

class Product2

Builder <|.. ConcreteBuilder1
Builder <|.. ConcreteBuilder2
Director --> Builder : directs
ConcreteBuilder1 ..> Product1 : <<creates>>
ConcreteBuilder2 ..> Product2 : <<creates>>

note right of Director
  In proiect:
  Builder = TrusaBuilder
  Director = TrusaDirector
  Product1 = TrusaMedicala
  makeSimpleProduct() = ConstructTrusaVacanta()
  makeFullFeaturedProduct() = ConstructTrusaAuto()
end note
@enduml
```

---

## 5. Prototype

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

interface Prototype {
  + clone() : Prototype
}

class ConcretePrototype1 {
  - field1 : int
  --
  + ConcretePrototype1(prototype : ConcretePrototype1)
  + clone() : Prototype
}

class ConcretePrototype2 {
  - field1 : string
  - field2 : float
  --
  + ConcretePrototype2(prototype : ConcretePrototype2)
  + clone() : Prototype
}

class SubclassPrototype {
  - field3 : bool
  --
  + clone() : Prototype
}

class Client {
  + someOperation() : void
}

Prototype <|.. ConcretePrototype1
Prototype <|.. ConcretePrototype2
ConcretePrototype2 <|-- SubclassPrototype
Client --> Prototype : uses clone()

note right of Prototype
  In proiect:
  Prototype = IPrototip (Cloneaza())
  ConcretePrototype1 = Medicament
  ConcretePrototype2 = EchipamentMedical
  SubclassPrototype = PachetProduse
end note
@enduml
```

---

## 6. Adapter

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

class Client {
  + someOperation() : void
}

interface ClientInterface {
  + method(data : SpecialData) : void
}

class Adapter {
  - adaptee : Service
  --
  + method(data : SpecialData) : void
}

class Service {
  + serviceMethod(data : SpecialData) : void
}

ClientInterface <|.. Adapter
Client --> ClientInterface : uses
Adapter --> Service : adapts

note right of Adapter
  In proiect:
  Client = Form1 / StocManager
  ClientInterface = Produs (abstract)
  Adapter = ProdusAdapter
  Service = FurnizorExternProdus
  method() = ObtineDetalii()
  serviceMethod() = GetProductName()
end note
@enduml
```

---

## 7. Bridge

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

class Abstraction {
  # implementation : Implementation
  --
  + Abstraction(i : Implementation)
  + feature1() : void
  + feature2() : void
}

class RefinedAbstraction {
  + feature1() : void
  + feature2() : void
  + refinedFeature() : void
}

interface Implementation {
  + operationImpl() : void
}

class ConcreteImplementationA {
  + operationImpl() : void
}

class ConcreteImplementationB {
  + operationImpl() : void
}

Abstraction <|-- RefinedAbstraction
Abstraction o--> Implementation : has
Implementation <|.. ConcreteImplementationA
Implementation <|.. ConcreteImplementationB

note right of Abstraction
  In proiect:
  Abstraction = Notificator
  RefinedAbstraction = NotificatorUrgent
  Implementation = IPlatformaTrimitere
  ConcreteImplementationA = TrimitereSms
  ConcreteImplementationB = TrimitereEmail
  operationImpl() = Trimite(mesaj)
end note
@enduml
```

---

## 8. Composite

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

interface Component {
  + execute() : void
}

class Leaf {
  + execute() : void
}

class Composite {
  - children : List<Component>
  --
  + add(c : Component) : void
  + remove(c : Component) : void
  + getChildren() : List<Component>
  + execute() : void
}

class Client {
  + someOperation(c : Component) : void
}

Component <|.. Leaf
Component <|.. Composite
Composite o--> "*" Component : children
Client --> Component : uses

note right of Composite
  In proiect:
  Component = Produs (abstract)
  Leaf = Medicament, EchipamentMedical
  Composite = PachetProduse
  execute() = ObtineDetalii()
  add() = AdaugaInPachet()
  Pret = suma automata a copiilor
end note
@enduml
```

---

## 9. Decorator

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

interface Component {
  + execute() : void
}

class ConcreteComponent {
  + execute() : void
}

abstract class BaseDecorator {
  - wrappee : Component
  --
  + BaseDecorator(c : Component)
  + execute() : void
}

class ConcreteDecoratorA {
  + execute() : void
  + extraA() : void
}

class ConcreteDecoratorB {
  + execute() : void
  + extraB() : void
}

Component <|.. ConcreteComponent
Component <|.. BaseDecorator
BaseDecorator <|-- ConcreteDecoratorA
BaseDecorator <|-- ConcreteDecoratorB
BaseDecorator o--> Component : wraps

note right of BaseDecorator
  In proiect:
  Component = Produs (abstract)
  ConcreteComponent = Medicament
  BaseDecorator = ProdusDecorator
  ConcreteDecoratorA = AmbalajCadouDecorator
  execute() = ObtineDetalii()
  extraA() = Pret + 5 MDL
end note
@enduml
```

---

## 10. Facade

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

class Client1
class Client2

class Facade {
  - linksToSubsystemObjects
  --
  + subsystemOperation1() : void
  + subsystemOperation2() : void
}

class AdditionalFacade {
  + anotherOperation() : void
}

package "Subsystem" {
  class SubsystemClass1 {
    + operation1() : void
  }
  class SubsystemClass2 {
    + operation2() : void
  }
  class SubsystemClass3 {
    + operation3() : void
  }
}

Client1 --> Facade
Client2 --> AdditionalFacade
AdditionalFacade --> Facade
Facade --> SubsystemClass1
Facade --> SubsystemClass2
Facade --> SubsystemClass3

note right of Facade
  In proiect:
  Facade = FarmacieFacade
  SubsystemClass1 = SistemStoc
  SubsystemClass2 = SistemPlata
  SubsystemClass3 = SistemFacturare
  subsystemOperation() = EfectueazaVanzare()
end note
@enduml
```

---

## 11. Flyweight

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

class FlyweightFactory {
  - cache : Flyweight[]
  --
  + getFlyweight(repeatingState) : Flyweight
  + listFlyweights() : void
}

class Flyweight {
  - repeatingState : string
  --
  + Flyweight(s : string)
  + operation(uniqueState : string) : void
}

class Context {
  - uniqueState : string
  - flyweight : Flyweight
  --
  + Context(s : string, f : Flyweight)
  + operation() : void
}

class Client {
  + addCarToPoliceDatabase() : void
}

FlyweightFactory --> Flyweight : creates/caches
Context --> Flyweight : uses
Client --> FlyweightFactory
Client --> Context : creates

note right of FlyweightFactory
  In proiect:
  FlyweightFactory = CategorieFactory
  Flyweight = CategorieFlyweight
  repeatingState = Tip, Descriere (intrinsic)
  uniqueState = numeProdus (extrinsic)
end note
@enduml
```

---

## 12. Proxy

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

interface ServiceInterface {
  + operation() : void
}

class Service {
  + operation() : void
}

class Proxy {
  - realService : Service
  --
  + Proxy(s : Service)
  + checkAccess() : bool
  + logAccess() : void
  + operation() : void
}

class Client {
  + doSomething(s : ServiceInterface) : void
}

ServiceInterface <|.. Service
ServiceInterface <|.. Proxy
Proxy --> Service : delegates
Client --> ServiceInterface : uses

note right of Proxy
  In proiect:
  ServiceInterface = IAccesBazaDate
  Service = RealBazaDate
  Proxy = ProxyBazaDate
  checkAccess() = verifica rolul (Manager/Farmacist)
  operation() = StergeProdus()
end note
@enduml
```

---

## 13. Chain of Responsibility

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

interface Handler {
  + setNext(h : Handler) : Handler
  + handle(request) : void
}

abstract class BaseHandler {
  - nextHandler : Handler
  --
  + setNext(h : Handler) : Handler
  + handle(request) : void
}

class ConcreteHandler1 {
  + handle(request) : void
}

class ConcreteHandler2 {
  + handle(request) : void
}

class Client {
  + someOperation() : void
}

Handler <|.. BaseHandler
BaseHandler <|-- ConcreteHandler1
BaseHandler <|-- ConcreteHandler2
Client --> Handler : uses
ConcreteHandler1 --> ConcreteHandler2 : next

note right of Handler
  In proiect:
  Handler = IAprobareHandler
  ConcreteHandler1 = FarmacistHandler (<=5%)
  ConcreteHandler2 = ManagerHandler (<=15%)
  + DirectorHandler (<=100%)
  handle() = GestioneazaCererea(procent)
end note
@enduml
```

---

## 14. Command

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

class Invoker {
  - onStart : Command
  - onFinish : Command
  --
  + setOnStart(c : Command) : void
  + setOnFinish(c : Command) : void
  + doSomethingImportant() : void
}

interface Command {
  + execute() : void
}

class ConcreteCommand1 {
  - receiver : Receiver
  - params
  --
  + ConcreteCommand1(r, p)
  + execute() : void
}

class ConcreteCommand2 {
  - backup : int
  --
  + saveBackup() : void
  + undo() : void
  + execute() : void
}

class Receiver {
  + operation(a, b, c) : void
}

class Client {
  + someOperation() : void
}

Command <|.. ConcreteCommand1
Command <|.. ConcreteCommand2
Invoker --> Command : invokes
ConcreteCommand1 --> Receiver : calls
Client --> Receiver : creates
Client --> ConcreteCommand1 : creates

note right of Invoker
  In proiect:
  Invoker = CasaDeMarcat
  Command = ICommand
  ConcreteCommand1 = ComandaVanzare
  Receiver = SistemGestiune
  execute() + undo() implementate
end note
@enduml
```

---

## 15. Iterator

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

interface Iterator {
  + getNext() : void
  + hasMore() : bool
}

interface IterableCollection {
  + createIterator() : Iterator
}

class ConcreteIterator {
  - collection : ConcreteCollection
  - iterationState : int
  --
  + getNext() : void
  + hasMore() : bool
}

class ConcreteCollection {
  + createIterator() : Iterator
}

class Client {
  + someOperation() : void
}

Iterator <|.. ConcreteIterator
IterableCollection <|.. ConcreteCollection
ConcreteIterator --> ConcreteCollection : references
Client --> Iterator : uses
Client --> IterableCollection : uses

note right of IterableCollection
  In proiect:
  IterableCollection = IIterableCollection
  ConcreteCollection = DulapMedicamente
  Iterator = IIterator
  ConcreteIterator = IteratorDulap
  createIterator() = CreateIterator()
end note
@enduml
```

---

## 16. Mediator

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

interface Mediator {
  + notify(sender : Component, event : string) : void
}

class ConcreteMediator {
  - componentA : ComponentA
  - componentB : ComponentB
  --
  + notify(sender : Component, event : string) : void
  + reactOnA() : void
  + reactOnB() : void
}

abstract class BaseComponent {
  # mediator : Mediator
  --
  + BaseComponent(m : Mediator)
  + setMediator(m : Mediator) : void
}

class ComponentA {
  + doA() : void
}

class ComponentB {
  + doB() : void
  + doC() : void
}

Mediator <|.. ConcreteMediator
BaseComponent <|-- ComponentA
BaseComponent <|-- ComponentB
ConcreteMediator --> ComponentA
ConcreteMediator --> ComponentB
ComponentA --> Mediator : notifies
ComponentB --> Mediator : notifies

note right of ConcreteMediator
  In proiect:
  Mediator = IMediator
  ConcreteMediator = CentralaFarmacie
  ComponentA = DepartamentVanzari
  ComponentB = DepartamentDepozit
  notify() = Notifica(sender, eveniment)
end note
@enduml
```

---

## 17. Memento

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

class Originator {
  - state : string
  --
  + setState(s : string) : void
  + save() : Memento
  + restore(m : Memento) : void
}

interface Memento {
  + getName() : string
  + getDate() : string
}

class ConcreteMemento {
  - state : string
  - date : string
  --
  + ConcreteMemento(s : string)
  + getState() : string
  + getName() : string
  + getDate() : string
}

class Caretaker {
  - mementos : List<Memento>
  - originator : Originator
  --
  + backup() : void
  + undo() : void
  + showHistory() : void
}

Memento <|.. ConcreteMemento
Originator ..> ConcreteMemento : <<creates>>
Caretaker --> Originator : uses
Caretaker o--> Memento : stores

note right of Originator
  In proiect:
  Originator = CosOriginator
  Memento = CosMemento
  Caretaker = IstoricCosCaretaker
  save() = Salveaza()
  restore() = Restaureaza()
  backup() = SalveazaStarea()
end note
@enduml
```

---

## 18. Observer

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

class Publisher {
  - subscribers : Subscriber[]
  - mainState : int
  --
  + subscribe(s : Subscriber) : void
  + unsubscribe(s : Subscriber) : void
  + notifySubscribers() : void
  + mainBusinessLogic() : void
}

interface Subscriber {
  + update(context : Publisher) : void
}

class ConcreteSubscriberA {
  + update(context : Publisher) : void
}

class ConcreteSubscriberB {
  + update(context : Publisher) : void
}

Publisher o--> "*" Subscriber : notifies
Subscriber <|.. ConcreteSubscriberA
Subscriber <|.. ConcreteSubscriberB

note right of Publisher
  In proiect:
  Publisher = ProdusPublisher
  Subscriber = ISubscriber
  ConcreteSubscriberA = SistemAprovizionare
  ConcreteSubscriberB = FarmacistAbonat
  mainBusinessLogic() = ModificaStoc()
  Notifica cand stoc < 10 buc
end note
@enduml
```

---

## 19. State

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

class Context {
  - state : State
  --
  + Context(s : State)
  + changeState(s : State) : void
  + doThis() : void
  + doThat() : void
}

interface State {
  + doThis(context : Context) : void
  + doThat(context : Context) : void
}

class ConcreteStateA {
  + doThis(context : Context) : void
  + doThat(context : Context) : void
}

class ConcreteStateB {
  + doThis(context : Context) : void
  + doThat(context : Context) : void
}

State <|.. ConcreteStateA
State <|.. ConcreteStateB
Context --> State : has state
ConcreteStateA --> ConcreteStateB : transition

note right of Context
  In proiect:
  Context = ComandaAprovizionare
  State = IStareComanda
  ConcreteStateA = StareNoua
  ConcreteStateB = StareInProcesare
  + StareLivrata
  doThis() = Proceseaza()
  doThat() = Livreaza()
end note
@enduml
```

---

## 20. Strategy

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

class Context {
  - strategy : Strategy
  --
  + setStrategy(s : Strategy) : void
  + doSomeBusinessLogic() : void
}

interface Strategy {
  + execute(data) : void
}

class ConcreteStrategyA {
  + execute(data) : void
}

class ConcreteStrategyB {
  + execute(data) : void
}

class Client {
  + someOperation() : void
}

Strategy <|.. ConcreteStrategyA
Strategy <|.. ConcreteStrategyB
Context --> Strategy : uses
Client --> Context : configures

note right of Context
  In proiect:
  Context = CalculatorPretFinal
  Strategy = IStrategieDiscount
  ConcreteStrategyA = FaraDiscount
  ConcreteStrategyB = DiscountFidelitate (-10%)
  + DiscountPensionar (-20%)
  execute() = AplicaDiscount(total)
end note
@enduml
```

---

## 21. Template Method

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

abstract class AbstractClass {
  + templateMethod() : void
  # step1() : void
  # {abstract} step2() : void
  # step3() : void
  # {abstract} step4() : void
}

class ConcreteClass1 {
  # step2() : void
  # step4() : void
}

class ConcreteClass2 {
  # step2() : void
  # step4() : void
}

AbstractClass <|-- ConcreteClass1
AbstractClass <|-- ConcreteClass2

note right of AbstractClass
  In proiect:
  AbstractClass = RaportTemplate
  templateMethod() = GenereazaRaport()
  step1() = CulegeDate() (comun)
  step2() = FormateazaRaport() (abstract)
  step3() = SalveazaFisier() (abstract)
  step4() = PrinteazaRaport() (comun)
  ConcreteClass1 = RaportZilnicVanzari (CSV)
  ConcreteClass2 = RaportStocCritic (TXT)
end note
@enduml
```

---

## 22. Visitor

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

interface Visitor {
  + visitConcreteComponentA(e : ConcreteComponentA) : void
  + visitConcreteComponentB(e : ConcreteComponentB) : void
}

class ConcreteVisitor1 {
  + visitConcreteComponentA(e : ConcreteComponentA) : void
  + visitConcreteComponentB(e : ConcreteComponentB) : void
}

class ConcreteVisitor2 {
  + visitConcreteComponentA(e : ConcreteComponentA) : void
  + visitConcreteComponentB(e : ConcreteComponentB) : void
}

interface Component {
  + accept(v : Visitor) : void
}

class ConcreteComponentA {
  + accept(v : Visitor) : void
  + exclusiveMethodOfConcreteComponentA() : void
}

class ConcreteComponentB {
  + accept(v : Visitor) : void
  + specialMethodOfConcreteComponentB() : void
}

class Client {
  + someOperation() : void
}

Visitor <|.. ConcreteVisitor1
Visitor <|.. ConcreteVisitor2
Component <|.. ConcreteComponentA
Component <|.. ConcreteComponentB
Client --> Visitor : uses
Client --> Component : uses
ConcreteVisitor1 ..> ConcreteComponentA : visits
ConcreteVisitor1 ..> ConcreteComponentB : visits

note right of Visitor
  In proiect:
  Visitor = IVisitorExport
  ConcreteVisitor1 = ExportXmlVisitor
  Component = IDocumentFarmacie
  ConcreteComponentA = RetetaCompensata
  ConcreteComponentB = FacturaFirma
  accept() -> v.Visit(this) = Double Dispatch
end note
@enduml
```

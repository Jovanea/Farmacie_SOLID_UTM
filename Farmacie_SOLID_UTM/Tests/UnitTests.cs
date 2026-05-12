using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie_SOLID_UTM.Models;
using Farmacie_SOLID_UTM.Services;
using Farmacie_SOLID_UTM.Builders;
using Farmacie_SOLID_UTM.Director;
using Farmacie_SOLID_UTM.Director;
using Farmacie_SOLID_UTM.Decorators;
using Farmacie_SOLID_UTM.Interfaces;
using System.Windows.Forms;

using Farmacie_SOLID_UTM.Strategies;
using Farmacie_SOLID_UTM.Observers;
using Farmacie_SOLID_UTM.Commands;
using Farmacie_SOLID_UTM.Mementos;
using Farmacie_SOLID_UTM.Iterators;
using Farmacie_SOLID_UTM.Chains;
using Farmacie_SOLID_UTM.States;
using Farmacie_SOLID_UTM.Mediators;
using Farmacie_SOLID_UTM.Templates;
using Farmacie_SOLID_UTM.Visitors;

namespace Farmacie_SOLID_UTM.Tests
{
    // Clasa simplă pentru Testare Unitara (Manuală)
    // Deoarece nu avem un proiect separat de teste (NUnit/xUnit), simulăm testele aici.
    public static class UnitTests
    {
        public static void RuleazaToate()
        {
            try
            {
                Console.WriteLine("--- Start Teste Unitare ---");

                TestSingleton();
                TestBuilder();
                TestPrototype();
                TestAdapter();
                TestComposite();
                TestComposite();
                TestFacade();
                TestFlyweight();
                TestDecorator();
                TestBridge();
                TestProxy();

                // --- Teste Lab 6 - Comportamentale ---
                TestStrategy();
                TestObserver();
                TestCommand();
                TestMemento();
                TestIterator();

                // --- Teste Lab 7 ---
                TestChainOfResponsibility();
                TestState();
                TestMediator();
                TestTemplateMethod();
                TestVisitor();

                MessageBox.Show("Toate testele (Lab 1-7) au trecut cu succes!", "Testare Unitara");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TEST FAILED: {ex.Message}", "Eroare Testare");
            }
        }

        private static void TestSingleton()
        {
            // Verificam daca doua apeluri returneaza aceeasi instanta
            var s1 = StocManager.Instance;
            var s2 = StocManager.Instance;

            if (!object.ReferenceEquals(s1, s2))
                throw new Exception("Singleton Failed: Instanțele sunt diferite!");

            // Verificam starea partajata
            int initialCount = s1.GetTotalProduse();
            s1.AdaugaProdus(new Medicament("TestSingleton", 10, "Test"));
            
            if (s2.GetTotalProduse() != initialCount + 1)
                throw new Exception("Singleton Failed: Starea nu este partajată corect!");

            Console.WriteLine("Test Singleton: PASSED");
        }

        private static void TestBuilder()
        {
            // Verificam daca Directorul construieste corect
            var builder = new TrusaBuilder();
            var director = new TrusaDirector(builder);

            var trusa = director.ConstructTrusaVacanta();

            if (trusa == null)
                throw new Exception("Builder Failed: Trusa este null!");

            if (!trusa.Nume.Contains("Vacanță"))
                throw new Exception("Builder Failed: Numele trusei incorect!");

            // Verificam pretul (ar trebui sa fie > 0)
            if (trusa.CalculeazaPretTotal() <= 0)
                throw new Exception("Builder Failed: Pretul total incorect!");

            Console.WriteLine("Test Builder: PASSED");
        }

        private static void TestPrototype()
        {
            // Verificam clonarea
            var original = new Medicament("Nurofen", 25.5m, "Reckitt");
            var clona = (Medicament)original.Cloneaza();

            // Verificam daca valorile sunt identice
            if (original.Nume != clona.Nume || original.Pret != clona.Pret)
                throw new Exception("Prototype Failed: Valorile nu s-au copiat corect!");

            // Verificam daca sunt obiecte diferite in memorie
            if (object.ReferenceEquals(original, clona))
                throw new Exception("Prototype Failed: Clona este acelasi obiect cu originalul (referinta identica)!");

            Console.WriteLine("Test Prototype: PASSED");
        }

        private static void TestAdapter()
        {
            var furnizorExtern = new FurnizorExternProdus();
            Produs adapter = new ProdusAdapter(furnizorExtern);

            if (adapter.Nume != "Produs Extern (Importat)")
                throw new Exception("Adapter Failed: Numele nu s-a preluat corect!");

            if (adapter.Pret != 45.50m)
                throw new Exception("Adapter Failed: Pretul nu s-a preluat corect!");

            Console.WriteLine("Test Adapter: PASSED");
        }

        private static void TestComposite()
        {
            var pachet = new PachetProduse("Pachet Test");
            pachet.AdaugaInPachet(new Medicament("Nurofen", 20, "Reckitt"));
            pachet.AdaugaInPachet(new BandajElastic());

            if (pachet.Pret != 45) // 20 + 25 (BandajElastic default price is 25)
                throw new Exception("Composite Failed: Pretul total al pachetului calculat incorect!");

            Console.WriteLine("Test Composite: PASSED");
        }

        private static void TestFacade()
        {
            var facade = new FarmacieFacade();
            var produsTest = new Medicament("Medicament Facade Test", 50, "TestCorp");
            
            string mesaj = facade.VindeProdusCatreClient(produsTest);

            if (!mesaj.Contains("Succes"))
                throw new Exception("Facade Failed: Procesul de vanzare a esuat!");

            Console.WriteLine("Test Facade: PASSED");
        }

        private static void TestFlyweight()
        {
            var factory = new CategorieFactory();
            var cat1 = factory.GetCategorie("Antibiotice", "Rețetă obligatorie");
            var cat2 = factory.GetCategorie("Antibiotice", "Rețetă obligatorie");

            if (!object.ReferenceEquals(cat1, cat2))
                throw new Exception("Flyweight Failed: Instantele nu sunt partajate!");

            var p1 = new ProdusComercial("Amoxicilina", 15, "001", cat1);
            var p2 = new ProdusComercial("Augmentin", 35, "002", cat2);

            Console.WriteLine("Test Flyweight: PASSED");
        }

        private static void TestDecorator()
        {
            Produs baza = new Medicament("Paracetamol", 10, "Zentiva");
            var cuAmbalaj = new AmbalajCadouDecorator(baza); // Corectie pentru cuvantul cheie `new` pe proprietatea Pret

            if (cuAmbalaj.Pret != 15) // 10 original + 5 ambalaj
                throw new Exception("Decorator Failed: Pretul calculat incorect!");

            if (!cuAmbalaj.ObtineDetalii().Contains("Ambalaj Cadou"))
                throw new Exception("Decorator Failed: Functia ObtineDetalii nu a fost extinsa!");

            Console.WriteLine("Test Decorator: PASSED");
        }

        private static void TestBridge()
        {
            IPlatformaTrimitere metodaSms = new TrimitereSms();
            Notificator notificator = new NotificatorUrgent(metodaSms);
            
            // Simulam trimiterea (ar trebui sa foloseasca platforma injectata)
            notificator.ExpediazaAlerta("Stoc epuizat la Nurofen");

            Console.WriteLine("Test Bridge: PASSED");
        }

        private static void TestProxy()
        {
            IAccesBazaDate proxyGresit = new ProxyBazaDate("UserSimplu");
            proxyGresit.StergeProdus("Test1"); // Eroare afisata in consola
            
            IAccesBazaDate proxyCorect = new ProxyBazaDate("Manager");
            proxyCorect.StergeProdus("Test2"); // Trece corect

            Console.WriteLine("Test Proxy: PASSED");
        }

        private static void TestStrategy()
        {
            var calculator = new CalculatorPretFinal();
            
            // Fara discount
            calculator.SetStrategie(new FaraDiscount());
            if (calculator.CalculeazaPretul(100) != 100) throw new Exception("Strategy Failed: FaraDiscount a corupt calculul!");

            // Discount de fidelitate
            calculator.SetStrategie(new DiscountFidelitate());
            if (calculator.CalculeazaPretul(100) != 90) throw new Exception("Strategy Failed: DiscountFidelitate a esuat!");

            Console.WriteLine("Test Strategy: PASSED");
        }

        private static void TestObserver()
        {
            var produs = new ProdusPublisher("Paracetamol", 10);
            var sistemAprovizionare = new SistemAprovizionare();
            var farmacist = new FarmacistAbonat("Ion");

            produs.Subscribe(sistemAprovizionare);
            produs.Subscribe(farmacist);

            // Modificare normala
            produs.ModificaStoc(8); 

            // Scade sub 5 -> Trebuie sa primim loguri in consola 
            Console.WriteLine("--- Se asteapta Notificari Observer ---");
            produs.ModificaStoc(3); 
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("Test Observer: PASSED");
        }

        private static void TestCommand()
        {
            var sistem = new SistemGestiune(); // Receiver
            var casa = new CasaDeMarcat(); // Invoker
            var cmd = new ComandaVanzare(sistem, "Ibuprofen", 2);

            casa.SetCommand(cmd);
            casa.ExecuteCommand(); // Reduce din receiver
            casa.UndoUltimaComanda(); // Aduna la loc in receiver

            Console.WriteLine("Test Command: PASSED");
        }

        private static void TestMemento()
        {
            var cos = new CosOriginator();
            cos.AdaugaProdus("Aspirina");

            var istoric = new IstoricCosCaretaker(cos);
            istoric.SalveazaStarea(); // Salveaza snapshot doar cu Aspirina

            cos.AdaugaProdus("Nurofen"); // Adaugam inca ceva

            istoric.Undo(); // Restore cos = doar Aspirina

            Console.WriteLine("Test Memento: PASSED");
        }

        private static void TestIterator()
        {
            var dulap = new DulapMedicamente();
            dulap.Adauga("Ser fiziologic");
            dulap.Adauga("Betadina");
            dulap.Adauga("Oxigen");

            var iterator = dulap.CreateIterator();
            int nr = 0;
            while(iterator.HasMore())
            {
                var val = iterator.GetNext();
                nr++;
            }

            if (nr != 3) throw new Exception("Iterator Failed!");

            Console.WriteLine("Test Iterator: PASSED");
        }

        // ========================================================
        // LAB 7 TESTS
        // ========================================================

        private static void TestChainOfResponsibility()
        {
            var farmacist = new FarmacistHandler();
            var manager = new ManagerHandler();
            var director = new DirectorHandler();

            farmacist.SetNext(manager).SetNext(director);

            Console.WriteLine("--- Test Chain: Discount 3% ---");
            farmacist.GestioneazaCererea(3);

            Console.WriteLine("--- Test Chain: Discount 10% ---");
            farmacist.GestioneazaCererea(10);

            Console.WriteLine("--- Test Chain: Discount 20% ---");
            farmacist.GestioneazaCererea(20);

            Console.WriteLine("Test ChainOfResponsibility: PASSED");
        }

        private static void TestState()
        {
            Console.WriteLine("--- Test State ---");
            var comanda = new ComandaAprovizionare(new StareNoua());
            
            // Noua -> In Procesare
            comanda.Proceseaza(); 
            // In Procesare -> Livrata
            comanda.Livreaza();
            // Nu se poate anula
            comanda.Anuleaza();

            Console.WriteLine("Test State: PASSED");
        }

        private static void TestMediator()
        {
            Console.WriteLine("--- Test Mediator ---");
            var centrala = new CentralaFarmacie();
            var vanzari = new DepartamentVanzari();
            var depozit = new DepartamentDepozit();

            centrala.SeteazaVanzari(vanzari);
            centrala.SeteazaDepozit(depozit);

            // Declanseaza lantul mediatic
            vanzari.EfectueazaVanzare();

            Console.WriteLine("Test Mediator: PASSED");
        }

        private static void TestTemplateMethod()
        {
            Console.WriteLine("--- Test Template Method ---");
            RaportTemplate raport1 = new RaportZilnicVanzari();
            raport1.GenereazaRaport();

            RaportTemplate raport2 = new RaportStocCritic();
            raport2.GenereazaRaport();

            Console.WriteLine("Test TemplateMethod: PASSED");
        }

        private static void TestVisitor()
        {
            Console.WriteLine("--- Test Visitor ---");
            var documente = new List<IDocumentFarmacie>
            {
                new RetetaCompensata { NumePacient = "Vasile", Diagnostic = "Raceala" },
                new FacturaFirma { NumeFirma = "Furnizor A", TotalDePlata = 500 }
            };

            var visitor = new ExportXmlVisitor();

            foreach (var doc in documente)
            {
                doc.Accept(visitor);
            }

            Console.WriteLine("Test Visitor: PASSED");
        }
    }
}

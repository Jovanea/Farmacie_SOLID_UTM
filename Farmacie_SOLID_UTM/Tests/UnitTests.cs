using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie_SOLID_UTM.Models;
using Farmacie_SOLID_UTM.Services;
using Farmacie_SOLID_UTM.Builders;
using Farmacie_SOLID_UTM.Director;
using System.Windows.Forms;

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

                MessageBox.Show("Toate testele au trecut cu succes!", "Testare Unitara");
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
    }
}

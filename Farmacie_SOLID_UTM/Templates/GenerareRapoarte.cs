using System;

namespace Farmacie_SOLID_UTM.Templates
{
    // Template Method: Definește scheletul unui algoritm, permițând subclaselor să suprascrie anumiți pași.

    // Clasa Abstractă (Template)
    public abstract class RaportTemplate       ////
    {
        // Aceasta este "Template Method" - nu se poate suprascrie (sau nu ar trebui)
        // Definește exact ordinea pașilor
        public void GenereazaRaport()
        {
            CulegeDate();
            FormateazaRaport();
            PrinteazaRaport();
            Console.WriteLine("[Template] Raportul a fost finalizat.\n");
        }

        // Pas comun 1
        protected void CulegeDate()
        {
            Console.WriteLine("[Template] 1. Culegere date din baza de date centrală...");
        }

        // Pas abstract (trebuie implementat de copii)
        protected abstract void FormateazaRaport();

        // Pas comun 2
        protected void PrinteazaRaport()
        {
            Console.WriteLine("[Template] 3. Printare raport către imprimanta principală...");
        }
    }

    // Implementare 1: Raport Zilnic de Vânzări
    public class RaportZilnicVanzari : RaportTemplate         /////
    {
        protected override void FormateazaRaport()
        {
            Console.WriteLine("[Template] 2. Se formatează datele în fișier CSV (Vânzări).");
        }
    }

    // Implementare 2: Raport Stoc Critic
    public class RaportStocCritic : RaportTemplate             ////
    {
        protected override void FormateazaRaport()
        {
            Console.WriteLine("[Template] 2. Se formatează datele în document PDF (Alertă Stoc!).");
        }
    }
}

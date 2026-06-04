using System;
using System.IO;
using System.Text;
using Farmacie_SOLID_UTM.Services;

namespace Farmacie_SOLID_UTM.Templates
{
    // Template Method: Definește scheletul unui algoritm, permițând subclaselor să suprascrie anumiți pași.

    public abstract class RaportTemplate
    {
        protected string _continut = "";

        // Template Method — ordinea pașilor e fixată, nu poate fi schimbată
        public string GenereazaRaport()
        {
            CulegeDate();
            FormateazaRaport();
            string cale = SalveazaFisier();
            PrinteazaRaport(cale);
            return cale;
        }

        // Pas 1: comun pentru toate rapoartele
        protected void CulegeDate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Data raport: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.AppendLine("Farmacie: FarmSys UTM");
            sb.AppendLine("Produse in stoc: " + StocManager.Instance.GetTotalProduse());
            sb.AppendLine();
            sb.AppendLine("--- PRODUSE ---");
            foreach (var p in StocManager.Instance.GetProduse())
                sb.AppendLine(p.Nume + ";" + p.Pret.ToString("F2") + ";" + p.Cantitate);
            _continut = sb.ToString();
        }

        // Pas 2: diferit pentru fiecare subclasă
        protected abstract void FormateazaRaport();

        // Pas 3: salvează fișierul și returnează calea
        protected abstract string SalveazaFisier();

        // Pas 4: comun
        protected void PrinteazaRaport(string cale)
        {
            Console.WriteLine("[Template] Raport salvat la: " + cale);
        }
    }

    // Implementare 1: CSV
    public class RaportZilnicVanzari : RaportTemplate
    {
        protected override void FormateazaRaport()
        {
            // Formatare CSV: adaugă header
            _continut = "Denumire;Pret (MDL);Cantitate\n" + _continut;
        }

        protected override string SalveazaFisier()
        {
            string cale = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "RaportVanzari_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");
            File.WriteAllText(cale, _continut, Encoding.UTF8);
            return cale;
        }
    }

    // Implementare 2: TXT cu aspect de PDF (fără librării externe)
    public class RaportStocCritic : RaportTemplate
    {
        protected override void FormateazaRaport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("     RAPORT STOC CRITIC — FARMSYS");
            sb.AppendLine("========================================");
            sb.AppendLine(_continut);
            sb.AppendLine("--- PRODUSE CU STOC CRITIC (< 10 buc) ---");
            foreach (var p in StocManager.Instance.GetProduse())
                if (p.Cantitate < 10)
                    sb.AppendLine("  !! " + p.Nume + "  —  " + p.Cantitate + " buc ramase");
            sb.AppendLine("========================================");
            _continut = sb.ToString();
        }

        protected override string SalveazaFisier()
        {
            string cale = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "RaportStocCritic_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            File.WriteAllText(cale, _continut, Encoding.UTF8);
            return cale;
        }
    }
}

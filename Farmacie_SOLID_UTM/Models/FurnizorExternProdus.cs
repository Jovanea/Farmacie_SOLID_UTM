using System;

namespace Farmacie_SOLID_UTM.Models
{
    // Clasa Incompatibilă (Adaptee) - simulează o clasă primită de la un terț
    public class FurnizorExternProdus
    {
        public string GetDenumire()
        {
            return "Produs Extern (Importat)";
        }

        public double GetPretNet()
        {
            return 45.50; // Preț fictiv de la furnizor
        }

        public string InformatiiSuplimentare()
        {
            return "Furnizor: MediTech Global";
        }
    }
}

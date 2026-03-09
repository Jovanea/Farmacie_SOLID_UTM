using System;

namespace Farmacie_SOLID_UTM.Services
{
    // Subsistem complex 2: Facturarea
    public class SistemFacturare
    {
        public void EmiteBon(string numeProdus, decimal suma)
        {
            Console.WriteLine($"[Facturare] S-a emis bonul fiscal pentru: {numeProdus} - {suma} MDL");
            // Logica complexă de generare PDF, printare...
        }
    }
}

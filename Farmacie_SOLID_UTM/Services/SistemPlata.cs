using System;

namespace Farmacie_SOLID_UTM.Services
{
    // Subsistem complex 1: Plata
    public class SistemPlata
    {
        public bool ProceseazaPlata(decimal suma)
        {
            Console.WriteLine($"[Plată] Se procesează plata de {suma} MDL...");
            // Logica complexă de conectare la bancă...
            return true;
        }
    }
}

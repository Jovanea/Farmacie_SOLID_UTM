using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Abstract Factory: Interfața pentru crearea familiilor de obiecte
    public interface ITrusaFactory
    {
        MedicamentDurere CreareMedicamentDurere();
        Bandaj CreareBandaj();
    }
}

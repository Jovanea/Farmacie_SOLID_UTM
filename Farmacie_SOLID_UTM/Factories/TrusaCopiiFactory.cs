using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Concrete Factory 2
    public class TrusaCopiiFactory : ITrusaFactory
    {
        public MedicamentDurere CreareMedicamentDurere()
        {
            return new SiropDurere();
        }

        public Bandaj CreareBandaj()
        {
            return new PlastureColorat();
        }
    }
}

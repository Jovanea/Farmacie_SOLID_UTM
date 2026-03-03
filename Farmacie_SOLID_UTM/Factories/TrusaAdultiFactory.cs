using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Concrete Factory 1
    public class TrusaAdultiFactory : ITrusaFactory
    {
        public MedicamentDurere CreareMedicamentDurere()
        {
            return new ParacetamolForte();
        }

        public Bandaj CreareBandaj()
        {
            return new BandajElastic();
        }
    }
}

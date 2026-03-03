using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Concrete Creator B
    public class EchipamentFactory : ProdusFactory
    {
        public override Produs CreazaProdus(string nume, decimal pret, string extra)
        {
            // 'extra' este interpretat ca TipEchipament pentru EchipamentMedical
            return new EchipamentMedical(nume, pret, extra);
        }
    }
}

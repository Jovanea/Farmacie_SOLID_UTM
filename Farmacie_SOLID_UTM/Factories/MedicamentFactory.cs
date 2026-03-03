using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Concrete Creator A
    public class MedicamentFactory : ProdusFactory
    {
        public override Produs CreazaProdus(string nume, decimal pret, string extra)
        {
            // 'extra' este interpretat ca Producator pentru Medicament
            return new Medicament(nume, pret, extra);
        }
    }
}

using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Factories
{
    // Factory Method Pattern - Creator
    public abstract class ProdusFactory
    {
        // Factory Method
        public abstract Produs CreazaProdus(string nume, decimal pret, string extra);
    }
}

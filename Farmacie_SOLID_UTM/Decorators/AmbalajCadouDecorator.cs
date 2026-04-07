using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Decorators
{
    // Extinde comportamentul unui produs adăugându-i opțiunea de "Ambalaj Cadou" la runtime
    public class AmbalajCadouDecorator : ProdusDecorator
    {
        private const decimal COST_AMBALAJ = 5.0m;

        public AmbalajCadouDecorator(Produs produs) : base(produs)
        {
        }

        // Adăugăm la prețul vechi, costul ambalajului superior
        public new decimal Pret 
        {
            get { return _produs.Pret + COST_AMBALAJ; }
        }

        // Adăugăm text în plus fără a distruge detalile native
        public override string ObtineDetalii()
        {
            return base.ObtineDetalii() + " [+Ambalaj Cadou VIP (5 MDL)]";
        }

        public override Produs Cloneaza()
        {
            // Clonăm produsul interior și îl punem într-un nou decorator identic
            return new AmbalajCadouDecorator(this._produs.Cloneaza());
        }
    }
}

using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Decorators
{
    // Decoratorul de bază este de tip Produs, dar și CONȚINE un Produs (Componenta)
    public abstract class ProdusDecorator : Produs
    {
        protected Produs _produs;

        public ProdusDecorator(Produs produs) : base(produs.Nume, produs.Pret)
        {
            _produs = produs;
        }

        // Deleagă execuția către produsul real înfășurat
        public override string ObtineDetalii()
        {
            return _produs.ObtineDetalii();
        }
    }
}

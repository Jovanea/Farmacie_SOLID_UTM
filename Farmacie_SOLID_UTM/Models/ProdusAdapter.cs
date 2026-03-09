using System;

namespace Farmacie_SOLID_UTM.Models
{
    // Adaptorul (Adapter) - face clasa incompatibilă să funcționeze ca un Produs standard
    public class ProdusAdapter : Produs
    {
        private readonly FurnizorExternProdus _furnizorExtern;

        public ProdusAdapter(FurnizorExternProdus furnizorExtern) 
            : base(furnizorExtern.GetDenumire(), (decimal)furnizorExtern.GetPretNet())
        {
            _furnizorExtern = furnizorExtern;
        }

        public override string ObtineDetalii()
        {
            return $"[Adaptat] {_furnizorExtern.GetDenumire()} - Preț: {_furnizorExtern.GetPretNet()} MDL ({_furnizorExtern.InformatiiSuplimentare()})";
        }

        public override Produs Cloneaza()
        {
            // Pentru simplitate, returnăm o nouă instanță creată la fel
            return new ProdusAdapter(_furnizorExtern);
        }
    }
}

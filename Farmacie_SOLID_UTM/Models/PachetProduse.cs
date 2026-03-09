using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacie_SOLID_UTM.Models
{
    // Composite - Un produs care conține alte produse
    public class PachetProduse : Produs
    {
        private List<Produs> _produse = new List<Produs>();

        // Constructorul folosește prețul 0 inițial, îl vom calcula dinamic
        public PachetProduse(string numePachet) : base(numePachet, 0)
        {
        }

        // Suprascriem proprietatea Pret pentru a calcula suma tuturor produselor din pachet
        public new decimal Pret 
        {
            get { return _produse.Sum(p => p.Pret); }
        }

        public void AdaugaInPachet(Produs produs)
        {
            _produse.Add(produs);
        }

        public void ScoateDinPachet(Produs produs)
        {
            _produse.Remove(produs);
        }

        public override string ObtineDetalii()
        {
            string detalii = $"[Pachet] {Nume} - Preț Total: {Pret} MDL\nComponente:\n";
            foreach (var produs in _produse)
            {
                detalii += $"  - {produs.ObtineDetalii()}\n";
            }
            return detalii.TrimEnd();
        }

        public override Produs Cloneaza()
        {
            var copiePachet = new PachetProduse(this.Nume);
            foreach (var produs in _produse)
            {
                // Clonăm și conținutul (Deep Copy)
                copiePachet.AdaugaInPachet(produs.Cloneaza());
            }
            return copiePachet;
        }
    }
}

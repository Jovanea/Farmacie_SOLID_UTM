using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacie_SOLID_UTM.Models
{
    public abstract class Produs
    {
        // Încapsulare: Proprietăți protejate (set) și publice (get)
        public string Nume { get; protected set; }
        public decimal Pret { get; protected set; }

        public Produs(string nume, decimal pret)
        {
            Nume = nume;
            Pret = pret;
        }

        // Polimorfism: Metodă ce va fi implementată diferit de subclase
        public abstract string ObtineDetalii();
    }
}

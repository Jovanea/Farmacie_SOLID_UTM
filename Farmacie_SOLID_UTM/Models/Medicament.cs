using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacie_SOLID_UTM.Models
{
    // Moștenire (Inheritance)
    public class Medicament : Produs
    {
        public string Producator { get; private set; }

        public Medicament(string nume, decimal pret, string producator)
            : base(nume, pret)
        {
            Producator = producator;
        }

        public override string ObtineDetalii()
        {
            return $"{Nume} ({Producator}) - {Pret} MDL";
        }
    }
}

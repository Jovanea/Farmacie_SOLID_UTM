using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacie_SOLID_UTM.Models
{
    // Moștenire: EchipamentMedical este tot un Produs
    // LSP (Liskov Substitution Principle): Obiectele de tip EchipamentMedical pot înlocui obiectele de tip Produs fără erori.
    public class EchipamentMedical : Produs
    {
        public string TipEchipament { get; private set; }

        public EchipamentMedical(string nume, decimal pret, string tip) : base(nume, pret)
        {
            TipEchipament = tip;
        }

        public override string ObtineDetalii() => $"{Nume} (Echipament: {TipEchipament}) - {Pret} MDL";
    }
}

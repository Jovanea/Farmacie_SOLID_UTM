using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacie_SOLID_UTM.Models
{
    // Moștenire (Inheritance): Medicament extinde funcționalitatea clasei de bază Produs
    // LSP (Liskov Substitution Principle): Această clasă poate fi folosită oriunde este așteptat un 'Produs'
    public class Medicament : Produs
    {
        public string Producator { get; private set; }

        public Medicament(string nume, decimal pret, string producator)
            : base(nume, pret)
        {
            Producator = producator;
        }

        // Polimorfism: Implementăm comportamentul specific pentru Medicament
        public override string ObtineDetalii()
        {
            return $"{Nume} ({Producator}) - {Pret} MDL";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Models
{
    // Produsul Complex pentru Builder Pattern
    public class TrusaMedicala
    {
        private List<Produs> _continut = new List<Produs>();
        public string Nume { get; set; }

        public void AdaugaProdus(Produs p)
        {
            _continut.Add(p);
        }

        public string ListeazaContinut()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- {Nume} ---");
            foreach (var p in _continut)
            {
                sb.AppendLine($"- {p.Nume} ({p.Pret} MDL)");
            }
            return sb.ToString();
        }

        public decimal CalculeazaPretTotal()
        {
            decimal total = 0;
            foreach (var p in _continut)
            {
                total += p.Pret;
            }
            return total;
        }
    }
}

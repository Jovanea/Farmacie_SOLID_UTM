using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie_SOLID_UTM.Interfaces;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Services
{
    // SRP(Single Responsibility Principle): Această clasă are o singură responsabilitate - "salvarea" (afișarea) datelor
    public class ConsolaStorage : IStocare
    {
        public void Salveaza(Produs p)
        {
            // Simulează o salvare.
            Console.WriteLine($"[LOG]: Produsul {p.Nume} a fost procesat.");
        }
    }
}

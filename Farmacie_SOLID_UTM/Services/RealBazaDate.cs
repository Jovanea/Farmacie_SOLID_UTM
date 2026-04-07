using System;
using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Subiectul Real - serviciul de date greu/important
    public class RealBazaDate : IAccesBazaDate
    {
        public void StergeProdus(string numeProdus)
        {
            // Logica simulată de ștergere definitivă
            Console.WriteLine($"[DB_DELETE_EXECUTAT] Produsul '{numeProdus}' a fost șters definitiv din baza de date.");
        }
    }
}

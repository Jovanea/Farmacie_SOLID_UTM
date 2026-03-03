using System;
using System.Collections.Generic;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Services
{
    // Singleton Pattern: Asigura o singura instanta a stocului in toata aplicatia.
    public class StocManager
    {
        private static StocManager _instance;
        private static readonly object _lock = new object();

        private List<Produs> _produse;

        // Private Constructor: Previne instantierea din afara
        private StocManager()
        {
            _produse = new List<Produs>();
            Console.WriteLine("StocManager Initializat (Singleton)");
        }

        public static StocManager Instance
        {
            get
            {
                // Thread-Safe Singleton (Double-Check Locking)
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new StocManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AdaugaProdus(Produs p)
        {
            _produse.Add(p);
            Console.WriteLine($"[StocManager] Adaugat: {p.Nume}");
        }

        public List<Produs> GetProduse()
        {
            return _produse;
        }

        public int GetTotalProduse()
        {
            return _produse.Count;
        }
    }
}

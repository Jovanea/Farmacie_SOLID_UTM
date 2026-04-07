using System;
using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Intermediarul (Proxy) pentru controlul accesului (Protection Proxy)
    public class ProxyBazaDate : IAccesBazaDate
    {
        private RealBazaDate _bazaDateReala;
        private string _rolUtilizatorCurent;

        public ProxyBazaDate(string rolUtilizator)
        {
            _rolUtilizatorCurent = rolUtilizator;
        }

        public void StergeProdus(string numeProdus)
        {
            // Verificăm permisiunile înainte de a instanția/delega sarcina subiectului real
            if (_rolUtilizatorCurent == "Manager")
            {
                if (_bazaDateReala == null)
                {
                    _bazaDateReala = new RealBazaDate();
                }
                
                Console.WriteLine("[PROXY_VALIDARE] Acces de Manager detectat. Comanda permisă.");
                _bazaDateReala.StergeProdus(numeProdus);
            }
            else
            {
                Console.WriteLine($"[PROXY_EROARE] Acces interzis! Rolul '{_rolUtilizatorCurent}' nu are dreptul de a șterge din baza de date.");
            }
        }
    }
}

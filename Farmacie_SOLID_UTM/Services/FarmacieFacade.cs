using System;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Services
{
    // Fațada (Facade) - Simplifică interacțiunea cu sistemele complexe de la spate
    public class FarmacieFacade
    {
        private StocManager _stocManager;
        private SistemPlata _plati;
        private SistemFacturare _facturare;

        // Constructorul inițializează subsistemele
        public FarmacieFacade()
        {
            _stocManager = StocManager.Instance;
            _plati = new SistemPlata();
            _facturare = new SistemFacturare();
        }

        // Metoda simplă oferită clientului (Form1 / Utlizator)
        public string VindeProdusCatreClient(Produs produs)
        {
            Console.WriteLine($"\n--- Începe procesul de vânzare pentru: {produs.Nume} ---");

            // 1. Verificăm dacă există în stoc (simplificat, presupunem că îl adăugăm ca vândut)
            _stocManager.AdaugaProdus(produs);
            Console.WriteLine("[Stoc] Produs înregistrat pentru ieșire.");

            // 2. Procesăm plata
            bool plataReusita = _plati.ProceseazaPlata(produs.Pret);

            // 3. Dacă plata a reușit, emitem bonul
            if (plataReusita)
            {
                _facturare.EmiteBon(produs.Nume, produs.Pret);
                return $"Succes! {produs.Nume} a fost vândut cu succes pe prețul de {produs.Pret} MDL.";
            }

            return "Eroare: Plata nu a putut fi procesată.";
        }
    }
}

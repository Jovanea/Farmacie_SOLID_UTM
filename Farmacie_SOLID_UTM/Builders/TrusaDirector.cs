using System;
using System.Collections.Generic;
using System.Text;
using Farmacie_SOLID_UTM.Models;
using Farmacie_SOLID_UTM.Builders;

namespace Farmacie_SOLID_UTM.Director
{
    // Director Pattern: Definește pașii de construcție pentru diferite tipuri de truse
    // Clasa Director lucrează cu Builder-ul pentru a produce obiecte complexe după rețete specifice.
    public class TrusaDirector
    {
        private TrusaBuilder _builder;

        // Constructor Injection: Directorul primește Builder-ul cu care va lucra
        public TrusaDirector(TrusaBuilder builder)
        {
            _builder = builder;
        }

        // Metoda 1: Construiește o Trusă de Vacanță (Exemplu complex)
        public TrusaMedicala ConstructTrusaVacanta()
        {
            return _builder.StartTrusa("Trusa Vacanță Completă")
                           .AdaugaMedicament("Ibuprofen", 20.0m, "BioFarm")
                           .AdaugaMedicament("Paracetamol", 10.0m, "Terapia")
                           .AdaugaBandaj("Plasturi Rezistenți la apă", 15.0m)
                           .AdaugaBandaj("Dezinfectant Gel", 12.5m)
                           .Build();
        }

        // Metoda 2: Construiește o Trusă Auto (Exemplu minimal)
        public TrusaMedicala ConstructTrusaAuto()
        {
            return _builder.StartTrusa("Trusa Auto Obligatorie")
                           .AdaugaBandaj("Fașă Sterilă", 5.0m)
                           .AdaugaBandaj("Comprese", 8.0m)
                           .AdaugaMedicament("Spirt Sanitar", 9.0m, "Mona")
                           .Build();
        }
    }
}

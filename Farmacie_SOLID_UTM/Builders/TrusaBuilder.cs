using System;
using System.Collections.Generic;
using System.Text;
using Farmacie_SOLID_UTM.Models;
using Farmacie_SOLID_UTM.Factories;

namespace Farmacie_SOLID_UTM.Builders
{
    // Builder Pattern: Separă construcția unui obiect complex de reprezentarea sa
    public class TrusaBuilder
    {
        private TrusaMedicala _trusa;
        private MedicamentFactory _medFactory = new MedicamentFactory();
        private EchipamentFactory _echipFactory = new EchipamentFactory();

        public TrusaBuilder StartTrusa(string nume)
        {
            _trusa = new TrusaMedicala();
            _trusa.Nume = nume;
            return this;
        }

        public TrusaBuilder AdaugaMedicament(string nume, decimal pret, string producator)
        {
            var med = _medFactory.CreazaProdus(nume, pret, producator);
            _trusa.AdaugaProdus(med);
            return this; // Fluent Interface
        }

        public TrusaBuilder AdaugaBandaj(string nume, decimal pret)
        {
            // Aici simplificam putin, folosim EchipamentFactory care face Bandaj/Echipament
            var bandaj = _echipFactory.CreazaProdus(nume, pret, "Bandaj General"); 
            _trusa.AdaugaProdus(bandaj);
            return this;
        }

        public TrusaMedicala Build()
        {
            return _trusa;
        }
    }
}

using System;

namespace Farmacie_SOLID_UTM.Models
{
    // Obiectul Flyweight care conține starea intrinsecă (partajată)
    // Va împărți "Descrierea lungă" și "Nume Categorie" pentru zeci de mii de produse similare
    public class CategorieFlyweight
    {
        public string NumeCategorie { get; private set; }
        public string DescriereStandard { get; private set; }

        public CategorieFlyweight(string numeCategorie, string descriereStandard)
        {
            NumeCategorie = numeCategorie;
            DescriereStandard = descriereStandard;
        }

        public void AfiseazaDetalii()
        {
            Console.WriteLine($"[Flyweight] Categorie: {NumeCategorie} | Descriere: {DescriereStandard}");
        }
    }
}

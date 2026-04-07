using System;

namespace Farmacie_SOLID_UTM.Models
{
    // Contextul: clasa care reține o resursă partajată (Flyweight) și propria stare extrinsecă (unică)
    public class ProdusComercial : Produs
    {
        public string CodDeBare { get; private set; }
        
        // Referință către starea partajată (toate medicamentele de cap folosesc exact același obiect Categorie)
        public CategorieFlyweight Categorie { get; private set; }

        public ProdusComercial(string nume, decimal pret, string codDeBare, CategorieFlyweight categorie) 
            : base(nume, pret)
        {
            CodDeBare = codDeBare;
            Categorie = categorie;
        }

        public override string ObtineDetalii()
        {
            return $"Produs: {Nume} | Preț: {Pret} MDL | Cod: {CodDeBare} \n  -> Categorie: {Categorie.NumeCategorie} (Memorie partajată)";
        }

        public override Produs Cloneaza()
        {
            // Face o copie a obiectului curent (Shallow Copy)
            return (Produs)this.MemberwiseClone();
        }
    }
}

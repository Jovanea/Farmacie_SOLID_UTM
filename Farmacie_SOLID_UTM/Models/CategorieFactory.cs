using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Models
{
    // Factory care construiește și gestionează memoria obiectelor Flyweight
    public class CategorieFactory
    {
        private Dictionary<string, CategorieFlyweight> _categorii = new Dictionary<string, CategorieFlyweight>();

        public CategorieFlyweight GetCategorie(string numeCategorie, string descriereStandard)
        {
            // Dacă categoria a fost deja creată, o refolosim instant din memorie
            if (!_categorii.ContainsKey(numeCategorie))
            {
                _categorii[numeCategorie] = new CategorieFlyweight(numeCategorie, descriereStandard);
            }
            
            return _categorii[numeCategorie];
        }

        // Metodă ajutătoare pentru a demonstra câte obiecte sunt cu adevărat în memorie
        public int NumarCategoriiCreate => _categorii.Count;
    }
}

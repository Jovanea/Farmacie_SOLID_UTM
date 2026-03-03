namespace Farmacie_SOLID_UTM.Models
{
    // Abstract Product B
    public abstract class Bandaj : EchipamentMedical
    {
        public Bandaj(string nume, decimal pret, string tip) 
            : base(nume, pret, tip)
        {
        }
    }
}

namespace Farmacie_SOLID_UTM.Models
{
    // Abstract Product A
    public abstract class MedicamentDurere : Medicament
    {
        public MedicamentDurere(string nume, decimal pret, string producator) 
            : base(nume, pret, producator)
        {
        }
    }
}

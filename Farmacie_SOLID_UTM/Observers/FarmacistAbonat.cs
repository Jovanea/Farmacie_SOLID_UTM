using System;

namespace Farmacie_SOLID_UTM.Observers
{
    // Concrete Subscriber 2
    public class FarmacistAbonat : ISubscriber
    {
        private string _nume;

        public FarmacistAbonat(string nume)
        {
            _nume = nume;
        }

        // + update(context)
        public void Update(ProdusPublisher context)
        {
            Console.WriteLine($"[Farmacist {_nume}]: Atentie! Trebuie sa redirectionezi clientii pentru produsul {context.NumeProdus}. Stoc ramas: {context.GetStoc()}");
        }
    }
}

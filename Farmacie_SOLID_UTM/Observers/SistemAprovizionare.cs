using System;

namespace Farmacie_SOLID_UTM.Observers
{
    // Concrete Subscriber 1
    public class SistemAprovizionare : ISubscriber
    {
        // + update(context)
        public void Update(ProdusPublisher context)
        {
            Console.WriteLine($"[Sistem Aprovizionare]: Produsul {context.NumeProdus} are stoc critic! (Ramas: {context.GetStoc()} buc). Lansare comanda la furnizori...");
        }
    }
}

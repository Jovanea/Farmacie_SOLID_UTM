using System;

namespace Farmacie_SOLID_UTM.States
{
    // State Pattern: Un obiect își schimbă comportamentul când starea sa internă se schimbă.

    // Contextul (Comanda propriu-zisă)
    public class ComandaAprovizionare                     ////
    {
        private StareComanda _stareCurenta;

        public ComandaAprovizionare(StareComanda stareInitiala)
        {
            Console.WriteLine("[State] O nouă comandă a fost creată.");
            TransitionTo(stareInitiala);
        }

        public void TransitionTo(StareComanda stare)
        {
            _stareCurenta = stare;
            _stareCurenta.SetContext(this);
        }

        // Delegăm cererile către starea curentă
        public void Proceseaza() => _stareCurenta.Proceseaza();
        public void Anuleaza() => _stareCurenta.Anuleaza();
        public void Livreaza() => _stareCurenta.Livreaza();
    }

    // Clasa abstractă de Stare (cuprinde și setarea contextului conform indicației din poză)
    public abstract class StareComanda                          ////
    {
        protected ComandaAprovizionare _context;

        public void SetContext(ComandaAprovizionare context)
        {
            _context = context;
        }

        public abstract void Proceseaza();
        public abstract void Anuleaza();
        public abstract void Livreaza();
    }

    // Stare 1: Nouă
    public class StareNoua : StareComanda              ////
    {
        public override void Proceseaza()
        {
            Console.WriteLine("[State] Trecem comanda din 'Nouă' în 'În Procesare'.");
            _context.TransitionTo(new StareInProcesare());
        }

        public override void Anuleaza()
        {
            Console.WriteLine("[State] Comanda 'Nouă' a fost anulată rapid.");
        }

        public override void Livreaza()
        {
            Console.WriteLine("[State] EROARE: Nu poți livra o comandă care nu a fost procesată!");
        }
    }

    // Stare 2: În Procesare
    public class StareInProcesare : StareComanda                 ////
    {
        public override void Proceseaza()
        {
            Console.WriteLine("[State] Comanda se află deja în procesare.");
        }

        public override void Anuleaza()
        {
            Console.WriteLine("[State] Comanda 'În Procesare' a fost anulată cu costuri adiționale.");
        }

        public override void Livreaza()
        {
            Console.WriteLine("[State] Trecem comanda din 'În Procesare' în 'Livrată'.");
            _context.TransitionTo(new StareLivrata());
        }
    }

    // Stare 3: Livrată
    public class StareLivrata : StareComanda                    ////
    {
        public override void Proceseaza()
        {
            Console.WriteLine("[State] Comanda a fost deja livrată, nu se mai procesează.");
        }

        public override void Anuleaza()
        {
            Console.WriteLine("[State] EROARE: Nu poți anula o comandă care a fost deja livrată!");
        }

        public override void Livreaza()
        {
            Console.WriteLine("[State] Comanda a fost deja livrată.");
        }
    }
}

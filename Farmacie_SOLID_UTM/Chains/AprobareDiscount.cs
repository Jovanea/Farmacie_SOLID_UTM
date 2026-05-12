using System;

namespace Farmacie_SOLID_UTM.Chains
{
    // Chain of Responsibility: O cerere (aprobare discount) trece printr-un lanț de handler-i
    // până când cineva are autoritatea să o rezolve.

    // Interfața de bază pentru Handler
    public interface IHandlerAprobare
    {
        IHandlerAprobare SetNext(IHandlerAprobare handler);
        void GestioneazaCererea(decimal procentDiscount);
    }

    // Clasa abstractă ajutătoare (opțional, dar recomandat)
    public abstract class BaseHandlerAprobare : IHandlerAprobare
    {
        private IHandlerAprobare _nextHandler;

        public IHandlerAprobare SetNext(IHandlerAprobare handler)
        {
            _nextHandler = handler;
            // Returnăm handlerul pasat pentru a permite legarea în lanț: h1.SetNext(h2).SetNext(h3)
            return handler;
        }

        public virtual void GestioneazaCererea(decimal procentDiscount)
        {
            if (_nextHandler != null)
            {
                _nextHandler.GestioneazaCererea(procentDiscount);
            }
            else
            {
                Console.WriteLine($"[Chain] Niciun nivel nu a putut aproba discountul de {procentDiscount}%.");
            }
        }
    }

    // Handler 1: Farmacistul (aprobă până la 5%)
    public class FarmacistHandler : BaseHandlerAprobare
    {
        public override void GestioneazaCererea(decimal procentDiscount)
        {
            if (procentDiscount <= 5)
            {
                Console.WriteLine($"[Chain] Farmacistul a aprobat discountul de {procentDiscount}%.");
            }
            else
            {
                Console.WriteLine($"[Chain] Farmacistul NU poate aproba {procentDiscount}%. Trimite cererea mai sus.");
                base.GestioneazaCererea(procentDiscount);
            }
        }
    }

    // Handler 2: Managerul (aprobă până la 15%)
    public class ManagerHandler : BaseHandlerAprobare
    {
        public override void GestioneazaCererea(decimal procentDiscount)
        {
            if (procentDiscount <= 15)
            {
                Console.WriteLine($"[Chain] Managerul a aprobat discountul de {procentDiscount}%.");
            }
            else
            {
                Console.WriteLine($"[Chain] Managerul NU poate aproba {procentDiscount}%. Trimite cererea mai sus.");
                base.GestioneazaCererea(procentDiscount);
            }
        }
    }

    // Handler 3: Directorul (aprobă orice peste 15%)
    public class DirectorHandler : BaseHandlerAprobare
    {
        public override void GestioneazaCererea(decimal procentDiscount)
        {
            Console.WriteLine($"[Chain] Directorul a aprobat discountul imens de {procentDiscount}%.");
        }
    }
}

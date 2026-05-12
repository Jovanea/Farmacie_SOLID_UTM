using System;

namespace Farmacie_SOLID_UTM.Mediators
{
    // Mediator Pattern: Centralizează comunicarea între obiecte, evitând cuplarea directă.

    // Interfața Mediator
    public interface IMediatorFarmacie
    {
        void TrimiteMesaj(ComponentaFarmacie expeditor, string eveniment);
    }

    // Mediatorul Concret (Turnul de Control)
    public class CentralaFarmacie : IMediatorFarmacie
    {
        private DepartamentVanzari _vanzari;
        private DepartamentDepozit _depozit;

        public void SeteazaVanzari(DepartamentVanzari v)
        {
            _vanzari = v;
            _vanzari.SetMediator(this);
        }

        public void SeteazaDepozit(DepartamentDepozit d)
        {
            _depozit = d;
            _depozit.SetMediator(this);
        }

        public void TrimiteMesaj(ComponentaFarmacie expeditor, string eveniment)
        {
            // Dacă Vânzările raportează o vânzare, Mediatorul anunță automat Depozitul
            if (expeditor == _vanzari && eveniment == "VanzareNoua")
            {
                Console.WriteLine("[Mediator] Centrala a prins vânzarea. Alertează depozitul pentru scădere stoc...");
                _depozit.ScadeStoc();
            }
            // Dacă Depozitul raportează stoc gol, Mediatorul oprește Vânzările
            else if (expeditor == _depozit && eveniment == "StocZero")
            {
                Console.WriteLine("[Mediator] Centrala a prins alerta de stoc zero. Blochează casa de marcat...");
                _vanzari.BlocheazaVanzarile();
            }
        }
    }

    // Componenta de bază
    public abstract class ComponentaFarmacie
    {
        protected IMediatorFarmacie _mediator;

        public void SetMediator(IMediatorFarmacie mediator)
        {
            _mediator = mediator;
        }
    }

    // Componenta 1: Departamentul de Vânzări
    public class DepartamentVanzari : ComponentaFarmacie
    {
        public void EfectueazaVanzare()
        {
            Console.WriteLine("[Vânzări] S-a vândut un produs!");
            _mediator.TrimiteMesaj(this, "VanzareNoua");
        }

        public void BlocheazaVanzarile()
        {
            Console.WriteLine("[Vânzări] Casa de marcat a fost blocată automat.");
        }
    }

    // Componenta 2: Departamentul de Depozit
    public class DepartamentDepozit : ComponentaFarmacie
    {
        public void ScadeStoc()
        {
            Console.WriteLine("[Depozit] Stocul a fost actualizat (scăzut).");
            // Simulăm că s-a terminat stocul
            SemnaleazaStocZero();
        }

        public void SemnaleazaStocZero()
        {
            Console.WriteLine("[Depozit] ALARMĂ: Stocul a ajuns la zero!");
            _mediator.TrimiteMesaj(this, "StocZero");
        }
    }
}

using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Observers
{
    // Publisher
    public class ProdusPublisher
    {
        // - subscribers: Subscriber[]
        private List<ISubscriber> _subscribers = new List<ISubscriber>();
        
        // - mainState
        private int _stocPrincipal;
        public string NumeProdus { get; private set; }

        public ProdusPublisher(string nume, int stocInitial)
        {
            NumeProdus = nume;
            _stocPrincipal = stocInitial;
        }

        public int GetStoc()
        {
            return _stocPrincipal;
        }

        // + subscribe(s: Subscriber)
        public void Subscribe(ISubscriber s)
        {
            if (!_subscribers.Contains(s))
            {
                _subscribers.Add(s);
            }
        }

        // + unsubscribe(s: Subscriber)
        public void Unsubscribe(ISubscriber s)
        {
            _subscribers.Remove(s);
        }

        // + notifySubscribers()
        public void NotifySubscribers()
        {
            foreach (var s in _subscribers)
            {
                // Păstrăm referența trecând "(this)" conform diagramei
                s.Update(this);
            }
        }

        // + mainBusinessLogic()
        public void ModificaStoc(int cantitateNoua)
        {
            _stocPrincipal = cantitateNoua;
            
            // Logica: Dacă stocul e critic (ex: sub 5 buc), notificam.
            if (_stocPrincipal < 5)
            {
                NotifySubscribers();
            }
        }
    }
}

using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Mementos
{
    // Se definesc pe acelasi fisier pt simplitate de legatura academica
    
    // Originator 
    public class CosOriginator                                                ///
    {
        // - state
        private List<string> _state = new List<string>();

        public void AdaugaProdus(string nume)
        {
            _state.Add(nume);
        }

        public void Afisare()
        {
            System.Console.WriteLine("Cos curent: " + string.Join(", ", _state));
        }

        public string AfiseazaContinut()
        {
            return string.Join("\n", _state);
        }

        // + save(): Memento
        public IMemento Save()
        {
            return new CosMemento(new List<string>(_state));
        }

        // + restore(m: Memento)
        public void Restore(IMemento m)
        {
            if (m is CosMemento memento)
            {
                this._state = memento.GetState();
            }
        }

        // Memento Interface (Ascunde starea)
        public interface IMemento { }                                          ///

        // Memento
        // Conform imaginii "Implementation based on nested classes"
        private class CosMemento : IMemento                                    ///
        {
            // - state
            private List<string> _state;

            // - Memento(state)
            public CosMemento(List<string> state)
            {
                this._state = state;
            }

            // - getState()
            public List<string> GetState()
            {
                return _state;
            }
        }
    }

    // Caretaker
    public class IstoricCosCaretaker                                                      ///
    {
        // - originator
        private CosOriginator _originator;
        
        // - history: Memento[]
        private Stack<CosOriginator.IMemento> _history = new Stack<CosOriginator.IMemento>();

        public IstoricCosCaretaker(CosOriginator originator)
        {
            _originator = originator;
        }

        // + doSomething() (aici se numeste SaveState)
        public void SalveazaStarea()
        {
            _history.Push(_originator.Save());
        }

        // + undo()
        public void Undo()
        {
            if (_history.Count > 0)
            {
                var m = _history.Pop();
                _originator.Restore(m);
                System.Console.WriteLine("[Undo efectuat la istoric cos]");
            }
        }
    }
}

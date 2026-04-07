using System;
using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Commands
{
    // Invoker
    public class CasaDeMarcat
    {
        // - command
        private ICommand _comandaCurenta;
        private Stack<ICommand> _istoric = new Stack<ICommand>();

        // + setCommand(command)
        public void SetCommand(ICommand c)
        {
            _comandaCurenta = c;
        }

        // + executeCommand()
        public void ExecuteCommand()
        {
            if (_comandaCurenta != null)
            {
                _comandaCurenta.Execute();
                _istoric.Push(_comandaCurenta);
            }
        }

        // Metoda auxiliara pentru testare
        public void UndoUltimaComanda()
        {
            if (_istoric.Count > 0)
            {
                var cmd = _istoric.Pop();
                cmd.Undo();
            }
        }
    }
}

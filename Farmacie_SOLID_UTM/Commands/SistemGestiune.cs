using System;

namespace Farmacie_SOLID_UTM.Commands
{
    // Receiver
    public class SistemGestiune
    {
        // operation(params)
        public void ReduStoc(string produs, int cantitate)
        {
            Console.WriteLine($"[Sistem Gestiune (Receiver)]: Elimin in stoc {cantitate} buc. de {produs}.");
        }

        // operation(params) pentru Undo
        public void AdaugaStoc(string produs, int cantitate)
        {
            Console.WriteLine($"[Sistem Gestiune (Receiver)]: Readuc pe stoc {cantitate} buc. de {produs}.");
        }
    }
}

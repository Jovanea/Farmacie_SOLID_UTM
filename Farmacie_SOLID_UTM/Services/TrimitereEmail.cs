using System;
using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Implementarea concretă 1 (Email)
    public class TrimitereEmail : IPlatformaTrimitere
    {
        public void Trimite(string mesaj)
        {
            Console.WriteLine($"[Trimitere EMAIL] ---> {mesaj}");
        }
    }
}

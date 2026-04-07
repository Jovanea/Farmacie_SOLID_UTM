using System;
using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Implementarea concretă 2 (SMS)
    public class TrimitereSms : IPlatformaTrimitere
    {
        public void Trimite(string mesaj)
        {
            Console.WriteLine($"[Telefon SMS trimis] ---> {mesaj}");
        }
    }
}

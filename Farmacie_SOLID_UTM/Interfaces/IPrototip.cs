using System;
using System.Collections.Generic;
using System.Text;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Interfaces
{
    // Prototype Pattern: Interfața pentru clonarea obiectelor
    public interface IPrototip
    {
        Produs Cloneaza();
    }
}

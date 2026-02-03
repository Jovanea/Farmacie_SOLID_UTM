using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM.Interfaces
{
    // ISP (Interface Segregation Principle): Interfața conține doar metodele strict necesare,
    // evitând obligarea claselor de a implementa funcționalități inutile.
    public interface IStocare
    {
        void Salveaza(Produs p);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Farmacie_SOLID_UTM.Interfaces;
using Farmacie_SOLID_UTM.Services;

namespace Farmacie_SOLID_UTM
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // DIP: Aici în "Composition Root" decidem ce implementare concretă să folosim.
            // Putem schimba ConsolaStorage cu orice altă clasă care implementează IStocare fără a afecta restul aplicației.
            // Mai târziu aici pot pune SQLStorage în loc de ConsolaStorage
            IStocare serviciuStocare = new ConsolaStorage();

            // Pornim aplicația pasând serviciul necesar
            Application.Run(new Form1(serviciuStocare));
        }
    }
}

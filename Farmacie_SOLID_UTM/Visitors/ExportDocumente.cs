using System;
using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Visitors
{
    // Visitor Pattern: Separă un algoritm de structura de obiecte pe care operează.

    // Interfața Visitor
    public interface IVisitorExport                  ////
    {
        void Visit(RetetaCompensata reteta);
        void Visit(FacturaFirma factura);
    }

    // Interfața Element
    public interface IDocumentFarmacie               ////
    {
        void Accept(IVisitorExport visitor);
    }

    // Element Concret 1
    public class RetetaCompensata : IDocumentFarmacie      ////// ---->
    {
        public string NumePacient { get; set; } = "Ion Popescu";
        public string Diagnostic { get; set; } = "Gripa";

        public void Accept(IVisitorExport visitor)
        {
            // Obligatoriu pentru patternul Visitor (Double Dispatch)
            visitor.Visit(this);
        }
    }

    // Element Concret 2
    public class FacturaFirma : IDocumentFarmacie          ////
    {
        public string NumeFirma { get; set; } = "Farmacia Centrala SRL";
        public decimal TotalDePlata { get; set; } = 15000m;

        public void Accept(IVisitorExport visitor)
        {
            visitor.Visit(this);
        }
    }

    // Concrete Visitor: Extrage datele in XML
    public class ExportXmlVisitor : IVisitorExport           ///// 
    {
        public void Visit(RetetaCompensata reteta)
        {
            Console.WriteLine("[Visitor] Exportă Rețeta în XML:");
            Console.WriteLine($"    <Reteta><Pacient>{reteta.NumePacient}</Pacient><Diagnostic>{reteta.Diagnostic}</Diagnostic></Reteta>");
        }

        public void Visit(FacturaFirma factura)
        {
            Console.WriteLine("[Visitor] Exportă Factura în XML:");
            Console.WriteLine($"    <Factura><Firma>{factura.NumeFirma}</Firma><Total>{factura.TotalDePlata}</Total></Factura>");
        }
    }
}

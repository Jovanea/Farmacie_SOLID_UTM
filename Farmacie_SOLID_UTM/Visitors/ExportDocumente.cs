using System;
using System.IO;
using System.Text;

namespace Farmacie_SOLID_UTM.Visitors
{
    // Visitor Pattern: Separă un algoritm de structura de obiecte pe care operează.

    public interface IVisitorExport
    {
        void Visit(RetetaCompensata reteta);
        void Visit(FacturaFirma factura);
    }

    public interface IDocumentFarmacie
    {
        void Accept(IVisitorExport visitor);
    }

    public class RetetaCompensata : IDocumentFarmacie
    {
        public string NumePacient { get; set; } = "Ion Popescu";
        public string Diagnostic  { get; set; } = "Gripa Sezoniera";

        public void Accept(IVisitorExport visitor)
        {
            visitor.Visit(this);   // Double Dispatch
        }
    }

    public class FacturaFirma : IDocumentFarmacie
    {
        public string  NumeFirma    { get; set; } = "DepozitFarm SRL";
        public decimal TotalDePlata { get; set; } = 14500m;

        public void Accept(IVisitorExport visitor)
        {
            visitor.Visit(this);
        }
    }

    // Visitor concret: exportă în XML și salvează fișierul
    public class ExportXmlVisitor : IVisitorExport
    {
        private readonly StringBuilder _xml = new StringBuilder();

        public ExportXmlVisitor()
        {
            _xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            _xml.AppendLine("<ExportCNAS data=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\">");
        }

        public void Visit(RetetaCompensata reteta)
        {
            _xml.AppendLine("  <Reteta>");
            _xml.AppendLine("    <Pacient>" + reteta.NumePacient + "</Pacient>");
            _xml.AppendLine("    <Diagnostic>" + reteta.Diagnostic + "</Diagnostic>");
            _xml.AppendLine("    <DataEliberare>" + DateTime.Now.ToString("dd/MM/yyyy") + "</DataEliberare>");
            _xml.AppendLine("  </Reteta>");
        }

        public void Visit(FacturaFirma factura)
        {
            _xml.AppendLine("  <Factura>");
            _xml.AppendLine("    <Firma>" + factura.NumeFirma + "</Firma>");
            _xml.AppendLine("    <Total currency=\"MDL\">" + factura.TotalDePlata + "</Total>");
            _xml.AppendLine("    <DataEmitere>" + DateTime.Now.ToString("dd/MM/yyyy") + "</DataEmitere>");
            _xml.AppendLine("  </Factura>");
        }

        // Salvează XML-ul pe Desktop și returnează calea + conținutul
        public (string cale, string xml) Salveaza()
        {
            _xml.AppendLine("</ExportCNAS>");
            string continut = _xml.ToString();
            string cale = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "ExportCNAS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml");
            File.WriteAllText(cale, continut, Encoding.UTF8);
            return (cale, continut);
        }
    }
}

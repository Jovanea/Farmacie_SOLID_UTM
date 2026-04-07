using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Abstractizarea din Bridge
    // Separăm ideea de a trimite o notificare, de CUM este trimisă fizic
    public class Notificator
    {
        protected IPlatformaTrimitere _platforma;

        // Podul propriu-zis (Bridge) este creat prin Dependency Injection în constructor
        public Notificator(IPlatformaTrimitere platforma)
        {
            _platforma = platforma;
        }

        public virtual void ExpediazaAlerta(string text)
        {
            // Apelăm metoda de pe implementare fără a ști/depinde de ea (SMS sau Mail)
            _platforma.Trimite(text);
        }
    }
}

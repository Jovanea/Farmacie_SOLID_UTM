using Farmacie_SOLID_UTM.Interfaces;

namespace Farmacie_SOLID_UTM.Services
{
    // Abstractizare extinsă (Refined Abstraction)
    // Putem extinde complet independent partea de notificare, fără a atinge partea de SMS/Email
    public class NotificatorUrgent : Notificator
    {
        public NotificatorUrgent(IPlatformaTrimitere platforma) : base(platforma)
        {
        }

        public override void ExpediazaAlerta(string text)
        {
            // Transformă automat mesajul într-unul strident (urgent)
            text = "[! ALERTĂ URGENTĂ OP. STOC !] " + text.ToUpper();
            base.ExpediazaAlerta(text);
        }
    }
}

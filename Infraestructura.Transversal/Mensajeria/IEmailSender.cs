using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Mensajeria
{
    public interface IEmailSender
    {
        void SendEmail(MailMessage message);
    }
}

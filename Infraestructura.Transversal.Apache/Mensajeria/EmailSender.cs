using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using TrsvMjs = Sipecom.FactElec.Pymes.Infraestructura.Transversal.Mensajeria;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Mensajeria
{
    public class EmailSender : TrsvMjs.IEmailSender
    {
        #region Campos

        private readonly IEmailConfiguration m_configuration;

        #endregion

        #region Constructores

        public EmailSender(IEmailConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;

            m_configuration.Initialize();
        }

        #endregion

        #region Metodos privados

        private MailMessage CrearMensaje(TrsvMjs.MailMessage message)
        {
            var retorno = new MailMessage();

            retorno.From = new MailAddress(message.FromAddress, message.FromName);

            if (message.ToAddress != null)
            {
                foreach (var address in message.ToAddress.Where(toValue => !string.IsNullOrWhiteSpace(toValue)))
                {
                    retorno.To.Add(address.Trim());
                }
            }

            if (!String.IsNullOrEmpty(message.ReplyTo))
            {
                retorno.ReplyToList.Add(new MailAddress(message.ReplyTo, message.ReplyToName));
            }

            if (message.Cc != null)
            {
                foreach (var address in message.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    retorno.CC.Add(address.Trim());
                }
            }

            if (message.Bcc != null)
            {
                foreach (var address in message.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    retorno.Bcc.Add(address.Trim());
                }
            }

            retorno.Subject = message.Subject;
            retorno.Body = message.Body;
            retorno.IsBodyHtml = true;

            if (
                !string.IsNullOrEmpty(message.AttachmentFilePath)
                && File.Exists(message.AttachmentFilePath))
            {
                var attachment = new Attachment(message.AttachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(message.AttachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(message.AttachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(message.AttachmentFilePath);

                if (!string.IsNullOrEmpty(message.AttachmentFileName))
                {
                    attachment.Name = message.AttachmentFileName;
                }

                retorno.Attachments.Add(attachment);
            }

            return retorno;
        }

        private SmtpClient CrearCliente()
        {
            var cliente = new SmtpClient();

            cliente.Host = m_configuration.Host;
            cliente.Port = m_configuration.Port;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = m_configuration.UseDefaultCredentials;

            if (!m_configuration.UseDefaultCredentials)
            {
                cliente.Credentials = new NetworkCredential(m_configuration.UserName, m_configuration.Password);
            }

            cliente.EnableSsl = m_configuration.EnableSsl;
            cliente.Timeout = m_configuration.Timeout;

            return cliente;
        }

        #endregion

        #region IEmailSender

        public void SendEmail(TrsvMjs.MailMessage message)
        {
            using (var mensaje = CrearMensaje(message))
            {
                using (var client = CrearCliente())
                {
                    client.Send(mensaje);
                }
            }
        }

        #endregion
    }
}

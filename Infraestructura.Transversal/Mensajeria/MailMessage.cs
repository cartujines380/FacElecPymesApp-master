using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Mensajeria
{
    public class MailMessage
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string FromAddress { get; set; }

        public string FromName { get; set; }

        public IEnumerable<string> ToAddress { get; set; }

        public string ReplyTo { get; set; }

        public string ReplyToName { get; set; }

        public IEnumerable<string> Bcc { get; set; }

        public IEnumerable<string> Cc { get; set; }

        public string AttachmentFilePath { get; set; }

        public string AttachmentFileName { get; set; }
    }
}

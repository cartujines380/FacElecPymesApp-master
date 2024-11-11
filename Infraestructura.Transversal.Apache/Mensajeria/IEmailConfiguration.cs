using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Mensajeria
{
    public interface IEmailConfiguration
    {
        void Initialize();

        string Host { get; set; }

        int Port { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        bool UseDefaultCredentials { get; set; }

        bool EnableSsl { get; set; }

        int Timeout { get; set; }
    }
}

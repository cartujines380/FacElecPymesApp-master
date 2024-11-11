using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Mensajes
{
    public class ResetearClaveResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public string ClaveNueva { get; set; }
    }
}

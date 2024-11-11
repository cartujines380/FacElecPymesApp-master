using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class ConsRolTransaccionResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public RolTransaccionData RolResult { get; set; }
    }
}

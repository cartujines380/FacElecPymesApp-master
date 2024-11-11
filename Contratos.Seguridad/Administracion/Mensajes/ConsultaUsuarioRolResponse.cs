using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class ConsultaUsuarioRolResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public RolData RolResult { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class ConsultaUsuariosResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public IEnumerable<UsuarioConsultaData> Usuarios { get; set; }
    }
}

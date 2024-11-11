using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class RegistrarAplicacionResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public string UsuarioId { get; set; }

        public string SitioMaquina { get; set; }

        public string Maquina { get; set; }

        public string Token { get; set; }

        public string SitioToken { get; set; }

        //public string SitioUsuario { get; set; }

        public ICollection<ServidorData> Servidores { get; set; }
    }
}

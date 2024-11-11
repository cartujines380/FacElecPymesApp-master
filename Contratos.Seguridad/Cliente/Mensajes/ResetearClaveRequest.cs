using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Mensajes
{
    public class ResetearClaveRequest
    {
        public string UsuarioNombre { set; get; }

        public string UsuarioId { set; get; }

        public string Maquina { get; set; }

        public string Token { get; set; }
    }
}

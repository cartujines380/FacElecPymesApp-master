using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class RegistraUsuarioResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public ParticipanteData Participante { get; set; }

        public string Estado { get; set; }

        public string Token { get; set; }

        public string Maquina { get; set; }

        public ICollection<TransaccionData> Transacciones { get; set; }

        public ICollection<AplicacionData> Aplicaciones { get; set; }
    }
}

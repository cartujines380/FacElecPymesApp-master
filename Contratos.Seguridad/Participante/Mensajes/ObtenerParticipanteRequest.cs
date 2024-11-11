using System;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ObtenerParticipanteRequest
    {
        public int ParticipanteId { get; set; }

        public Sesion Sesion { get; set; }
    }
}

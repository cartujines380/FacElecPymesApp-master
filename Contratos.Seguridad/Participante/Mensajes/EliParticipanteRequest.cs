using System;
using System.Collections.Generic;
using System.Text;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class EliParticipanteRequest
    {
        public string ParticipanteId { get; set; }

        public Sesion Sesion { get; set; }
    }
}

using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ParticipanteData
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public IdentificacionData Identificacion { get; set; }

        public string TipoResgistro { get; set; }
    }
}

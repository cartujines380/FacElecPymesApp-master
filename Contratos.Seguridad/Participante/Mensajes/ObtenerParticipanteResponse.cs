using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ObtenerParticipanteResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public ParticipanteData Participante { get; set; }

        public PersonaData Persona { get; set; }

        public EmpresaData Empresa { get; set; }

        public ContactoData Contacto { get; set; }

        public string UsuarioId { get; set; }

        public string TipoLogin { get; set; }

        public string Estado { get; set; }
    }
}

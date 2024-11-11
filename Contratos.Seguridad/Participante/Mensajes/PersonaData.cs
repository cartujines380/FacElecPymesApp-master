using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class PersonaData
    {
        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Genero { get; set; }

        public string EstadoCivil { get; set; }

        public DateTime? FechaNacimiento { get; set; }
    }
}

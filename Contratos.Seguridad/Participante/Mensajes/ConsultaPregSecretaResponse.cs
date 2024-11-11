using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ConsultaPregSecretaResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public string PreguntaSecreta { get; set; } 

        public string UrlAD { get; set; }
    }
}

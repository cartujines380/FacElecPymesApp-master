using System;
using System.Collections.Generic;
using System.Text;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ConsultaRespSecretaRequest
    {
        public string UsuarioId;

        public string PreguntaSecreta;

        public string RespuestaSecreta;

        public Sesion Sesion;

        public string UrlAD;
    }
}

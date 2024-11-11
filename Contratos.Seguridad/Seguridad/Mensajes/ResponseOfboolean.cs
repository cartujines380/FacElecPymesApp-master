using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class ResponseOfboolean
    {
        public bool[] datosField { set; get; }

        public string datosJsonField { set; get; }

        public Error errorField { set; get; }

        public bool estadoField { set; get; }

        public bool estadoFieldSpecified { set; get; }

        public DateTime fechaHoraField { set; get; }

        public bool fechaHoraFieldSpecified { set; get; }

        public string mensajeField { set; get; }

        public string mensajeExtendidoField { set; get; }

        public EnumsGeneralesStatusMensaje tipoMensajeField { set; get; }

        public bool tipoMensajeFieldSpecified { set; get; }
    }
}

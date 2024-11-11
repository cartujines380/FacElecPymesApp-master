using System;

using Sipecom.FactElec.Pymes.Negocios.Base.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    public class CambioClaveResponse : Response
    {
        public bool Result { get; set; }

        public string Codigo { get; set; }

        public string Mensaje { get; set; }
    }
}

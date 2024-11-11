using System;

using Sipecom.FactElec.Pymes.Negocios.Base.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    public class ResetearClaveResponse : Response
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        //public string ClaveNueva { get; set; }
    }
}

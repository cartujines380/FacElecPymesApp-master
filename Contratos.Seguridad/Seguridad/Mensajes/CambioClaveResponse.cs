using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class CambioClaveResponse
    {
        public bool Result { get; set; }

        public string Codigo { get; set; }

        public string Mensaje { get; set; }
    }
}

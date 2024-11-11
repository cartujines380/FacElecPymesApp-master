using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class ServidorData
    {
        public string Id { get; set; }

        public string Tipo { get; set; }

        public IDictionary<string, string> Parametros { get; set; }
    }
}

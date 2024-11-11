using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class EncriptarResponse
    {
        public string Codigo { get; set; }

        public string Mensaje { get; set; }

        public string DatoEncriptado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class CambioClaveRequest
    {
        public string UsuarioId { set; get; }

        public string ClaveAntigua { set; get; }

        public string ClaveNueva { set; get; }

        public Sesion Sesion { set; get; }
    }
}

using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class RolTransaccionData
    {
        public string Nombre { get; set; }

        public string Estado { get; set; }

        public List<ElementoRolTransaccion> Datos { get; set; }

        public RolTransaccionData()
        {
            Datos = new List<ElementoRolTransaccion>();
        }
    }
}

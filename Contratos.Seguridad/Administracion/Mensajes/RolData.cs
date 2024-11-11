using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class RolData
    {
        public string Nombre { get; set; }

        public string Estado { get; set; }

        public List<ElementoRol> Datos { get; set; }

        public RolData()
        {
            Datos = new List<ElementoRol>();
        }
    }
}

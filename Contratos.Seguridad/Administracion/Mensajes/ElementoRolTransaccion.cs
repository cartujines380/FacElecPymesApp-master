using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class ElementoRolTransaccion
    {
        public int IdOrganizacion { get; set; }

        public string DescripcionOrganizacion { get; set; }

        public int IdTransaccion { get; set; }

        public string DescripcionTransaccion { get; set; }

        public int IdOpcion { get; set; }

        public string DescripcionOpcion { get; set; }
    }
}

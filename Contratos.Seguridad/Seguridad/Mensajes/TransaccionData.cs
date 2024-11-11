using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class TransaccionData
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public OrganizacionData Organizacion { get; set; }

        public OpcionData Opcion { get; set; }
    }
}

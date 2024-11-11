using System;

using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public class RolTransaccion : Entity
    {
        public int RolId { get; set; }

        public Rol Rol { get; set; }

        public int OrganizacionId { get; set; }

        public string OrganizacionDescripcion { get; set; }

        public int TransaccionId { get; set; }

        public Transaccion Transaccion { get; set; }

        public int OpcionId { get; set; }

        public Opcion Opcion { get; set; }
    }
}

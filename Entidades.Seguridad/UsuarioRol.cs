using System;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public class UsuarioRol : Entity
    {
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int RolId { get; set; }

        public Rol Rol { get; set; }

        public string Estado { get; set; }
    }
}

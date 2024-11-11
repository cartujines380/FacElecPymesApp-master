using System;
using System.Collections.Generic;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public abstract class Usuario : Entity
    {
        #region Constantes

        private const string ESTADO_ACTIVO = "A";

        private const string ESTADO_ACTIVO_TEMPORAL = "AC";

        private const string ESTADO_INACTIVO = "I";

        #endregion

        #region Campos

        private HashSet<UsuarioRol> m_roles;

        #endregion

        #region Propiedades

        public Identificacion Identificacion { get; set; }

        public abstract string Tipo { get; set; }

        public string Estado { get; set; }

        public abstract string NombreCompleto { get; }

        public string Nombre { get; set; }

        public Contacto DatosContacto { get; set; }

        public string PreguntaSecreta { get; set; }

        public string RespuestaSecreta { get; set; }

        public string FraseSecreta { get; set; }

        public string TipoParticipante { get; set; }

        public string TipoLoginCodigo { get; set; }

        public string Semilla { get; set; }

        public string Existe { get; set; }

        public ICollection<UsuarioRol> Roles
        {
            get
            {
                if (m_roles == null)
                {
                    m_roles = new HashSet<UsuarioRol>();
                }

                return m_roles;
            }
            set
            {
                m_roles = new HashSet<UsuarioRol>(value);
            }
        }

        #endregion

        #region Metodos publicos

        public bool EstaActivo()
        {
            return (Estado == ESTADO_ACTIVO);
        }

        public bool EstaActivoTemporal()
        {
            return (Estado == ESTADO_ACTIVO_TEMPORAL);
        }

        public void Activar()
        {
            Estado = ESTADO_ACTIVO;
        }

        public UsuarioRol AnadirRol(Rol rol, string estado)
        {
            var usuRol = new UsuarioRol()
            {
                UsuarioId = Id,
                Usuario = this,
                RolId = rol.Id,
                Rol = rol,
                Estado = estado
            };

            Roles.Add(usuRol);

            return usuRol;
        }

        #endregion
    }
}

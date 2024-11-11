using System;
using System.Collections.Generic;

using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public class Rol : Entity
    {
        #region Campos

        private HashSet<RolTransaccion> m_transacciones;

        #endregion

        #region Propiedades

        public string Nombre { get; set; }

        public string Estado { get; set; }

        public ICollection<RolTransaccion> Transacciones
        {
            get
            {
                if (m_transacciones == null)
                {
                    m_transacciones = new HashSet<RolTransaccion>();
                }

                return m_transacciones;
            }
            set
            {
                m_transacciones = new HashSet<RolTransaccion>(value);
            }
        }

        #endregion

        #region Metodos publicos

        public RolTransaccion AnadirTransaccion(Transaccion transaccion, Opcion opcion, int organizacionId)
        {
            var rolTrx = new RolTransaccion()
            {
                RolId = Id,
                Rol = this,
                OrganizacionId = organizacionId,
                TransaccionId = transaccion.Id,
                Transaccion = transaccion,
                OpcionId = opcion.Id,
                Opcion = opcion
            };

            Transacciones.Add(rolTrx);

            return rolTrx;
        }

        #endregion
    }
}

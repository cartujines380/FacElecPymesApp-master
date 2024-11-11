using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.Entidades.Seguridad;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Seguridad
{
    public class UsuarioRepository : IUsuarioRepository
    {
        #region Campos

        private readonly Database m_database;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public UsuarioRepository(IDatabaseProvider databaseProvider)
        {
            if (databaseProvider == null)
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }

            m_database = databaseProvider.GetDatabase();

            if (m_database == null)
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }
        }

        #endregion

        #region IUsuarioRepository

        public Usuario Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public Usuario Add(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public void ResetearClave(string usuarioId, string clave)
        {
            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONSULTA_Llama_ResetClave]"))
            {
                m_database.AddInParameter(cmd, "PI_IdUsuario", DbType.String, usuarioId);
                m_database.AddInParameter(cmd, "PI_ClaveNueva", DbType.String, clave);

                m_database.ExecuteNonQuery(cmd);
            }
        }

        public void CambiarClave(string usuarioId, string claveAnt, string claveNueva)
        {
            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONSULTA_CambiaClave_ADM]"))
            {
                m_database.AddInParameter(cmd, "PI_IdUsuario", DbType.String, usuarioId);
                m_database.AddInParameter(cmd, "PI_Clave", DbType.String, claveAnt);
                m_database.AddInParameter(cmd, "PI_ClaveNueva", DbType.String, claveNueva);

                m_database.ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                //Desechar
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       

        ~UsuarioRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}

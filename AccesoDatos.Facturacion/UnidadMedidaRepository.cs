using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        #region Campos

        private readonly Database m_database;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public UnidadMedidaRepository(IDatabaseProvider databaseProvider)
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

        #region Metodos privados

        private void Leer(IDataReader reader, UnidadMedida unidadMedida)
        {
            unidadMedida.IdUnidadMedida = reader.ColumnToInt32("IdUnidadMedida") ?? 0;
            unidadMedida.NuevaUnidad = reader.ColumnToString("NuevaUnidad");
        }

        #endregion

        #region IUnidadMedidaRepository

        public UnidadMedida Add(UnidadMedida entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UnidadMedida entity)
        {
            throw new NotImplementedException();
        }

        public UnidadMedida Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnidadMedida> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnidadMedida> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnidadMedida> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo)
        {
            var retorno = new List<UnidadMedida>();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESARUC", ruc),
                    new XAttribute("CODIGOPROD", codArticulo),
                    new XAttribute("ESADMIN", true),
                    new XAttribute("ESTRANSPORTISTA", false)
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Inventario].[Inv_Mant_OperacionesInventario]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "M");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var bodega = new UnidadMedida();
                        Leer(reader, bodega);

                        retorno.Add(bodega);
                    }
                }
            }

            return retorno;
        }

        public void Update(UnidadMedida entity)
        {
            throw new NotImplementedException();
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

        ~UnidadMedidaRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

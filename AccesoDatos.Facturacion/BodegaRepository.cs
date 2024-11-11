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
    public class BodegaRepository : IBodegaRepository
    {

        #region Campos

        private readonly Database m_database;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public BodegaRepository(IDatabaseProvider databaseProvider)
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

        private void Leer(IDataReader reader, Bodega bodega)
        {
            bodega.IdBodega = reader.ColumnToInt32("IdBodega") ?? 0;
            bodega.Nombre = reader.ColumnToString("Nombre");
        }

        #endregion

        #region IArticuloRepository

        public Bodega Add(Bodega entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Bodega entity)
        {
            throw new NotImplementedException();
        }

        public Bodega Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bodega> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bodega> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bodega> ObtenerBodegaPorEmpresa(string ruc)
        {
            var retorno = new List<Bodega>();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESARUC", ruc),
                    new XAttribute("ESADMIN", true),
                    new XAttribute("ESTRANSPORTISTA", false)

                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Inventario].[Mant_Bodegas]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "B");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var bodega = new Bodega();
                        Leer(reader, bodega);

                        retorno.Add(bodega);
                    }
                }
            }

            return retorno;
        }

        public void Update(Bodega entity)
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

        ~BodegaRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

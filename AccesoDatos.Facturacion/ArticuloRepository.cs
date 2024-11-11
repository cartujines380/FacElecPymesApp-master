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
    public class ArticuloRepository : IArticuloRepository
    {

        #region Campos

        private readonly Database m_database;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public ArticuloRepository(IDatabaseProvider databaseProvider)
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

        private void Leer(IDataReader reader, Articulo articulo)
        {
            articulo.Codigo = reader.ColumnToString("CodigoProducto");
            articulo.Nombre = reader.ColumnToString("Nombre");
            articulo.Descripcion = reader.ColumnToString("Descripcion");
            articulo.IdMoneda = reader.ColumnToString("Moneda");
            articulo.IdIVA = reader.ColumnToString("PorcentajeIVA");
            articulo.IdICE = reader.ColumnToString("PorcentajeICE");
            articulo.RastreoInventario = reader.ColumnToString("RastreoInventario");
            articulo.Stock = reader.ColumnToInt32("Stock") ?? 0;
            articulo.Estado = reader.ColumnToString("Estado");
            articulo.Cantidad = reader.ColumnToString("cantidad");
            articulo.Bodega = reader.ColumnToString("Bodega");
            articulo.PrecioUnidadBaseDecimal = reader.ColumnToDecimal("PrecioVenta") ?? 0;
            articulo.PrecioUnidadBase = articulo.PrecioUnidadBaseDecimal.ToString("N2");
        }

        private string Leer2(IDataReader reader)
        {
            var mensaje = reader.ColumnToString("Mensaje");

            return mensaje;
        }

        #endregion

        #region IArticuloRepository

        public Articulo Add(Articulo entity)
        {
            var retorno = new Articulo();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESA", entity != null ? entity.RucEmpresa : ""),
                    new XAttribute("EMPRESAANT", entity != null ? entity.RucEmpresa : ""),
                    new XAttribute("ESADMIN", true),
                    new XAttribute("CODIGO", entity != null ? entity.Codigo : ""),
                    new XAttribute("NOMBRE", entity != null ? entity.Nombre : ""),
                    new XAttribute("ESTADO", "A"),
                    new XAttribute("PRECIO", entity != null ? entity.PrecioUnidadBase.ToString() : ""),
                    new XAttribute("PORCIVA", entity != null ? entity.IdIVA : ""),
                    new XAttribute("PORCICE", entity != null ? entity.IdICE : ""),
                    new XAttribute("STOCK", entity != null ? entity.Cantidad.ToString() : ""),
                    new XAttribute("TIPOINV", entity != null ? entity.TipoInventario.ToString() : ""),
                    new XAttribute("USUARIO", "usrmtraframe")
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Inventario].[Consultar_Productos]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "G");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno.Mensaje = Leer2(reader);
                    }
                }
            }

            return retorno;
        }

        public void Delete(Articulo entity)
        {
            throw new NotImplementedException();
        }

        public Articulo Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Articulo> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Articulo> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public void Update(Articulo entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Articulo> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista, string idUsuario)
        {
            var retorno = new List<Articulo>();
            var _criterioBusquedad = criterioBusquedad == null ? "" : criterioBusquedad;

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESARUC", ruc),
                    new XAttribute("ESTRANSPORTISTA", esTransportista),
                    new XAttribute("CODIGO", _criterioBusquedad),
                    new XAttribute("NOMBRE", _criterioBusquedad),
                    new XAttribute("USUARIO", idUsuario),
                    new XAttribute("TIPOINV", 1)
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Inventario].[Consultar_Productos]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "D");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var articulo = new Articulo();
                        Leer(reader, articulo);

                        retorno.Add(articulo);
                    }
                }
            }

            return retorno;
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

        ~ArticuloRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class CatalogoRepository : ICatalogoRepository
    {
        #region Campos

        private readonly Database m_database;
        private bool m_desechado = false;

        #endregion

        #region Constructores

        public CatalogoRepository(IDatabaseProvider databaseProvider)
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

        private void Leer(IDataReader reader, Impuesto catalago)
        {
            catalago.Codigo = reader.ColumnToString("codigo");
            catalago.Concepto = reader.ColumnToString("descripcion");
            catalago.Porcentaje = reader.ColumnToDecimal("valorImpPor") ?? 0;
        }

        private void Leer(IDataReader reader, Catalogo catalago)
        {
            catalago.Codigo = reader.ColumnToString("codigo");
            catalago.Detalle = reader.ColumnToString("Descripcion");
            catalago.SubCodigo = reader.ColumnToString("DescAlterno");
            catalago.Estado = reader.ColumnToString("Estado");
        }

        private void LeerCatalogo(IDataReader reader, Catalogo catalago)
        {
            catalago.Codigo = reader.ColumnToString("codigo");
            catalago.Detalle = reader.ColumnToString("detalle");
        }

        private void LeerCatalogoGeneral(IDataReader reader, Catalogo catalago)
        {
            catalago.Tabla = reader.ColumnToInt32("tabla") ?? 0;
            catalago.Codigo = reader.ColumnToString("codigo");
            catalago.Detalle = reader.ColumnToString("detalle");
        }

        private void LeerFormaPago(IDataReader reader, Catalogo catalago)
        {
            catalago.Codigo = reader.ColumnToString("codigo");
            catalago.Detalle = reader.ColumnToString("detalle");
            catalago.Contador = reader.ColumnToInt32("contador") ?? 0;
        }

        private void LeerImpuesto(IDataReader reader, ImpuestoData impuesto)
        {
            impuesto.Codigo = reader.ColumnToString("codigo");
            impuesto.Descripcion = reader.ColumnToString("descripcion");
            impuesto.Valor = reader.ColumnToDecimal("valorImpPor") ?? 0;
            impuesto.ImpuestoRetener = reader.ColumnToInt32("impuestoRetener") ?? 0;
        }

        private void LeerImpuestoRetencion(IDataReader reader, ImpuestoRetencion impuesto)
        {
            impuesto.Id = reader.ColumnToInt32("id") ?? 0;
            impuesto.ImpuestoRetenerId = reader.ColumnToInt32("impuestoRetener") ?? 0;
            impuesto.Codigo = reader.ColumnToString("codigo");
            impuesto.Porcentaje = reader.ColumnToDecimal("valPorcentaje") ?? 0;
            impuesto.TipoRetencion = reader.ColumnToString("tipoRetencion");
            impuesto.PorcentajeDescripcion = reader.ColumnToString("Porcentaje");
        }

        #endregion

        #region ICatalagoRepository

        public Catalogo Add(Catalogo entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Catalogo entity)
        {
            throw new NotImplementedException();
        }

        public Catalogo Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Catalogo> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Catalogo> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public void Update(Catalogo entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Catalogo> ObtenerCatalogo(string nombreTabla)
        {
            var retorno = new List<Catalogo>();

            using (var cmd = m_database.GetStoredProcCommand("[Catalogo].[Ctl_ConsCatalogo]"))
            {
                m_database.AddInParameter(cmd, "PI_Tabla", DbType.String, nombreTabla);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var catalogo = new Catalogo();
                        LeerCatalogo(reader, catalogo);

                        retorno.Add(catalogo);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<Impuesto> ObtenerImpuestoPorCodigo(string tipo)
        {
            var retorno = new List<Impuesto>();

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_IMPUESTO]"))
            {
                m_database.AddInParameter(cmd, "PI_Tipo", DbType.Int32, tipo);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var impuetso = new Impuesto();
                        Leer(reader, impuetso);

                        retorno.Add(impuetso);
                    }
                }
            }

            return retorno;

        }

        public IEnumerable<Catalogo> ObtenerCatalogoPais(int codigo)
        {
            var retorno = new List<Catalogo>();

            using (var cmd = m_database.GetStoredProcCommand("[Catalogo].[Ctl_P_ConsultaCatalogoFw]"))
            {
                m_database.AddInParameter(cmd, "PI_CodigoTabla", DbType.Int32, codigo);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var catalogo = new Catalogo();
                        Leer(reader, catalogo);

                        retorno.Add(catalogo);
                    }
                }
            }

            return retorno;

        }

        public IEnumerable<Catalogo> ObtenerCatalogoProvinciaCiudad(int codigo,  string descAlterno)
        {
            var retorno = new List<Catalogo>();

            using (var cmd = m_database.GetStoredProcCommand("[Catalogo].[Ctl_P_ConsultaCatalogoFwDireccion]"))
            {
                m_database.AddInParameter(cmd, "PI_CodigoTabla", DbType.Int32, codigo);
                m_database.AddInParameter(cmd, "PI_DescAlterno", DbType.String, descAlterno);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var catalogo = new Catalogo();
                        Leer(reader, catalogo);

                        retorno.Add(catalogo);
                    }
                }
            }

            return retorno;

        }

        public IEnumerable<Catalogo> ObtenerCatalogoGeneral(int codigoTabla)
        {
            var retorno = new List<Catalogo>();

            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONS_INFORMACION_GENERAL_SIPECOM]"))
            {
                m_database.AddInParameter(cmd, "IdTablaDoc", DbType.Int32, codigoTabla);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var catalogo = new Catalogo();
                        LeerCatalogoGeneral(reader, catalogo);

                        retorno.Add(catalogo);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<Catalogo> ObtenerFormaPago()
        {
            var retorno = new List<Catalogo>();

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_FORMAPAGO]"))
            {
                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var formaPago = new Catalogo();
                        LeerFormaPago(reader, formaPago);

                        retorno.Add(formaPago);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<ImpuestoData> ObtenerImpuestos()
        {
            var retorno = new List<ImpuestoData>();

            using (var cmd = m_database.GetStoredProcCommand("[Inventario].[Consultar_Productos]"))
            {
                var xml = new XmlDocument();
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xml.OuterXml);
                m_database.AddInParameter(cmd, "Accion", DbType.String, "I");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var impuesto = new ImpuestoData();
                        LeerImpuesto(reader, impuesto);

                        retorno.Add(impuesto);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<ImpuestoRetencion> ObtenerImpuestosRetencionFuente(int impuestoRetenerId, string tipoRetencion)
        {
            var retorno = new List<ImpuestoRetencion>();

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_IMPUESTORETENIDO]"))
            {
                var xml = new XmlDocument();
                m_database.AddInParameter(cmd, "PI_ImpuestoRetener", DbType.Int32, impuestoRetenerId);
                m_database.AddInParameter(cmd, "PI_TipoRetencion", DbType.String, tipoRetencion);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var impuesto = new ImpuestoRetencion();
                        LeerImpuestoRetencion(reader, impuesto);

                        retorno.Add(impuesto);
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

        ~CatalogoRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

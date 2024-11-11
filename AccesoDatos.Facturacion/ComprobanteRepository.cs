using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class ComprobanteRepository : IComprobanteRepository
    {
        #region Campos

        private readonly Database m_database;
        private bool m_desechado = false;

        #endregion

        #region Constructores

        public ComprobanteRepository(IDatabaseProvider databaseProvider)
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

        #region IComprobanteRepository

        public Comprobante Add(Comprobante entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Comprobante entity)
        {
            throw new NotImplementedException();
        }

        public Comprobante Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comprobante> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comprobante> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comprobante> ObtenerComprobantes(FiltroModel request)
        {
            var retorno = new List<Comprobante>();
            string fechaInicio = (request != null && request.FechaInicio != null) ? request.FechaInicio.ToString("yyyy/MM/dd") : "";
            string fechaFin = (request != null && request.FechaFin != null) ? request.FechaFin.ToString("yyyy/MM/dd") : "";
            string ruc = (request != null && request.Establecimiento != null) ? request.Establecimiento.Ruc : "";
            string tipos = (request != null && request.Tipos != null) ? request.Tipos : "";

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("FECHA_INI", fechaInicio),
                    new XAttribute("FECHA_FIN", fechaFin),
                    new XAttribute("DOCUMENTO", tipos),
                    new XAttribute("NUMCOMPROBANTE", request.NumeroComprobante ?? ""),
                    new XAttribute("IDENTIFICACION_CLIENTE", request.IdentificacionCliente ?? ""),
                    new XAttribute("ESTADO", request.CodigoEstado ?? ""),
                    new XAttribute("CODIGOERROR", request.CodigoError ?? ""),
                    new XAttribute("RUC", ruc),
                    new XAttribute("pag_idx", request.PageIndex),
                    new XAttribute("pag_tam", request.PageSize),
                    new XAttribute("USUARIO", request.IdUsuario ?? "")
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONSDOCUMENTOS_ADMIN_BETA_APP]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlConsultarDoc", DbType.Xml, xmlDoc.ToString());

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var comprobante = new Comprobante();
                        Leer(reader, comprobante);

                        retorno.Add(comprobante);
                    }
                }
            }

            return retorno;
        }

        public Comprobante ObtenerComprobantesXML(InfoComprobanteModel request)
        {
            var xml = new List<Comprobante>();
           
            int idDocumento = request != null ? request.IdDocumento : 0;
            string tipoBase = request != null ? request.TipoBase : "";

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("DOCUMENTO", idDocumento),
                    new XAttribute("TipoBase", tipoBase)
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[CONSADMIN_CONS_DOC_XML]"))
            {
                m_database.AddInParameter(cmd, "@PI_XmlId", DbType.Xml, xmlDoc.ToString());

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var comprobante = new Comprobante();
                        LeerDos(reader, comprobante);

                        xml.Add(comprobante);
                    }
                }
            }

            var retorno = xml.FirstOrDefault();

            return retorno;
        }

        private void Leer(IDataReader reader, Comprobante comprobante)
        {
            comprobante.IdDocumento = reader.ColumnToInt32("IdDocumento") ?? 0;
            comprobante.Ruc = reader.ColumnToString("RUC");
            comprobante.Establecimiento = reader.ColumnToString("Establecimiento");
            comprobante.FechaEmision = reader.ColumnToString("FechaEmision");
            comprobante.TipoDoc = reader.ColumnToString("TipoDoc");
            comprobante.Numero = reader.ColumnToString("Numero");
            comprobante.IdentificacionCliente = reader.ColumnToString("IdentificacionCliente");
            comprobante.MontoFacturado = reader.ColumnToDecimal("MontoFacturado") ?? 0;
            comprobante.TipoBase = reader.ColumnToString("TipoBase");
        }

        private void LeerDos(IDataReader reader, Comprobante comprobante)
        {
            comprobante.Documento = reader.ColumnToString("DOCUMENTO");
            comprobante.Acceso = reader.ColumnToString("ACCESO");
            comprobante.Autorizacion = reader.ColumnToString("AUTORIZACION");
            comprobante.Numero = reader.ColumnToString("NUMERO");
            comprobante.TipoDoc = reader.ColumnToString("TipoDoc");
            comprobante.DocumentoXML = reader.ColumnToString("DOCUMENTOXML");
        }

        public void Update(Comprobante entity)
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

        ~ComprobanteRepository()
        {
            Dispose(false);
        }

        #endregion

    }
}

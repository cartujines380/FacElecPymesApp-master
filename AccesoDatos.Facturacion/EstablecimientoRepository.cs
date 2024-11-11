using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class EstablecimientoRepository : IEstablecimientoRepository
    {
        #region Constantes

        public const string SECTION_KEY = "FacturacionElectronica";

        #endregion

        #region Campos

        private readonly Database m_database;
        private bool m_desechado = false;
        private readonly IConfiguration m_configuration;

        #endregion

        #region Constructores

        public EstablecimientoRepository(IDatabaseProvider databaseProvider, IConfiguration configuration)
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

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;
        }

        #endregion

        #region Metodos privados

        private void Leer(IDataReader reader, Establecimiento establecimiento)
        {
            establecimiento.Ruc = reader.ColumnToString("Ruc");
            establecimiento.RazonSocial = reader.ColumnToString("RazonSocial");
            establecimiento.CorreoReset = reader.ColumnToString("CorreoResetPassword");
        }

        private void LeerDos(IDataReader reader, Establecimiento establecimiento)
        {
            establecimiento.Ruc = reader.ColumnToString("Ruc");
            establecimiento.RazonSocial = reader.ColumnToString("RazonSocial");
            establecimiento.ContribEsp = reader.ColumnToString("EsContribuyenteEspecial");
            establecimiento.ObligadoContab = reader.ColumnToBoolean("obligadoContabilidad");
            establecimiento.DirMatriz = reader.ColumnToString("DirMatriz");
        }

        private void LeerData(IDataReader reader, EstablecimientoData establecimientoData)
        {
            establecimientoData.Ruc = reader.ColumnToString("Ruc");
            establecimientoData.RazonSocial = reader.ColumnToString("RazonSocial");
            establecimientoData.EsContribuyenteEspecial = reader.ColumnToString("EsContribuyenteEspecial");
            establecimientoData.ObligadoContabilidad = reader.ColumnToBoolean("obligadoContabilidad");
            establecimientoData.MatrizDireccion = reader.ColumnToString("DirMatriz");
            establecimientoData.EstablecimientoCodigo = reader.ColumnToString("Establecimiento");
            establecimientoData.PuntoEmision = reader.ColumnToString("PtoEmision");
        }

        private void LeerData3(IDataReader reader, EstablecimientoData establecimientoData)
        {
            establecimientoData.Ruc = reader.ColumnToString("RUC");
            establecimientoData.RazonSocial = reader.ColumnToString("RazonSocial");
            establecimientoData.PuntoEmisionTransportista = reader.ColumnToString("PuntoEmisionTransportista");
            establecimientoData.RazonSocialTransportista = reader.ColumnToString("RazonSocialTransportista");
            establecimientoData.RucTransportista = reader.ColumnToString("RucTransportista");
            establecimientoData.Regimen = reader.ColumnToString("Regimen");
        }

        private void LeerMicro(IDataReader reader, RegimenMicroempresaData regimenData)
        {
            regimenData.FechaInicio = reader.ColumnToDateTime("FechaInicio");
            regimenData.FechaFin = reader.ColumnToDateTime("FechaFin");
        }

        private void LeerRimpe(IDataReader reader, RimpeData rimpeData)
        {
            rimpeData.Tipo = reader.ColumnToString("tipoRIMPE");
            rimpeData.Detalle = reader.ColumnToString("detalle");
        }

        private void LeerPlan(IDataReader reader, EstablecimientoData rimpeData)
        {
            rimpeData.Ruc = reader.ColumnToString("Ruc");
            rimpeData.RazonSocial = reader.ColumnToString("RazonSocial");
            rimpeData.NombrePlan = reader.ColumnToString("NombrePlan");
            rimpeData.CantidadDocDesde = reader.ColumnToString("CantidadDocDesde");
            rimpeData.CantidadDocHasta = reader.ColumnToString("CantidadDocHasta");
            rimpeData.FechaInicioPlan = reader.ColumnToString("FechaInicioPlan");
            rimpeData.FechaFinPlan = reader.ColumnToString("FechaFinPlan");
        }

        private string LeerDireccionSucursal(IDataReader reader)
        {
            var dirSucursal = reader.ColumnToString("DireccionEstablec");

            return dirSucursal;
        }

        #endregion

        #region IEstablecimientoRepository

        public Establecimiento Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Establecimiento> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Establecimiento> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public Establecimiento Add(Establecimiento entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Establecimiento entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Establecimiento entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuario(string usuarioId)
        {
            var retorno = new List<Establecimiento>();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("IDENTIFICACION", usuarioId)
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_ACT_DATOS_ADM]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var establecimiento = new Establecimiento();
                        Leer(reader, establecimiento);

                        retorno.Add(establecimiento);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuarioCmb(string usuarioId)
        {
            var retorno = new List<Establecimiento>();
            var rolAdm = m_configuration.GetSection(SECTION_KEY).GetSection("RolAdm").Value;

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_EMPRESA]"))
            {
                m_database.AddInParameter(cmd, "IdUsuario", DbType.String, usuarioId);
                m_database.AddInParameter(cmd, "RolAdm", DbType.String, rolAdm);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var establecimiento = new Establecimiento();
                        LeerDos(reader, establecimiento);

                        retorno.Add(establecimiento);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosPorUsuario(string usuarioId)
        {
            var retorno = new List<EstablecimientoData>();
            var rolAdm = m_configuration.GetSection(SECTION_KEY).GetSection("RolAdm").Value;

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_EMPRESA]"))
            {
                m_database.AddInParameter(cmd, "IdUsuario", DbType.String, usuarioId);
                m_database.AddInParameter(cmd, "RolAdm", DbType.String, rolAdm);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var establecimientoData = new EstablecimientoData();
                        LeerData(reader, establecimientoData);

                        retorno.Add(establecimientoData);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosTransportitasPorUsuario(string usuarioId)
        {
            var retorno = new List<EstablecimientoData>();

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_CONS_Operadora_Transportista]"))
            {
                m_database.AddInParameter(cmd, "IdUsuario", DbType.String, usuarioId);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var establecimientoData = new EstablecimientoData();
                        LeerData3(reader, establecimientoData);

                        retorno.Add(establecimientoData);
                    }
                }
            }

            return retorno;
        }

        public EstablecimientoData ObtenerDataEstablecimientoPorUsuarioEmpresa(string usuarioId, string empresaRuc)
        {
            var establecimientos = ObtenerDataEstablecimientosPorUsuario(usuarioId);

            return establecimientos
                .Where(x => x.Ruc == empresaRuc)
                .FirstOrDefault();
        }

        public EstablecimientoData ObtenerDataEstablecimientoTransportistaPorUsuarioEmpresa(string usuarioId, string empresaRuc)
        {
            var establecimientos = ObtenerDataEstablecimientosTransportitasPorUsuario(usuarioId);

            return establecimientos
                .Where(x => x.Ruc == empresaRuc)
                .FirstOrDefault();
        }

        public IEnumerable<RegimenMicroempresaData> ObtenerRegimenMicroempresaData(string empresaRuc, DateTime fechaEmision)
        {
            var retorno = new List<RegimenMicroempresaData>();

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONSESTABLECIMIENTO_REGMICROEMP]"))
            {
                m_database.AddInParameter(cmd, "PI_Ruc", DbType.String, empresaRuc);
                m_database.AddInParameter(cmd, "PI_FECHAEMISION", DbType.DateTime, fechaEmision);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var regimenData = new RegimenMicroempresaData()
                        {
                            Ruc = empresaRuc
                        };

                        LeerMicro(reader, regimenData);

                        retorno.Add(regimenData);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<RimpeData> ObtenerRimpeData(string empresaRuc)
        {
            var retorno = new List<RimpeData>();

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONSESTABLECIMIENTO_RIMPE]"))
            {
                m_database.AddInParameter(cmd, "PI_Ruc", DbType.String, empresaRuc);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var rimpeData = new RimpeData();

                        LeerRimpe(reader, rimpeData);

                        retorno.Add(rimpeData);
                    }
                }
            }

            return retorno;
        }

        public EstablecimientoData Obtenerplan(string empresaRuc)
        {
            var plan = new List<EstablecimientoData>();

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[SIPE_CONSULTA_PLANES]"))
            {
                m_database.AddInParameter(cmd, "RUC", DbType.String, empresaRuc);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var rimpeData = new EstablecimientoData();

                        LeerPlan(reader, rimpeData);

                        plan.Add(rimpeData);
                    }
                }
            }

            var retorno = plan.FirstOrDefault();

            return retorno;
        }

        public string ObtenerDireccionSucursal(InfoComprobanteModel info)
        {
            var retorno = string.Empty;

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONS_DIRECCION_SUCURSAL]"))
            {
                m_database.AddInParameter(cmd, "Ruc", DbType.String, info.Ruc);
                m_database.AddInParameter(cmd, "Estab", DbType.String, info.Establecimiento);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno = LeerDireccionSucursal(reader);
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

        ~EstablecimientoRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

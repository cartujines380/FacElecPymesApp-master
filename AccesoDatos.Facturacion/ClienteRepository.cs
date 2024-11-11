using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class ClienteRepository : IClienteRepository
    {
        #region Campos

        private readonly Database m_database;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public ClienteRepository(IDatabaseProvider databaseProvider)
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

        private void Leer(IDataReader reader, Cliente cliente)
        {
            cliente.RucEmpresa = reader.ColumnToString("RucEmp");
            cliente.Correo = reader.ColumnToString("direccion_elec");
            cliente.RazonSocial = reader.ColumnToString("razon_social");
            cliente.Identificacion = reader.ColumnToString("identificacion");
            cliente.Estado = reader.ColumnToString("estado");
            cliente.Telefono = reader.ColumnToString("telefono");
            cliente.Celular = reader.ColumnToString("celular");
            cliente.Direccion = reader.ColumnToString("direccion");
            cliente.TipoIdentificacion = reader.ColumnToString("tipo_identificacion");
            cliente.IdTipoIdentificacion = reader.ColumnToString("TipoId");
            cliente.TipoEntidad = reader.ColumnToString("TipoEntidad");
            cliente.Provincia = reader.ColumnToString("provincia");
            cliente.Ciudad = reader.ColumnToString("ciudad");
            cliente.IdTipoEntidad = reader.ColumnToString("IdTipoEntidad");
        }

        private string Leer2(IDataReader reader)
        {
            var esTransportista = reader.ColumnToString("EsTransportista");

            return esTransportista;
        }

        private string Leer3(IDataReader reader)
        {
            var mensaje = reader.ColumnToString("Mensaje");

            return mensaje;
        }

        private string LeerDireccionCliente(IDataReader reader)
        {
            var mensaje = reader.ColumnToString("direccion");

            return mensaje;
        }

        #endregion

        #region IClienteRepository

        public Cliente Add(Cliente entity)
        {
            var retorno = new Cliente();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESA", entity != null ? entity.RucEmpresa : ""),
                    new XAttribute("EMPRESAANT", entity != null ? entity.RucEmpresaAnterior : ""),
                    new XAttribute("NOMBRES", entity != null ? entity.RazonSocial : ""),
                    new XAttribute("IDENTIFICACION", entity != null ? entity.Identificacion : ""),
                    new XAttribute("DIRECCIONELEC", entity != null ? entity.Correo : ""),
                    new XAttribute("DIRECCION", entity != null ? entity.Direccion : ""),
                    new XAttribute("TELEFONO", entity != null ? entity.Telefono : ""),
                    new XAttribute("CELULAR", entity != null ? entity.Celular : ""),
                    new XAttribute("TIPOID", entity != null ? entity.IdTipoIdentificacion : ""),
                    new XAttribute("ESTADO", entity != null ? entity.Estado : ""),
                    new XAttribute("TRANSPORTISTA", ""),
                    new XAttribute("PLACA", ""),
                    new XAttribute("Usuario", entity != null ? entity.IdUsuario : ""),
                    new XAttribute("PROVINCIA", entity != null ? entity.Provincia : ""),
                    new XAttribute("CIUDAD", entity != null ? entity.Ciudad : ""),
                    new XAttribute("TIPOENTIDAD", entity != null ? entity.IdTipoEntidad : "")
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONS_ACT_DATOS_ADMIN]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "I");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno.Mensaje = Leer3(reader);
                    }
                }
            }

            return retorno;
        }

        public void Delete(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Cliente Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public void Update(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> ObtenerClientesPorEmpresa(ObtenerClientesPorEmpresaRequest request)
        {
            var retorno = new List<Cliente>();

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("n",
                    new XAttribute("EMPRESARUC", request != null ? request.Ruc : ""),
                    new XAttribute("IDENTIFICACION", request != null ? request.Identificacion : ""),
                    new XAttribute("TIPOID", request != null ? request.TipoIdentificacion : ""),
                    new XAttribute("ESTRANSPORTISTA", request != null && request.EsTransportista),
                    new XAttribute("ESADMIN", true),
                    new XAttribute("Usuario", request != null ? request.IdUsuario : ""),
                    new XAttribute("pag_idx", request != null ? request.PaginaIndice : ""),
                    new XAttribute("pag_tam", request != null ? request.PaginaTamanio : "")
                )
            );

            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONS_ACT_DATOS_ADMIN]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlActualiza", DbType.Xml, xmlDoc.ToString());
                m_database.AddInParameter(cmd, "Accion", DbType.String, "C");

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var cliente = new Cliente();
                        Leer(reader, cliente);

                        retorno.Add(cliente);
                    }
                }
            }

            return retorno;
        }

        public bool EsTransportista(string idUsuario)
        {
            var retorno = false;

            using (var cmd = m_database.GetStoredProcCommand("[Cliente].[Cli_CONS_SI_ES_TRANSPORTISTA]"))
            {
                m_database.AddInParameter(cmd, "IdUsuario", DbType.String, idUsuario);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var esTransportista = Leer2(reader);
                        retorno = esTransportista == "SI";
                    }
                }
            }

            return retorno;
        }

        public string ObtenerDireccionCliente(InfoComprobanteModel info)
        {
            var retorno = string.Empty;

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONS_DIRECCION_CLIENTE]"))
            {
                m_database.AddInParameter(cmd, "Ruc", DbType.String, info.Ruc);
                m_database.AddInParameter(cmd, "Ident", DbType.String, info.Identificacion);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno = LeerDireccionCliente(reader);
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

        ~ClienteRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

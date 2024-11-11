using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Agentes.Framework.Administracion.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Comun;
using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Servicios;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Administracion.Servicios
{
    public class FrameworkAdministracionAgenteService : IAdministracionAgenteService
    {
        #region Campos

        private readonly ISoapClientFactory<ServicioAdministracionSoapClient> m_factory;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public FrameworkAdministracionAgenteService(
            ISoapClientFactory<ServicioAdministracionSoapClient> factory
        )
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            m_factory = factory;
        }

        #endregion

        #region Metodos privados

        private XDocument CrearConsultaRolTransaccionRequest(ConsRolTransaccionRequest1 request)
        {
            var sesion = request.requerimiento.Sesion;

            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Usuario",
                    new XAttribute("PS_IdUsuario", sesion.UsuarioId),
                    new XAttribute("PS_Token", sesion.Token),
                    new XAttribute("PS_Maquina", sesion.Maquina),
                    new XAttribute("PS_UsrSitio", sesion.UsrSitio),
                    new XAttribute("PS_TokenSitio", sesion.TokenSitio),
                    new XAttribute("PS_MaqSitio", sesion.MaqSitio),
                    new XAttribute("PS_IdEmpresa", sesion.EmpresaId),
                    new XAttribute("PS_IdSucursal", sesion.SucursalId),
                    new XAttribute("PS_IdAplicacion", sesion.AplicacionId),
                    new XAttribute("PS_Formulario", sesion.Formulario),
                    new XAttribute("PS_Login", sesion.Login),
                    
                    new XAttribute("IdRol", request.requerimiento.idRol),
                    new XAttribute("Sucursal0", request.requerimiento.sucursal0),
                    new XAttribute("CodEmpresa", sesion.EmpresaId)
                )
            );
            
            return retorno;
        }

        private ConsRolTransaccionResponse CrearConsultaRolTransaccionResponse(ArrayOfXElement response)
        {
            var retorno = new ConsRolTransaccionResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_Ok,
                RolResult = new RolTransaccionData()
            };

            var respondeDS = Convertidor.ToDataSet(response);

            var estadoDtr = respondeDS.Tables["TblEstado"]
                .AsEnumerable()
                .First();

            var codErr = Convert.ToString(estadoDtr.Field<int?>("CodError") ?? -1);
            var msjErr = estadoDtr.Field<string>("MsgError");

            if (!(string.Compare(codErr, "0") == 0))
            {
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;

                return retorno;
            }

            retorno.RolResult.Datos = respondeDS.Tables[0]
                .AsEnumerable()
                .Select(ert => new ElementoRolTransaccion
                {
                    IdTransaccion = ert.Field<int>("idtransaccion"),
                    IdOpcion = ert.Field<int>("idopcion"),
                    IdOrganizacion = ert.Field<int>("idorganizacion"),
                    DescripcionOpcion = ert.Field<string>("DesOpcion"),
                    DescripcionOrganizacion = ert.Field<string>("DesOrganizacion"),
                    DescripcionTransaccion = ert.Field<string>("DesTransaccion")
                })
                .ToList();

            return retorno;
        }

        private string CrearConsultaUsuarioRolRequest(ConsultaUsuarioRolRequest1 request)
        {
            var sesion = request.requerimiento.Sesion;

            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Usuario",
                    new XAttribute("PS_IdUsuario", sesion.UsuarioId),
                    new XAttribute("PS_Token", sesion.Token),
                    new XAttribute("PS_Maquina", sesion.Maquina),
                    new XAttribute("PS_UsrSitio", sesion.UsrSitio),
                    new XAttribute("PS_TokenSitio", sesion.TokenSitio),
                    new XAttribute("PS_MaqSitio", sesion.MaqSitio),
                    new XAttribute("PS_IdEmpresa", sesion.EmpresaId),
                    new XAttribute("PS_IdSucursal", sesion.SucursalId),
                    new XAttribute("PS_IdAplicacion", sesion.AplicacionId),
                    new XAttribute("PS_Formulario", sesion.Formulario),
                    new XAttribute("PS_Login", sesion.Login),
                    new XAttribute("IdUsuario", request.requerimiento.IdUsuario)
                )
            );

            return retorno.ToString();
        }

        private ConsultaUsuarioRolResponse CrearConsultaUsuarioRolResponse(ArrayOfXElement response)
        {
            var retorno = new ConsultaUsuarioRolResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_Ok,
                RolResult = new RolData()
            };

            var respondeDS = Convertidor.ToDataSet(response);

            var estadoDtr = respondeDS.Tables["TblEstado"]
                .AsEnumerable()
                .First();

            var codErr = Convert.ToString(estadoDtr.Field<int?>("CodError") ?? -1);
            var msjErr = estadoDtr.Field<string>("MsgError");

            if (!(string.Compare(codErr, "0") == 0))
            {
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;

                return retorno;
            }
            
            retorno.RolResult.Datos = respondeDS.Tables[0]
                .AsEnumerable()
                .Select(er => new ElementoRol
                {
                    IdRol = er.Field<int>("IdRol"),
                    NombreRol = er.Field<string>("NombreRol"),
                    Estado = er.Field<string>("Estado"),
                })
                .ToList();

            return retorno;
        }

        #endregion

        #region IAdministracionAgenteService

        public ConsRolTransaccionResponse ConsultaRolTransaccion(ConsRolTransaccionRequest1 request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => ConsultaRolTransaccionAsync(request));
        }

        public async Task<ConsRolTransaccionResponse> ConsultaRolTransaccionAsync(ConsRolTransaccionRequest1 request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ConsRolTransaccionResponse response = null;

            ServicioAdministracionSoapClient cliente = null;

            try
            {
                var xmlParam = CrearConsultaRolTransaccionRequest(request).ToString();

                cliente = m_factory.CreateClient();

                var regCatResponse = await cliente.consRolTransaccionAsync(xmlParam);

                response = CrearConsultaRolTransaccionResponse(regCatResponse);

                await cliente.CloseAsync();
            }
            catch (CommunicationException cme)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuCommunication, cme);
                throw new InvalidOperationException(Mensajes.exc_RegUsuCommunication);
            }
            catch (TimeoutException toe)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuTimeout, toe);
                throw new InvalidOperationException(Mensajes.exc_RegUsuTimeout);
            }
            catch (Exception ex)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuGeneral, ex);
                throw new InvalidOperationException(Mensajes.exc_RegUsuGeneral);
            }

            return response;
        }

        public ConsultaUsuarioRolResponse ConsultaUsuarioRol(ConsultaUsuarioRolRequest1 request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => ConsultaUsuarioRolAsync(request));
        }

        public async Task<ConsultaUsuarioRolResponse> ConsultaUsuarioRolAsync(ConsultaUsuarioRolRequest1 request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ConsultaUsuarioRolResponse response = null;

            ServicioAdministracionSoapClient cliente = null;

            try
            {
                var xmlParam = CrearConsultaUsuarioRolRequest(request);

                cliente = m_factory.CreateClient();

                var regCatResponse = await cliente.consultaUsuarioRolAsync(xmlParam);

                response = CrearConsultaUsuarioRolResponse(regCatResponse);

                await cliente.CloseAsync();
            }
            catch (CommunicationException cme)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuCommunication, cme);
                throw new InvalidOperationException(Mensajes.exc_RegUsuCommunication);
            }
            catch (TimeoutException toe)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuTimeout, toe);
                throw new InvalidOperationException(Mensajes.exc_RegUsuTimeout);
            }
            catch (Exception ex)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuGeneral, ex);
                throw new InvalidOperationException(Mensajes.exc_RegUsuGeneral);
            }

            return response;
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

            }
            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FrameworkAdministracionAgenteService()
        {
            Dispose(false);
        }

        #endregion
    }
}

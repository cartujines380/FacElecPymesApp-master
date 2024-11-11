using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Xml;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Servicios
{
    public class FrameworkSeguridadAgenteService : ISeguridadAgenteService
    {
        #region Campos

        private readonly ISoapClientFactory<ServicioSeguridadSoapClient> m_factory;
        private readonly ICriptografiaService m_criptografiaService;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public FrameworkSeguridadAgenteService(
            ISoapClientFactory<ServicioSeguridadSoapClient> factory,
            ICriptografiaService criptografiaService
        )
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (criptografiaService == null)
            {
                throw new ArgumentNullException(nameof(criptografiaService));
            }

            m_factory = factory;
            m_criptografiaService = criptografiaService;
        }

        #endregion

        #region Metodos privados

        private RegistrarAplicacionResponse CrearRegistrarAplicacionResponse(string response)
        {
            var retorno = new RegistrarAplicacionResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_Ok
            };

            var responseDoc = XDocument.Parse(response);

            var rootEle = responseDoc.Root;

            var codErr = rootEle.GetAttributeValue("CodError");
            var msjErr = rootEle.GetAttributeValue("MsgError");

            if(!(string.Compare(codErr, "0") == 0))
            {
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;
                
                return retorno;
            }

            retorno.SitioMaquina = rootEle.GetAttributeValue("PS_MaqSitio");
            retorno.SitioToken = rootEle.GetAttributeValue("PS_TokenSitio");
            retorno.UsuarioId = rootEle.GetAttributeValue("PS_IdUsuario");
            retorno.Maquina = rootEle.GetAttributeValue("PS_Maquina");
            retorno.Token = rootEle.GetAttributeValue("PS_Token");

            retorno.Servidores = rootEle
                .Element("Servidores")?
                .Elements("Servidor")
                .Select(s => new ServidorData
                {
                    Id = s.GetAttributeValue("Id"),
                    Tipo = s.GetAttributeValue("Tipo"),
                    Parametros = s.Attributes()
                        .Where(sa => !(
                            (sa.Name.LocalName == "Id")
                            || (sa.Name.LocalName == "Tipo")
                            )
                        )
                        .ToDictionary(sa => sa.Name.LocalName, sa => sa.Value)
                })
                .ToList();

            return retorno;
        }

        private XDocument CrearRegistraUsuarioRequest(RegistraUsuarioRequest request)
        {
            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Usuario",
                    new XAttribute("PS_IdUsuario", request.UsuarioId),
                    new XAttribute("PS_Token", "1"),
                    new XAttribute("PS_Maquina", request.Maquina),
                    new XAttribute("PS_UsrSitio", request.SitioUsuario),
                    new XAttribute("PS_TokenSitio", request.SitioToken),
                    new XAttribute("PS_MaqSitio", request.SitioMaquina),
                    new XAttribute("PS_IdEmpresa", request.EmpresaId),
                    new XAttribute("PS_IdSucursal", request.SucursalId),
                    new XAttribute("PS_IdAplicacion", request.AplicacionId),
                    new XAttribute("PS_Formulario", "1"),
                    new XAttribute("PS_Login", "S"),

                    new XAttribute("PS_Clave", request.Clave),
                    new XAttribute("Perfil", request.Perfil),
                    new XAttribute("PS_Dominio", request.Dominio)
                )
            );

            return retorno;
        }

        private RegistraUsuarioResponse CrearRegistraUsuarioResponse(XDocument request, string response)
        {
            var retorno = new RegistraUsuarioResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_Ok
            };

            var responseDoc = XDocument.Parse(response);

            var rootEle = responseDoc.Root;

            var codErr = rootEle.GetAttributeValue("CodError");
            var msjErr = rootEle.GetAttributeValue("MsgError");

            if (!(string.Compare(codErr, "0") == 0))
            {
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;

                return retorno;
            }

            retorno.Participante = new ParticipanteData()
            {
                Id = int.Parse(rootEle.GetAttributeValue("IdParticipante")),
                Nombre = rootEle.GetAttributeValue("NombreParticipante"),
                Identificacion = rootEle.GetAttributeValue("IdentParticipante")
            };

            retorno.Estado = rootEle.GetAttributeValue("Estado");
            retorno.Token = request.Root.GetAttributeValue("PS_Token");
            retorno.Maquina = request.Root.GetAttributeValue("PS_Maquina");

            retorno.Transacciones = rootEle
                .Element("Registro")?
                .Elements("Row")
                .Select(td => new TransaccionData
                {
                    Id = int.Parse(td.GetAttributeValue("Transaccion")),
                    Descripcion = td.GetAttributeValue("DescTrans"),
                    Organizacion = new OrganizacionData()
                    {
                        Id = int.Parse(td.GetAttributeValue("Organizacion")),
                        Descripcion = td.GetAttributeValue("DescOrg")
                    },
                    Opcion = new OpcionData()
                    {
                        Id = int.Parse(td.GetAttributeValue("Opcion")),
                        Descripcion = td.GetAttributeValue("DescOpcion")
                    }
                })
                .ToList();

            retorno.Aplicaciones = rootEle
                .Element("Registro")?
                .Elements("Aplicacion")
                .Select(ad => new AplicacionData
                {
                    Id = int.Parse(ad.GetAttributeValue("IdAplicacion")),
                    Nombre = ad.GetAttributeValue("Nombre"),
                    Url = ad.GetAttributeValue("Url")
                })
                .ToList();

            return retorno;
        }

        private XDocument CrearCambioClaveRequest(CambioClaveRequest request)
        {
            var sesion = request.Sesion;

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
                    
                    new XAttribute("IdUsuario", request.UsuarioId),
                    new XAttribute("ClaveOld", request.ClaveAntigua),
                    new XAttribute("ClaveNew", request.ClaveNueva)
                )
            );

            return retorno;
        }

        private CambioClaveResponse CrearCambioClaveResponse(string response)
        {
            var retorno = new CambioClaveResponse()
            {
                Result = true,
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_CambioClave
            };

            var responseDoc = XDocument.Parse(response);

            var rootEle = responseDoc.Root;

            var codErr = rootEle.GetAttributeValue("CodError");
            var msjErr = rootEle.GetAttributeValue("MsgError");

            if (!(string.Compare(codErr, "0") == 0))
            {
                retorno.Result = false;
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;
            }

            return retorno;
        }

        private EncriptarResponse CrearEncriptarResponse(string response)
        {
            return new EncriptarResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_CambioClave,
                DatoEncriptado = response
            };
        }        

        #endregion

        #region ISeguridadAgenteService

        public RegistrarAplicacionResponse RegistrarAplicacion(RegistrarAplicacionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => RegistrarAplicacionAsync(request));
        }

        public async Task<RegistrarAplicacionResponse> RegistrarAplicacionAsync(RegistrarAplicacionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            RegistrarAplicacionResponse response = null;

            ServicioSeguridadSoapClient cliente = null;

            try
            {
                cliente = m_factory.CreateClient();

                var responseEncrip = await cliente.LoginAplicacionAsync(request.AplicacionId, request.UsuarioId);

                var responseDecrip = m_criptografiaService.Desencriptar(responseEncrip);

                response = CrearRegistrarAplicacionResponse(responseDecrip);

                await cliente.CloseAsync();
            }
            catch (CommunicationException cme)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegAppCommunication, cme);
                throw new InvalidOperationException(Mensajes.exc_RegAppCommunication);
            }
            catch (TimeoutException toe)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegAppTimeout, toe);
                throw new InvalidOperationException(Mensajes.exc_RegAppTimeout);
            }
            catch (Exception ex)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegAppGeneral, ex);
                throw new InvalidOperationException(Mensajes.exc_RegAppGeneral);
            }

            return response;
        }

        public RegistraUsuarioResponse RegistrarUsuario(RegistraUsuarioRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => RegistrarUsuarioAsync(request));
        }

        public async Task<RegistraUsuarioResponse> RegistrarUsuarioAsync(RegistraUsuarioRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            RegistraUsuarioResponse response = null;

            ServicioSeguridadSoapClient cliente = null;

            try
            {
                var requestDecrip = CrearRegistraUsuarioRequest(request);

                var xmlParam = m_criptografiaService.Encriptar(requestDecrip.ToString());

                cliente = m_factory.CreateClient();

                var responseEncrip = await cliente.RegistraUserAsync(xmlParam);

                var responseDecrip = m_criptografiaService.Desencriptar(responseEncrip);

                response = CrearRegistraUsuarioResponse(requestDecrip, responseDecrip);

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

        public CambioClaveResponse CambiarClave(CambioClaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => CambiarClaveAsync(request));
        }

        public async Task<CambioClaveResponse> CambiarClaveAsync(CambioClaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CambioClaveResponse response = null;

            ServicioSeguridadSoapClient cliente = null;

            try
            {
                var xmlParam = CrearCambioClaveRequest(request);

                cliente = m_factory.CreateClient();

                var cambioClaveResponse = await cliente.CambioClaveAsync(xmlParam.ToString());

                response = CrearCambioClaveResponse(cambioClaveResponse);

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

        public EncriptarResponse Encriptar(EncriptarRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => EncriptarAsync(request));
        }

        public async Task<EncriptarResponse> EncriptarAsync(EncriptarRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            EncriptarResponse response = null;

            ServicioSeguridadSoapClient cliente = null;

            try
            {
                cliente = m_factory.CreateClient();

                var encriptarResponse = await cliente.EncryptAsync(request.Original, request.Llave);

                response = CrearEncriptarResponse(encriptarResponse);

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

            //Liberar objetos no manejados

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FrameworkSeguridadAgenteService()
        {
            Dispose(false);
        }

        #endregion
    }
}

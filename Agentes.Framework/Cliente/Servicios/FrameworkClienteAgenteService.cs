using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes.Mensajes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Servicios;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Xml;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Servicios
{
    public class FrameworkClienteAgenteService : IClienteAgenteService
    {
        #region Campos

        private readonly ISoapClientFactory<ServicioClienteSoapClient> m_factory;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public FrameworkClienteAgenteService(
            ISoapClientFactory<ServicioClienteSoapClient> factory
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

        private string CrearResetearClaveRequest(ResetearClaveRequest request)
        {
            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Login",
                    new XAttribute("Usuario", request.UsuarioNombre),
                    new XAttribute("PS_IdUsuario", request.UsuarioId),
                    new XAttribute("PS_Token", request.Token),
                    new XAttribute("PS_Maquina", request.Maquina)
                )
            );

            return retorno.ToString();
        }

        private ResetearClaveResponse CrearResetearClaveResponse(ResetClaveResponse response)
        {
            var retorno = new ResetearClaveResponse()
            {
                Codigo = CodigosRespuesta.Ok,
                Mensaje = Mensajes.inf_Ok
            };

            var responseDoc = XDocument.Parse(response.Body.ResetClaveResult);

            var rootEle = responseDoc.Root;

            var codErr = rootEle.GetAttributeValue("CodError");
            var msjErr = rootEle.GetAttributeValue("MsgError");

            if (string.Compare(codErr, "0") == 0)
            {
                retorno.ClaveNueva = rootEle.GetAttributeValue("ClaveNueva");
            }
            else
            {
                retorno.Codigo = codErr;
                retorno.Mensaje = msjErr;
            }

            return retorno;
        }

        #endregion

        #region IClienteAgenteService

        public ResetearClaveResponse ResetearClave(ResetearClaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return AsyncHelper.RunSync(() => ResetearClaveAsync(request));
        }

        public async Task<ResetearClaveResponse> ResetearClaveAsync(ResetearClaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ResetearClaveResponse response = null;

            ServicioClienteSoapClient cliente = null;

            try
            {
                var xmlParam = CrearResetearClaveRequest(request);

                cliente = m_factory.CreateClient();

                var resClaveResponse = await cliente.ResetClaveAsync(xmlParam);

                response = CrearResetearClaveResponse(resClaveResponse);

                await cliente.CloseAsync();
            }
            catch (CommunicationException cme)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuCommunication, cme);
                throw new InvalidOperationException(Mensajes.exc_ResClaveCommunication);
            }
            catch (TimeoutException toe)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuTimeout, toe);
                throw new InvalidOperationException(Mensajes.exc_ResClaveTimeout);
            }
            catch (Exception ex)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_RegUsuGeneral, ex);
                throw new InvalidOperationException(Mensajes.exc_ResClaveGeneral);
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

        ~FrameworkClienteAgenteService()
        {
            Dispose(false);
        }

        #endregion
    }
}

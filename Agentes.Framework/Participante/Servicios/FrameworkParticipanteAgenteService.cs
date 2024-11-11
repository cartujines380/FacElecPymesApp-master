using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Servicios;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Agentes.Framework.Recursos;
using Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Agentes;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Xml;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Servicios
{
    public class FrameworkParticipanteAgenteService : IParticipanteAgenteService
    {
        #region Campos

        private readonly ISoapClientFactory<ServicioParticipanteSoapClient> m_factory;
        private bool m_desechado = false;

        #endregion

        #region Constructores

        public FrameworkParticipanteAgenteService(ISoapClientFactory<ServicioParticipanteSoapClient> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            m_factory = factory;
        }

        #endregion

        #region Metodos privados

        private DateTime? AFecha(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            DateTime aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out aux))
            {
                return aux;
            }

            return null;
        }

        private XDocument CrearObtenerParticipanteRequest(ObtenerParticipanteRequest request)
        {
            var sesion = request.Sesion;

            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Usuario",
                    new XAttribute("PS_IdUsuario", sesion.UsuarioId),
                    new XAttribute("PS_Token", string.Empty),
                    new XAttribute("PS_Maquina", sesion.Maquina),
                    new XAttribute("PS_UsrSitio", sesion.UsrSitio),
                    new XAttribute("PS_TokenSitio", sesion.TokenSitio),
                    new XAttribute("PS_MaqSitio", sesion.MaqSitio),
                    new XAttribute("PS_IdEmpresa", sesion.EmpresaId),
                    new XAttribute("PS_IdSucursal", sesion.SucursalId),
                    new XAttribute("PS_IdAplicacion", sesion.AplicacionId),
                    new XAttribute("PS_Formulario", string.Empty),
                    new XAttribute("PS_Login", sesion.Login),

                    new XAttribute("IdParticipante", request.ParticipanteId.ToString())
                )
            );

            return retorno;
        }

        private ObtenerParticipanteResponse CrearObtenerParticipanteResponse(int participanteId, string response)
        {
            var retorno = new ObtenerParticipanteResponse()
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

            if(!rootEle.HasElements)
            {
                retorno.Codigo = CodigosRespuesta.ErrorGeneral;
                retorno.Mensaje = "No existe registro  de participante";

                return retorno;
            }

            var firstEle = rootEle.Elements().First();

            var consSolResponse = new
            {
                TipoParticipante = firstEle.GetAttributeValue("TipoParticipante"),
                TipoPartRegistro = firstEle.GetAttributeValue("TipoPartRegistro"),
                TipoPartRegistroCod = firstEle.GetAttributeValue("DesTipoPartRegistro"),
                IdTipoIdentificacion = firstEle.GetAttributeValue("IdTipoIdentificacion"),
                Identificacion = firstEle.GetAttributeValue("Identificacion"),
                PrimerNombre = firstEle.GetAttributeValue("Nombre1"),
                SegundoNombre = firstEle.GetAttributeValue("Nombre2"),
                PrimerApellido = firstEle.GetAttributeValue("Apellido1"),
                SegundoApellido = firstEle.GetAttributeValue("Apellido2"),
                Genero = firstEle.GetAttributeValue("Sexo"),
                EstadoCivil = firstEle.GetAttributeValue("EstadoCivil"),
                FechaNacimiento = AFecha(firstEle.GetAttributeValue("FechaNacimiento")) ?? DateTime.Now,
                Ruc = firstEle.GetAttributeValue("Identificacion"),
                NombreEmpresa = firstEle.GetAttributeValue("Nombre"),
                IdUsuario = firstEle.GetAttributeValue("IdUsuario"),
                Correo = firstEle.GetAttributeValue("Correo"),
                Direccion = firstEle.GetAttributeValue("Direccion"),
                IdPais = firstEle.GetAttributeValue("IdPais"),
                IdProvincia = firstEle.GetAttributeValue("IdProvincia"),
                IdCiudad = firstEle.GetAttributeValue("IdCiudad"),
                Telefono = firstEle.GetAttributeValue("Telefono"),
                TelefonoAlt = firstEle.GetAttributeValue("DDI"),
                Comentario = firstEle.GetAttributeValue("Comentario")
            };

            var esPersona = (consSolResponse.TipoParticipante == "P");

            retorno.UsuarioId = consSolResponse.IdUsuario;

            retorno.Participante = new ParticipanteData()
            {
                Id = participanteId,
                Tipo = consSolResponse.TipoParticipante,
                TipoResgistro = consSolResponse.TipoPartRegistro,
                Identificacion = new IdentificacionData()
                {
                    Tipo = consSolResponse.IdTipoIdentificacion,
                    Codigo = (esPersona ? consSolResponse.Identificacion : consSolResponse.Ruc)
                }
            };

            //consSolResponse.TipoPartRegistro;

            if (esPersona)
            {
                retorno.Persona = new PersonaData()
                {
                    PrimerNombre = consSolResponse.PrimerNombre,
                    SegundoNombre = consSolResponse.SegundoNombre,
                    PrimerApellido = consSolResponse.PrimerApellido,
                    SegundoApellido = consSolResponse.SegundoApellido,
                    Genero = consSolResponse.Genero,
                    EstadoCivil = consSolResponse.EstadoCivil,
                    FechaNacimiento = consSolResponse.FechaNacimiento
                };
            }
            else
            {
                retorno.Empresa = new EmpresaData()
                {
                    Nombre = consSolResponse.NombreEmpresa
                };
            }

            retorno.Contacto = new ContactoData()
            {
                Correo = consSolResponse.Correo,
                Direccion = consSolResponse.Direccion,
                PaisId = consSolResponse.IdPais,
                ProvinciaId = consSolResponse.IdProvincia,
                CiudadId = consSolResponse.IdCiudad,
                Telefono = consSolResponse.Telefono,
                TelefonoAlternativo = consSolResponse.TelefonoAlt,
                Comentario = consSolResponse.Comentario
            };

            return retorno;
        }

        private XDocument CrearObtenerRegistroUsuarioRequest(ObtenerParticipanteRequest request, string usuarioId)
        {
            var sesion = request.Sesion;

            var retorno = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "yes"),
                new XElement("Usuario",
                    new XAttribute("PS_IdUsuario", sesion.UsuarioId),
                    new XAttribute("PS_Token", string.Empty),
                    new XAttribute("PS_Maquina", sesion.Maquina),
                    new XAttribute("PS_UsrSitio", sesion.UsrSitio),
                    new XAttribute("PS_TokenSitio", sesion.TokenSitio),
                    new XAttribute("PS_MaqSitio", sesion.MaqSitio),
                    new XAttribute("PS_IdEmpresa", sesion.EmpresaId),
                    new XAttribute("PS_IdSucursal", sesion.SucursalId),
                    new XAttribute("PS_IdAplicacion", sesion.AplicacionId),
                    new XAttribute("PS_Formulario", string.Empty),
                    new XAttribute("PS_Login", sesion.Login),

                    new XAttribute("IdUsuario", usuarioId)
                )
            );

            return retorno;
        }

        private void LlenarObtenerParticipanteResponse(ObtenerParticipanteResponse response, string regUsuResponse)
        {
            var responseDoc = XDocument.Parse(regUsuResponse);

            var rootEle = responseDoc.Root;

            var codErr = rootEle.GetAttributeValue("CodError");
            var msjErr = rootEle.GetAttributeValue("MsgError");

            if (!(string.Compare(codErr, "0") == 0))
            {
                response.Codigo = codErr;
                response.Mensaje = msjErr;

                return;
            }

            if (!rootEle.HasElements)
            {
                response.Codigo = CodigosRespuesta.ErrorGeneral;
                response.Mensaje = "No existe registros de usuario";

                return;
            }

            var firstEle = rootEle.Elements().First();

            response.Estado = firstEle.GetAttributeValue("Estado");
        }

        #endregion

        #region IParticipanteAgenteService

        public ObtenerParticipanteResponse ObtenerParticipante(ObtenerParticipanteRequest request)
        {
            return AsyncHelper.RunSync(() => ObtenerParticipanteAsync(request));
        }

        public async Task<ObtenerParticipanteResponse> ObtenerParticipanteAsync(ObtenerParticipanteRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ObtenerParticipanteResponse response = null;

            ServicioParticipanteSoapClient cliente = null;

            try
            {
                var xmlParam = CrearObtenerParticipanteRequest(request).ToString();

                cliente = m_factory.CreateClient();

                var solPartResponse = await cliente.ConsSolicitudPartAsync(xmlParam);

                response = CrearObtenerParticipanteResponse(request.ParticipanteId, solPartResponse);

                var xmpParamRegUsu = CrearObtenerRegistroUsuarioRequest(request, response.UsuarioId).ToString();

                var regUsResponse = await cliente.ConsUsuarioRegistroAsync(xmpParamRegUsu);

                LlenarObtenerParticipanteResponse(response, regUsResponse);

                await cliente.CloseAsync();
            }
            catch (CommunicationException cme)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_ObtPartCommunication, cme);
                throw new InvalidOperationException(Mensajes.exc_ObtPartCommunication);
            }
            catch (TimeoutException toe)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_ObtPartTimeout, toe);
                throw new InvalidOperationException(Mensajes.exc_ObtPartTimeout);
            }
            catch (Exception ex)
            {
                if (cliente != null)
                {
                    cliente.Abort();
                }

                //m_logger.Error(Mensajes.exc_ObtPartGeneral, ex);
                throw new InvalidOperationException(Mensajes.exc_ObtPartGeneral);
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
                //if (m_cliente != null)
                //{
                //    m_cliente = null;
                //}
            }

            //Liberar objetos no manejados

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FrameworkParticipanteAgenteService()
        {
            Dispose(false);
        }

        #endregion

    }
}

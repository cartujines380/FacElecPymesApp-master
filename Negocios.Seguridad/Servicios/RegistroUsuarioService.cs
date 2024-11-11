using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.AccesoDatos.Seguridad;
using AgtSegMsj = Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios;
using AgtCliMsj = Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;
using NegPriRec = Sipecom.FactElec.Pymes.Negocios.Seguridad.Recursos;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Mensajeria;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class RegistroUsuarioService : IRegistroUsuarioService
    {
        #region Campos

        private readonly IConfiguracionServicioSeguridad m_configuracion;
        private readonly ISeguridadAgenteService m_seguridadAgSrv;
        private readonly IDatoAplicacionService m_datoAplicacionService;
        private readonly IDatoSesionService m_datoSesionService;
        private readonly IClienteAgenteService m_clienteAgenteService;
        private readonly IUsuarioRepository m_usuarioRepository;
        private readonly IEstablecimientoRepository m_establecimientoRepository;
        private readonly IEmailSender m_emailSender;

        private bool m_desechado = false;

        #endregion

        #region Contructores
        
        public RegistroUsuarioService(
            IConfiguracionServicioSeguridad configuracion,
            ISeguridadAgenteService seguridadAgenteService,
            IDatoAplicacionService datoAplicacionService,
            IDatoSesionService datoSesionService,
            IClienteAgenteService clienteAgenteService,
            IUsuarioRepository usuarioRepository,
            IEstablecimientoRepository establecimientoRepository,
            IEmailSender emailSender
        )
        {
            m_configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            m_seguridadAgSrv = seguridadAgenteService ?? throw new ArgumentNullException(nameof(seguridadAgenteService));
            m_datoAplicacionService = datoAplicacionService ?? throw new ArgumentNullException(nameof(datoAplicacionService));
            m_datoSesionService = datoSesionService ?? throw new ArgumentNullException(nameof(datoSesionService));
            m_clienteAgenteService = clienteAgenteService ?? throw new ArgumentNullException(nameof(clienteAgenteService));
            m_usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            m_establecimientoRepository = establecimientoRepository ?? throw new ArgumentNullException(nameof(establecimientoRepository));
            m_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));

            m_configuracion.Initialize();
        }

        #endregion

        #region Metodos privados

        private AgtSegMsj.RegistraUsuarioRequest ObtenerRegistrarUsuarioRequest(string usuario, string contrasena)
        {
            var appData = m_datoAplicacionService.ObtenerAplicacionData();

            if (appData == null)
            {
                throw new InvalidOperationException("Sesion de aplicacion no existe");
            }

            return new AgtSegMsj.RegistraUsuarioRequest
            {
                UsuarioId = usuario,
                Clave = contrasena,
                Maquina = m_configuracion.Maquina,
                SitioUsuario = m_configuracion.UsuarioSitio,
                SitioToken = appData.SitioToken,
                SitioMaquina = appData.SitioMaquina,
                EmpresaId = m_configuracion.EmpresaId,
                SucursalId = m_configuracion.SucursalId,
                AplicacionId = m_configuracion.AplicacionId,
                Dominio = m_configuracion.Dominio,
                Perfil = m_configuracion.Perfil
            };
        }

        private UsuarioData CrearUsuario(AgtSegMsj.RegistraUsuarioResponse response, string usuario)
        {
            return new UsuarioData()
            {
                Id = response.Participante.Id,
                Nombre = response.Participante.Nombre,
                NombreUsuario = usuario,
                Token = response.Token,
                Maquina = response.Maquina
            };
        }

        private AgtSegMsj.CambioClaveRequest ObtenerCambiarClaveRequest(string usuario, string claveAnt, string claveNueva)
        {
            var sesion = m_datoSesionService.ObtenerSesionAgente();

            return new AgtSegMsj.CambioClaveRequest()
            {
                UsuarioId = usuario,
                ClaveAntigua = claveAnt,
                ClaveNueva = claveNueva,
                Sesion = sesion
            };
        }

        private AgtCliMsj.ResetearClaveRequest ObtenerResetearClaveRequest(string usuario)
        {
            //var sesion = m_datoSesionService.ObtenerSesionAgente();
            var appData = m_datoAplicacionService.ObtenerAplicacionData();
            return new AgtCliMsj.ResetearClaveRequest()
            {
                UsuarioNombre = usuario,
                UsuarioId = appData.UsuarioId,
                Maquina = appData.Maquina,
                Token = appData.Token
            };
        }

        private string LeerCorreoPlantilla()
        {
            if (File.Exists(m_configuracion.CorreoPlantillaRuta))
            {
                return File.ReadAllText(m_configuracion.CorreoPlantillaRuta);
            }
            else
            {
                //Loguear archivo no existe.
            }

            return string.Empty;
        }

        private string ObtenerCuerpoMensaje(string usuarioId, string usuarioNombre, string clave)
        {
            var retorno = LeerCorreoPlantilla();

            retorno = retorno.Replace("@Texto_Usuario", usuarioId);
            retorno = retorno.Replace("@Texto1_Nombres", usuarioNombre);
            retorno = retorno.Replace("@Texto2_Numero", clave);

            return retorno;
        }

        private bool EnviarCorreo(string usuarioId, string clave)
        {
            var retorno = false;

            try
            {
                var estb = m_establecimientoRepository.ObtenerEstablecimientosPorUsuario(usuarioId).FirstOrDefault();

                if (
                    (estb == null)
                    || string.IsNullOrEmpty(estb.CorreoReset)
                )
                {
                    throw new InvalidOperationException("No hay destinatarios de correo");
                }

                var destinatariosMsj = new List<string>(estb.CorreoReset.Split(";"));

                var cuerpoMsj = ObtenerCuerpoMensaje(usuarioId, estb.RazonSocial, clave);

                var mail = new MailMessage
                {
                    FromAddress = m_configuracion.CorreoRemitente,
                    FromName = m_configuracion.CorreoRemitente,
                    ToAddress = destinatariosMsj,
                    Subject = "Recuperar contraseña",
                    Body = cuerpoMsj
                };

                m_emailSender.SendEmail(mail);

                retorno = true;
            }
            catch (Exception ex)
            {
                //Loguear error
            }

            return retorno;
        }

        #endregion

        #region IRegistroUsuarioService

        public AutenticarResponse Autenticar(string usuario, string contrasena)
        {
            return AsyncHelper.RunSync(() => AutenticarAsync(usuario, contrasena));
        }

        public async Task<AutenticarResponse> AutenticarAsync(string usuario, string contrasena)
        {
            var retorno = new AutenticarResponse();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                retorno.AgregarValidacion(NegPriRec.Mensajes.val_UsuarioContrasenaVacias);
                return retorno;
            }

            var request = ObtenerRegistrarUsuarioRequest(usuario, contrasena);

            var response = await m_seguridadAgSrv.RegistrarUsuarioAsync(request);

            if (response.Codigo == NegPriRec.CodigosRespuesta.Ok)
            {
                retorno.Usuario = CrearUsuario(response, usuario);
            }
            else
            {
                retorno.AgregarValidacion(response.Mensaje);
            }

            return retorno;
        }

        public CambioClaveResponse CambiarClave(string usuario, string claveAnt, string claveNueva, string confirmarClave)
        {
            var retorno = new CambioClaveResponse();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(claveAnt))
            {
                retorno.AgregarValidacion(NegPriRec.Mensajes.val_UsuarioContrasenaVacias);
                return retorno;
            }

            if (string.IsNullOrEmpty(claveNueva) || string.IsNullOrEmpty(confirmarClave))
            {
                retorno.AgregarValidacion(NegPriRec.Mensajes.val_ContrasenaNuevaVacia);
                return retorno;
            }

            if (claveNueva != confirmarClave)
            {
                retorno.AgregarValidacion(NegPriRec.Mensajes.val_ContrasenaNoSonIguales);
                return retorno;
            }

            m_usuarioRepository.CambiarClave(usuario, claveAnt, claveNueva);

            return retorno;
        }

        public Task<CambioClaveResponse> CambiarClaveAsync(string usuario, string claveAnt, string claveNueva, string confirmarClave)
        {
            return Task.FromResult(CambiarClave(usuario, claveAnt, claveNueva, confirmarClave));
        }

        public ResetearClaveResponse ResetearClave(string usuario)
        {
            return AsyncHelper.RunSync(() => ResetearClaveAsync(usuario));
        }

        public async Task<ResetearClaveResponse> ResetearClaveAsync(string usuario)
        {
            var retorno = new ResetearClaveResponse();

            if (string.IsNullOrEmpty(usuario))
            {
                retorno.AgregarValidacion(NegPriRec.Mensajes.val_UsuarioContrasenaVacias);
                return retorno;
            }

            var request = ObtenerResetearClaveRequest(usuario);

            var response = await m_clienteAgenteService.ResetearClaveAsync(request);

            if (!(response.Codigo == NegPriRec.CodigosRespuesta.Ok))
            {
                retorno.AgregarValidacion(response.Mensaje);
                return retorno;
            }

            retorno.Codigo = response.Codigo;
            retorno.Mensaje = response.Mensaje;

            if (EnviarCorreo(usuario, response.ClaveNueva))
            {
                m_usuarioRepository.ResetearClave(usuario, response.ClaveNueva);
            }

            return retorno;
        }

        #endregion

        #region IDispose

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                m_seguridadAgSrv.Dispose();
                m_datoSesionService.Dispose();
                m_clienteAgenteService.Dispose();
                m_usuarioRepository.Dispose();
                m_establecimientoRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RegistroUsuarioService()
        {
            Dispose(false);
        }

        #endregion
    }
}

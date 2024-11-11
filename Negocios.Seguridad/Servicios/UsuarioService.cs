using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Servicios;
using Sipecom.FactElec.Pymes.Entidades.Seguridad;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Recursos;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        #region Campos

        private readonly IConfiguracionServicioSeguridad m_configuracion;
        private readonly IDatoAplicacionService m_datoAplicacionService;
        private readonly IParticipanteAgenteService m_participanteAgSrv;

        private bool m_desechado = false;

        #endregion

        #region Constructores

        public UsuarioService(
            IConfiguracionServicioSeguridad configuracion,
            IDatoAplicacionService datoAplicacionService,
            IParticipanteAgenteService participanteAgSrv
        )
        {
            m_configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            m_datoAplicacionService = datoAplicacionService ?? throw new ArgumentNullException(nameof(datoAplicacionService));
            m_participanteAgSrv = participanteAgSrv ?? throw new ArgumentNullException(nameof(participanteAgSrv));

            m_configuracion.Initialize();
        }

        #endregion

        #region Metodos privados

        private ObtenerParticipanteRequest CrearRequest(int usuarioId)
        {
            var appData = m_datoAplicacionService.ObtenerAplicacionData();

            return new ObtenerParticipanteRequest()
            {
                ParticipanteId = usuarioId,
                Sesion = new Sesion()
                {
                    AplicacionId = m_configuracion.AplicacionId,
                    EmpresaId = m_configuracion.EmpresaId,
                    SucursalId = m_configuracion.SucursalId,
                    UsuarioId = m_configuracion.UsuarioSitio,
                    Login = m_configuracion.Login,
                    Maquina = m_configuracion.Maquina,
                    TokenSitio = appData != null ? appData.SitioToken : "",
                    MaqSitio = appData != null ? appData.SitioMaquina : "",
                    UsrSitio = m_configuracion.UsuarioSitio
                }
            };
        }

        private Usuario CrearUsuario(ObtenerParticipanteResponse response)
        {
            var retorno = UsuarioFactory.CrearUsuario(response.Participante.Tipo);

            if (retorno == null)
            {
                return null;
            }

            retorno.ChangeCurrentIdentity(response.Participante.Id);

            retorno.Identificacion = new Identificacion()
            {
                Tipo = response.Participante.Identificacion.Tipo,
                Codigo = response.Participante.Identificacion.Codigo
            };

            retorno.Estado = response.Estado;
            retorno.Nombre = response.UsuarioId;

            retorno.TipoParticipante = response.Participante.TipoResgistro;

            retorno.DatosContacto = new Contacto()
            {
                Direccion = response.Contacto.Direccion,
                Telefono = response.Contacto.Telefono,
                CorreoElectronico = response.Contacto.Correo,
                PaisCodigo = response.Contacto.PaisId,
                ProvinciaCodigo = response.Contacto.ProvinciaId,
                CiudadCodigo = response.Contacto.CiudadId,
                Comentario = response.Contacto.Comentario
            };

            if (retorno is Persona)
            {
                var persona = retorno as Persona;

                persona.PrimerApellido = response.Persona.PrimerApellido;
                persona.SegundoApellido = response.Persona.SegundoApellido;
                persona.PrimerNombre = response.Persona.PrimerNombre;
                persona.SegundoNombre = response.Persona.SegundoNombre;
                persona.GeneroCodigo = response.Persona.Genero;
                persona.FechaNacimiento = response.Persona.FechaNacimiento.Value;
                persona.EstadoCivilCodigo = response.Persona.EstadoCivil;

            }
            else if (retorno is Empresa)
            {
                var empresa = retorno as Empresa;

                empresa.NombreEmpresa = response.Empresa.Nombre;
            }

            return retorno;
        }

        #endregion

        #region IUsuarioService

        public Usuario ObtenerUsuario(int usuarioId)
        {
            var request = CrearRequest(usuarioId);

            var response = m_participanteAgSrv.ObtenerParticipante(request);

            if (response.Codigo != CodigosRespuesta.Ok)
            {
                throw new Exception(response.Mensaje);
            }

            return CrearUsuario(response);
        }

        public async Task<Usuario> ObtenerUsuarioAsync(int usuarioId)
        {
            var request = CrearRequest(usuarioId);

            var response = await m_participanteAgSrv.ObtenerParticipanteAsync(request);

            if (response.Codigo != CodigosRespuesta.Ok)
            {
                throw new Exception(response.Mensaje);
            }

            return CrearUsuario(response);
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
                m_participanteAgSrv.Dispose();
            }

            //Liberar objetos no manejados

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UsuarioService()
        {
            Dispose(false);
        }

        #endregion
    }
}

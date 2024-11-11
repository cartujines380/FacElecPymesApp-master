using System;
using System.Linq;
using System.Threading.Tasks;

using AgtSegMsj = Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Recursos;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public class RegistroAplicacionService : IRegistroAplicacionService
    {
        #region Campos

        private readonly IConfiguracionServicioSeguridad m_configuracion;
        private readonly ISeguridadAgenteService m_seguridadAgSrv;
        private bool m_desechado = false;

        #endregion

        #region Contructores

        public RegistroAplicacionService(
            IConfiguracionServicioSeguridad configuracion,
            ISeguridadAgenteService seguridadAgenteService
        )
        {
            m_configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            m_seguridadAgSrv = seguridadAgenteService ?? throw new ArgumentNullException(nameof(seguridadAgenteService));

            m_configuracion.Initialize();
        }

        #endregion

        #region Metodos privados

        private AgtSegMsj.RegistrarAplicacionRequest CrearRequest()
        {
            return new AgtSegMsj.RegistrarAplicacionRequest()
            {
                AplicacionId = m_configuracion.AplicacionId,
                UsuarioId = m_configuracion.UsuarioSitio
            };
        }

        private AplicacionData CrearAplicacionData(AgtSegMsj.RegistrarAplicacionResponse response)
        {
            return new AplicacionData()
            {
                Id = m_configuracion.AplicacionId,
                UsuarioId = response.UsuarioId,//m_configuracion.UsuarioSitio,
                Maquina = response.Maquina,
                Token = response.Token,
                SitioMaquina = response.SitioMaquina,
                SitioToken = response.SitioToken,
                Servidores = response.Servidores.Select(s => new ServidorData
                {
                    Id = s.Id,
                    Tipo = s.Tipo,
                    Parametros = s.Parametros
                })
                .ToList()
            };
        }

        #endregion

        #region IRegistroAplicacionService

        public AutenticarAplicacionResponse AutenticarAplicacion()
        {
            var retorno = new AutenticarAplicacionResponse();

            var request = CrearRequest();

            var response = m_seguridadAgSrv.RegistrarAplicacion(request);

            if (response.Codigo == CodigosRespuesta.Ok)
            {
                retorno.Aplicacion = CrearAplicacionData(response);
            }
            else
            {
                retorno.AgregarValidacion(response.Mensaje);
            }

            return retorno;
        }

        public async Task<AutenticarAplicacionResponse> AutenticarAplicacionAsync()
        {
            var retorno = new AutenticarAplicacionResponse();

            var request = CrearRequest();

            var response = await m_seguridadAgSrv.RegistrarAplicacionAsync(request);

            if (response.Codigo == CodigosRespuesta.Ok)
            {
                retorno.Aplicacion = CrearAplicacionData(response);
            }
            else
            {
                retorno.AgregarValidacion(response.Mensaje);
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
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RegistroAplicacionService()
        {
            Dispose(false);
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;
using LoggerFactory = Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo.LoggerFactory;
using ILogger = Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo.ILogger;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        #region Campos

        private static readonly ILogger m_logger = LoggerFactory.CreateLogger<EstablecimientoController>();

        private readonly IEstablecimientoService m_establecimientoService;
        private readonly IAutenticacionService m_autenticacionService;
        private readonly string usuarioId;

        #endregion

        #region Constructores

        public EstablecimientoController(
            IEstablecimientoService establecimientoService,
            IAutenticacionService authorizationService
        )
        {
            m_establecimientoService = establecimientoService ?? throw new ArgumentNullException(nameof(establecimientoService));
            m_autenticacionService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        #endregion

        [HttpGet("obtenerestablecimientocmb")]
        public IActionResult ObtenerEstablecimientosPorUsuarioCmb()
        {
            m_logger.Info("si vale Pichu");
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();

            var response = m_establecimientoService.ObtenerEstablecimientosPorUsuarioCmb(usuario.NombreUsuario);
            return Ok(response);
        }

        [HttpGet("obtenerestablecimiento")]
        public IActionResult ObtenerEstablecimientosPorUsuario()
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();

            var response = m_establecimientoService.ObtenerEstablecimientosPorUsuario(usuario.NombreUsuario);
            return Ok(response);
        }

        [HttpGet("obtenerplan/{empresaRuc}")]
        public IActionResult Obtenerplan(string empresaRuc)
        {
            var response = m_establecimientoService.Obtenerplan(empresaRuc);
            return Ok(response);
        }

        [HttpGet("obtenerestablecimientotransportista")]
        public IActionResult ObtenerDataEstablecimientosTransportitasPorUsuario()
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            var response = m_establecimientoService.ObtenerDataEstablecimientosTransportitasPorUsuario(usuario.NombreUsuario);
            return Ok(response);
        }

        [HttpPost("obtenerdireccionsucursal")]
        public IActionResult ObtenerDireccionSucursal(InfoComprobanteModel info)
        {
            var response = m_establecimientoService.ObtenerDireccionSucursal(info);
            return Ok(response);
        }
    }
}

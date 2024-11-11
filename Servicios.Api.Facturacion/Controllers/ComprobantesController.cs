using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        #region Campos

        private readonly IComprobanteService m_comprobanteService;
        private readonly IAutenticacionService m_autenticacionService;

        #endregion

        #region Constructores

        public ComprobantesController(IComprobanteService comprobanteService,
            IAutenticacionService authorizationService)
        {
            m_comprobanteService = comprobanteService ?? throw new ArgumentNullException(nameof(comprobanteService));
            m_autenticacionService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        #endregion

        [HttpPost("obtenercomprobantes")]
        public IActionResult ObtenerComprobantes(FiltroModel request)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            request.IdUsuario = usuario.NombreUsuario;

            var comprobantes = m_comprobanteService.ObtenerComprobantes(request);

            return Ok(comprobantes);
        }

        [HttpPost("obtenercomprobantesxml")]
        public IActionResult ObtenerComprobantesXML(InfoComprobanteModel request)
        {
            var comprobantes = m_comprobanteService.ObtenerComprobantesXML(request);

            return Ok(comprobantes);
        }
    }
}

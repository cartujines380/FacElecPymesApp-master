using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Mensajes;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        #region Campos

        private readonly IFacturaService m_facturaService;
        private readonly IAutenticacionService m_autenticacionService;

        #endregion

        #region Constructores

        public FacturaController(
            IFacturaService facturaService,
            IAutenticacionService authorizationService
        )
        {
            m_facturaService = facturaService ?? throw new ArgumentNullException(nameof(facturaService));
            m_autenticacionService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        #endregion

        #region Numero de Factura

        [HttpGet("obtenerestablecimiento/{ruc}/{esTransportista}")]
        public IActionResult ObtenerEstablecimiento(string ruc, string esTransportista)
        {
            var response = m_facturaService.ObtenerEstablecimiento(ruc, esTransportista);

            return Ok(response);
        }

        [HttpGet("obtenerpuntoemision/{ruc}/{establecimiento}/{esTransportista}")]
        public IActionResult ObtenerPtoEmision(string ruc, string establecimiento, string esTransportista)
        {
            var response = m_facturaService.ObtenerPtoEmision(ruc, establecimiento, esTransportista);
            
            return Ok(response);
        }

        [HttpPost("obtenersecuencial")]
        public IActionResult ObtenerSecuencial(ObtenerSecuencialRequest request)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            var usuarioId = usuario.NombreUsuario;
            request.Usuario = usuarioId;
            var response = m_facturaService.ObtenerSecuencial(request);

            return Ok(response);
        }

        [HttpGet("obtenertotalcomprobante/{ruc}/{estado}")]
        public IActionResult ObtenerTotalComprobante(string ruc, string estado)
        {
            var response = m_facturaService.ObtenerTotalComprobante(ruc, estado);

            return Ok(response);
        }

        [HttpGet("obtenertotalcomprobanteportipo/{ruc}/{estado}/{tipoDoc}")]
        public IActionResult ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc)
        {
            var response = m_facturaService.ObtenerTotalComprobantePorTipo(ruc, estado, tipoDoc);

            return Ok(response);
        }

        [HttpPost("guardar")]
        public IActionResult GuardarFactura(GuardarFacturaRequest request)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            var usuarioId = usuario.NombreUsuario;
            request.Usuario = usuarioId;
            var response = m_facturaService.GuardarFactura(request);

            return Ok(response);
        }

        #endregion

    }
}

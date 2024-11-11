using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        #region Campos

        private readonly IClienteService m_clienteService;
        private readonly IAutenticacionService m_autenticacionService;

        #endregion

        #region Constructores

        public ClienteController(
            IClienteService clienteService,
            IAutenticacionService authorizationService
        )
        {
            m_clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            m_autenticacionService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        #endregion

        [HttpPost("obtenerclientesporempresa")]
        public IActionResult ObtenerClientesPorEmpresa(ObtenerClientesPorEmpresaRequest request)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            request.IdUsuario = usuario.NombreUsuario;

            var response = m_clienteService.ObtenerClientesPorEmpresa(request);
            return Ok(response);
        }

        [HttpGet("estransportista")]
        public IActionResult EsTransportista()
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();

            var response = m_clienteService.EsTransportista(usuario.NombreUsuario);
            return Ok(response);
        }

        [HttpPost("guardarcliente")]
        public IActionResult GuardarCliente(Cliente entity)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            entity.IdUsuario = usuario.NombreUsuario;

            var response = m_clienteService.Add(entity);
            return Ok(response);
        }

        [HttpPost("obtenerdireccioncliente")]
        public IActionResult ObtenerDireccionCliente(InfoComprobanteModel info)
        {
            var response = m_clienteService.ObtenerDireccionCliente(info);
            return Ok(response);
        }
    }
}

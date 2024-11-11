using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        #region Campos

        private readonly IArticuloService m_articuloService;
        private readonly IAutenticacionService m_autenticacionService;

        #endregion

        #region Constructores

        public ArticuloController(
            IArticuloService articuloService,
             IAutenticacionService authorizationService
        )
        {
            m_articuloService = articuloService ?? throw new ArgumentNullException(nameof(articuloService));
            m_autenticacionService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        #endregion

        [HttpGet("obtenerarticuloporempresa/{criterioBusquedad}/{ruc}/{esTransportista}")]
        public IActionResult ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista)
        {
            var usuario = m_autenticacionService.ObtenerUsuarioDataAutenticado();
            var response = m_articuloService.ObtenerArticulosPorEmpresa(criterioBusquedad, ruc, esTransportista, usuario.NombreUsuario);
            return Ok(response);
        }

        [HttpPost("guardararticulo")]
        public IActionResult GuardarArticulo(Articulo entity)
        {
            var response = m_articuloService.Add(entity);
            return Ok(response);
        }
    }
}

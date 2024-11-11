using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadMedidaController : ControllerBase
    {
        #region Campos

        private readonly IUnidadMedidaService m_unidadMedidaService;

        #endregion

        #region Constructores

        public UnidadMedidaController(
            IUnidadMedidaService unidadMedidaService
        )
        {
            m_unidadMedidaService = unidadMedidaService ?? throw new ArgumentNullException(nameof(unidadMedidaService));
        }

        [HttpGet("obtenerunidadmedidaporarticulo/{ruc}/{codArticulo}")]
        public IActionResult ObtenerBodegaPorEmpresa(string ruc, string codArticulo)
        {
            var response = m_unidadMedidaService.ObtenerUnidadMedidaPorArticulo(ruc, codArticulo);
            return Ok(response);
        }

        #endregion
    }
}

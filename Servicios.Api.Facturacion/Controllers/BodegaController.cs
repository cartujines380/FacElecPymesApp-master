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
    public class BodegaController : ControllerBase
    {
        #region Campos

        private readonly IBodegaService m_bodegaService;

        #endregion

        #region Constructores

        public BodegaController(
            IBodegaService bodegaService
        )
        {
            m_bodegaService = bodegaService ?? throw new ArgumentNullException(nameof(bodegaService));
        }

        [HttpGet("obtenerbodegaporempresa/{ruc}")]
        public IActionResult ObtenerBodegaPorEmpresa(string ruc)
        {
            var response = m_bodegaService.ObtenerBodegaPorEmpresa(ruc);
            return Ok(response);
        }

        #endregion
    }
}

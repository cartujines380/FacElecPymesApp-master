using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        #region Campos

        private readonly ICatalogoService m_catalogoService;

        #endregion

        #region Constructores

        public CatalogoController(
            ICatalogoService catalagoService
        )
        {
            m_catalogoService = catalagoService ?? throw new ArgumentNullException(nameof(catalagoService));
        }

        #endregion

        [HttpGet("obtener/{nombreTabla}")]
        public IActionResult ObtenerCatalogo(string nombreTabla)
        {
            var response = m_catalogoService.ObtenerCatalogo(nombreTabla);
            return Ok(response);
        }

        [HttpGet("obtenerimpuestoportipo/{tipo}")]
        public IActionResult ObtenerImpuestoPorCodigo(string tipo)
        {
            var response = m_catalogoService.ObtenerImpuestoPorCodigo(tipo);
            return Ok(response);
        }

        [HttpGet("obtenerpais/{codigo}")]
        public IActionResult ObtenerCatalogoPais(int codigo)
        {
            var response = m_catalogoService.ObtenerCatalogoPais(codigo);
            return Ok(response);
        }

        [HttpGet("obtenerprovinciaciudad/{codigo}/{descAlterno}")]
        public IActionResult ObtenerCatalogoProvinciaCiudad(int codigo, string descAlterno)
        {
            var response = m_catalogoService.ObtenerCatalogoProvinciaCiudad(codigo, descAlterno);
            return Ok(response);
        }

        [HttpGet("obtenergeneral/{codigo}")]
        public IActionResult ObtenerCatalogoGeneral(int codigo)
        {
            var response = m_catalogoService.ObtenerCatalogoGeneral(codigo);
            return Ok(response);
        }

        [HttpGet("obtenerformapago")]
        public IActionResult ObtenerFormaPago()
        {
            var response = m_catalogoService.ObtenerFormaPago();
            return Ok(response);
        }

    }
}

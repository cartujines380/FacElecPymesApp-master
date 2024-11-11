using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            // Aquí puedes personalizar la lógica para mostrar la vista de error apropiada.
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

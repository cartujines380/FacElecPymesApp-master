using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Helpers;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService m_interaction;
        private readonly IWebHostEnvironment m_environment;
        private readonly ILogger m_logger;

        public HomeController(
            IIdentityServerInteractionService interaction,
            IWebHostEnvironment environment,
            ILogger<HomeController> logger
        )
        {
            m_interaction = interaction;
            m_environment = environment;
            m_logger = logger;
        }

        public IActionResult Index()
        {
            if (m_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            m_logger.LogInformation("Pagina inicio deshabilitada. Returning 404.");
            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await m_interaction.GetErrorContextAsync(errorId);

            if (message != null)
            {
                vm.Error = message;

                if (!m_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
    }
}

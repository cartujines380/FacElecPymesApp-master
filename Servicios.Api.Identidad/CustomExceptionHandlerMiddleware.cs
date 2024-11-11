using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Controllers;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Aquí puedes realizar cualquier lógica adicional que necesites antes de redirigir a la página de error.

                // Por ejemplo, puedes redirigir a una página de error personalizada.
                context.Response.Redirect(Path.Combine("/Error", "Index"));
                return;
            }
        }
    }
}

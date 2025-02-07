﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Helpers
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                var headers = context.HttpContext.Response.Headers;

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!headers.ContainsKey("X-Content-Type-Options"))
                {
                    headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                if (!headers.ContainsKey("X-Frame-Options"))
                {
                    headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
                // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
                //csp += "upgrade-insecure-requests;";
                // also an example if you need client images to be displayed from twitter
                // csp += "img-src 'self' https://pbs.twimg.com;";

                // once for standards compliant browsers
                if (!headers.ContainsKey("Content-Security-Policy"))
                {
                    headers.Add("Content-Security-Policy", csp);
                }
                // and once again for IE
                if (!headers.ContainsKey("X-Content-Security-Policy"))
                {
                    headers.Add("X-Content-Security-Policy", csp);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                var referrer_policy = "no-referrer";

                if (!headers.ContainsKey("Referrer-Policy"))
                {
                    headers.Add("Referrer-Policy", referrer_policy);
                }
            }
        }
    }
}

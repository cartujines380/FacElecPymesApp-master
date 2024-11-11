using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Autofac;
using Sipecom.FactElec.Pymes.Servicios.Api.Base.Services;
using Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Infrastructure;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion
{
    public class Startup
    {
        private const string POLICY_SCOPE = "ApiScope";
        private const string FACTURACION_API = "Servicios.Api.Facturacion";

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            //Obtiene url de servidor de identidad
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            //Adiciona servicio de autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = identityUrl;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    if (Environment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(POLICY_SCOPE, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", FACTURACION_API);
                });
            });

            //Adicion de implementacion de IHttpContextAccessor
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();//Adicionado
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization(POLICY_SCOPE);
            });

            lifetime.ApplicationStarted.Register(() =>
            {
                // Perform post-startup activities here
                var appSrv = app.ApplicationServices.GetService<IAplicacionService>();
                appSrv.IniciaSesion();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                // Perform on-stopping activities here
            });

            lifetime.ApplicationStopped.Register(() =>
            {
                // Perform post-stopped activities here
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DependenciasModule());
        }
    }
}

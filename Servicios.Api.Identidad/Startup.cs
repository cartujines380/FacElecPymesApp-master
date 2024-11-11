using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Sipecom.FactElec.Pymes.Servicios.Api.Base.Services;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Configuracion;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Infrastructure;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Services;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad
{
    public class Startup
    {
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
            services.AddControllersWithViews();

            services.AddMvc();

            //services.AddMemoryCache();

            var clientUrls = new Dictionary<string, string>();

            clientUrls.Add(Config.XAMARIN_CLIENT_KEY, Configuration.GetValue<string>("XamarinClientUrl"));

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                //Elimina problema de 'The issuer is invalid'
                if (Environment.IsDevelopment())
                {
                    options.IssuerUri = "null";
                }
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(clientUrls))
            .AddProfileService<ProfileService>();//Adiciona servicio de perfil personalizado

            if (Environment.IsDevelopment())
            {
                //Solo en desarrolo aplicamos credencial temporal
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //Aplicamos certificado de firma de produccion(Pendiente)
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();


            app.UseRouting();

            //Adicionado
           app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
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

using System;

using Autofac;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Cache;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Cache;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Criptografia;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Servicios.Api.Base.Services;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Json;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Json;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Rastreo;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Facturacion.Infrastructure
{
    public class DependenciasModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Fabricas estaticas
            LoggerFactory.SetCurrent(new Log4NetLoggerFactory());

            //Transversales
            builder.RegisterType<JsonNetSerializer>().As<IJsonSerializer>();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<CriptografiaService>().As<ICriptografiaService>().SingleInstance();

            //Configuraciones
            builder.RegisterType<FrameworkEndpointConfiguration>().AsSelf().InstancePerDependency();

            //Base datos
            builder.RegisterType<AplicacionConnectionStringResolver>().As<IConnectionStringResolver>();
            builder.RegisterType<ConfigurationAccesor>().As<IConfigurationAccesor>();
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();

            builder.Register(cc =>
            {
                var dbProvider = cc.Resolve<IDatabaseProvider>();

                return dbProvider.GetDatabase();
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            //Repositorios
            builder.RegisterType<CatalogoRepository>().As<ICatalogoRepository>();
            builder.RegisterType<EstablecimientoRepository>().As<IEstablecimientoRepository>();
            builder.RegisterType<ArticuloRepository>().As<IArticuloRepository>();
            builder.RegisterType<ClienteRepository>().As<IClienteRepository>();
            builder.RegisterType<FacturaRepository>().As<IFacturaRepository>();
            builder.RegisterType<BodegaRepository>().As<IBodegaRepository>();
            builder.RegisterType<UnidadMedidaRepository>().As<IUnidadMedidaRepository>();
            builder.RegisterType<ComprobanteRepository>().As<IComprobanteRepository>();

            //Fabricas
            builder.RegisterType<ServicioSeguridadSoapClientFactory>().As<ISoapClientFactory<ServicioSeguridadSoapClient>>();

            //Agentes de seguridad
            builder.RegisterType<FrameworkSeguridadAgenteService>().As<ISeguridadAgenteService>();
            
            //Servicios
            builder.RegisterType<ConfiguracionServicioSeguridad>().As<IConfiguracionServicioSeguridad>();
            builder.RegisterType<DatoAplicacionService>().As<IDatoAplicacionService>();
            builder.RegisterType<RegistroAplicacionService>().As<IRegistroAplicacionService>();
            builder.RegisterType<ArticuloService>().As<IArticuloService>();
            builder.RegisterType<CatalogoService>().As<ICatalogoService>();
            builder.RegisterType<EstablecimientoService>().As<IEstablecimientoService>();
            builder.RegisterType<ClienteService>().As<IClienteService>();
            builder.RegisterType<FacturaService>().As<IFacturaService>();
            builder.RegisterType<BodegaService>().As<IBodegaService>();
            builder.RegisterType<UnidadMedidaService>().As<IUnidadMedidaService>();
            builder.RegisterType<ComprobanteService>().As<IComprobanteService>();
            builder.RegisterType<AutenticacionService>().As<IAutenticacionService>().InstancePerLifetimeScope();

            //Servicios de api
            builder.RegisterType<AplicacionService>().As<IAplicacionService>();
        }
    }
}

using System;

using Autofac;

using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.AccesoDatos.Seguridad;
using Sipecom.FactElec.Pymes.Agentes.Framework.Configuraciones;
using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Cliente.Servicios;
using Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Participante.Servicios;
using Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Agentes;
using Sipecom.FactElec.Pymes.Agentes.Framework.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Agentes.Soap.Base;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Servicios;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Servicios;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Cache;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Json;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Mensajeria;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Cache;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Criptografia;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Json;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Mensajeria;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones;
using Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios;
using Sipecom.FactElec.Pymes.Servicios.Api.Base.Services;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Rastreo;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Infrastructure
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
            builder.RegisterType<AppSettingsEmailConfiguration>().As<IEmailConfiguration>();
            builder.RegisterType<EmailSender>().As<IEmailSender>();

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
            builder.RegisterType<UsuarioRepository>().As<IUsuarioRepository>();
            builder.RegisterType<EstablecimientoRepository>().As<IEstablecimientoRepository>();

            //Fabricas
            builder.RegisterType<ServicioSeguridadSoapClientFactory>().As<ISoapClientFactory<ServicioSeguridadSoapClient>>();
            builder.RegisterType<ServiceParticipanteClientFactory>().As<ISoapClientFactory<ServicioParticipanteSoapClient>>();
            builder.RegisterType<ServicioClienteSoapClientFactory>().As<ISoapClientFactory<ServicioClienteSoapClient>>();

            //Agentes de seguridad
            builder.RegisterType<FrameworkSeguridadAgenteService>().As<ISeguridadAgenteService>();
            builder.RegisterType<FrameworkParticipanteAgenteService>().As<IParticipanteAgenteService>();
            builder.RegisterType<FrameworkClienteAgenteService>().As<IClienteAgenteService>();

            //Servicios
            builder.RegisterType<ConfiguracionServicioSeguridad>().As<IConfiguracionServicioSeguridad>();
            builder.RegisterType<DatoAplicacionService>().As<IDatoAplicacionService>();
            builder.RegisterType<DatoSesionService>().As<IDatoSesionService>();
            builder.RegisterType<RegistroAplicacionService>().As<IRegistroAplicacionService>();
            builder.RegisterType<RegistroUsuarioService>().As<IRegistroUsuarioService>();
            builder.RegisterType<AutenticacionService>().As<IAutenticacionService>().InstancePerLifetimeScope();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>();

            //Servicios de api
            builder.RegisterType<AplicacionService>().As<IAplicacionService>();
        }
    }
}

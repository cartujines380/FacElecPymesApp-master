using Plugin.Connectivity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Infrastructure;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            InicializarAplicacion();
            InicializarNavegacion();
        }

        private void InicializarAplicacion()
        {
            Bootstrapper.Instance.Run();
        }

        private Task InicializarNavegacion()
        {
            var navigationService = Bootstrapper.Instance.Container.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

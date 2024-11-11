using System;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class PrincipalViewModel : ViewModelBase
    {
        public PrincipalViewModel()
        {

        }

        public ICommand CerrarSesionCommand => new Command(async () => await CerrarSesionAsync());

        private async Task CerrarSesionAsync()
        {
            await m_navigationService.NavigateToAsync<IniciarSesionViewModel>(new CerrarSesionParametros { CerrarSesion = true });
        }
    }
}

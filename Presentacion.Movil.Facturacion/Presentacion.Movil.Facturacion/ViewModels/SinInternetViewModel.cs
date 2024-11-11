using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class SinInternetViewModel : ViewModelBase
    {
        #region Command

        public ICommand VolverIniciarSesionCommand => new Command(async () => await VolverIniciarSesionAsync());

        #endregion

        private async Task VolverIniciarSesionAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {

            }
            else
            {
                await m_navigationService.NavigateToAsync<IniciarSesionViewModel>();
            }
        }

    }
}

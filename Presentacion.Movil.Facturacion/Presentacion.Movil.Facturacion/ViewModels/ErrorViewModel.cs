using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ErrorViewModel : ViewModelBase
    {
        #region Command

        public ICommand VolverInicioCommand => new Command(async () => await VolverInicioAsync());

        #endregion

        #region Metodos

        private async Task VolverInicioAsync()
        {
            await m_navigationService.NavigateToAsync<IniciarSesionViewModel>();
            await m_navigationService.RemoveLastFromBackStackAsync();
        }

        #endregion
    }
}

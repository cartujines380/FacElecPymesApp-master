using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalSinInternetViewModel : ViewModelBase
    {
        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        #endregion

        public override Task InitializeAsync(object navigationData)
        {
          
            return base.InitializeAsync(navigationData);
        }

        private async Task CerrarModalAsync()
        {
            
            await m_navigationService.NavigateBackModalAsync();
           
        }
    }
}

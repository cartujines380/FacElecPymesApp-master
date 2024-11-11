using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalDetalleCalculoViewModel : ViewModelBase
    {
        #region Campos

        private FacturaModel m_detalle;

        #endregion
        #region Propiedades

        public FacturaModel Detalle
        {
            get
            {
                return m_detalle;
            }
            set
            {
                m_detalle = value;
                RaisePropertyChanged(() => Detalle);
            }
        }

        #endregion


        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        #endregion

        #region Metodos

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            var detalle = navigationData as FacturaModel;
            Detalle = detalle;
            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}

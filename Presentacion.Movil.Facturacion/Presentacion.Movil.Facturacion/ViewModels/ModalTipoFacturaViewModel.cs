using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo;
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
    public class ModalTipoFacturaViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<CatalogoModel> m_tipoFactura;
        private CatalogoModel tipoFacturaSeleccionado;

        #endregion

        #region Propiedades

        public ObservableCollection<CatalogoModel> TipoFactura
        {
            get 
            { 
                return m_tipoFactura; 
            }
            set
            {
                m_tipoFactura = value;
                RaisePropertyChanged(() => TipoFactura);
            }
        }

        public CatalogoModel TipoFacturaSeleccionado
        {
            get
            {
                return tipoFacturaSeleccionado;
            }
            set
            {
                tipoFacturaSeleccionado = value;
                _ = Seleccion();
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        #endregion

        #region Metodos

        private async Task Seleccion()
        {
            var tipoFactura = tipoFacturaSeleccionado.Detalle;
            var infoFactura = new FacturaModel();
            infoFactura.TipoFactura = tipoFactura;
            infoFactura.EsEdicion = false;
            switch (tipoFactura)
            {
                case "Factura de Reembolso":
                    await m_navigationService.NavigateToModalAsync<ModalReembolsoViewModel>(infoFactura);
                    break;
                case "Factura de Exportación":
                    await m_navigationService.NavigateToModalAsync<ModalExportacionViewModel>(infoFactura);
                    break;
                default:
                    var info = new FacturaModel
                    {
                        TipoFactura = tipoFactura
                    };
                    await m_navigationService.NavigateBackModalAsync(info);
                    break;
            }
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            var tipoFactura = navigationData as ObservableCollection<CatalogoModel>;

            TipoFactura = tipoFactura;

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}

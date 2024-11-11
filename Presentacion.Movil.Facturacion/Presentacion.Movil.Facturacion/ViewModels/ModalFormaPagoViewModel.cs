using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalFormaPagoViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<CatalogoModel> m_formaPago;

        private CatalogoModel formaPagoSeleccionado;

        #endregion

        #region Propiedades

        public ObservableCollection<CatalogoModel> FormaPagoData { get; set; }

        public ObservableCollection<CatalogoModel> FormaPago
        {
            get
            {
                return m_formaPago;
            }
            set
            {
                m_formaPago = value;
                RaisePropertyChanged(() => FormaPago);
            }
        }

        public CatalogoModel FormaPagoSeleccionado
        {
            get
            {
                return formaPagoSeleccionado;
            }
            set
            {
                formaPagoSeleccionado = value;
                _ = Seleccion();
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand BuscarCommand => new Command<string>(BuscarAsync);

        #endregion

        #region Metodos

        private async Task Seleccion()
        {
            await m_navigationService.NavigateToModalAsync<ModalFormaPagoDetalleViewModel>(formaPagoSeleccionado);
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private void BuscarAsync(string text)
        {
            if (text != "")
            {
                var formaPago = FormaPagoData.Where(x => x.Detalle.ToUpper().Contains(text.ToUpper()));
                var myObservableCollection = new ObservableCollection<CatalogoModel>(formaPago);
                FormaPago = myObservableCollection;
            }
            else
            {
                FormaPago = FormaPagoData;
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            var formaPago = navigationData as ObservableCollection<CatalogoModel>;
            FormaPago = formaPago;
            FormaPagoData = formaPago;
            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}

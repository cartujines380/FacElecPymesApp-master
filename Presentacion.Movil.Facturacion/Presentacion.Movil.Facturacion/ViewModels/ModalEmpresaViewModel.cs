using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalEmpresaViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<EstablecimientoModel> m_establecimiento;
        private EstablecimientoModel establecimientoSeleccionado;

        #endregion

        #region Propiedades

        public ObservableCollection<EstablecimientoModel> EstablecimientoData { get; set; }

        public ObservableCollection<EstablecimientoModel> Establecimiento
        {
            get
            {
                return m_establecimiento;
            }
            set
            {
                m_establecimiento = value;
                RaisePropertyChanged(() => Establecimiento);
            }
        }


        public EstablecimientoModel EstablecimientoSeleccionado
        {
            get
            {
                return establecimientoSeleccionado;
            }
            set
            {
                establecimientoSeleccionado = value;
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
            await m_navigationService.NavigateBackModalAsync(establecimientoSeleccionado);
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private void BuscarAsync(string text)
        {
            if (text != "")
            {
                var empresas = EstablecimientoData.Where(x => x.RazonSocial.ToUpper().Contains(text.ToUpper()));
                var myObservableCollection = new ObservableCollection<EstablecimientoModel>(empresas);
                Establecimiento = myObservableCollection;
            }
            else
            {
                Establecimiento = EstablecimientoData;
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            var establecimientos = navigationData as ObservableCollection<EstablecimientoModel>;
            Establecimiento = establecimientos;
            EstablecimientoData = establecimientos;

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}

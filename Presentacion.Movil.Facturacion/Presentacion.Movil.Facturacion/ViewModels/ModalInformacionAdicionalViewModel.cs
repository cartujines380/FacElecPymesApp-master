using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalInformacionAdicionalViewModel : ViewModelBase
    {
        #region Campos

        private string m_codigo;
        private string m_valor;
        private int m_id;

        #endregion

        #region Propiedades

        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Codigo
        {
            get
            {
                return m_codigo;
            }
            set
            {
                m_codigo = value;
                RaisePropertyChanged(() => Codigo);
            }
        }

        public string Valor
        {
            get
            {
                return m_valor;
            }
            set
            {
                m_valor = value;
                RaisePropertyChanged(() => Valor);
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());
        public ICommand GuardarInfoAdicionalComannd => new Command(async () => await GuardarInfoAdicionalComanndAsync());

        #endregion

        #region Metodos

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task GuardarInfoAdicionalComanndAsync()
        {
            var infoAcional = new CatalogoModel
            {
                Id = Id,
                Codigo = Codigo,
                Detalle = Valor
            };

            await m_navigationService.NavigateBackModalAsync(infoAcional);
        }

        public override Task InitializeAsync(object navigationData)
        {
            var infoAdicional = navigationData as InformacionAdicional;

            Id  = infoAdicional != null ? infoAdicional.Id : 0;
            Codigo = infoAdicional != null ? infoAdicional.Codigo : "";
            Valor = infoAdicional != null ? infoAdicional.Valor : "";

            return base.InitializeAsync(navigationData);
        }

        #endregion

    }
}

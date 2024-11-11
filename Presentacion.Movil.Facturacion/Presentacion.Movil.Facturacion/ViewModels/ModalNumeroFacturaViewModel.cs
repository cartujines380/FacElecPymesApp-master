using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalNumeroFacturaViewModel : ViewModelBase
    {
        #region Campos

        private ObservableCollection<NumeroFacturaModel> m_codEstablecimiento;
        private NumeroFacturaModel codEstablecimientoSeleccionado;
        private ObservableCollection<NumeroFacturaModel> m_codPuntoEmision;
        private NumeroFacturaModel codPuntoEmisionSeleccionado;
        private int m_indexEstablecimiento;
        private int m_indexPtoEmision;

        private readonly IFacturaService m_facturaService;

        #endregion

        #region Propiedades

        public string Ruc { get; set; }

        public ObservableCollection<NumeroFacturaModel> ListCodEstablecimiento
        {
            get
            {
                return m_codEstablecimiento;
            }
            set
            {
                m_codEstablecimiento = value;
                RaisePropertyChanged(() => ListCodEstablecimiento);
            }
        }


        public NumeroFacturaModel CodEstablecimientoSeleccionado
        {
            get
            {
                return codEstablecimientoSeleccionado;
            }
            set
            {
                codEstablecimientoSeleccionado = value;
                _ = SeleccionCodEstablecimiento();
            }
        }

        public ObservableCollection<NumeroFacturaModel> ListPuntoEmision
        {
            get
            {
                return m_codPuntoEmision;
            }
            set
            {
                m_codPuntoEmision = value;
                RaisePropertyChanged(() => ListPuntoEmision);
            }
        }


        public NumeroFacturaModel PuntoEmisionSeleccionado
        {
            get
            {
                return codPuntoEmisionSeleccionado;
            }
            set
            {
                codPuntoEmisionSeleccionado = value;
                _ = SeleccionPuntoEmision();
            }
        }

        public int IndexEstablecimiento
        {
            get
            {
                return m_indexEstablecimiento;
            }
            set
            {
                m_indexEstablecimiento = value;
                RaisePropertyChanged(() => IndexEstablecimiento);
            }
        }

        public int IndexPtoEmision
        {
            get
            {
                return m_indexPtoEmision;
            }
            set
            {
                m_indexPtoEmision = value;
                RaisePropertyChanged(() => IndexPtoEmision);
            }
        }

        

        public string CodEstablecimiento { get; set; }
        public string CodPtoEmision { get; set; }
        public string Secuencial { get; set; }

        #endregion

        #region Constructor

        public ModalNumeroFacturaViewModel(IFacturaService facturaService)
        {
            m_facturaService = facturaService ?? throw new ArgumentNullException(nameof(facturaService));
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());
        public ICommand GuardarNumeroFacturaComannd => new Command(async () => await GuardarNumeroFacturaAsync());

        #endregion

        #region Metodos

        public override Task InitializeAsync(object navigationData)
        {
            var request = navigationData as RequestModel;
            Ruc = request.RucEmpresa;
            ListCodEstablecimiento = request.ListCodEstablecimiento;
            var item = ListCodEstablecimiento.FirstOrDefault();

            IndexEstablecimiento = ListCodEstablecimiento.IndexOf(item);

            return base.InitializeAsync(navigationData);
        }

        private async Task SeleccionCodEstablecimiento()
        {
            CodEstablecimiento = codEstablecimientoSeleccionado.CodEstablecimiento;
            ShowLoading();
            var codPtoEmision = await m_facturaService.ObtenerCodigoPuntoEmision(Ruc, CodEstablecimiento, false);
            HideLoading();

            ListPuntoEmision = codPtoEmision;

            var item = codPtoEmision.FirstOrDefault();

            IndexPtoEmision = codPtoEmision.IndexOf(item);

        }

        private async Task SeleccionPuntoEmision()
        {
            CodEstablecimiento = codEstablecimientoSeleccionado.CodEstablecimiento;
            CodPtoEmision = codPuntoEmisionSeleccionado.CodPuntoEmision;
            ShowLoading();
            var _secuencial = await m_facturaService.ObtenerSecuencial(Ruc, CodEstablecimiento, CodPtoEmision);
            HideLoading();

            _secuencial++;
            var sec = _secuencial.ToString().PadLeft(9, '0');
            Secuencial = sec;
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private async Task GuardarNumeroFacturaAsync()
        {
            var numFactura = new NumeroFacturaModel
            {
                CodEstablecimiento = CodEstablecimiento,
                CodPuntoEmision = CodPtoEmision,
                CodSecuencial = Secuencial
            };
           
            await m_navigationService.NavigateBackModalAsync(numFactura);
        }

        

        #endregion
    }
}

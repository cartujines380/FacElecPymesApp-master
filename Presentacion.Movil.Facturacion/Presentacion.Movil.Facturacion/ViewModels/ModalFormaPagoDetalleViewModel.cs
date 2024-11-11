using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalFormaPagoDetalleViewModel : ViewModelBase
    {
        #region Campos

        private string m_metodoPago;
        private CatalogoModel m_formaPago;
        private string m_monto;
        private string m_tiempo;
        private string m_plazo;
        private FormaPagoModel m_formaPagoDetalle;

        #endregion

        #region Propiedades

        public string MetodoPagoCodigo { get; set; }

        public string MetodoPago
        {
            get
            {
                return m_metodoPago;
            }
            set
            {
                m_metodoPago = value;
                RaisePropertyChanged(() => MetodoPago);
            }
        }

        public string Monto
        {
            get
            {
                return m_monto;
            }
            set
            {
                m_monto = value;
                RaisePropertyChanged(() => Monto);
            }
        }

        public string TiempoSeleccionado
        {
            get
            {
                return m_tiempo;
            }
            set
            {
                m_tiempo = value;
                RaisePropertyChanged(() => TiempoSeleccionado);
            }
        }

        public string Plazo
        {
            get
            {
                return m_plazo;
            }
            set
            {
                m_plazo = value;
                RaisePropertyChanged(() => Plazo);
            }
        }

        public CatalogoModel FormaPago
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

        public FormaPagoModel FormaPagoDetalle
        {
            get
            {
                return m_formaPagoDetalle;
            }
            set
            {
                m_formaPagoDetalle = value;
                RaisePropertyChanged(() => FormaPagoDetalle);
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());
        public ICommand GuardarFormaPagoComannd => new Command(async () => await GuardarFormaPagoAsync());

        private async Task GuardarFormaPagoAsync()
        {
            var formaPago = new FormaPagoModel
            {
                FormaPagoCodigo = MetodoPagoCodigo,
                MetodoPago = MetodoPago,
                Monto = Convert.ToDecimal(Monto),
                TiempoCodigo = ObtenerCodigoTiempo(TiempoSeleccionado),
                Plazo = Plazo
            };

            await m_navigationService.NavigateBackModalAsync();
            await m_navigationService.NavigateBackModalAsync(formaPago);
        }

        #endregion

        #region Metodos

        private string ObtenerCodigoTiempo(string valor)
        {
            var codigo = "";
            switch (valor)
            {
                case "Dias":
                    codigo = "01";
                    break;
                case "Meses":
                    codigo = "02";
                    break;
                case "Años":
                    codigo = "03";
                    break;
                default:
                    break;
            }

            return codigo;
        }

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            var formaPago = navigationData as CatalogoModel;

            MetodoPagoCodigo = formaPago.Codigo;
            MetodoPago = formaPago.Detalle;

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}

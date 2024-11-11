using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class FacturaExitoViewModel : ViewModelBase
    {

        #region Campos

        private string m_numeroFactura;
        private string m_valorFactura;
        private string m_cliente;
        private string m_identificacion;

        #endregion

        #region Propiedades

        public string NumeroFactura
        {
            get
            {
                return m_numeroFactura;
            }
            set
            {
                m_numeroFactura = value;
                RaisePropertyChanged(() => NumeroFactura);
            }
        }

        public string ValorFactura
        {
            get
            {
                return m_valorFactura;
            }
            set
            {
                m_valorFactura = value;
                RaisePropertyChanged(() => ValorFactura);
            }
        }

        public string Cliente
        {
            get
            {
                return m_cliente;
            }
            set
            {
                m_cliente = value;
                RaisePropertyChanged(() => Cliente);
            }
        }

        public string Identificacion
        {
            get
            {
                return m_identificacion;
            }
            set
            {
                m_identificacion = value;
                RaisePropertyChanged(() => Identificacion);
            }
        }
        #endregion

        #region Command

        public ICommand VolverInicioCommand => new Command(async () => await VolverInicioAsync());

       

        #endregion

        #region Metodos

        public override Task InitializeAsync(object navigationData)
        {
            var datos = navigationData as FacturaModel;

            if (datos != null)
            {
                var estab = datos.EstablecimientoNumero;
                var ptoEmision =  datos.PuntoEmisionNumero;
                var secuencial = datos.Secuencial;

                NumeroFactura = estab + "-" + ptoEmision + "-" + secuencial;
                ValorFactura =  "$" + datos.Total.ToString();
                Cliente = datos.RazonSocialProveedor;
                Identificacion = datos.Identificacion;
            }
            

            return base.InitializeAsync(navigationData);
        }

        private async Task VolverInicioAsync()
        {
            await m_navigationService.NavigateToAsync<MenuViewModel>();
            await m_navigationService.RemoveLastFromBackStackAsync();
        }

        #endregion
    }
}

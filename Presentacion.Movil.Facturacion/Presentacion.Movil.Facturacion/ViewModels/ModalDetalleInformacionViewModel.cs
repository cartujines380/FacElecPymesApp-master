using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.UnidadMedida;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels
{
    public class ModalDetalleInformacionViewModel : ViewModelBase
    {

        #region Campos

        private string m_codigo;
        private string m_nombre;
        private decimal m_precioUnidadBase;
        private string m_precioFinalStr;
        private int m_cantidad;
        private ObservableCollection<BodegaModel> m_listBodega;
        private ObservableCollection<UnidadMedidaModel> m_listUnidadMedida;
        private ObservableCollection<ImpuestoModel> m_listImpuestoIva;
        private ObservableCollection<ImpuestoModel> m_listImpuestoIce;
        private ImpuestoModel m_impuestoIvaSeleccionado;
        private ImpuestoModel m_impuestoIceSeleccionado;

        private const string IMPUESTO_CERO_PORCIENTO = "0";
        private const string IMPUESTO_DOCE_PORCIENTO = "2";
        private const string IMPUESTO_CATORCE_PORCIENTO = "3";
        private const string IMPUESTO_NO_OBJETO_IVA = "6";
        private const string IMPUESTO_EXCENTO_IVA = "7";
        private const string IMPUESTO_NO_GRABA_ICE = "8";

        #endregion

        #region Propiedades

        private decimal SubtotalSinImpuesto { get; set; }
        
        private decimal SubtotalCeroPorciento { get; set; }
        
        private decimal SubtotalDocePorciento { get; set; }
        
        private decimal SubtotalNoObjetoIva { get; set; }
        
        private decimal SubtotalExentoIva { get; set; }
        
        private decimal PrecioFinal { get; set; }

        public string CodigoIce { get; set; }

        public decimal PorcentajeIva { get; set; }

        public string CodigoIva { get; set; }

        public decimal IvaCalculado { get; set; }

        public decimal IceCalculado { get; set; }

        public decimal PorcentajeIce { get; set; }

        public decimal PrecioFinalConIva { get; set; }


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

        public string Nombre
        {
            get
            {
                return m_nombre;
            }
            set
            {
                m_nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }

        public decimal PrecioUnidadBase
        {
            get
            {
                return m_precioUnidadBase;
            }
            set
            {
                m_precioUnidadBase = value;
                RaisePropertyChanged(() => PrecioUnidadBase);
            }
        }

        public int Cantidad
        {
            get
            {
                CalcularPrecioFinal();
                return m_cantidad;
            }
            set
            {
                m_cantidad = value;
                RaisePropertyChanged(() => Cantidad);
            }
        }

        public string PrecioFinalConIvaStr
        {
            get
            {
                return m_precioFinalStr;
            }
            set
            {
                m_precioFinalStr = value;
                RaisePropertyChanged(() => PrecioFinalConIvaStr);
            }
        }

       
        public ObservableCollection<BodegaModel> ListBodega
        {
            get
            {
                return m_listBodega;
            }
            set
            {
                m_listBodega = value;
                RaisePropertyChanged(() => ListBodega);
            }
        }

        public ObservableCollection<UnidadMedidaModel> ListUnidadMedida
        {
            get
            {
                return m_listUnidadMedida;
            }
            set
            {
                m_listUnidadMedida = value;
                RaisePropertyChanged(() => ListUnidadMedida);
            }
        }

        public ObservableCollection<ImpuestoModel> ListImpuestoIVA
        {
            get
            {
                return m_listImpuestoIva;
            }
            set
            {
                m_listImpuestoIva = value;
                RaisePropertyChanged(() => ListImpuestoIVA);
            }
        }

        public ObservableCollection<ImpuestoModel> ListImpuestoICE
        {
            get
            {
                return m_listImpuestoIce;
            }
            set
            {
                m_listImpuestoIce = value;
                RaisePropertyChanged(() => ListImpuestoICE);
            }
        }

        public ImpuestoModel ImpuestoIvaSeleccionado
        {
            get
            {
                
                return m_impuestoIvaSeleccionado;
            }
            set
            {
                m_impuestoIvaSeleccionado = value;
                PorcentajeIva = m_impuestoIvaSeleccionado.Porcentaje;
                CodigoIva = m_impuestoIvaSeleccionado.Codigo;
                CalcularPrecioFinal();
            }
        }

        public ImpuestoModel ImpuestoIceSeleccionado
        {
            get
            {
                return m_impuestoIceSeleccionado;
            }
            set
            {
                m_impuestoIceSeleccionado = value;
                PorcentajeIce = m_impuestoIceSeleccionado.Porcentaje;
                CodigoIce = m_impuestoIceSeleccionado.Codigo;
                CalcularPrecioFinalInterno();
            }
        }

        #endregion

        #region Command

        public ICommand CerrarModalComannd => new Command(async () => await CerrarModalAsync());

        public ICommand GuardarDetalleComannd => new Command(async () => await GuardarDetalleComanndAsync());

        #endregion

        #region Metodos

        private async Task CerrarModalAsync()
        {
            await m_navigationService.NavigateBackModalAsync();
        }

        private void CalcularPrecioFinal()
        {
            CalcularPrecioFinalInterno();
        }

        private void CalcularPrecioFinalInterno()
        {
            SubtotalSinImpuesto = 0M;
            SubtotalDocePorciento = 0M;
            SubtotalCeroPorciento = 0M;
            SubtotalNoObjetoIva = 0M;
            IvaCalculado = 0M;
            PrecioFinalConIva = 0M;

            if (CodigoIva == IMPUESTO_DOCE_PORCIENTO || CodigoIva == IMPUESTO_CATORCE_PORCIENTO)
            {
                SubtotalDocePorciento = m_cantidad * PrecioUnidadBase;
                IceCalculado = SubtotalDocePorciento * PorcentajeIce;
                SubtotalDocePorciento = SubtotalDocePorciento + IceCalculado;
                IvaCalculado = SubtotalDocePorciento * PorcentajeIva;
                PrecioFinalConIva = SubtotalDocePorciento + IvaCalculado;
                PrecioFinalConIvaStr = PrecioFinalConIva.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_CERO_PORCIENTO)
            {
                SubtotalCeroPorciento = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalCeroPorciento.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_NO_OBJETO_IVA)
            {
                SubtotalNoObjetoIva = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalNoObjetoIva.ToString("N2");
            }

            if (CodigoIva == IMPUESTO_EXCENTO_IVA)
            {
                SubtotalExentoIva = m_cantidad * PrecioUnidadBase;
                PrecioFinalConIvaStr = SubtotalExentoIva.ToString("N2");
            }

            SubtotalSinImpuesto = SubtotalDocePorciento - IceCalculado + SubtotalCeroPorciento + SubtotalNoObjetoIva + SubtotalExentoIva;
            PrecioFinal = PrecioFinalConIva + SubtotalCeroPorciento + SubtotalNoObjetoIva + SubtotalExentoIva;
        }

        private async Task GuardarDetalleComanndAsync()
        {
            var detalle = new ArticuloModel
            {
                Codigo = Codigo,
                Descripcion = Nombre,
                Bodega = "",
                CantidadArticulo = Cantidad,
                PrecioUnidadBase = PrecioUnidadBase.ToString("N2"),
                SubtotalDocePorciento = SubtotalDocePorciento,
                SubtotalCeroPorciento = SubtotalCeroPorciento,
                SubtotalSinImpuesto = SubtotalSinImpuesto,
                SubtotalNoObjetoIva = SubtotalNoObjetoIva,
                SubtotalExentoIva = SubtotalExentoIva,
                IvaCalculadoArticulo = IvaCalculado,
                IceCalculadoArticulo = IceCalculado,
                PrecioFinal = PrecioFinal,
                ImpuestoIVA = ImpuestoIvaSeleccionado,
                ImpuestoICE = ImpuestoIceSeleccionado
            };

            await m_navigationService.NavigateBackModalAsync();
            await m_navigationService.NavigateBackModalAsync(detalle);
        }

        public override Task InitializeAsync(object navigationData)
        {
            var articulo = navigationData as ArticuloModel;

            Codigo = articulo.Codigo;
            Nombre = articulo.Nombre;
            ListBodega = articulo.ListBodegas;
            ListUnidadMedida = articulo.ListUnidadMedida;
            ListImpuestoIVA = articulo.ListImpuestoIVA;
            ListImpuestoICE = articulo.ListImpuestoICE;
            PrecioUnidadBase = ConvertirCadenaDecimal(articulo.PrecioUnidadBase);
            PrecioFinalConIvaStr = "0.00";

            return base.InitializeAsync(navigationData);
        }

        private decimal ConvertirCadenaDecimal(string cadena)
        {
            decimal resultado;

            // Crear un array de caracteres con el separador decimal esperado
            char[] separadores = { ',' };

            // Establecer el formato con la cultura adecuada
            CultureInfo cultura = CultureInfo.CreateSpecificCulture("es-ES");

            // Intentar convertir la cadena a decimal
            if (decimal.TryParse(cadena, NumberStyles.Float, cultura, out resultado))
            {
            }

            return resultado;


        }

        #endregion
    }
}

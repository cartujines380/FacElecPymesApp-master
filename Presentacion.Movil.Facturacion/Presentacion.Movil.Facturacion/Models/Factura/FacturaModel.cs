using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class FacturaModel
    {
        public bool EsEdicion { get; set; }

        public string TipoFactura { get; set; }

        public string GuiaRemision { get; set; }

        public string Secuencial { get; set; }

        public string EmpresaRuc { get; set; }

        public string EstablecimientoNumero { get; set; }

        public string PuntoEmisionNumero { get; set; }

        public string MatrizDireccion { get; set; }

        public DateTime? FechaEmision { get; set; }

        public string ContribuyenteEspecialNumero { get; set; }

        public string ObligadoContabilidad { get; set; }

        public bool? ObligadoCont { get; set; }

        public string TipoIdentificacionCodigo { get; set; }

        public string RazonSocial { get; set; }

        public string Identificacion { get; set; }

        public string RazonSocialProveedor { get; set; }

        public string Moneda { get; set; }

        public string CompradorDireccion { get; set; }

        public string ReceptorEmail { get; set; }

        public decimal SubtotalSinImpuestos { get; set; }

        public decimal TotalDescuento { get; set; }

        public decimal Total { get; set; }

        public decimal IvaDocePorCiento { get; set; }

        public decimal Ice { get; set; }

        public decimal SubtotalCeroPorciento { get; set; }

        public decimal SubtotalDocePorciento { get; set; }

        public decimal SubtotalNoObjetoIva { get; set; }

        public decimal SubtotalExentoIva { get; set; }

        public decimal ImpuestoRenta { get; set; }

        public string Regimen { get; set; }

        public string RucTransportista { get; set; }

        public string PuntoEmisionTransportista { get; set; }

        public string RazonSocialTransporista { get; set; }


        public ICollection<FacturaDetalleModel> Detalle { get; set; }

        public ICollection<FormaPagoModel> FormasPago { get; set; }

        public ObservableCollection<InformacionAdicional> Adicionales { get; set; }

        public bool EsReembolso { get; set; }

        public bool EsExportacion { get; set; }

        public bool Estransportista { get; set; }

        public FacturaReembolsoModel Reembolso { get; set; }

        public string DefinicionTermino { get; set; }

        public string DefTerminoSinImpuesto { get; set; }

        public string PuertoEmbarque { get; set; }

        public string PaisAdquisicionCodigo { get; set; }

        public CatalogoModel PaisAdquisicion { get; set; }

        public string PaisOrigenCodigo { get; set; }

        public CatalogoModel PaisOrigen { get; set; }

        public string LugarConvenio { get; set; }

        public string PuertoDestino { get; set; }

        public string PaisDestinoCodigo { get; set; }

        public CatalogoModel PaisDestino { get; set; }

        public decimal FleteInternacional { get; set; }

        public decimal SeguroInternacional { get; set; }

        public decimal GastosAduaneros { get; set; }

        public decimal GastosTransporte { get; set; }

        public FacturaModel()
        {
            Reembolso = new FacturaReembolsoModel();
        }

    }
}

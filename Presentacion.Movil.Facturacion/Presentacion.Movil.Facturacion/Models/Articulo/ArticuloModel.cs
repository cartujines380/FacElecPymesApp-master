using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.UnidadMedida;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo
{
    public class ArticuloModel
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int CantidadArticulo { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaModifica { get; set; }

        public string Estado { get; set; }

        public string IdMoneda { get; set; }

        public string IdIVA { get; set; }

        public string IdICE { get; set; }

        public int IdUnidadMedida { get; set; }

        public string RastreoInventario { get; set; }

        public int Stock { get; set; }

        public string Usuario { get; set; }

        public string RucEmpresa { get; set; }

        public int TipoInventario { get; set; }

        public int IdCategoria { get; set; }

        public string IdMetodoCosteo { get; set; }

        public int IdBodega { get; set; }

        public string Bodega { get; set; }

        public int CodBodega { get; set; }

        public int IdUnidadBase { get; set; }

        public string UltimoCosto { get; set; }

        public string CalculoCosto { get; set; }

        public string PrecioUnidadBase { get; set; }

        public decimal PrecioUnidadBaseDecimal { get; set; }

        public string Cantidad { get; set; }

        public decimal PrecioFinal { get; set; }

        public string InformacionDetalle { get; set; }

        public string Icono { get; set; }

        public decimal PrecioSinIva { get; set; }

        public decimal IvaCalculadoArticulo { get; set; }

        public decimal IceCalculadoArticulo { get; set; }

        public decimal SubtotalSinImpuesto { get; set; }

        public decimal SubtotalCeroPorciento { get; set; }

        public decimal SubtotalDocePorciento { get; set; }

        public decimal SubtotalNoObjetoIva { get; set; }

        public decimal SubtotalExentoIva { get; set; }

        public ObservableCollection<BodegaModel> ListBodegas { get; set; }

        public ObservableCollection<UnidadMedidaModel> ListUnidadMedida { get; set; }

        public ObservableCollection<ImpuestoModel> ListImpuestoIVA { get; set; }

        public ImpuestoModel ImpuestoIVA { get; set; }

        public ObservableCollection<ImpuestoModel> ListImpuestoICE { get; set; }

        public ImpuestoModel ImpuestoICE { get; set; }

        public string Mensaje { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class FacturaReembolsoModel
    {
        private int m_secuencial;

        public string Codigo { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalBaseImponible { get; set; }

        public decimal? TotalImpuesto { get; set; }

        public FacturaDetalleReembolsoModel InfoDetalle { get; set; }

        public ICollection<FacturaDetalleReembolsoModel> Detalle { get; set; }

        public FacturaReembolsoModel()
        {
            InfoDetalle = new FacturaDetalleReembolsoModel();
            Detalle = new List<FacturaDetalleReembolsoModel>();
            m_secuencial = 0;
        }
    }
}

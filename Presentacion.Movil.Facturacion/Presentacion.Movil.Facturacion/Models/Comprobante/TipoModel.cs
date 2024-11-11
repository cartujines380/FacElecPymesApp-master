using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante
{
    public class TipoModel
    {
        public bool Todos { get; set; }

        public bool Factura { get; set; }

        public bool FacturaExportacion { get; set; }

        public bool FacturaReembolso { get; set; }

        public bool FacturaTransportista { get; set; }

        public bool NotaCredito { get; set; }

        public bool NotaDebito { get; set; }

        public bool GuiaRemision { get; set; }

        public bool LiquidacionCompra { get; set; }

        public bool ComprobanteRetencion { get; set; }
    }
}

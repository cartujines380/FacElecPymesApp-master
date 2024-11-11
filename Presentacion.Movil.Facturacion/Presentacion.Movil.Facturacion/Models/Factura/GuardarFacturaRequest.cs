using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class GuardarFacturaRequest
    {
        public FacturaModel Factura { get; set; }

        public string Usuario { get; set; }
    }
}

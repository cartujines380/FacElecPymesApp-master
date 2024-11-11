using System;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Mensajes
{
    public class GuardarFacturaRequest
    {
        public Factura Factura { get; set; }

        public string Usuario { get; set; }
    }
}

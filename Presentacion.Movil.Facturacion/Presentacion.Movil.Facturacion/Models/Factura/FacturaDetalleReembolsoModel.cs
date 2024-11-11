using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class FacturaDetalleReembolsoModel
    {
        public int Id { get; set; }

        public ProveedorModel Proveedor { get; set; }

        public DocumentoModel Documento { get; set; }

        public ImpuestoReembolsoModel Impuesto { get; set; }

        public FacturaDetalleReembolsoModel()
        {
            Proveedor = new ProveedorModel();
            Documento = new DocumentoModel();
            Impuesto = new ImpuestoReembolsoModel();
        }
    }
}

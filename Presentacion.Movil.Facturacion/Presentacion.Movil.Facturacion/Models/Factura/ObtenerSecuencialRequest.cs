using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class ObtenerSecuencialRequest
    {
        public string Usuario { get; set; }

        public string EmpresaRuc { get; set; }

        public string DocumentoTipo { get; set; }

        public string Establecimiento { get; set; }

        public string PuntoEmision { get; set; }

        public string Tipo { get; set; }
    }
}

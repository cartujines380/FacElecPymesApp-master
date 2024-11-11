using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class InfoComprobanteModel
    {
        public int IdDocumento { get; set; }
        public string TipoBase { get; set; }
        public string Establecimiento { get; set; }
        public string Ruc { get; set; }
        public string Identificacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Servicios.Api.GenerarPDF.Models
{
    public class InfoComprobanteModel
    {
        public int IdDocumento { get; set; }
        public string Ruc { get; set; }
        public string TipoComprobante { get; set; }
        public string NumeroComprobante { get; set; }
        public string Identificacion { get; set; }
        public string Fecha { get; set; }
        public string Valor { get; set; }
        public string TipoBase { get; set; }
        public string Establecimiento { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models
{
    public class ImpuestoModel
    {
        public int ImpuestoRetener { get; set; }

        public string Codigo { get; set; }

        public string Concepto { get; set; }

        public decimal Porcentaje { get; set; }

        public string TipoRetencion { get; set; }

        public string Estado { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public string Usuario { get; set; }

        public decimal Valor { get; set; }
    }
}

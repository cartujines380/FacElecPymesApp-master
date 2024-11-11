using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models
{
    public class FormaPagoModel
    {
        public string FormaPagoCodigo { get; set; }

        public string MetodoPago { get; set; }

        public decimal? Monto { get; set; }

        public string MontoStr
        {
            get
            {
                return UtilFormato.ACadena(Monto);
            }
            set
            {
                Monto = UtilFormato.ADecimal(value);
            }
        }

        public string TiempoCodigo { get; set; }

        public string Tiempo { get; set; }

        public string Plazo { get; set; }

    }
}

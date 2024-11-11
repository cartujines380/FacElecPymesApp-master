using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class ImpuestoRetencion
    {
        public int Id { get; set; }

        public int ImpuestoRetenerId { get; set; }

        public string Codigo { get; set; }

        public decimal? Porcentaje { get; set; }

        public string PorcentajeStr
        {
            get
            {
                return UtilFormato.ACadena(Porcentaje);
            }
            set
            {
                Porcentaje = UtilFormato.ADecimal(value);
            }
        }

        public string TipoRetencion { get; set; }

        public string PorcentajeDescripcion { get; set; }
    }
}

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class ImpuestoReembolsoModel
    {
        public string Codigo { get; set; }

        public string TipoImpuetsoCodigo { get; set; }

        public string PorcentajeCodigo { get; set; }

        public decimal? PorcentajeValor { get; set; }

        public string PorcentajeValorStr
        {
            get
            {
                return UtilFormato.ACadena(PorcentajeValor);
            }
            set
            {
                PorcentajeValor = UtilFormato.ADecimal(value);
            }
        }

        public decimal? BaseImponible { get; set; }

        public string BaseImponibleStr
        {
            get
            {
                return UtilFormato.ACadena(BaseImponible);
            }
            set
            {
                BaseImponible = UtilFormato.ADecimal(value);
            }
        }

        public decimal? ImpuestosValor
        {
            get
            {
                return UtilFormato.Truncate((BaseImponible ?? 0M) * (PorcentajeValor ?? 0M), 2);
            }
        }

        public decimal? ValorTotal
        {
            get
            {
                return UtilFormato.Truncate((BaseImponible ?? 0M) + (ImpuestosValor ?? 0M), 2);
            }
        }

        public string ValorTotalStr
        {
            get
            {
                return UtilFormato.ACadena(ValorTotal);
            }
        }

    }
}

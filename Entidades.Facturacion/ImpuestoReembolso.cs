using System;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class ImpuestoReembolso
    {
        public string Codigo { get; set; }

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



        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(Codigo))
                resultado.AnadirError("El código de impuesto es requerido");

            if (string.IsNullOrEmpty(PorcentajeCodigo))
                resultado.AnadirError("El porcentaje de impuesto es requerido");

            if (!BaseImponible.HasValue)
                resultado.AnadirError("La base imponible es requerida");
            else if (!(BaseImponible.Value > 0M))
                resultado.AnadirError("La base imponible debe ser mayor a cero");

            return resultado;
        }
    }

}

using System;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class FacturaDetalle : Entity
    {
        public string ProductoCodigo { get; set; }

        public string Descripcion { get; set; }

        public decimal? Cantidad { get; set; }

        public string CantidadStr
        {
            get
            {
                return UtilFormato.ACadena(Cantidad, 6);
            }
            set
            {
                Cantidad = UtilFormato.ADecimal(value);
            }
        }

        public decimal? Descuento { get; set; }

        public string DescuentoStr
        {
            get
            {
                return UtilFormato.ACadena(Descuento);
            }
            set
            {
                Descuento = UtilFormato.ADecimal(value);
            }
        }

        public decimal? PrecioUnitario { get; set; }

        public string PrecioUnitarioStr
        {
            get
            {
                return UtilFormato.ACadena(PrecioUnitario, 6);
            }
            set
            {
                PrecioUnitario = UtilFormato.ADecimal(value);
            }
        }

        public string PorcentajeIvaCodigo { get; set; }

        public decimal? PorcentajeIvaValor { get; set; }

        public string PorcentajeIvaValorStr
        {
            get
            {
                return UtilFormato.ACadena(PorcentajeIvaValor);
            }
            set
            {
                PorcentajeIvaValor = UtilFormato.ADecimal(value);
            }
        }

        public string AplicaIce { get; set; }

        public string IceCodigo { get; set; }

        public decimal? IceValor { get; set; }

        public string IceValorStr
        {
            get
            {
                return UtilFormato.ACadena(IceValor);
            }
            set
            {
                IceValor = UtilFormato.ADecimal(value);
            }
        }

        public string PorcentajeRetencionCodigo { get; set; }

        public decimal? PorcentajeRetencionValor { get; set; }

        public decimal? ImpuestosValorIceSum { get; set; }

        public decimal? BaseImponibleIceSum { get; set; }

        public string PorcentajeRetencionValorStr
        {
            get
            {
                return UtilFormato.ACadena(PorcentajeRetencionValor);
            }
            set
            {
                PorcentajeRetencionValor = UtilFormato.ADecimal(value);
            }
        }

        public decimal? SubtotalParcial
        {
            get
            {
                return (Cantidad ?? 0M) * (PrecioUnitario ?? 0M);
            }
        }

        public decimal? Subtotal
        {
            get
            {
                return UtilFormato.Truncate((SubtotalParcial ?? 0M) - (Descuento ?? 0M), 2);
            }
        }

        public string SubtotalStr
        {
            get
            {
                return UtilFormato.ACadena(Subtotal);
            }
        }
        public decimal? BaseImponible
        {
            get
            {
                return Subtotal;
            }
        }

        public decimal? BaseImponibleConImpuestoICE
        {
            get
            {
                return Subtotal + ImpuestosValorIce;
            }
        }

        public decimal? TarifaRetencion
        {
            get
            {
                return UtilFormato.Truncate((PorcentajeRetencionValor ?? 0M) * (BaseImponible ?? 0M), 2);
            }
        }

        public decimal? ImpuestosValor
        {
            get
            {
                return UtilFormato.Truncate((BaseImponible ?? 0M) * (PorcentajeIvaValor ?? 0M), 3);
            }
        }

        public decimal? ImpuestosValorConImpuestoICE
        {
            get
            {
                return UtilFormato.Truncate((BaseImponibleConImpuestoICE ?? 0M) * (PorcentajeIvaValor ?? 0M), 3);
            }
        }

        public decimal? ImpuestosValorIce
        {
            get
            {
                return UtilFormato.Truncate((BaseImponible ?? 0M) * (IceValor ?? 0M), 2);
            }
        }

        public decimal? PrecioTotal
        {
            get
            {
                return UtilFormato.Truncate((Subtotal ?? 0M) + (ImpuestosValorConImpuestoICE ?? 0M) + (ImpuestosValorIce ?? 0M), 3);
            }
        }

        public ImpuestoData ImpuestoModelo
        {
            get
            {
                return new ImpuestoData() { Codigo = PorcentajeIvaCodigo, Valor = PorcentajeIvaValor };
            }
        }

        public bool EsVacio
        {
            get
            {
                return (
                    string.IsNullOrEmpty(ProductoCodigo)
                    && string.IsNullOrEmpty(Descripcion)
                    && string.IsNullOrEmpty(CantidadStr)
                    && string.IsNullOrEmpty(PrecioUnitarioStr)
                    && string.IsNullOrEmpty(DescuentoStr)
                );
            }
        }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(ProductoCodigo))
                resultado.AnadirError("El código de producto es requerido");

            if (string.IsNullOrEmpty(Descripcion))
                resultado.AnadirError("La descripción es requerida");

            if (string.IsNullOrEmpty(CantidadStr))
                resultado.AnadirError("La cantidad es requerida");

            if (string.IsNullOrEmpty(DescuentoStr))
                resultado.AnadirError("El descuento es requerido");

            if (string.IsNullOrEmpty(PrecioUnitarioStr))
                resultado.AnadirError("El precio unitario es requerido");

            if (string.IsNullOrEmpty(PorcentajeIvaCodigo))
                resultado.AnadirError("El IVA es requerido");

            return resultado;
        }
    }

}

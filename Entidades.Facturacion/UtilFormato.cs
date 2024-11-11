using System;
using System.Globalization;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class UtilFormato
    {
        public const string FORMATO_MONEDA = "F2";
        public const string FORMATO_FECHA = "dd/MM/yyyy";
        public const string FORMATO_FECHA_HORA = "dd/MM/yyyy HH:mm:ss";

        public static IFormatProvider ProveedorFormato
        {
            get
            {
                return CultureInfo.InvariantCulture;
            }
        }

        public static string ACadena(decimal? valor)
        {
            if (!valor.HasValue)
            {
                return string.Empty;
            }

            return valor.Value.ToString(FORMATO_MONEDA, ProveedorFormato);
        }

        public static string ACadena(decimal? valor, int cantidadDecimales)
        {
            if (!valor.HasValue)
            {
                return string.Empty;
            }

            var formatoMoneda = "F" + cantidadDecimales.ToString();

            return valor.Value.ToString(formatoMoneda, ProveedorFormato);
        }

        public static decimal? ADecimal(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            if (decimal.TryParse(valor, NumberStyles.Any, ProveedorFormato, out var aux))
            {
                return aux;
            }

            return null;
        }

        public static string ACadena(DateTime? valor)
        {
            if (!valor.HasValue)
            {
                return string.Empty;
            }

            return valor.Value.ToString(FORMATO_FECHA, ProveedorFormato);
        }

        public static DateTime? ADateTime(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            var aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, FORMATO_FECHA, ProveedorFormato, DateTimeStyles.None, out aux))
                return aux;

            return null;
        }

        public static DateTime? ADate(string valor)
        {
            return ADateTime(valor);
        }

        public static decimal? Truncate(decimal? valor, int precision)
        {
            return Math.Round(valor.Value, precision, MidpointRounding.AwayFromZero);
        }

        public static decimal? TruncateDecimal(decimal? valor, int precision)
        {
            if (!valor.HasValue)
            {
                return valor;
            }

            decimal stepper = Convert.ToDecimal(Math.Pow(10, precision));
            decimal tmp = Math.Truncate(stepper * valor.Value);
            return tmp / stepper;
        }

        public static string ACadenaFechaHora(DateTime? valor)
        {
            if (!valor.HasValue)
            {
                return string.Empty;
            }

            return valor.Value.ToString(FORMATO_FECHA_HORA, ProveedorFormato);
        }

        public static DateTime? ADateTimeFechaHora(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            var aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, FORMATO_FECHA_HORA, ProveedorFormato, DateTimeStyles.None, out aux))
            {
                return aux;
            }

            return null;
        }
    }
}

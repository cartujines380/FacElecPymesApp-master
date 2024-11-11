using System;
using System.Globalization;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models
{
    public class UtilFormato
    {
        public const string FORMATO_MONEDA = "F2";
        public const string FORMATO_FECHA = "dd/MM/yyyy";
        public const string FORMATO_FECHA_DOS = "MM/dd/yyyy";
        public const string FORMATO_FECHA_HORA = "dd/MM/yyyy HH:mm:ss";

        internal static IFormatProvider ProveedorFormato
        {
            get
            {
                return CultureInfo.InvariantCulture;
            }
        }

        internal static string ACadena(decimal? valor)
        {
            if (!valor.HasValue)
                return string.Empty;

            return valor.Value.ToString(FORMATO_MONEDA, ProveedorFormato);
        }

        internal static string ACadena(decimal? valor, int cantidadDecimales)
        {
            if (!valor.HasValue)
                return string.Empty;

            var formatoMoneda = "F" + cantidadDecimales.ToString();

            return valor.Value.ToString(formatoMoneda, ProveedorFormato);
        }

        internal static decimal? ADecimal(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return default(Decimal?);

            decimal aux;

            if (decimal.TryParse(valor, NumberStyles.Any, ProveedorFormato, out aux))
                return aux;

            return default(Decimal?);
        }

        internal static string ACadena(DateTime? valor)
        {
            if (!valor.HasValue)
                return string.Empty;

            return valor.Value.ToString(FORMATO_FECHA, ProveedorFormato);
        }

        internal static DateTime? ADateTime(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return default(DateTime?);

            DateTime aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, FORMATO_FECHA_DOS, ProveedorFormato, DateTimeStyles.None, out aux))
                return aux;

            return default(DateTime?);
        }

        internal static DateTime? ADate(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return default(DateTime?);

            DateTime aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, FORMATO_FECHA, ProveedorFormato, DateTimeStyles.None, out aux))
                return aux;

            return default(DateTime?);
        }

        internal static decimal? Truncate(decimal? valor, int precision)
        {
            return Math.Round(valor.Value, precision, MidpointRounding.AwayFromZero);
        }

        internal static decimal? TruncateDecimal(decimal? valor, int precision)
        {
            if (!valor.HasValue)
                return valor;

            decimal stepper = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(stepper * valor.Value);
            return tmp / stepper;
        }

        internal static string ACadenaFechaHora(DateTime? valor)
        {
            if (!valor.HasValue)
                return string.Empty;

            return valor.Value.ToString(FORMATO_FECHA_HORA, ProveedorFormato);
        }

        internal static DateTime? ADateTimeFechaHora(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return default(DateTime?);

            DateTime aux = DateTime.MinValue;

            if (DateTime.TryParseExact(valor, FORMATO_FECHA_HORA, ProveedorFormato, DateTimeStyles.None, out aux))
                return aux;

            return default(DateTime?);
        }
    }
}



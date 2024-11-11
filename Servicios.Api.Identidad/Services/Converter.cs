using System;
using System.Globalization;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Services
{
    internal static class Converter
    {
        internal static string ToString(int? valor)
        {
            return valor.HasValue ? valor.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        }

        internal static int? ToInt32(string valor)
        {
            int retorno = default(int);

            if (int.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out retorno))
            {
                return retorno;
            }

            return (int?)null;
        }

    }
}

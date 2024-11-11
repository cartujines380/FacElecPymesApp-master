using System;
using System.Linq;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers
{
    public static class UriHelper
    {
        public static string CombineUri(params string[] uriParts)
        {
            var uri = string.Empty;

            if ((uriParts == null) || (!uriParts.Any()))
            {
                return uri;
            }

            var trims = new char[] { '\\', '/' };

            uri = (uriParts[0] ?? string.Empty).TrimEnd(trims);

            for (int i = 1; i < uriParts.Count(); i++)
            {
                uri = string.Format("{0}/{1}", uri.TrimEnd(trims), (uriParts[i] ?? string.Empty).TrimStart(trims));
            }

            return uri;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura
{
    public class DocumentoModel
    {
        public string Codigo { get; set; }

        public string EstablecimientoCodigo { get; set; }

        public string PuntoEmisionCodigo { get; set; }

        public string Secuencial { get; set; }

        public DateTime? FechaEmision { get; set; }

        public string FechaEmisionStr
        {
            get
            {
                return UtilFormato.ACadena(FechaEmision);
            }
            set
            {
                FechaEmision = UtilFormato.ADateTime(value);
            }
        }

        public string NumeroAutorizacion { get; set; }

        public string Numero
        {
            get
            {
                var retorno = new List<string>();

                if (!string.IsNullOrEmpty(EstablecimientoCodigo))
                    retorno.Add(EstablecimientoCodigo);

                if (!string.IsNullOrEmpty(PuntoEmisionCodigo))
                    retorno.Add(PuntoEmisionCodigo);

                if (!string.IsNullOrEmpty(Secuencial))
                    retorno.Add(Secuencial);

                return string.Join("-", retorno.ToArray());
            }
        }
    }
}

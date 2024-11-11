using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Documento
    {
        private const int ESTABLECIMIENTO_LONGITUD_MAXIMA = 3;
        private const int PUNTO_EMISION_LONGITUD_MAXIMA = 3;
        private const int SECUENCIAL_LONGITUD_MAXIMA = 9;
        private const int NUMERO_AUTORIZACION_LONGITUD_MAXIMA = 49;
        private const int NUMERO_AUTORIZACION_LONGITUD_MINIMA = 10;

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

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(Codigo))
                resultado.AnadirError("El tipo de documento es requerido");

            if (string.IsNullOrEmpty(EstablecimientoCodigo))
                resultado.AnadirError("El número de establecimiento del número de comprobante es requerido");
            else if (EstablecimientoCodigo.Length != ESTABLECIMIENTO_LONGITUD_MAXIMA)
                resultado.AnadirError("El número de establecimiento del número de comprobante es invalido");

            if (string.IsNullOrEmpty(PuntoEmisionCodigo))
                resultado.AnadirError("El número de punto de emisión del número de comprobante es requerido");
            else if (PuntoEmisionCodigo.Length != PUNTO_EMISION_LONGITUD_MAXIMA)
                resultado.AnadirError("El número de punto de emisión del número de comprobante es invalido");

            if (string.IsNullOrEmpty(Secuencial))
                resultado.AnadirError("El número de secuencial del número de comprobante es requerido");
            else if (Secuencial.Length != SECUENCIAL_LONGITUD_MAXIMA)
                resultado.AnadirError("El número de secuencial del número de comprobante es invalido");

            if (!FechaEmision.HasValue)
                resultado.AnadirError("La fecha de emisión es requerida");

            if (string.IsNullOrEmpty(NumeroAutorizacion))
                resultado.AnadirError("El número de autorización es requerido");
            else if (NumeroAutorizacion.Length < NUMERO_AUTORIZACION_LONGITUD_MINIMA & NumeroAutorizacion.Length > NUMERO_AUTORIZACION_LONGITUD_MAXIMA)
                resultado.AnadirError("El número de autorización es invalido.");

            return resultado;
        }
    }
}



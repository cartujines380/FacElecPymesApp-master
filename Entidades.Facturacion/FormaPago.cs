using System;
using System.Collections.Generic;
using System.Linq;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class FormaPago : Entity
    {
        public string FormaPagoCodigo { get; set; }

        public decimal? Monto { get; set; }

        public string MontoStr
        {
            get
            {
                return UtilFormato.ACadena(Monto);
            }
            set
            {
                Monto = UtilFormato.ADecimal(value);
            }
        }

        public string TiempoCodigo { get; set; }

        public string Plazo { get; set; }

        public bool EsVacio
        {
            get
            {
                if (FormaPagoCodigo == "01")
                {
                    return string.IsNullOrEmpty(MontoStr);
                }
                else
                {
                    return (
                        string.IsNullOrEmpty(FormaPagoCodigo)
                        && string.IsNullOrEmpty(MontoStr)
                        && string.IsNullOrEmpty(TiempoCodigo)
                        && string.IsNullOrEmpty(Plazo)
                    );
                }
            }
        }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(FormaPagoCodigo))
                resultado.AnadirError("La forma de pago es requerida");

            if (string.IsNullOrEmpty(MontoStr))
                resultado.AnadirError("El monto es requerido");

            if (!(FormaPagoCodigo == "01"))
            {
                if (string.IsNullOrEmpty(TiempoCodigo))
                    resultado.AnadirError("El tiempo es requerido");

                if (string.IsNullOrEmpty(Plazo))
                    resultado.AnadirError("El plazo es requerido");
            }

            return resultado;
        }
    }

}

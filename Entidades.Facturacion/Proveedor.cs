using System;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Proveedor
    {
        public string TipoIdentificacionCodigo { get; set; }

        public string Identificacion { get; set; }

        public string RazonSocial { get; set; }

        public string PaisPagoCodigo { get; set; }

        public string Tipo { get; set; }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(Identificacion))
                resultado.AnadirError("El número de identificación es requerido");
            else
            {
                if ((TipoIdentificacionCodigo == "04") && (Identificacion.Length != 13))
                    resultado.AnadirError("La longitud del RUC no es el adecuado");

                if ((TipoIdentificacionCodigo == "05") && (Identificacion.Length != 10))
                    resultado.AnadirError("La longitud de la Cédula no es la adecuada");
            }

            if (string.IsNullOrEmpty(PaisPagoCodigo))
                resultado.AnadirError("El país es requerido");

            if (string.IsNullOrEmpty(Tipo))
                resultado.AnadirError("El tipo de proveedor es requerido");

            return resultado;
        }
    }


}

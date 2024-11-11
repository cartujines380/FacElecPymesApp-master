using System;
using System.Collections.Generic;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class InformacionAdicional : Entity
    {
        public string Codigo { get; set; }

        public string Valor { get; set; }

        public string ValorStr { get; set; }

        public string Clave { get; set; }

        public bool EsVacio
        {
            get
            {
                return (
                    string.IsNullOrEmpty(Codigo)
                    && string.IsNullOrEmpty(Valor)
                );
            }
        }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(Codigo))
                resultado.AnadirError("El codigo es requerido");

            if (string.IsNullOrEmpty(Valor))
                resultado.AnadirError("El valor es requerido");

            return resultado;
        }
    }

}

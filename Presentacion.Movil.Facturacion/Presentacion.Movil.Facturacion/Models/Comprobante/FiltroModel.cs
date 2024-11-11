using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante
{
    public class FiltroModel
    {
        public EstablecimientoModel Establecimiento { get; set; }

        public TipoModel Tipo { get; set; }

        public List<string> ListTipo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }

        public string CodigoEstado { get; set; }

        public string NumeroComprobante { get; set; }

        public string IdentificacionCliente { get; set; }

        public string CodigoError { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    }

}

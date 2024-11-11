using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class FiltroModel
    {
        public Establecimiento Establecimiento { get; set; }

        public List<string> ListTipo { get; set; }

        public string Tipos { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }

        public string CodigoEstado { get; set; }

        public string NumeroComprobante { get; set; }

        public string IdentificacionCliente { get; set; }

        public string CodigoError { get; set; }

        public string IdUsuario { get; set; }

        public string IdDocumento { get; set; }

        public string TipoBase { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}

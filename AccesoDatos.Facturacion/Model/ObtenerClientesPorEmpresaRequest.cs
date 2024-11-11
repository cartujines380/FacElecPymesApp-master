using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model
{
    public class ObtenerClientesPorEmpresaRequest
    {
        public string TipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public string Ruc { get; set; }

        public bool EsTransportista { get; set; }

        public string PaginaIndice { get; set; }

        public string PaginaTamanio { get; set; }

        public string IdUsuario { get; set; }
    }
}

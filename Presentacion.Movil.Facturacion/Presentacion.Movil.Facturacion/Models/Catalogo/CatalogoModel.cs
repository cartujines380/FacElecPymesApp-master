using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo
{
    public class CatalogoModel
    {
        public int Id { get; set; }

        public int Tabla { get; set; }
        
        public string Codigo { get; set; }

        public string Detalle { get; set; } 

        public string SubCodigo { get; set; }

        public string Estado { get; set; }

        public int Contador { get; set; }

        public string Icono { get; set; }
    }
}

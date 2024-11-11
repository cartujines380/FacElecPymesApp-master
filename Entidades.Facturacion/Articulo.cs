using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Articulo : Entity<string>
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaModifica { get; set; }

        public string Estado { get; set; }

        public string IdMoneda { get; set; }

        public string IdIVA { get; set; }

        public string IdICE { get; set; }

        public int IdUnidadMedida { get; set; }

        public string RastreoInventario { get; set; }

        public int Stock { get; set; }

        public string Usuario { get; set; }

        public string RucEmpresa { get; set; }

        public string RucEmpresaAnterior { get; set; }

        public int TipoInventario { get; set; }

        public int IdCategoria { get; set; }

        public string IdMetodoCosteo { get; set; }

        public int IdBodega { get; set; }

        public string Bodega { get; set; }

        public int CodBodega { get; set; }

        public int IdUnidadBase { get; set; }

        public string UltimoCosto { get; set; }

        public string CalculoCosto { get; set; }

        public decimal PrecioUnidadBaseDecimal { get; set; }

        public string PrecioUnidadBase { get; set; }

        public string Cantidad { get; set; }

        public string Mensaje { get; set; }

    }
}

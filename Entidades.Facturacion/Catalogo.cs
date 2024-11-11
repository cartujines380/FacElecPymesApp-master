using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Catalogo : Entity
    {
        public int Tabla
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
            }
        }

        public string Codigo { get; set; }

        public string Detalle { get; set; }

        public decimal ValorImpuesto { get; set; }

        public string SubCodigo { get; set; }

        public string ImpuestoRetener { get; set; }

        public string Estado { get; set; }

        public int Contador { get; set; }
    }
}

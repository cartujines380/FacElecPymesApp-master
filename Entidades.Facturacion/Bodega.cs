using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Bodega : Entity
    {
        public int IdBodega
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

        public string CodigoBodega { get; set; }

        public string Nombre { get; set; }
    }
}

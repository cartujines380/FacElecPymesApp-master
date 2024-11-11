using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class UnidadMedida : Entity
    {
        public int IdUnidadMedida
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

        public string NuevaUnidad { get; set; }
    }
}

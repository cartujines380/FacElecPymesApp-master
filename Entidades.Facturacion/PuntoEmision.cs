using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class PuntoEmision : Entity
    {
        public int IdPtoEmision
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

        public int IdSucursal { get; set; }

        public string CodPuntoEmision { get; set; }

        public string Descripcion { get; set; }

        public int IdSecOrigen { get; set; }

        public string Estado { get; set; }
    }
}

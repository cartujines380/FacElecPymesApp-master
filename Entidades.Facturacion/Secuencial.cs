using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Secuencial : Entity
    {
        public int IdSecuencial
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

        public string Ruc { get; set; }

        public int CodSecuencial { get; set; }

    }
}

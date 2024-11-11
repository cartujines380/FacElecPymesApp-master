using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Sucursal : Entity
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

        public string CodEstablecimiento { get; set; }

        public string NombreSucursal { get; set; }

        public string DireccionEstablecimiento { get; set; }

        public string RucSocio { get; set; }

        public string UsuarioSocio { get; set; }

        public string Estado { get; set; }

        public int? IdActividadEconomica { get; set; }

        public string LogoComercio { get; set; }

        public bool? Matriz { get; set; }
    }
}

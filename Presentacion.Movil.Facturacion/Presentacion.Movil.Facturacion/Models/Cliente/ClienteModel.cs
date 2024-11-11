using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Cliente
{
    public class ClienteModel
    {
        public string Identificacion { get; set; }

        public string RazonSocial { get; set; }

        public string Correo { get; set; }

        public DateTime FechaIngresa { get; set; }

        public DateTime? FechaModifiacion { get; set; }

        public string Estado { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string TipoIdentificacion { get; set; }

        public string IdTipoIdentificacion { get; set; }

        public string CorreoAdicional { get; set; }

        public string AliasSucursal { get; set; }

        public string CodigoSucursal { get; set; }

        public string Celular { get; set; }

        public string Provincia { get; set; }

        public string Ciudad { get; set; }

        public string ClienteOf { get; set; }

        public string Tipo { get; set; }

        public int? Transportista { get; set; }

        public string Placa { get; set; }

        public string RucEmpresa { get; set; }

        public string RucEmpresaAnterior { get; set; }

        public string IdTipoEntidad { get; set; }

        public string TipoEntidad { get; set; }

        public string Mensaje { get; set; }

        public string RucRazonSocial
        {
            get
            {
                return RucEmpresa + " - " + RazonSocial;
            }
        }
    }
}

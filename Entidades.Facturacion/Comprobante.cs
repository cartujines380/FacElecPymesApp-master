using Sipecom.FactElec.Pymes.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Comprobante : Entity<string>
    {
        public int IdDocumento { get; set; }
        public string Ruc { get; set; }
        public string Establecimiento { get; set; }
        public string FechaEmision { get; set; }
        public string IdentificacionCliente { get; set; }
        public decimal MontoFacturado { get; set; }
        public string Numero { get; set; }
        public string TipoDoc { get; set; }
        public string TipoBase { get; set; }
        public string Documento { get; set; }
        public string DocumentoXML { get; set; }
        public string Autorizacion { get; set; }
        public string Acceso { get; set; }
    }
}

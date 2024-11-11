using System;

using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Establecimiento : Entity<string>
    {
        public string Ruc
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

        public string DirMatriz { get; set; }

        public string Clave { get; set; }

        public string ContribEsp { get; set; }

        public string RazonSocial { get; set; }

        public string NombreComercial { get; set; }

        public string Estado { get; set; }
        
        public string CorreoAviso { get; set; }

        public string CorreoReset { get; set; }

        public int? DiasAlerta { get; set; }

        public int? DiasMsjRespaldo { get; set; }

        public bool? ObligadoContab { get; set; }

        public DateTime? FechaExpiracion { get; set; }
        
        public string EstablecimientoCodigo { get; set; }

        public string PtoEmisionCodigo { get; set; }

        public string Tipo { get; set; }

        public string PlanId { get; set; }

        public DateTime? FechaInicioPlan { get; set; }

        public DateTime? FechaFinPlan { get; set; }

        public string Firma { get; set; }

        public string ImagenLogo { get; set; }

        public string RepresentanteLegal { get; set; }

        public string IdentificacionRepresentante { get; set; }

        public string ProvinciaId { get; set; }

        public string CiudadId { get; set; }

        public string Celular { get; set; }

        public int? AvisoCaducidadPlan { get; set; }

        public DateTime? FechaExpedicion { get; set; }

        public bool Comprimir { get; set; }
    }
}

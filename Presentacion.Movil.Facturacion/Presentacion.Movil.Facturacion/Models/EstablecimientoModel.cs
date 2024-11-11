using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models
{
    public class EstablecimientoModel
    {
        public string Ruc { get; set; }

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

        public string NombrePlan { get; set; }

        public string CantidadDocDesde { get; set; }

        public string CantidadDocHasta { get; set; }

        public string FechaInicioPlan { get; set; }

        public string FechaFinPlan { get; set; }

        public string EstablecimientoTransportista { get; set; }

        public string RucTransportista { get; set; }

        public string PuntoEmisionTransportista { get; set; }

        public string RazonSocialTransportista { get; set; }

        public string Regimen { get; set; }
    }
}

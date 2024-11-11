using System;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class EstablecimientoData
    {
        public string Ruc { get; set; }

        public string RazonSocial { get; set; }

        public string EsContribuyenteEspecial { get; set; }

        public bool? ObligadoContabilidad { get; set; }

        public string MatrizDireccion { get; set; }

        public string EstablecimientoTransportista { get; set; }

        public string EstablecimientoCodigo { get; set; }

        public string PuntoEmision { get; set; }

        public string Regimen { get; set; }

        public string RucTransportista { get; set; }

        public string PuntoEmisionTransportista { get; set; }

        public string RazonSocialTransportista { get; set; }

        public string NombrePlan { get; set; }

        public string CantidadDocDesde { get; set; }

        public string CantidadDocHasta { get; set; }

        public string FechaInicioPlan { get; set; }

        public string FechaFinPlan { get; set; }
    }
}

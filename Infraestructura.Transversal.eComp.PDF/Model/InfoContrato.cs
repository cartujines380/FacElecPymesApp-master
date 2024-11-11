using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.eComp.PDF.Model
{
    public class InfoContrato
    {
        public string RazonSocial { get; set; }
        public string Ruc { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string CorreoAvisoCertificado { get; set; }
        public string RepresentanteLegal { get; set; }
        public string IdentificacionRepresentante { get; set; }
        public string NombrePlan { get; set; }
        public string Precio { get; set; }
        public string CantidadDocHasta { get; set; }
        public string Factura { get; set; }
        public string NotaCredito { get; set; }
        public string NotaDebito { get; set; }
        public string GuiaRemision { get; set; }
        public string ComprobanteRet { get; set; }
        public string NumeroDocumento { get; set; }
        public string EmailContable { get; set; }
        public string EmailSoporte { get; set; }
        public string UrlSipecom { get; set; }
        public string FechaCompletaInicioPlan { get; set; }
        public string FechaInicioPlan { get; set; }
        public string FechaFinPlan { get; set; }

    }
}

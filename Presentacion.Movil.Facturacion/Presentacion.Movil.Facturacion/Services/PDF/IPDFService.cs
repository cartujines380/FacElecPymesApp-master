using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.PDF
{
    public interface IPDFService
    {
        Task<PDFModel> GenerarPDF(ComprobanteModel infoComp);
    }
}

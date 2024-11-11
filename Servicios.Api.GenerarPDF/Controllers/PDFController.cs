using Infraestructura.Transversal.eComp.PDF;
using Servicios.Api.GenerarPDF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Servicios.Api.GenerarPDF.Controllers
{
    public class PDFController : ApiController
    {
        [HttpPost]
        public PDFModel GenerarPDF(Comprobante infoComp)
        {
            var retorno = new PDFModel();
            var pdf = new GeneraPDF();
            var dirLogo = System.Configuration.ConfigurationManager.AppSettings["RutaImagenesArchivoRIDE"];
            var logoSucursal = infoComp.Ruc + "_" + infoComp.Establecimiento + ".png";
            var rutaLogo = string.Empty;
            if (File.Exists(Path.Combine(dirLogo, logoSucursal)))
            {
                rutaLogo = Path.Combine(dirLogo, infoComp.Ruc + ".png");
            }
            else
            {
                rutaLogo = Path.Combine(dirLogo, "Logo_Defecto.png");
            }

            retorno.PDFbtye = pdf.GeneraFactura(infoComp.DocumentoXML, infoComp.Documento, rutaLogo, infoComp.DirSucursal,infoComp.DirCliente, "en-US");

            return retorno;
        }
    }
}
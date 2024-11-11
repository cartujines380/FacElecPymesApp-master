using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Infraestructura.Transversal.eComp.PDF.Controller
{
    public class ITextEvents : PdfPageEventHelper
    {
        private string pRutaLogo = string.Empty;

        public ITextEvents(string RutaLogo)
        {
            pRutaLogo = RutaLogo;
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            string RutaLogo = string.Empty;
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.TotalWidth = 540F;
            if (!string.IsNullOrEmpty(pRutaLogo))
            {
                RutaLogo = Path.Combine(Path.GetDirectoryName(pRutaLogo), "logo.png");
                if (!File.Exists(RutaLogo))
                {
                    RutaLogo = Path.Combine(Path.GetDirectoryName(pRutaLogo), "logo.png");
                }
            }
            if (File.Exists(RutaLogo))
            {
                iTextSharp.text.Image LogoPYMES = null;
                LogoPYMES = iTextSharp.text.Image.GetInstance(RutaLogo);
                LogoPYMES.ScaleToFit(140f, 100f);
                tabFot.AddCell(new PdfPCell(LogoPYMES)
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER
                });
            }
            tabFot.WriteSelectedRows(0, -1, 23, document.Bottom + 4, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}

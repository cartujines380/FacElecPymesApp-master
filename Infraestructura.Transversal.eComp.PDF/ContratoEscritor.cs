using Infraestructura.Transversal.eComp.PDF.Model;
using Infraestructura.Transversal.eComp.PDF.Recursos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.eComp.PDF
{
    internal class ContratoEscritor
    {
        #region Campos

        private readonly Stream m_flujo;

        private PdfWriter m_writer;
        private Document m_documento;
        private Font m_fuenteArialOnce;
        private Font m_m_fuenteArialNegritaOnce;
        private Font m_fuenteArialNegritaDoce;

        #endregion

        #region Constructores

        internal ContratoEscritor(Stream flujo)
        {
            if (flujo == null)
            {
                throw new ArgumentNullException(nameof(flujo));
            }

            m_flujo = flujo;

            InicializarFuentes();
        }

        #endregion

        #region Metodos internos

        internal void Escribir(InfoContrato InfoCliente, InfoContrato InfoSipecom, InfoContrato infoGeneral, string RutaLogoSipecom, string RutaLogoCliente, List<string> ListDocumentos)
        {
            m_documento = new Document();
            m_writer = PdfWriter.GetInstance(m_documento, m_flujo);
            m_documento.Open();
            var contenido = ObtenerContenidoContrato(InfoCliente, InfoSipecom, infoGeneral, ListDocumentos);

            #region Titulo
            Chunk c_titulo = new Chunk(contenido.Titulo, m_fuenteArialNegritaDoce);
            Chunk c_NombreCliente = new Chunk(" (" + InfoCliente.RazonSocial + ")", m_fuenteArialOnce);
            Phrase ph_Titulo = new Phrase();
            ph_Titulo.Add(c_titulo);
            ph_Titulo.Add(c_NombreCliente);

            Paragraph pf_titulo = new Paragraph();
            pf_titulo.Add(ph_Titulo);
            pf_titulo.Alignment = Element.ALIGN_CENTER;
            m_documento.Add(pf_titulo);
            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Primer Parrafo
            Chunk c1_NombreEmpresa = new Chunk(contenido.NombreEmpresa, m_m_fuenteArialNegritaOnce);
            Chunk c1_textoUno = new Chunk(" con número de", m_fuenteArialOnce);
            Chunk c1_RucEmpresa = new Chunk(" " + contenido.RucEmpresa, m_m_fuenteArialNegritaOnce);
            Chunk c1_TextoDos = new Chunk(", " + contenido.C1_TextoDos, m_fuenteArialOnce);
            Chunk c1_TextoTres = new Chunk(" \"LA EMPRESA\" De otra parte: ", m_m_fuenteArialNegritaOnce);
            Chunk c1_TextoCuatro = new Chunk(" La Empresa, Negocio o Persona Natural " + InfoCliente.RazonSocial + " con número de ", m_fuenteArialOnce);
            Chunk c1_TextoCinco = new Chunk(" RUC ", m_m_fuenteArialNegritaOnce);
            Chunk c1_TextoSeis = new Chunk(" " + contenido.C1_TextoSeis, m_fuenteArialOnce);
            Chunk c1_TextoSiete = new Chunk(" \"EL CLIENTE\", ", m_m_fuenteArialNegritaOnce);
            Chunk c1_TextoOcho = new Chunk(contenido.C1_TextoOcho, m_fuenteArialOnce);

            Phrase ph_Uno = new Phrase();
            ph_Uno.Add(c1_NombreEmpresa);
            ph_Uno.Add(c1_textoUno);
            ph_Uno.Add(c1_RucEmpresa);
            ph_Uno.Add(c1_TextoDos);
            ph_Uno.Add(c1_TextoTres);
            ph_Uno.Add(c1_TextoCuatro);
            ph_Uno.Add(c1_TextoCinco);
            ph_Uno.Add(c1_TextoSeis);
            ph_Uno.Add(c1_TextoSiete);
            ph_Uno.Add(c1_TextoOcho);

            Paragraph pf_Uno = new Paragraph();
            pf_Uno.Add(ph_Uno);
            pf_Uno.Alignment = Element.ALIGN_JUSTIFIED;
            m_documento.Add(pf_Uno);
            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Segundo Parrago

            #region Chunk 2
            Chunk c2_textoUno = new Chunk("1.- OBJETO DE CONTRATO.", m_m_fuenteArialNegritaOnce);
            Chunk c2_textoDos = new Chunk(" Brindar a", m_fuenteArialOnce);
            Chunk c2_textoTres = new Chunk(" EL CLIENTE ", m_m_fuenteArialNegritaOnce);
            Chunk c2_textoCuatro = new Chunk(contenido.C2_TextoCuatro, m_fuenteArialOnce);
            Chunk c2_DireccionElectronica = new Chunk(" " + infoGeneral.UrlSipecom, m_m_fuenteArialNegritaOnce);
            Chunk c2_textoCinco = new Chunk("; este sistema permitirá a ", m_fuenteArialOnce);
            Chunk c2_textoSeis = new Chunk(contenido.C2_TextoSeis, m_fuenteArialOnce);
            var documentos = ObtenerNombreDocumento(ListDocumentos);

            Chunk c2_textoSiete = new Chunk(documentos + ". ", m_fuenteArialOnce);
            #endregion

            #region Chunk 3
            Chunk c3_textoUno = new Chunk("2.- FIRMA DIGITAL. " + c2_textoTres, m_m_fuenteArialNegritaOnce);
            Chunk c3_textoDos = new Chunk("tendrá la obligación de obtener la firma digital en formato archivo. ", m_fuenteArialOnce);
            #endregion

            #region Chunk 4
            Chunk c4_textoUno = new Chunk("3.- USO. " + c2_textoTres, m_m_fuenteArialNegritaOnce);
            Chunk c4_textoDos = new Chunk(contenido.C4_TextoDos, m_fuenteArialOnce);
            Chunk c4_textoTres = new Chunk(" INVOICEC ", m_m_fuenteArialNegritaOnce);
            Chunk c4_textoCuatro = new Chunk(" y el servidor del  ", m_fuenteArialOnce);
            Chunk c4_textoCinco = new Chunk("SRI. ", m_m_fuenteArialNegritaOnce);
            #endregion

            #region Chunk 5
            var empresa = "LA EMPRESA";
            Chunk c5_textoUno = new Chunk("4.- RESPALDOS. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c5_textoDos = new Chunk(" a través de sus recursos se compromete a ", m_fuenteArialOnce);
            Chunk c5_textoTres = new Chunk("RESPALDAR ", m_m_fuenteArialNegritaOnce);
            Chunk c5_textoCuatro = new Chunk(" los documentos digitales de ", m_fuenteArialOnce);
            Chunk c5_textoCinco = new Chunk("EL CLIENTE ", m_m_fuenteArialNegritaOnce);
            Chunk c5_textoSeis = new Chunk("automáticamente haciendo una copia exacta a otro servidor del XML autorizado por el ", m_fuenteArialOnce);
            Chunk c5_SRI = new Chunk("SRI ", m_m_fuenteArialNegritaOnce);
            Chunk c5_textoSiete = new Chunk(contenido.C5_TextoSiete, m_fuenteArialOnce);
            #endregion

            #region Chunk 6
            Chunk c6_textoUno = new Chunk("5.- FALLAS CON EL SERVIDOR DEL SRI. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c6_textoDos = new Chunk(" " + contenido.C6_TextoDos, m_fuenteArialOnce);
            #endregion

            #region Chunk 7
            Chunk c7_textoUno = new Chunk("6.- PROVEEDOR DE INTERNET. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c7_textoDos = new Chunk(" no se hace responsable por fallos de su proveedor de internet local o fallas de redes internas en", m_fuenteArialOnce);
            Chunk c7_textoTres = new Chunk(" EL CLIENTE. ", m_m_fuenteArialNegritaOnce);
            #endregion

            #region Chunk 8
            Chunk c8_textoUno = new Chunk("7.- CONFIDENCIALIDAD. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c8_textoDos = new Chunk(" " + contenido.C8_TextoDos, m_fuenteArialOnce);
            Chunk c8_DireccionElectronica = new Chunk(" " + infoGeneral.UrlSipecom, m_m_fuenteArialNegritaOnce);
            Chunk c8_textoTres = new Chunk(" ya que la información de los XML es de carácter público. ", m_fuenteArialOnce);
            #endregion

            #region Chunk 9
            Chunk c9_textoUno = new Chunk("8.- PLANES Y PRECIOS. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c9_textoDos = new Chunk(" pone a consideración ", m_fuenteArialOnce);
            Chunk c9_textoTres = new Chunk("DEL CLIENTE ", m_m_fuenteArialNegritaOnce);
            Chunk c9_textoCuatro = new Chunk(contenido.C9_TextoCuatro, m_fuenteArialOnce);
            Chunk c9_textoCinco = new Chunk("SIPECOM S.A. y EL CLIENTE:", m_m_fuenteArialNegritaOnce);
            #endregion



            Phrase ph_Dos = new Phrase();

            #region Chunk 2
            ph_Dos.Add(c2_textoUno);
            ph_Dos.Add(c2_textoDos);
            ph_Dos.Add(c2_textoTres);
            ph_Dos.Add(c2_textoCuatro);
            ph_Dos.Add(c2_DireccionElectronica);
            ph_Dos.Add(c2_textoCinco);
            ph_Dos.Add(c2_textoTres);
            ph_Dos.Add(c2_textoSeis);
            ph_Dos.Add(c2_textoSiete);
            #endregion

            #region Chunk 3
            ph_Dos.Add(c3_textoUno);
            ph_Dos.Add(c3_textoDos);
            #endregion

            #region Chunk 4
            ph_Dos.Add(c4_textoUno);
            ph_Dos.Add(c4_textoDos);
            ph_Dos.Add(c4_textoTres);
            ph_Dos.Add(c4_textoCuatro);
            ph_Dos.Add(c4_textoCinco);
            #endregion

            #region Chunk 5
            ph_Dos.Add(c5_textoUno);
            ph_Dos.Add(c5_textoDos);
            ph_Dos.Add(c5_textoTres);
            ph_Dos.Add(c5_textoCuatro);
            ph_Dos.Add(c5_textoCinco);
            ph_Dos.Add(c5_textoSeis);
            ph_Dos.Add(c5_SRI);
            ph_Dos.Add(c5_textoSiete);
            #endregion

            #region Chunk 6
            ph_Dos.Add(c6_textoUno);
            ph_Dos.Add(c6_textoDos);
            #endregion

            #region Chunk 7
            ph_Dos.Add(c7_textoUno);
            ph_Dos.Add(c7_textoDos);
            ph_Dos.Add(c7_textoTres);
            #endregion

            #region Chunk 8
            ph_Dos.Add(c8_textoUno);
            ph_Dos.Add(c8_textoDos);
            ph_Dos.Add(c8_DireccionElectronica);
            ph_Dos.Add(c8_textoTres);
            #endregion

            #region Chunk 9
            ph_Dos.Add(c9_textoUno);
            ph_Dos.Add(c9_textoDos);
            ph_Dos.Add(c9_textoTres);
            ph_Dos.Add(c9_textoCuatro);
            ph_Dos.Add(c9_textoCinco);
            #endregion

            Paragraph pf_Dos = new Paragraph();
            pf_Dos.Add(ph_Dos);
            pf_Dos.Alignment = Element.ALIGN_JUSTIFIED;
            m_documento.Add(pf_Dos);
            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Tabla
            PdfPTable table = new PdfPTable(4);
            table.DefaultCell.FixedHeight = 30f;
            table.WidthPercentage = 100f;
            int[] firstTablecellwidth = { 20, 50, 20, 64 };
            table.SetWidths(firstTablecellwidth);
            // Esta es la primera fila
            PdfPCell cellNombrePlan = new PdfPCell(new Phrase("Nombre del Plan: ", FontFactory.GetFont("Arial", 11, Font.BOLD)));
            table.AddCell(cellNombrePlan);
            PdfPCell cellNombrePlan2 = new PdfPCell(new Phrase(InfoCliente.NombrePlan, FontFactory.GetFont("Arial", 11)));
            cellNombrePlan2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellNombrePlan2.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellNombrePlan2);

            PdfPCell cellPrecio = new PdfPCell(new Phrase("Precio: ", FontFactory.GetFont("Arial", 11, Font.BOLD)));
            table.AddCell(cellPrecio);
            PdfPCell cellPrecio2 = new PdfPCell(new Phrase("USD$ " + InfoCliente.Precio + " + IVA", FontFactory.GetFont("Arial", 11)));
            cellPrecio2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellPrecio2.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellPrecio2);

            // Segunda fila
            PdfPCell cellCoberturaPlan = new PdfPCell(new Phrase("Cobertura del Plan: ", FontFactory.GetFont("Arial", 11, Font.BOLD)));
            table.AddCell(cellCoberturaPlan);
            PdfPCell cellCoberturaPlan2 = new PdfPCell(new Phrase("Un(1) año calendario", FontFactory.GetFont("Arial", 11)));
            cellCoberturaPlan2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellCoberturaPlan2.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellCoberturaPlan2);

            PdfPCell cellFormaPago = new PdfPCell(new Phrase("Forma de pago: ", FontFactory.GetFont("Arial", 11, Font.BOLD)));
            table.AddCell(cellFormaPago);
            PdfPCell cellFormaPago2 = new PdfPCell(new Phrase("Efectivo - Con Uso de Sistema Financiero", FontFactory.GetFont("Arial", 11)));
            cellFormaPago2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFormaPago2.VerticalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellFormaPago2);
            m_documento.Add(table);
            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Tercer Parrafo

            #region Chunk 10
            Chunk c10_textoUno = new Chunk("9.- INCREMENTO DE PRECIO. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c10_textoDos = new Chunk(" asegura que durante el contrato vigente no habrá incremento de precio. Pasando la vigencia del contrato y si ", m_fuenteArialOnce);
            Chunk c10_textoTres = new Chunk(empresa, m_m_fuenteArialNegritaOnce);
            Chunk c10_textoCuatro = new Chunk(" amerita un incremento de precio para mejoras del sistema solo podrá aumentar un 5% del valor del plan seleccionado por ", m_fuenteArialOnce);
            Chunk c10_textoCinco = new Chunk("EL CLIENTE. ", m_m_fuenteArialNegritaOnce);
            #endregion

            #region Chunk 11
            Chunk c11_textoUno = new Chunk("10.- FORMAS DE PAGO. " + c2_textoTres, m_m_fuenteArialNegritaOnce);
            Chunk c11_textoDos = new Chunk(" deberá cancelar a ", m_fuenteArialOnce);
            Chunk c11_textoTres = new Chunk(empresa, m_m_fuenteArialNegritaOnce);
            Chunk c11_textoCuatro = new Chunk(" una sola vez la cantidad pactada por el uso del plan, una vez firmado el contrato, a través de: oficina en efectivo o cheque cruzado a nombre de ", m_fuenteArialOnce);
            Chunk c11_textoCinco = new Chunk(", un depósito o transferencia bancario; una vez realizado el pago ", m_fuenteArialOnce);
            Chunk c11_textoSeis = new Chunk(" deberá enviar un correo electrónico con el comprobante de depósito escaneado o captura de imagen de la transferencia bancaria a ", m_fuenteArialOnce);
            Chunk c11_textoSiete = new Chunk(infoGeneral.EmailContable + " ", m_m_fuenteArialNegritaOnce);
            #endregion

            #region Chunk 12
            Chunk c12_textoUno = new Chunk("11.- DURACIÓN DEL CONTRATO. " + empresa, m_m_fuenteArialNegritaOnce);
            Chunk c12_textoDos = new Chunk(" ofrece a", m_fuenteArialOnce);
            Chunk c12_textoTres = new Chunk(" un contrato de 12 meses, el contrato es válido a partir de la fecha y rúbrica del mismo, y será renovable por los 12 meses siguientes, a menos que ", m_fuenteArialOnce);
            Chunk c12_textoCuatro = new Chunk(", disponga su deseo de NO continuar usando este servicio. ", m_fuenteArialOnce);
            #endregion

            #region Chunk 13
            Chunk c13_textoUno = new Chunk("12.- CAMBIOS ", m_m_fuenteArialNegritaOnce);
            Chunk c13_textoDos = new Chunk(contenido.C13_TextoDos + " ", m_fuenteArialOnce);
            Chunk c13_textoTres = new Chunk("30 días de anticipación ", m_m_fuenteArialNegritaOnce);
            Chunk c13_textoCuatro = new Chunk("a la fecha de vencimiento del plan anual. ", m_fuenteArialOnce);
            #endregion

            #region Chunk 14
            Chunk c14_textoUno = new Chunk("13.- CANCELACIÓN DEL CONTRATO ", m_m_fuenteArialNegritaOnce);
            Chunk c14_textoDos = new Chunk("Si ", m_fuenteArialOnce);
            Chunk c14_textoTres = new Chunk("dispusiere la cancelación del contrato por razones ajenas al servicio ofrecido no se le devolverá valores totales ni parciales cancelados. ", m_fuenteArialOnce);
            #endregion

            #region Chunk 15
            Chunk c15_textoUno = new Chunk("14.- DISPOSICIONES FINALES. ", m_m_fuenteArialNegritaOnce);
            Chunk c15_textoDos = new Chunk(contenido.C15_TextoDos, m_fuenteArialOnce);
            #endregion


            Phrase ph_Tres = new Phrase();

            #region Chunk 10
            ph_Tres.Add(c10_textoUno);
            ph_Tres.Add(c10_textoDos);
            ph_Tres.Add(c10_textoTres);
            ph_Tres.Add(c10_textoCuatro);
            ph_Tres.Add(c10_textoCinco);
            #endregion

            #region Chunk 11
            ph_Tres.Add(c11_textoUno);
            ph_Tres.Add(c11_textoDos);
            ph_Tres.Add(c11_textoTres);
            ph_Tres.Add(c11_textoCuatro);
            ph_Tres.Add(c11_textoTres);
            ph_Tres.Add(c11_textoCinco);
            ph_Tres.Add(c2_textoTres);
            ph_Tres.Add(c11_textoSeis);
            ph_Tres.Add(c11_textoSiete);
            #endregion

            #region Chunk 12
            ph_Tres.Add(c12_textoUno);
            ph_Tres.Add(c12_textoDos);
            ph_Tres.Add(c2_textoTres);
            ph_Tres.Add(c12_textoTres);
            ph_Tres.Add(c2_textoTres);
            ph_Tres.Add(c12_textoCuatro);
            #endregion

            #region Chunk 13
            ph_Tres.Add(c13_textoUno);
            ph_Tres.Add(c13_textoDos);
            ph_Tres.Add(c13_textoTres);
            ph_Tres.Add(c13_textoCuatro);
            #endregion

            #region Chunk 14
            ph_Tres.Add(c14_textoUno);
            ph_Tres.Add(c14_textoDos);
            ph_Tres.Add(c2_textoTres);
            ph_Tres.Add(c14_textoTres);
            #endregion

            #region Chunk 15
            ph_Tres.Add(c15_textoUno);
            ph_Tres.Add(c15_textoDos);
            #endregion

            Paragraph pf_Tres = new Paragraph();
            pf_Tres.Add(ph_Tres);
            pf_Tres.Alignment = Element.ALIGN_JUSTIFIED;
            m_documento.Add(pf_Tres);

            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Fecha
            var fecha = DateTime.Now;
            var dia = fecha.Day;
            var mes = fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-EC"));
            var anio = fecha.Year;

            Chunk cf_textoUno = new Chunk(InfoCliente.FechaCompletaInicioPlan, m_fuenteArialOnce);
            Paragraph pf_Cuatro = new Paragraph();
            pf_Cuatro.Add(cf_textoUno);
            m_documento.Add(pf_Cuatro);
            #endregion

            m_documento.Add(Chunk.NEWLINE);
            m_documento.Add(Chunk.NEWLINE);
            m_documento.Add(Chunk.NEWLINE);
            m_documento.Add(Chunk.NEWLINE);

            #region Firmas
            PdfPTable tableFirma = new PdfPTable(2);
            tableFirma.DefaultCell.FixedHeight = 30f;
            tableFirma.WidthPercentage = 100f;

            try
            {
                var jpgSipecom = Image.GetInstance(RutaLogoSipecom);
                jpgSipecom.ScaleAbsolute(100f, 50f);

                PdfPCell cellImagenFirmaSipecom = new PdfPCell(jpgSipecom);
                cellImagenFirmaSipecom.Border = Rectangle.NO_BORDER;
                cellImagenFirmaSipecom.HorizontalAlignment = Element.ALIGN_CENTER;
                cellImagenFirmaSipecom.VerticalAlignment = Element.ALIGN_CENTER;
                tableFirma.AddCell(cellImagenFirmaSipecom);

            }
            catch (Exception)
            {
            }

            try
            {
                var jpgCliente = Image.GetInstance(RutaLogoCliente);
                jpgCliente.ScaleAbsolute(100f, 50f);

                PdfPCell cellImagenFirmaCliente = new PdfPCell(jpgCliente);
                cellImagenFirmaCliente.Border = Rectangle.NO_BORDER;
                cellImagenFirmaCliente.HorizontalAlignment = Element.ALIGN_CENTER;
                cellImagenFirmaCliente.VerticalAlignment = Element.ALIGN_CENTER;
                tableFirma.AddCell(cellImagenFirmaCliente);
            }
            catch (Exception)
            {
            }

            PdfPCell cellRayaSipecom = new PdfPCell(new Phrase("__________________________ ", FontFactory.GetFont("Arial", 11)));
            cellRayaSipecom.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRayaSipecom.VerticalAlignment = Element.ALIGN_CENTER;
            cellRayaSipecom.Border = 0;
            tableFirma.AddCell(cellRayaSipecom);

            PdfPCell cellRayaCliente = new PdfPCell(new Phrase("__________________________ ", FontFactory.GetFont("Arial", 11)));
            cellRayaCliente.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRayaCliente.VerticalAlignment = Element.ALIGN_CENTER;
            cellRayaCliente.Border = 0;
            tableFirma.AddCell(cellRayaSipecom);

            PdfPCell cellFirmaSipecom = new PdfPCell(new Phrase("Firma ", FontFactory.GetFont("Arial", 11)));
            cellFirmaSipecom.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFirmaSipecom.VerticalAlignment = Element.ALIGN_CENTER;
            cellFirmaSipecom.Border = 0;
            tableFirma.AddCell(cellFirmaSipecom);

            PdfPCell cellFirmaCliente = new PdfPCell(new Phrase("Firma", FontFactory.GetFont("Arial", 11)));
            cellFirmaCliente.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFirmaCliente.VerticalAlignment = Element.ALIGN_CENTER;
            cellFirmaCliente.Border = 0;
            tableFirma.AddCell(cellFirmaCliente);

            PdfPCell cellReprestSipeom = new PdfPCell(new Phrase(InfoSipecom.RepresentanteLegal, FontFactory.GetFont("Arial", 11)));
            cellReprestSipeom.HorizontalAlignment = Element.ALIGN_CENTER;
            cellReprestSipeom.VerticalAlignment = Element.ALIGN_CENTER;
            cellReprestSipeom.Border = 0;
            tableFirma.AddCell(cellReprestSipeom);

            PdfPCell cellReprestCliente = new PdfPCell(new Phrase("Nombre: " + InfoCliente.RepresentanteLegal, FontFactory.GetFont("Arial", 11)));
            cellReprestCliente.HorizontalAlignment = Element.ALIGN_CENTER;
            cellReprestCliente.VerticalAlignment = Element.ALIGN_CENTER;
            cellReprestCliente.Border = 0;
            tableFirma.AddCell(cellReprestCliente);

            PdfPCell cellIdentSipecom = new PdfPCell(new Phrase("CI. " + InfoSipecom.IdentificacionRepresentante, FontFactory.GetFont("Arial", 11)));
            cellIdentSipecom.HorizontalAlignment = Element.ALIGN_CENTER;
            cellIdentSipecom.VerticalAlignment = Element.ALIGN_CENTER;
            cellIdentSipecom.Border = 0;
            tableFirma.AddCell(cellIdentSipecom);

            PdfPCell cellIdentCliente = new PdfPCell(new Phrase("CI. " + InfoCliente.IdentificacionRepresentante, FontFactory.GetFont("Arial", 11)));
            cellIdentCliente.HorizontalAlignment = Element.ALIGN_CENTER;
            cellIdentCliente.VerticalAlignment = Element.ALIGN_CENTER;
            cellIdentCliente.Border = 0;
            tableFirma.AddCell(cellIdentCliente);

            m_documento.Add(tableFirma);
            #endregion

            m_writer.CloseStream = false;
            m_documento.Close();
        }

        #endregion

        #region Metodos privados

        #region Metodos de inicializacion

        private void InicializarFuentes()
        {
            m_fuenteArialOnce = FontFactory.GetFont("Arial", 11f, Font.NORMAL, BaseColor.BLACK);
            m_m_fuenteArialNegritaOnce = FontFactory.GetFont("Arial", 11f, Font.BOLD);
            m_fuenteArialNegritaDoce = FontFactory.GetFont("Arial", 12f, Font.BOLD);
        }

        #endregion

        private ContratoModelo ObtenerContenidoContrato(InfoContrato InfoCliente, InfoContrato InfoSipecom, InfoContrato infoGeneral, List<string> ListDocumentos)
        {
            var objContrato = new ContratoModelo();
            try
            {
                var numeroDocumento = (ListDocumentos != null && ListDocumentos.Count > 0) ? ListDocumentos.Count : 0;
                objContrato.Titulo = String.Format(ContratoInfo.CtrtTitulo);

                objContrato.NombreEmpresa = String.Format(ContratoInfo.CtrtNombreEmpresa, InfoSipecom.RazonSocial);
                objContrato.RucEmpresa = String.Format(ContratoInfo.CtrtRucEmpresa, InfoSipecom.Ruc);
                objContrato.C1_TextoDos = String.Format(ContratoInfo.Ctrt1TextoDos, InfoSipecom.Direccion, InfoSipecom.Ciudad, InfoSipecom.Provincia);
                objContrato.C1_TextoSeis = String.Format(ContratoInfo.Ctrt1TextoSeis, InfoCliente.Ruc, InfoCliente.Direccion, InfoCliente.Ciudad, InfoCliente.Celular, InfoCliente.CorreoAvisoCertificado);
                objContrato.C1_TextoOcho = String.Format(ContratoInfo.Ctrt1TextoOcho, InfoCliente.RepresentanteLegal + ".");

                objContrato.C2_TextoCuatro = String.Format(ContratoInfo.Ctrt2TextoCuatro);
                objContrato.C2_TextoSeis = String.Format(ContratoInfo.Ctrt2TextoSeis, numeroDocumento.ToString());

                objContrato.C4_TextoDos = String.Format(ContratoInfo.Ctrt4TextoDos, numeroDocumento.ToString());

                objContrato.C5_TextoSiete = String.Format(ContratoInfo.Ctrt5TextoSiete);

                objContrato.C6_TextoDos = String.Format(ContratoInfo.Ctrt6TextoDos);

                objContrato.C8_TextoDos = String.Format(ContratoInfo.Ctrt8TextoDos);

                objContrato.C9_TextoCuatro = String.Format(ContratoInfo.Ctrt9TextCuatro);

                objContrato.C13_TextoDos = String.Format(ContratoInfo.Ctrt13TextoDos);

                objContrato.C15_TextoDos = String.Format(ContratoInfo.Ctrt15TextoDos);
            }
            catch (Exception)
            {

            }


            return objContrato;
        }

        private string ObtenerNombreDocumento(List<string> listDocumento)
        {
            var documentos = "";
            foreach (var item in listDocumento)
            {
                documentos += " " + item + ",";
            }
            if (documentos.EndsWith(","))
            {
                documentos = documentos.Remove(documentos.Length - 1);
            }
            return documentos;
        }

        #endregion

        public class ContratoModelo
        {
            public string Titulo { get; set; }
            public string NombreEmpresa { get; set; }
            public string RucEmpresa { get; set; }
            public string C1_TextoDos { get; set; }
            public string C1_TextoSeis { get; set; }
            public string C1_TextoOcho { get; set; }
            public string C2_TextoCuatro { get; set; }
            public string C2_TextoSeis { get; set; }
            public string C4_TextoDos { get; set; }
            public string C5_TextoSiete { get; set; }
            public string C6_TextoDos { get; set; }
            public string C8_TextoDos { get; set; }
            public string C9_TextoCuatro { get; set; }
            public string C13_TextoDos { get; set; }
            public string C15_TextoDos { get; set; }
        }
    }
}

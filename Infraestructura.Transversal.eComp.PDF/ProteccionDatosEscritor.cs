using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.eComp.PDF
{
    public class ProteccionDatosEscritor
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

        internal ProteccionDatosEscritor(Stream flujo)
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

        internal void Escribir(string politica)
        {
            m_documento = new Document();
            m_writer = PdfWriter.GetInstance(m_documento, m_flujo);
            m_documento.Open();

            #region Titulo
            Chunk c_titulo = new Chunk("AUTORIZACIÓN PARA EL TRATAMIENTO DE DATOS PERSONALES", m_fuenteArialNegritaDoce);
            Phrase ph_Titulo = new Phrase();
            ph_Titulo.Add(c_titulo);

            Paragraph pf_titulo = new Paragraph();
            pf_titulo.Add(ph_Titulo);
            pf_titulo.Alignment = Element.ALIGN_CENTER;
            m_documento.Add(pf_titulo);
            #endregion

            m_documento.Add(Chunk.NEWLINE);

            #region Primer Parrafo

            Chunk c1_Descripcion = new Chunk(politica, m_fuenteArialOnce);

            Phrase ph_Uno = new Phrase();
            ph_Uno.Add(c1_Descripcion);

            Paragraph pf_Uno = new Paragraph();
            pf_Uno.Add(ph_Uno);
            pf_Uno.Alignment = Element.ALIGN_JUSTIFIED;
            m_documento.Add(pf_Uno);
            #endregion

            m_writer.CloseStream = false;
            m_documento.Close();
        }

        #endregion

        private void InicializarFuentes()
        {
            m_fuenteArialOnce = FontFactory.GetFont("Arial", 11f, Font.NORMAL, BaseColor.BLACK);
            m_m_fuenteArialNegritaOnce = FontFactory.GetFont("Arial", 11f, Font.BOLD);
            m_fuenteArialNegritaDoce = FontFactory.GetFont("Arial", 12f, Font.BOLD);
        }

    }
}

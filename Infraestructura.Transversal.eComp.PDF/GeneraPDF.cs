using Infraestructura.Transversal.eComp.PDF.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Infraestructura.Transversal.eComp.PDF
{
    public class GeneraPDF
    {
        public struct TableDetalleComprobante
        {
            public int desde;
            public int hasta;
            public PdfPTable Detalles;
        }

        public GeneraPDF()
        {


            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }
        }

        public struct TableDetalleFactura
        {
            public int desde;
            public int hasta;
            public PdfPTable Detalles;
        }

        public struct TableGuiRemision
        {
            public PdfPTable Cabecera;
            public PdfPTable Detalles;
        }
        private PdfPCell AgregarCeldaTablaLayout(PdfPTable tabla, PdfPTable tablaAgregar)
        {
            return tabla.AddCell(new PdfPCell(tablaAgregar)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 4f
            });
        }
        private PdfPCell AgregarCeldaTabla(PdfPTable tabla, PdfPTable tablaAgregar)
        {
            return tabla.AddCell(new PdfPCell(tablaAgregar)
            {
                CellEvent = new RoundRectangle(),
                Border = Rectangle.NO_BORDER
            });
        }
        private PdfPCell AgregarCeldaTablaEnlazada(PdfPTable tabla, PdfPTable tablaAgregar)
        {
            return tabla.AddCell(new PdfPCell(tablaAgregar)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0f
            });
        }

        public byte[] GeneraFactura(string pDocumento_autorizado, string pRutaLogo, string direccion = "", string direccionCliente = "", string pCultura = "en-US")
        {
            return GeneraFactura(null, pDocumento_autorizado, pRutaLogo, direccion, direccionCliente, pCultura);
        }
        /// <summary>
        /// Método para generación de factura RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraFactura(string p_Documento_noAutorizado, string pDocumento_autorizado, string pRutaLogo, string direccion = "", string direccionCliente = "", string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            string IVAride = "";
            DateTime FechaEmisionValida = DateTime.Now;

            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    XmlDocument oDocument_NoAutorizado = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "", sAmbienteVal = "", GuiaRemisiontxt = "", rimpe = "",
                           sNumAutorizacion = "", sObligadoContabilidad = "", RucTransportista = "", PuntoEmisionTrans = "", Regimen = "", RazonSocialTransportista = "", RegMicroEmp = "", AgenteRetencionValue = "";
                    Cultura = pCultura;
                    Boolean isTransportista = false;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);
                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {
                        try
                        {
                            oDocument_NoAutorizado.LoadXml(p_Documento_noAutorizado);

                            RucTransportista = oDocument_NoAutorizado.DocumentElement.Attributes["rr"].Value;
                            PuntoEmisionTrans = oDocument_NoAutorizado.DocumentElement.Attributes["pt"].Value;
                            RazonSocialTransportista = oDocument_NoAutorizado.DocumentElement.Attributes["rt"].Value;
                            Regimen = oDocument_NoAutorizado.DocumentElement.Attributes["rg"].Value;
                            isTransportista = true;

                        }
                        catch (Exception e)
                        {
                            isTransportista = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {
                        try
                        {
                            GuiaRemisiontxt = oDocument_NoAutorizado.DocumentElement.Attributes["rm"].Value;
                        }
                        catch (Exception)
                        {
                        }

                    }

                    bool Excep = false;
                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {

                        try
                        {
                            RegMicroEmp = oDocument_NoAutorizado.DocumentElement.Attributes["regmicroemp"].Value;
                        }
                        catch (Exception)
                        {
                            Excep = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {
                        try
                        {
                            rimpe = oDocument_NoAutorizado.DocumentElement.Attributes["ri"].Value;
                        }
                        catch (Exception)
                        {
                        }

                    }

                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    var establ = oDocument.SelectSingleNode("//infoTributaria/estab").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad").InnerText;

                    int registros = 0;
                    int PagLimite1 = 30;
                    int MaxPagina1 = 42;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    int NumRegistrosxPagina = 0;
                    int NumPaginas = 0;
                    Boolean PrimeraPagina = true;
                    float LimitePagina = 0;

                    List<TableDetalleFactura> ListDetalle = new List<TableDetalleFactura>();

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    Document documento = new Document();

                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                    iTextSharp.text.Font detAdicional = GetArial(6);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;
                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();

                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeAltoVariable = 0D;
                    var porcentajeAnchoVariable = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;
                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);


                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));



                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {
                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {
                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);

                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    RUC.PaddingTop = 7f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("F A C T U R A", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    Factura.PaddingTop = 7f;

                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA DE AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    FechaAutorizacion.PaddingBottom = 6f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    Ambiente.PaddingBottom = 6f;

                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    Emision.PaddingTop = 10f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        Emision.PaddingBottom = 10f;
                    }

                    innerTableD.AddCell(Emision);


                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    ClaveAcceso.PaddingTop = 6f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }

                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);
                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.PaddingTop = 6f;
                    ImgClaveAcceso.PaddingBottom = 7f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;
                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);
                   
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 10f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                RazonSocial.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            RazonSocial.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        RazonSocial.FixedHeight = 25f;
                    }

                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dirección Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 6f;

                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dirección Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 3f;
                        DirSucursal.PaddingTop = 6f;

                        innerTableL.AddCell(DirSucursal);
                    }

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro:  " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 5f;
                    ContribuyenteEspecial.PaddingTop = 6f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD:  " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    innerTableL.AddCell(ObligadoContabilidad);

                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (RegMicroEmp.ToLower().Equals("si"))
                            {
                                PdfPCell RegimenMicroEmpresa = new PdfPCell(new Paragraph("Contribuyente Régimen Microempresas.", cabecera));
                                RegimenMicroEmpresa.Border = Rectangle.NO_BORDER;
                                RegimenMicroEmpresa.Padding = 5f;
                                RegimenMicroEmpresa.PaddingBottom = 3f;
                                RegimenMicroEmpresa.PaddingTop = 3f;
                                innerTableL.AddCell(RegimenMicroEmpresa);
                            }
                        }
                    }



                    XmlNodeList IA = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    bool agenteRetencionInfoA = false;


                    foreach (XmlNode campoAdicional in IA)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "Agente")
                        {
                            PdfPCell AgenteRetencion = new PdfPCell(new Paragraph(campoAdicional.InnerText, cabecera));
                            AgenteRetencion.Border = Rectangle.NO_BORDER;
                            AgenteRetencion.Padding = 5f;
                            AgenteRetencion.PaddingBottom = 5f;
                            innerTableL.AddCell(AgenteRetencion);
                            agenteRetencionInfoA = true;
                        }
                    }

                    try
                    {
                        string agente = oDocument.SelectSingleNode("//infoTributaria/agenteRetencion").InnerText;
                        if (!string.IsNullOrEmpty(agente))
                        {
                            if (!agenteRetencionInfoA)
                            {
                                agente = agente.TrimStart(new Char[] { '0' });
                                PdfPCell AgenteRetencion = new PdfPCell(new Paragraph("Agente de Retención Resolución No." + agente, cabecera));
                                AgenteRetencion.Border = Rectangle.NO_BORDER;
                                AgenteRetencion.Padding = 5f;
                                AgenteRetencion.PaddingBottom = 5f;

                                innerTableL.AddCell(AgenteRetencion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        if (!string.IsNullOrEmpty(rimpe))
                        {
                            PdfPCell rimpe_ = new PdfPCell(new Paragraph(rimpe, cabecera));
                            rimpe_.Border = Rectangle.NO_BORDER;
                            rimpe_.Padding = 5f;
                            rimpe_.PaddingBottom = 3f;

                            innerTableL.AddCell(rimpe_);
                        }
                    }
                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 250f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);

                    #endregion

                    #region Logo
                    BaseColor color = null;
                    iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = null;
                    logo = new PdfPCell(jpgPrueba);

                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;



                    #endregion
                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);
                    #region DetalleCliente
                    PdfPTable tableDetalleCliente = new PdfPTable(4);
                    tableDetalleCliente.TotalWidth = 540f;
                    tableDetalleCliente.WidthPercentage = 100;
                    float[] DetalleClientewidths = new float[] { 30f, 120f, 30f, 40f };
                    tableDetalleCliente.SetWidths(DetalleClientewidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/razonSocialComprador").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("Identificación:", detalle)); ///Identificación RUC / CI: Cambio solicitado por BB
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/identificacionComprador").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                    tableDetalleCliente.AddCell(lblFechaEmisionCliente);

                    FechaEmisionValida = DateTime.ParseExact(oDocument.SelectSingleNode("//infoFactura/fechaEmision").InnerText, "dd/MM/yyyy", new CultureInfo(Cultura));

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.NO_BORDER;
                    tableDetalleCliente.AddCell(FechaEmisionCliente);
                    var lblGuiaRemision = new PdfPCell(new Paragraph("Guia Remisión:", detalle));
                    lblGuiaRemision.Border = Rectangle.NO_BORDER;
                    tableDetalleCliente.AddCell(lblGuiaRemision);

                    var GuiaRemision = new PdfPCell(new Paragraph((GuiaRemisiontxt == null ? "" : GuiaRemisiontxt), detalle));
                    GuiaRemision.Border = Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(GuiaRemision);

                    var lblDireccion = new PdfPCell(new Paragraph("Dirección:", detalle));
                    lblDireccion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblDireccion);
                    var _direccion = oDocument.SelectSingleNode("//direccionComprador");
                    var direccionComprador = _direccion != null ? oDocument.SelectSingleNode("//direccionComprador").InnerText : direccionCliente;
                    var Direccion = new PdfPCell(new Paragraph(direccionComprador, detalle));
                    Direccion.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    Direccion.Colspan = 4;
                    tableDetalleCliente.AddCell(Direccion);

                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(8); //10 //9
                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    tableDetalleFactura.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 30f, 120f, 20f, 25f, 25f, 25f, 26f, 28f };
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    PdfPTable tableDetalleFacturaPagina = new PdfPTable(10); //10 //9
                    tableDetalleFacturaPagina.TotalWidth = 540f;
                    tableDetalleFacturaPagina.WidthPercentage = 100;
                    tableDetalleFacturaPagina.LockedWidth = true;
                    float[] DetalleFacturawidthsPagina = new float[] { 30f, 20f, 20f, 120f, 57f, 23f, 23f, 25f, 26f, 28f };
                    tableDetalleFacturaPagina.SetWidths(DetalleFacturawidthsPagina);

                    var fontEncabezado = GetArial(7);
                    var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCant = new PdfPCell(new Paragraph("Unidades", fontEncabezado));
                    encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                    encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional1 = new PdfPCell(new Paragraph("TM", fontEncabezado));
                    encDetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional2 = new PdfPCell(new Paragraph("Importe Bruto", fontEncabezado));
                    encDetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Precio Unitario", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescuento = new PdfPCell(new Paragraph("Descuento", fontEncabezado));
                    encDescuento.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioTotal = new PdfPCell(new Paragraph("Importe Total", fontEncabezado));
                    encPrecioTotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encCodPrincipal);
                    tableDetalleFactura.AddCell(encDescripcion);
                    tableDetalleFactura.AddCell(encCant);
                    tableDetalleFactura.AddCell(encDetalleAdicional1);
                    tableDetalleFactura.AddCell(encPrecioUnitario);
                    tableDetalleFactura.AddCell(encDetalleAdicional2);
                    tableDetalleFactura.AddCell(encDescuento);
                    tableDetalleFactura.AddCell(encPrecioTotal);

                    PdfPCell CodPrincipal = null;
                    PdfPCell CodAuxiliar = null;
                    PdfPCell Cant;
                    PdfPCell Descripcion;
                    PdfPCell DetalleAdicional1;
                    PdfPCell DetalleAdicional2;
                    PdfPCell DetalleAdicional3;
                    PdfPCell PrecioUnitario;
                    PdfPCell Descuento;
                    PdfPCell PrecioTotal;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//detalles/detalle");
                    registros = Detalles.Count;

                    NumRegistrosxPagina = 1;
                    foreach (XmlNode Elemento in Detalles)
                    {
                        NumRegistrosxPagina = NumRegistrosxPagina + 1;
                        tableDetalleFacturaPagina = new PdfPTable(10);

                        CodPrincipal = new PdfPCell(new Phrase(Elemento["codigoPrincipal"].InnerText, detalle));
                        CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Cant = new PdfPCell(new Phrase(Elemento["cantidad"].InnerText, detalle));
                        Cant.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        Descripcion = new PdfPCell(new Phrase(Elemento["descripcion"].InnerText, detalle));
                        XmlNodeList DetallesAdicionales;

                        DetallesAdicionales = Elemento.SelectNodes("detallesAdicionales/detAdicional");
                        if (!(DetallesAdicionales[0] == null))
                        {
                            DetalleAdicional1 = new PdfPCell(new Phrase(DetallesAdicionales[0].Attributes["valor"].Value, detalle));
                            DetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        else
                        {
                            DetalleAdicional1 = new PdfPCell(new Phrase("", detalle));
                            DetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        if (!(DetallesAdicionales[1] == null))
                        {
                            DetalleAdicional2 = new PdfPCell(new Phrase(DetallesAdicionales[1].Attributes["valor"].Value, detalle));
                            DetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        else
                        {
                            String impTot = Elemento["precioTotalSinImpuesto"].InnerText;
                            String Desc = Elemento["descuento"].InnerText;
                            Double ImpBruto = Convert.ToDouble(impTot, CultureInfo.CreateSpecificCulture("en-US")) +
                                Convert.ToDouble(Desc, CultureInfo.CreateSpecificCulture("en-US"));
                            DetalleAdicional2 = new PdfPCell(new Phrase(ImpBruto.ToString("N2"), detalle));
                            DetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }

                        CultureInfo culture = new CultureInfo("en-US");

                        String precUnit = Elemento["precioUnitario"].InnerText;
                        Double pU = Convert.ToDouble(precUnit, CultureInfo.CreateSpecificCulture("en-US"));
                        PrecioUnitario = new PdfPCell(new Phrase(pU.ToString("N2"), detalle));
                        PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        Descuento = new PdfPCell(new Phrase(Elemento["descuento"].InnerText, detalle));
                        Descuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        String precTotal = Elemento["precioTotalSinImpuesto"].InnerText;
                        Double pT = Convert.ToDouble(precTotal, CultureInfo.CreateSpecificCulture("en-US"));
                        PrecioTotal = new PdfPCell(new Phrase(pT.ToString("N2"), detalle));
                        PrecioTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        if ((tableDetalleFacturaPagina.TotalHeight + LimitePagina) > 520 && PrimeraPagina)
                        {

                            PrimeraPagina = false;
                            TableDetalleFactura ItemDetalle;
                            ItemDetalle = new TableDetalleFactura();
                            ItemDetalle.desde = 0;
                            ItemDetalle.hasta = NumRegistrosxPagina;
                            ItemDetalle.Detalles = tableDetalleFactura;
                            ListDetalle.Add(ItemDetalle);
                            tableDetalleFactura = new PdfPTable(8); //10 //9
                            tableDetalleFactura.TotalWidth = 540f;
                            tableDetalleFactura.WidthPercentage = 100;
                            tableDetalleFactura.LockedWidth = true;
                            float[] DetalleFacturawidthsPaginas = new float[] { 30f, 120f, 20f, 25f, 25f, 25f, 26f, 28f };
                            tableDetalleFactura.SetWidths(DetalleFacturawidthsPaginas);
                            NumRegistrosxPagina = 1;
                        }
                        else if ((tableDetalleFacturaPagina.TotalHeight + LimitePagina) > (460 + (posDetalleCliente - 10 - tableDetalleCliente.TotalHeight)))
                        {
                            TableDetalleFactura ItemDetalle;
                            ItemDetalle = new TableDetalleFactura();
                            ItemDetalle.desde = 0;
                            ItemDetalle.hasta = NumRegistrosxPagina;
                            ItemDetalle.Detalles = tableDetalleFactura;
                            ListDetalle.Add(ItemDetalle);
                            tableDetalleFactura = new PdfPTable(8); //10 //9
                            tableDetalleFactura.TotalWidth = 540f;
                            tableDetalleFactura.WidthPercentage = 100;
                            tableDetalleFactura.LockedWidth = true;
                            float[] DetalleFacturawidthsPaginas = new float[] { 30f, 120f, 20f, 25f, 25f, 25f, 26f, 28f };
                            tableDetalleFactura.SetWidths(DetalleFacturawidthsPaginas);
                            NumRegistrosxPagina = 1;

                        }

                        tableDetalleFactura.AddCell(CodPrincipal);
                        tableDetalleFactura.AddCell(Descripcion);
                        tableDetalleFactura.AddCell(Cant);
                        tableDetalleFactura.AddCell(DetalleAdicional1);
                        tableDetalleFactura.AddCell(PrecioUnitario);
                        tableDetalleFactura.AddCell(DetalleAdicional2);
                        tableDetalleFactura.AddCell(Descuento);
                        tableDetalleFactura.AddCell(PrecioTotal);

                        LimitePagina = tableDetalleFactura.TotalHeight;

                        XmlNodeList DetallesImpuestos;
                        DetallesImpuestos = oDocument.SelectNodes("//detalles/detalle/impuestos/impuesto");
                        if (IVAride == "")
                        {
                            foreach (XmlNode Elementoimp in DetallesImpuestos)
                            {
                                if (Elementoimp["codigo"].InnerText == "2" && Elementoimp["codigoPorcentaje"].InnerText == "2")
                                {
                                    IVAride = Elementoimp["tarifa"].InnerText;
                                    break;
                                }

                                if (Elementoimp["codigo"].InnerText == "2" && Elementoimp["codigoPorcentaje"].InnerText == "3")
                                {
                                    IVAride = Elementoimp["tarifa"].InnerText;
                                    break;
                                }
                            }
                        }
                    }

                    if (tableDetalleFactura.TotalHeight > 0)
                    {
                        TableDetalleFactura ItemDetalle;
                        ItemDetalle = new TableDetalleFactura();
                        ItemDetalle.desde = 0;
                        ItemDetalle.hasta = NumRegistrosxPagina;
                        ItemDetalle.Detalles = tableDetalleFactura;
                        ListDetalle.Add(ItemDetalle);
                    }

                    #endregion

                    #region FormaPagos
                    var tableFormaPago = new PdfPTable(4);
                    tableFormaPago.TotalWidth = 250f;
                    float[] FormaPagosWidths = new float[] { 110f, 50f, 45f, 45f };
                    tableFormaPago.SetWidths(FormaPagosWidths);

                    var lblFormaPago = new PdfPCell(new Paragraph("Forma de Pago", fontEncabezado));
                    lblFormaPago.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var lblFPvalor = new PdfPCell(new Paragraph("Valor", fontEncabezado));
                    lblFPvalor.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblFPvalor.Padding = 2f;

                    var lblFPplazo = new PdfPCell(new Paragraph("Plazo", fontEncabezado));
                    lblFPplazo.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var lblFPTiempo = new PdfPCell(new Paragraph("Tiempo", fontEncabezado));
                    lblFPTiempo.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var lblBottomfp = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottomfp.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottomfp.Padding = 2f;
                    var Bottomfp = new PdfPCell(new Paragraph("  ", detalle));
                    Bottomfp.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottomfp.Padding = 2f;

                    XmlNodeList FormaPagos;
                    FormaPagos = oDocument.SelectNodes("//pagos/pago");

                    int banderapagos = 0;
                    XmlNodeList InfoAdicionalaux;
                    InfoAdicionalaux = oDocument.SelectNodes("//infoAdicional/campoAdicional");
                    foreach (XmlNode pago in FormaPagos)
                    {
                        //buscar descripcion de forma de pago

                        string datocodi = pago["formaPago"].InnerText;
                        string descripcifromapg = "";

                        if (banderapagos == 0)
                        {
                            tableFormaPago.AddCell(lblFormaPago);
                            tableFormaPago.AddCell(lblFPvalor);
                            tableFormaPago.AddCell(lblFPplazo);
                            tableFormaPago.AddCell(lblFPTiempo);
                            banderapagos = 1;
                        }


                        try
                        {
                            descripcifromapg = System.Configuration.ConfigurationManager.AppSettings["F" + datocodi];//Descripcion de la forma de pago parametrizada en el webconfig                        
                        }
                        catch (Exception exfpg) { }

                        var lblFormaPagoDes = new PdfPCell(new Paragraph(descripcifromapg.ToUpper(), detalle));
                        lblFormaPagoDes.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;//Rectangle.LEFT_BORDER;
                        lblFormaPagoDes.HorizontalAlignment = Rectangle.ALIGN_LEFT;

                        CultureInfo culture = new CultureInfo("en-US");

                        String MontoFormaPago = pago["total"].InnerText;
                        Double mFP = Convert.ToDouble(MontoFormaPago, CultureInfo.CreateSpecificCulture("en-US"));
                        var MontoFp = new PdfPCell(new Paragraph(mFP.ToString("N2"), detalle));
                        MontoFp.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                        MontoFp.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        tableFormaPago.AddCell(lblFormaPagoDes);
                        tableFormaPago.AddCell(MontoFp);

                        if (pago.ChildNodes.Count == 2)
                        {
                            var PlazoFP = new PdfPCell(new Paragraph(" ", detalle));
                            PlazoFP.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            PlazoFP.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                            var TiempoFP = new PdfPCell(new Paragraph(" ", detalle));
                            TiempoFP.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            TiempoFP.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                            tableFormaPago.AddCell(PlazoFP);
                            tableFormaPago.AddCell(TiempoFP);
                        }
                        else
                        {
                            var PlazoFP = new PdfPCell(new Paragraph(pago["plazo"].InnerText, detalle));
                            PlazoFP.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            PlazoFP.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                            var TiempoFP = new PdfPCell(new Paragraph(pago["unidadTiempo"].InnerText, detalle));
                            TiempoFP.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            TiempoFP.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                            tableFormaPago.AddCell(PlazoFP);
                            tableFormaPago.AddCell(TiempoFP);
                        }
                    }

                    #endregion

                    #region DetalleReembolso
                    PdfPTable tableReembolso = new PdfPTable(4);
                    XmlNodeList Reembolso = oDocument.SelectNodes("//reembolsos/reembolsoDetalle");
                    bool isReembolso = Reembolso.Count > 0;
                    if (isReembolso == true)
                    {
                        tableReembolso.TotalWidth = 250f;
                        tableReembolso.LockedWidth = true;
                        float[] ReembolsoWidths = new float[] { 35f, 50f, 40f, 30f };
                        tableReembolso.SetWidths(ReembolsoWidths);
                        string NoComprobante = string.Empty;
                        decimal TotalReembolso = 0, SumTotalReembolso = 0;
                        var lblIdentificacion = new PdfPCell(new Paragraph("Identificación", fontEncabezado));
                        lblIdentificacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        var lblComprobante = new PdfPCell(new Paragraph("No. Comprobante", fontEncabezado));
                        lblComprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        var lblFechaEmision = new PdfPCell(new Paragraph("Fecha Emisión", fontEncabezado));
                        lblFechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        var lblValorTotalReembolso = new PdfPCell(new Paragraph("Valor Total", fontEncabezado));
                        lblValorTotalReembolso.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        tableReembolso.AddCell(lblIdentificacion);
                        tableReembolso.AddCell(lblComprobante);
                        tableReembolso.AddCell(lblFechaEmision);
                        tableReembolso.AddCell(lblValorTotalReembolso);
                        try
                        {
                            PdfPCell txtIdentificacion, txtNoComprobante, txtFechaEmision, txtValorTotal;
                            foreach (XmlNode node in Reembolso)
                            {
                                TotalReembolso = 0;
                                txtIdentificacion = new PdfPCell(new Paragraph(node.SelectSingleNode("identificacionProveedorReembolso").InnerText, detAdicional));
                                txtIdentificacion.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                                txtIdentificacion.Padding = 2f;
                                txtNoComprobante = new PdfPCell(new Paragraph(node.SelectSingleNode("estabDocReembolso").InnerText + "-" + node.SelectSingleNode("ptoEmiDocReembolso").InnerText + "-" + node.SelectSingleNode("secuencialDocReembolso").InnerText, detAdicional));
                                txtNoComprobante.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                                txtNoComprobante.Padding = 2f;
                                txtFechaEmision = new PdfPCell(new Paragraph(node.SelectSingleNode("fechaEmisionDocReembolso").InnerText, detAdicional));
                                txtFechaEmision.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                                txtFechaEmision.Padding = 2f;
                                foreach (XmlNode detnode in node.SelectNodes("detalleImpuestos/detalleImpuesto"))
                                {
                                    TotalReembolso += Convert.ToDecimal(detnode.SelectSingleNode("baseImponibleReembolso").InnerText) + Convert.ToDecimal(detnode.SelectSingleNode("impuestoReembolso").InnerText);
                                }
                                SumTotalReembolso += TotalReembolso;
                                txtValorTotal = new PdfPCell(new Paragraph(TotalReembolso.ToString("0.#0"), detAdicional));
                                txtValorTotal.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                                txtValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                                txtValorTotal.Padding = 2f;
                                tableReembolso.AddCell(txtIdentificacion);
                                tableReembolso.AddCell(txtNoComprobante);
                                tableReembolso.AddCell(txtFechaEmision);
                                tableReembolso.AddCell(txtValorTotal);
                            }
                            txtIdentificacion = new PdfPCell(new Paragraph("TOTAL", detAdicional));
                            txtIdentificacion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                            txtIdentificacion.Padding = 2f;
                            txtNoComprobante = new PdfPCell(new Paragraph(" ", detAdicional));
                            txtNoComprobante.Border = Rectangle.BOTTOM_BORDER;
                            txtNoComprobante.Padding = 2f;
                            txtFechaEmision = new PdfPCell(new Paragraph(" ", detAdicional));
                            txtFechaEmision.Border = Rectangle.BOTTOM_BORDER;
                            txtFechaEmision.Padding = 2f;
                            txtValorTotal = new PdfPCell(new Paragraph(SumTotalReembolso.ToString("0.#0"), detAdicional));
                            txtValorTotal.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            txtValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            txtValorTotal.Padding = 2f;
                            tableReembolso.AddCell(txtIdentificacion);
                            tableReembolso.AddCell(txtNoComprobante);
                            tableReembolso.AddCell(txtFechaEmision);
                            tableReembolso.AddCell(txtValorTotal);
                        }
                        catch (Exception ex) { }
                    }
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    String PorComp = "";

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (!(campoAdicional.Attributes["nombre"].Value == "Bank Key"
                            || campoAdicional.Attributes["nombre"].Value == "Intermediary Bank information"
                            || campoAdicional.Attributes["nombre"].Value == "Beneficiary Bank information"
                            || campoAdicional.Attributes["nombre"].Value == "Beneficiary information"
                            || campoAdicional.Attributes["nombre"].Value == "Dirección Embarque"
                            || campoAdicional.Attributes["nombre"].Value == "Total Costo FOB"
                            || campoAdicional.Attributes["nombre"].Value == "Agente"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas"
                            || campoAdicional.Attributes["nombre"].Value == "PuntodeEmision"
                            || campoAdicional.Attributes["nombre"].Value == "RUC"
                            || campoAdicional.Attributes["nombre"].Value == "RazonSocial"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente"))
                        {
                            lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;

                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }
                    #endregion


                    #region DatosTransportista
                    var tableInfoTransportista = new PdfPTable(4);
                    if (isTransportista)
                    {
                        tableInfoTransportista.TotalWidth = 250f;
                        float[] InfoTransportistaWidths = new float[] { 60f, 80f, 60f, 40f };
                        tableInfoTransportista.SetWidths(InfoTransportistaWidths);

                        var lblCodigo2 = new PdfPCell(new Paragraph("Punto de Emisión: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;

                        lblCodigo2.Padding = 2f;

                        var Codigo2 = new PdfPCell(new Paragraph(PuntoEmisionTrans, detAdicional));
                        Codigo2.Border = Rectangle.TOP_BORDER;
                        Codigo2.Padding = 2f;

                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        var Codigo3 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo3.Border = Rectangle.TOP_BORDER;
                        Codigo3.Padding = 2f;
                        var Codigo4 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo4.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;

                        tableInfoTransportista.AddCell(Codigo3);
                        tableInfoTransportista.AddCell(Codigo4);

                        lblCodigo2 = new PdfPCell(new Paragraph("RUC: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER;

                        lblCodigo2.Padding = 2f;

                        Codigo2 = new PdfPCell(new Paragraph(RucTransportista, detAdicional));
                        Codigo2.Border = Rectangle.RIGHT_BORDER;
                        Codigo2.BorderColor = BaseColor.WHITE;
                        Codigo2.Padding = 2f;

                        var lblCodigo3 = new PdfPCell(new Paragraph("Contribuyente: ", detAdicional));
                        lblCodigo3.Border = Rectangle.LEFT_BORDER;
                        lblCodigo3.BorderColor = BaseColor.WHITE;
                        lblCodigo3.Padding = 2f;

                        Codigo3 = new PdfPCell(new Paragraph(Regimen, detAdicional));
                        Codigo3.Border = Rectangle.RIGHT_BORDER;

                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        tableInfoTransportista.AddCell(lblCodigo3);
                        tableInfoTransportista.AddCell(Codigo3);

                        lblCodigo2 = new PdfPCell(new Paragraph("Razón Social: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;

                        lblCodigo2.Padding = 2f;

                        Codigo2 = new PdfPCell(new Paragraph(RazonSocialTransportista, detAdicional));
                        Codigo2.Border = Rectangle.BOTTOM_BORDER;

                        Codigo2.Padding = 2f;

                        Codigo3 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo3.Border = Rectangle.BOTTOM_BORDER;
                        Codigo3.Padding = 2f;
                        Codigo4 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo4.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;


                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        tableInfoTransportista.AddCell(Codigo3);
                        tableInfoTransportista.AddCell(Codigo4);
                    }

                    #endregion

                    bool isExportacion = false;
                    string txtIncoTerm = "";
                    #region factura Exportación
                    var tableTotales = new PdfPTable(2);
                    tableTotales.TotalWidth = 180f;
                    float[] InfoTotales = new float[] { 125f, 55f };
                    tableTotales.SetWidths(InfoTotales);

                    if (!Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/comercioExterior"), null))
                    {
                        try
                        {
                            decimal valordecimal = 0;
                            isExportacion = true;
                            PdfPCell ComercioExterior = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/comercioExterior").InnerText, detalle));
                            ComercioExterior.Colspan = 2;
                            ComercioExterior.Border = Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                            ComercioExterior.Padding = 5f;
                            ComercioExterior.HorizontalAlignment = Element.ALIGN_CENTER;
                            tableInfoAdicional.AddCell(ComercioExterior);

                            txtIncoTerm = Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/incoTermFactura"), null) ? "" : oDocument.SelectSingleNode("//infoFactura/incoTermFactura").InnerText;
                            PdfPCell lblIncoTermFactura = new PdfPCell(new Paragraph("IncoTerm", detAdicional));
                            lblIncoTermFactura.Border = Rectangle.LEFT_BORDER;
                            lblIncoTermFactura.Padding = 2f;
                            tableInfoAdicional.AddCell(lblIncoTermFactura);
                            PdfPCell IncoTermFactura = new PdfPCell(new Paragraph(txtIncoTerm, detAdicional));
                            IncoTermFactura.Border = Rectangle.RIGHT_BORDER;
                            IncoTermFactura.Padding = 2f;
                            tableInfoAdicional.AddCell(IncoTermFactura);

                            PdfPCell lbllugarIncoTerm = new PdfPCell(new Paragraph("lugar IncoTerm", detAdicional));
                            lbllugarIncoTerm.Border = Rectangle.LEFT_BORDER;
                            lbllugarIncoTerm.Padding = 2f;
                            tableInfoAdicional.AddCell(lbllugarIncoTerm);
                            PdfPCell LugarIncoTerm = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/lugarIncoTerm"), null) ? "" : oDocument.SelectSingleNode("//infoFactura/lugarIncoTerm").InnerText, detAdicional));
                            LugarIncoTerm.Border = Rectangle.RIGHT_BORDER;
                            LugarIncoTerm.Padding = 2f;
                            tableInfoAdicional.AddCell(LugarIncoTerm);

                            PdfPCell lblpaisOrigen = new PdfPCell(new Paragraph("País Origen", detAdicional));
                            lblpaisOrigen.Border = Rectangle.LEFT_BORDER;
                            lblpaisOrigen.Padding = 2f;
                            tableInfoAdicional.AddCell(lblpaisOrigen);
                            PdfPCell PaisOrigen = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/paisOrigen"), null) ? "" : ReadyCatalogo.getCatalogo("pais", oDocument.SelectSingleNode("//infoFactura/paisOrigen").InnerText).Descripcion, detAdicional));
                            PaisOrigen.Border = Rectangle.RIGHT_BORDER;
                            PaisOrigen.Padding = 2f;
                            tableInfoAdicional.AddCell(PaisOrigen);

                            PdfPCell lblpuertoEmbarque = new PdfPCell(new Paragraph("Puerto Embarque", detAdicional));
                            lblpuertoEmbarque.Border = Rectangle.LEFT_BORDER;
                            lblpuertoEmbarque.Padding = 2f;
                            tableInfoAdicional.AddCell(lblpuertoEmbarque);
                            PdfPCell PuertoEmbarque = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/puertoEmbarque"), null) ? "" : oDocument.SelectSingleNode("//infoFactura/puertoEmbarque").InnerText, detAdicional));
                            PuertoEmbarque.Border = Rectangle.RIGHT_BORDER;
                            PuertoEmbarque.Padding = 2f;
                            tableInfoAdicional.AddCell(PuertoEmbarque);

                            PdfPCell lblpuertoDestino = new PdfPCell(new Paragraph("Puerto Destino", detAdicional));
                            lblpuertoDestino.Border = Rectangle.LEFT_BORDER;
                            lblpuertoDestino.Padding = 2f;
                            tableInfoAdicional.AddCell(lblpuertoDestino);
                            PdfPCell PuertoDestino = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/puertoDestino"), null) ? "" : oDocument.SelectSingleNode("//infoFactura/puertoDestino").InnerText, detAdicional));
                            PuertoDestino.Border = Rectangle.RIGHT_BORDER;
                            PuertoDestino.Padding = 2f;
                            tableInfoAdicional.AddCell(PuertoDestino);

                            PdfPCell lblpaisDestino = new PdfPCell(new Paragraph("Pais Destino", detAdicional));
                            lblpaisDestino.Border = Rectangle.LEFT_BORDER;
                            lblpaisDestino.Padding = 2f;
                            tableInfoAdicional.AddCell(lblpaisDestino);
                            PdfPCell PaisDestino = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/paisDestino"), null) ? "" : ReadyCatalogo.getCatalogo("pais", oDocument.SelectSingleNode("//infoFactura/paisDestino").InnerText).Descripcion, detAdicional));
                            PaisDestino.Border = Rectangle.RIGHT_BORDER;
                            PaisDestino.Padding = 2f;
                            tableInfoAdicional.AddCell(PaisDestino);

                            PdfPCell lblpaisAdquisicion = new PdfPCell(new Paragraph("Pais Adquisición", detAdicional));
                            lblpaisAdquisicion.Border = Rectangle.LEFT_BORDER;
                            lblpaisAdquisicion.Padding = 2f;
                            tableInfoAdicional.AddCell(lblpaisAdquisicion);
                            PdfPCell PaisAdquisicion = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/paisAdquisicion"), null) ? "" : ReadyCatalogo.getCatalogo("pais", oDocument.SelectSingleNode("//infoFactura/paisAdquisicion").InnerText).Descripcion, detAdicional));
                            PaisAdquisicion.Border = Rectangle.RIGHT_BORDER;
                            PaisAdquisicion.Padding = 2f;
                            tableInfoAdicional.AddCell(PaisAdquisicion);

                            PdfPCell lblincoTermTotalSinImpuestos = new PdfPCell(new Paragraph("IncoTerm Total Sin Impuestos", detAdicional));
                            lblincoTermTotalSinImpuestos.Border = Rectangle.LEFT_BORDER;
                            lblincoTermTotalSinImpuestos.Padding = 2f;
                            tableInfoAdicional.AddCell(lblincoTermTotalSinImpuestos);
                            PdfPCell IncoTermTotalSinImpuestos = new PdfPCell(new Paragraph(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/incoTermTotalSinImpuestos"), null) ? "" : oDocument.SelectSingleNode("//infoFactura/incoTermTotalSinImpuestos").InnerText, detAdicional));
                            IncoTermTotalSinImpuestos.Border = Rectangle.RIGHT_BORDER;
                            IncoTermTotalSinImpuestos.Padding = 2f;
                            tableInfoAdicional.AddCell(IncoTermTotalSinImpuestos);

                            valordecimal = decimal.Parse(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/fleteInternacional"), null) ? "0" : oDocument.SelectSingleNode("//infoFactura/fleteInternacional").InnerText, new CultureInfo(Cultura));
                            PdfPCell lblfleteInternacional = new PdfPCell(new Paragraph("FLETE INTERNACIONAL", detalle));
                            lblfleteInternacional.Padding = 2f;
                            tableTotales.AddCell(lblfleteInternacional);
                            PdfPCell FleteInternacional = new PdfPCell(new Paragraph(valordecimal.ToString("0.#0"), detalle));
                            FleteInternacional.Padding = 2f;
                            FleteInternacional.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotales.AddCell(FleteInternacional);

                            valordecimal = decimal.Parse(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/seguroInternacional"), null) ? "0" : oDocument.SelectSingleNode("//infoFactura/seguroInternacional").InnerText, new CultureInfo(Cultura));
                            PdfPCell lblseguroInternacional = new PdfPCell(new Paragraph("SEGURO INTERNACIONAL", detalle));
                            lblseguroInternacional.Padding = 2f;
                            tableTotales.AddCell(lblseguroInternacional);
                            PdfPCell SeguroInternacional = new PdfPCell(new Paragraph(valordecimal.ToString("0.#0"), detalle));
                            SeguroInternacional.Padding = 2f;
                            SeguroInternacional.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotales.AddCell(SeguroInternacional);

                            valordecimal = decimal.Parse(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/gastosAduaneros"), null) ? "0" : oDocument.SelectSingleNode("//infoFactura/gastosAduaneros").InnerText, new CultureInfo(Cultura));
                            PdfPCell lblgastosAduaneros = new PdfPCell(new Paragraph("GASTOS ADUANEROS", detalle));
                            lblgastosAduaneros.Padding = 2f;
                            tableTotales.AddCell(lblgastosAduaneros);
                            PdfPCell GastosAduaneros = new PdfPCell(new Paragraph(valordecimal.ToString("0.#0"), detalle));
                            GastosAduaneros.Padding = 2f;
                            GastosAduaneros.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotales.AddCell(GastosAduaneros);

                            valordecimal = decimal.Parse(Object.ReferenceEquals(oDocument.SelectSingleNode("//infoFactura/gastosTransporteOtros"), null) ? "0" : oDocument.SelectSingleNode("//infoFactura/gastosTransporteOtros").InnerText, new CultureInfo(Cultura));
                            PdfPCell lblgastosTransporteOtros = new PdfPCell(new Paragraph("GASTOS TRANSPORTE OTROS", detalle));
                            lblgastosTransporteOtros.Padding = 2f;
                            tableTotales.AddCell(lblgastosTransporteOtros);
                            PdfPCell GastosTransporteOtros = new PdfPCell(new Paragraph(valordecimal.ToString("0.#0"), detalle));
                            GastosTransporteOtros.Padding = 2f;
                            GastosTransporteOtros.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableTotales.AddCell(GastosTransporteOtros);

                            //JMENDEZ
                            XmlNodeList InfoAdicional1;
                            InfoAdicional1 = oDocument.SelectNodes("//infoAdicional/campoAdicional");
                            foreach (XmlNode campoAdicional in InfoAdicional1)
                            {
                                if (campoAdicional.Attributes["nombre"].Value == "Bank Key")
                                {
                                    PdfPCell infoBanco = new PdfPCell(new Paragraph("INFORMACIÓN BANCO", detalle));
                                    infoBanco.HorizontalAlignment = Element.ALIGN_CENTER;
                                    infoBanco.Colspan = 2;
                                    infoBanco.Padding = 3f;

                                    PdfPCell lblinfoBanco = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value + ": " + "\n" + "\n", detalle));
                                    lblinfoBanco.Border = Rectangle.LEFT_BORDER;
                                    lblinfoBanco.Padding = 2f;

                                    PdfPCell valorinfoBanco = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText + "\n" + "\n", detalle));
                                    valorinfoBanco.Border = Rectangle.RIGHT_BORDER;
                                    valorinfoBanco.Padding = 2f;

                                    tableInfoAdicional.AddCell(infoBanco);
                                    tableInfoAdicional.AddCell(lblinfoBanco);
                                    tableInfoAdicional.AddCell(valorinfoBanco);

                                }
                                else if (campoAdicional.Attributes["nombre"].Value == "Intermediary Bank information")
                                {
                                    PdfPCell intBankInfo = new PdfPCell(new Paragraph("INTERMEDIARY BANK INFORMATION", detalle));
                                    intBankInfo.HorizontalAlignment = Element.ALIGN_CENTER;
                                    intBankInfo.Colspan = 2;
                                    intBankInfo.Padding = 3f;

                                    string[] lv_Arr = campoAdicional.InnerText.Split('|', ':');

                                    PdfPCell lbl = new PdfPCell(new Paragraph(lv_Arr[0] + ":" + "\n" + lv_Arr[2] + ":" + "\n" + lv_Arr[4] + ":" + "\n" + "\n", detalle));
                                    lbl.UseAscender = true;
                                    lbl.Border = Rectangle.LEFT_BORDER;

                                    PdfPCell valor = new PdfPCell(new Paragraph(lv_Arr[1] + "\n" + lv_Arr[3] + "\n" + lv_Arr[5] + "\n" + "\n", detalle));
                                    valor.UseAscender = true;
                                    valor.Border = Rectangle.RIGHT_BORDER;

                                    tableInfoAdicional.AddCell(intBankInfo);
                                    tableInfoAdicional.AddCell(lbl);
                                    tableInfoAdicional.AddCell(valor);

                                }
                                else if (campoAdicional.Attributes["nombre"].Value == "Beneficiary Bank information")
                                {
                                    PdfPCell benBankInf = new PdfPCell(new Paragraph("BENEFICIARY BANK INFORMATION", detalle));
                                    benBankInf.HorizontalAlignment = Element.ALIGN_CENTER;
                                    benBankInf.Colspan = 2;
                                    benBankInf.Padding = 3f;

                                    string[] lv_Arr = campoAdicional.InnerText.Split('|', ':');

                                    PdfPCell lbl = new PdfPCell(new Paragraph(lv_Arr[0] + ":" + "\n" + lv_Arr[2] + ":" + "\n" + lv_Arr[4] + ":" + "\n" + lv_Arr[6] + ":" + "\n" + lv_Arr[8] + ":" + "\n" + "\n", detalle));
                                    lbl.UseAscender = true;
                                    lbl.Border = Rectangle.LEFT_BORDER;

                                    PdfPCell valor = new PdfPCell(new Paragraph(lv_Arr[1] + "\n" + lv_Arr[3] + "\n" + lv_Arr[5] + "\n" + lv_Arr[7] + "\n" + lv_Arr[9] + "\n" + "\n", detalle));
                                    valor.UseAscender = true;
                                    valor.Border = Rectangle.RIGHT_BORDER;

                                    tableInfoAdicional.AddCell(benBankInf);
                                    tableInfoAdicional.AddCell(lbl);
                                    tableInfoAdicional.AddCell(valor);

                                }
                                else if (campoAdicional.Attributes["nombre"].Value == "Beneficiary information")
                                {
                                    PdfPCell benInf = new PdfPCell(new Paragraph("BENEFICIARY INFORMATION", detalle));
                                    benInf.HorizontalAlignment = Element.ALIGN_CENTER;
                                    benInf.Colspan = 2;
                                    benInf.Padding = 3f;

                                    string[] lv_Arr = campoAdicional.InnerText.Split('|', ':');

                                    PdfPCell lbl = new PdfPCell(new Paragraph(lv_Arr[0] + ":" + "\n" + "\n" + lv_Arr[2] + ":" + "\n" + lv_Arr[4] + ":" + "\n" + lv_Arr[6] + ":", detalle));
                                    lbl.UseAscender = true;
                                    lbl.Border = Rectangle.LEFT_BORDER;

                                    PdfPCell valor = new PdfPCell(new Paragraph(lv_Arr[1] + "\n" + lv_Arr[3] + "\n" + lv_Arr[5] + "\n" + lv_Arr[7], detalle));
                                    valor.UseAscender = true;
                                    valor.Border = Rectangle.RIGHT_BORDER;

                                    tableInfoAdicional.AddCell(benInf);
                                    tableInfoAdicional.AddCell(lbl);
                                    tableInfoAdicional.AddCell(valor);

                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                    string lblValorTotal1 = isExportacion ? "COSTO " + txtIncoTerm : "VALOR A PAGAR";
                    string lblValorTotal2 = isExportacion ? "COSTO " + txtIncoTerm : "VALOR TOTAL";
                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);
                    #endregion

                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    decimal dpropina = 0;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "propina")
                        {
                            dpropina = decimal.Parse(campoAdicional.InnerText, nfi);
                        }
                    }

                    #region Totales
                    XmlNodeList Impuestos;
                    Impuestos = oDocument.SelectNodes("//infoFactura/totalConImpuestos/totalImpuesto");
                    decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0, dSubtotalExcento = 0, dIRBPNR = 0;
                    foreach (XmlNode Impuesto in Impuestos)
                    {
                        switch (Impuesto["codigo"].InnerText)
                        {
                            case "2":        // IVA
                                if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                                {
                                    dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2" || Impuesto["codigoPorcentaje"].InnerText == "3")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "6")    // No objeto de Impuesto
                                {
                                    dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "7")
                                {
                                    dSubtotalExcento = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                break;
                            case "3":       // ICE
                                dICE = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                break;
                            case "5":
                                dIRBPNR = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                break;
                        }
                    }

                    XmlNodeList xmlRetenciones;
                    xmlRetenciones = oDocument.SelectNodes("//retenciones/retencion");
                    decimal dRenta = 0, dIVAPresuntivo = 0;
                    string tarRenta = "0.00", tarIVAPresuntivo = "0.00";

                    if (xmlRetenciones.Count > 0)
                    {
                        foreach (XmlNode retencion in xmlRetenciones)
                        {
                            if (int.Parse(retencion["codigoPorcentaje"].InnerText) > 7)
                            {
                                dRenta = decimal.Parse(retencion["valor"].InnerText, new CultureInfo(Cultura));
                                tarRenta = (decimal.Parse(retencion["tarifa"].InnerText, new CultureInfo(Cultura)) * 100).ToString(nfi);
                            }
                            else
                            {
                                dIVAPresuntivo = decimal.Parse(retencion["valor"].InnerText, new CultureInfo(Cultura));
                                tarIVAPresuntivo = (decimal.Parse(retencion["tarifa"].InnerText, new CultureInfo(Cultura)) * 100).ToString(nfi);
                            }
                        }
                    }

                    if (IVAride == "")
                    {
                        string iva12val = (System.Configuration.ConfigurationManager.AppSettings["IVA12"]);
                        DateTime Dfecha12 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHA12"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha12 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHA12"], "dd/MM/yyyy", new CultureInfo(Cultura));

                        string iva14val = (System.Configuration.ConfigurationManager.AppSettings["IVA14"]);
                        DateTime Dfecha14 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHA14"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha14 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHA14"], "dd/MM/yyyy", new CultureInfo(Cultura));


                        string iva12desval = (System.Configuration.ConfigurationManager.AppSettings["IVADES12"]);
                        DateTime Dfecha12des = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHADES12"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha12des = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHADES12"], "dd/MM/yyyy", new CultureInfo(Cultura));

                        if (FechaEmisionValida >= Dfecha12 && FechaEmisionValida <= Hfecha12)
                        {
                            IVAride = iva12val;
                        }

                        if (FechaEmisionValida >= Dfecha14 && FechaEmisionValida <= Hfecha14)
                        {
                            IVAride = iva14val;
                        }

                        if (FechaEmisionValida >= Dfecha12des && FechaEmisionValida <= Hfecha12des)
                        {
                            IVAride = iva12desval;
                        }
                    }
                    IVAride = IVAride.Replace(".00", "");
                    var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL " + IVAride + "%", detalle));
                    lblSubTotal12.Padding = 2f;
                    var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12 == 0 ? "0.00" : dSubtotal12.ToString(nfi), detalle));
                    SubTotal12.Padding = 2f;
                    SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 0%", detalle));
                    lblSubTotal0.Padding = 2f;
                    var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0 == 0 ? "0.00" : dSubtotal0.ToString(nfi), detalle));
                    SubTotal0.Padding = 2f;
                    SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL NO OBJETO DE IVA", detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI == 0 ? "0.00" : dSubtotalNSI.ToString(nfi), detalle));
                    SubTotalNoSujetoIVA.Padding = 2f;
                    SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalExcentoIVA = new PdfPCell(new Paragraph("SUBTOTAL EXENTO DE IVA", detalle));
                    lblSubTotalExcentoIVA.Padding = 2f;
                    var SubTotalExcentoIVA = new PdfPCell(new Paragraph(dSubtotalExcento == 0 ? "0.00" : dSubtotalExcento.ToString(nfi), detalle));
                    SubTotalExcentoIVA.Padding = 2f;
                    SubTotalExcentoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS", detalle));
                    lblSubTotalSinImpuestos.Padding = 2f;
                    var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/totalSinImpuestos").InnerText, detalle));
                    SubTotalSinImpuestos.Padding = 2f;
                    SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblDescuento = new PdfPCell(new Paragraph("TOTAL DESCUENTO", detalle));
                    lblDescuento.Padding = 2f;
                    var TotalDescuento = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/totalDescuento").InnerText, detalle));
                    TotalDescuento.Padding = 2f;
                    TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblICE = new PdfPCell(new Paragraph("ICE", detalle));
                    lblICE.Padding = 2f;
                    var ICE = new PdfPCell(new Paragraph(dICE == 0 ? "0.00" : dICE.ToString(nfi), detalle));
                    ICE.Padding = 2f;
                    ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVA12 = new PdfPCell(new Paragraph("IVA " + IVAride + "%", detalle));
                    lblIVA12.Padding = 2f;
                    var IVA12 = new PdfPCell(new Paragraph(dIVA12.ToString(nfi), detalle));
                    IVA12.Padding = 2f;
                    IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIRBPNR = new PdfPCell(new Paragraph("IRBPNR", detalle));
                    lblIRBPNR.Padding = 2f;
                    var IRBPNR = new PdfPCell(new Paragraph(dIRBPNR == 0 ? "0.00" : dIRBPNR.ToString(nfi), detalle));
                    IRBPNR.Padding = 2f;
                    IRBPNR.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblPropina = new PdfPCell(new Paragraph("PROPINA", detalle));
                    lblPropina.Padding = 2f;
                    var Propina = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/propina").InnerText == "0.00" ? "-" : oDocument.SelectSingleNode("//infoFactura/propina").InnerText, detalle));
                    Propina.Padding = 2f;
                    Propina.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVAPresuntivo = new PdfPCell(new Paragraph(String.Format("IVA PRESUNTIVO {0}% ", tarIVAPresuntivo), detalle));
                    lblIVAPresuntivo.Padding = 2f;
                    var IVAPresuntivo = new PdfPCell(new Paragraph(decimal.Round(dIVAPresuntivo, 2).ToString(nfi), detalle));
                    IVAPresuntivo.Padding = 2f;
                    IVAPresuntivo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblRenta = new PdfPCell(new Paragraph("RENTA", detalle));
                    lblRenta.Padding = 2f;
                    var Renta = new PdfPCell(new Paragraph(decimal.Round(dRenta, 2).ToString(nfi), detalle));
                    Renta.Padding = 2f;
                    Renta.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblValorTotal = new PdfPCell(new Paragraph(lblValorTotal2, detalle));
                    lblValorTotal.Padding = 2f;
                    var ValorTotal = new PdfPCell(new Paragraph("$ " + oDocument.SelectSingleNode("//infoFactura/importeTotal").InnerText, detalle));
                    ValorTotal.Padding = 2f;
                    ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    decimal decImporteTotal = 0, decTotal = 0;
                    decImporteTotal = decimal.Parse(oDocument.SelectSingleNode("//infoFactura/importeTotal").InnerText, nfi);
                    decTotal = decImporteTotal + dIVAPresuntivo + dRenta;

                    var lblValorPagar = new PdfPCell(new Paragraph(lblValorTotal1, detalle));
                    lblValorPagar.Padding = 2f;
                    var ValorPagar = new PdfPCell(new Paragraph(decimal.Round(decTotal, 2).ToString(nfi), detalle));
                    ValorPagar.Padding = 2f;
                    ValorPagar.HorizontalAlignment = Rectangle.ALIGN_RIGHT;


                    var lblServicio = new PdfPCell(new Paragraph("SERVICIO 10%", detalle));
                    lblServicio.Padding = 2f;
                    var Servicio = new PdfPCell(new Paragraph(dpropina.ToString(nfi), detalle));
                    Servicio.Padding = 2f;
                    Servicio.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    if (isExportacion == false)
                    {
                        tableTotales.AddCell(lblSubTotal12);
                        tableTotales.AddCell(SubTotal12);
                        tableTotales.AddCell(lblSubTotal0);
                        tableTotales.AddCell(SubTotal0);
                        tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                        tableTotales.AddCell(SubTotalNoSujetoIVA);
                        tableTotales.AddCell(lblSubTotalExcentoIVA);
                        tableTotales.AddCell(SubTotalExcentoIVA);
                        tableTotales.AddCell(lblSubTotalSinImpuestos);
                        tableTotales.AddCell(SubTotalSinImpuestos);
                        tableTotales.AddCell(lblDescuento);
                        tableTotales.AddCell(TotalDescuento);
                        tableTotales.AddCell(lblIVA12);
                        tableTotales.AddCell(IVA12);
                        tableTotales.AddCell(lblICE);
                        tableTotales.AddCell(ICE);
                    }
                    tableTotales.AddCell(lblValorTotal);

                    if (xmlRetenciones.Count > 0)
                    {
                        if (isExportacion == false)
                        {
                            tableTotales.AddCell(lblIVAPresuntivo);
                            tableTotales.AddCell(IVAPresuntivo);
                            tableTotales.AddCell(lblRenta);
                            tableTotales.AddCell(Renta);
                        }
                        if (oDocument.SelectNodes("//compensaciones/compensacion").Count > 0)
                        {

                            XmlNodeList CompensacionesSolidaria;
                            CompensacionesSolidaria = oDocument.SelectNodes("//compensaciones/compensacion");
                            decimal tcompensa = 0;
                            foreach (XmlNode compensacion in CompensacionesSolidaria)
                            {
                                decimal valor = decimal.Round(decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura)), 2);
                                tcompensa = tcompensa + valor;
                            }

                            decimal nuevovalor = 0;
                            nuevovalor = decimal.Round(decimal.Parse(oDocument.SelectSingleNode("//infoFactura/importeTotal").InnerText, new CultureInfo(Cultura)), 2) + tcompensa;
                            ValorTotal = new PdfPCell(new Paragraph(nuevovalor.ToString(nfi), detalle));
                            ValorTotal.Padding = 2f;
                            ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            tableTotales.AddCell(ValorTotal);
                            foreach (XmlNode compensacion in CompensacionesSolidaria)
                            {
                                PdfPCell CompensacionSolidaria;

                                string datocodi = compensacion["codigo"].InnerText;
                                string descompensacion = "";
                                try
                                {
                                    descompensacion = System.Configuration.ConfigurationManager.AppSettings["CS" + datocodi];//Descripcion de la compensacion parametrizada en el webconfig                                      
                                }
                                catch (Exception excomp)
                                {

                                }

                                var lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "%", detalle));
                                if (datocodi.Equals("1"))
                                {
                                    lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "% IVA", detalle));
                                }
                                lblCompensacionSolidaria.Padding = 2f;

                                var ValorCompesa = decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura));
                                CompensacionSolidaria = new PdfPCell(new Paragraph(decimal.Round(ValorCompesa, 2).ToString(nfi), detalle));
                                CompensacionSolidaria.Padding = 2f;
                                CompensacionSolidaria.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                                if (ValorCompesa > 0)
                                {
                                    if (isExportacion == false)
                                    {
                                        tableTotales.AddCell(lblCompensacionSolidaria);
                                        tableTotales.AddCell(CompensacionSolidaria);
                                    }
                                }
                            }


                            var lblValorPagado = new PdfPCell(new Paragraph(lblValorTotal1, detalle));
                            lblValorTotal.Padding = 2f;
                            var ValorPagado = new PdfPCell(new Paragraph(decimal.Round(decTotal, 2).ToString(nfi), detalle));
                            ValorPagado.Padding = 2f;
                            ValorPagado.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            tableTotales.AddCell(lblValorPagado);
                            tableTotales.AddCell(ValorPagado);
                        }
                        else
                        {
                            tableTotales.AddCell(ValorTotal);
                        }
                    }

                    if (oDocument.SelectNodes("//compensaciones/compensacion").Count > 0)
                    {
                        XmlNodeList CompensacionesSolidaria;
                        CompensacionesSolidaria = oDocument.SelectNodes("//compensaciones/compensacion");
                        decimal tcompensa = 0;
                        foreach (XmlNode compensacion in CompensacionesSolidaria)
                        {
                            decimal valor = decimal.Round(decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura)), 2);
                            tcompensa = tcompensa + valor;
                        }

                        decimal nuevovalor = 0;
                        nuevovalor = decimal.Round(decimal.Parse(oDocument.SelectSingleNode("//infoFactura/importeTotal").InnerText, new CultureInfo(Cultura)), 2) + tcompensa;
                        ValorTotal = new PdfPCell(new Paragraph(nuevovalor.ToString(nfi), detalle));
                        ValorTotal.Padding = 2f;
                        ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        tableTotales.AddCell(ValorTotal);
                        foreach (XmlNode compensacion in CompensacionesSolidaria)
                        {
                            PdfPCell CompensacionSolidaria;

                            string datocodi = compensacion["codigo"].InnerText;
                            string descompensacion = "";
                            try
                            {
                                descompensacion = System.Configuration.ConfigurationManager.AppSettings["CS" + datocodi];//Descripcion de la compensacion parametrizada en el webconfig                                      
                            }
                            catch (Exception excomp)
                            {

                            }

                            var lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "%", detalle));
                            if (datocodi.Equals("1"))
                            {
                                lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "% IVA", detalle));
                            }
                            lblCompensacionSolidaria.Padding = 2f;

                            var ValorCompesa = decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura));
                            CompensacionSolidaria = new PdfPCell(new Paragraph(decimal.Round(ValorCompesa, 2).ToString(nfi), detalle));
                            CompensacionSolidaria.Padding = 2f;
                            CompensacionSolidaria.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            if (ValorCompesa > 0)
                            {
                                if (isExportacion == false)
                                {
                                    tableTotales.AddCell(lblCompensacionSolidaria);
                                    tableTotales.AddCell(CompensacionSolidaria);
                                }
                            }
                        }

                        var lblValorPagado = new PdfPCell(new Paragraph(lblValorTotal1, detalle));
                        lblValorTotal.Padding = 2f;
                        var ValorPagado = new PdfPCell(new Paragraph(decimal.Round(decTotal, 2).ToString(nfi), detalle));
                        ValorPagado.Padding = 2f;
                        ValorPagado.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        tableTotales.AddCell(lblValorPagado);
                        tableTotales.AddCell(ValorPagado);
                    }
                    else
                    {
                        tableTotales.AddCell(ValorTotal);
                    }

                    if (dpropina > 0)
                    {
                        if (isExportacion == false)
                        {
                            tableTotales.AddCell(lblServicio);
                            tableTotales.AddCell(Servicio);
                        }
                    }
                    #endregion
                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;
                    var posicion1 = 5;
                    posDetalleCliente = 785 - ContenedorD.TotalHeight - posicion1;
                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;

                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;
                    if (DifTableL < posDetalleCliente)
                    {
                        aumento = posDetalleCliente - DifTableL + 10;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleCliente) + 5;
                    }

                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);

                    var posicion = 8;
                    posDetalleFactura = (posDetalleCliente - posicion) - tableDetalleCliente.TotalHeight;

                    tableDetalleCliente.WriteSelectedRows(0, 5, 28, posDetalleCliente - aumento, canvas);

                    Boolean otrapagina = false;
                    foreach (TableDetalleFactura Itemdetalle in ListDetalle)
                    {
                        NumPaginas = NumPaginas + 1;
                        posInfoAdicional = Itemdetalle.Detalles.TotalHeight;
                    }

                    if ((posInfoAdicional + tableTotales.TotalHeight + 10) > (posInfoAdicional + tableFormaPago.TotalHeight + tableInfoAdicional.TotalHeight + 10))
                    {
                        posDetalleFactura = posInfoAdicional + tableTotales.TotalHeight + 10;
                    }
                    else
                    {
                        posDetalleFactura = posInfoAdicional + tableFormaPago.TotalHeight + tableInfoAdicional.TotalHeight + 10;
                    }

                    if (NumPaginas == 1 & isExportacion == true)
                    {

                        if (posDetalleFactura > 496)
                        {
                            NumPaginas = NumPaginas + 1;
                            otrapagina = true;
                        }
                    }
                    else if (NumPaginas == 1 & isExportacion == false)
                    {
                        if (posDetalleFactura > 520)
                        {
                            NumPaginas = NumPaginas + 1;
                            otrapagina = true;
                        }
                    }
                    else if (isExportacion == true)
                    {
                        if (posDetalleFactura > (200 + (posDetalleCliente - 10 - tableDetalleCliente.TotalHeight)))
                        {
                            NumPaginas = NumPaginas + 1;
                            otrapagina = true;
                        }
                    }
                    else if (isExportacion == false)
                    {
                        if (posDetalleFactura > (520 + (posDetalleCliente - 10 - tableDetalleCliente.TotalHeight)))
                        {
                            NumPaginas = NumPaginas + 1;
                            otrapagina = true;
                        }
                    }

                    oEvents.Contador = NumPaginas;

                    NumPaginas = 0;

                    foreach (TableDetalleFactura Itemdetalle in ListDetalle)
                    {
                        NumPaginas = NumPaginas + 1;
                        if (NumPaginas == 1)
                        {
                            writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                            Itemdetalle.Detalles.WriteSelectedRows(Itemdetalle.desde, Itemdetalle.hasta, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        }
                        else
                        {
                            writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                            documento.NewPage();
                            Itemdetalle.Detalles.WriteSelectedRows(Itemdetalle.desde, Itemdetalle.hasta, 28, 806 - aumento, canvas);

                        }
                    }
                    oEvents.Contador = NumPaginas;

                    if (otrapagina)
                    {
                        documento.NewPage();
                        tableFormaPago.WriteSelectedRows(0, 17, 28, 806 - aumento, canvas);
                        float TotalHeight = 806 - tableFormaPago.TotalHeight - 10;
                        if (isReembolso == true)
                        {
                            tableReembolso.WriteSelectedRows(0, 30, 28, TotalHeight - aumento, canvas);
                            TotalHeight = TotalHeight - tableReembolso.TotalHeight - 10;
                        }
                        tableInfoAdicional.WriteSelectedRows(0, tableInfoAdicional.Rows.Count, 28, TotalHeight - aumento, canvas);
                        if (isTransportista)
                        {
                            tableInfoTransportista.WriteSelectedRows(0, tableInfoTransportista.Rows.Count, 28, TotalHeight - 30 - aumento, canvas);
                        }
                        tableTotales.WriteSelectedRows(0, 17, 388, 806 - aumento, canvas);
                    }
                    else
                    {
                        if (NumPaginas == 1)
                        {
                            posDetalleFactura = posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - posInfoAdicional - 10;
                        }
                        else
                        {
                            if (posInfoAdicional < 660)
                            {
                                posDetalleFactura = 806 - posInfoAdicional - 10;
                            }
                            else
                            {
                                documento.NewPage();
                                posDetalleFactura = 810;
                            }
                        }
                        tableFormaPago.WriteSelectedRows(0, 17, 28, posDetalleFactura - aumento, canvas);
                        float TotalHeight = tableFormaPago.TotalHeight;
                        if (isReembolso == true)
                        {
                            tableReembolso.WriteSelectedRows(0, 30, 28, posDetalleFactura - tableFormaPago.TotalHeight - 10 - aumento, canvas);
                            TotalHeight = tableFormaPago.TotalHeight + tableReembolso.TotalHeight + 10;
                        }
                        tableInfoAdicional.WriteSelectedRows(0, tableInfoAdicional.Rows.Count, 28, posDetalleFactura - TotalHeight - 10 - aumento, canvas);
                        TotalHeight = TotalHeight + tableInfoAdicional.TotalHeight;

                        if (isTransportista)
                        {
                            tableInfoTransportista.WriteSelectedRows(0, 17, 28, posDetalleFactura - TotalHeight - 30 - aumento, canvas);
                        }
                       
                        tableTotales.WriteSelectedRows(0, 17, 388, posDetalleFactura - aumento, canvas);
                    }

                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Bytes;
        }

        /// <summary>
        /// Método para generación de guía de remisión RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraGuiaRemision(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US", string direccion = "", string direccionCliente = "")
        {
            iTextSharp.text.Font detAdicional = GetArial(6);
            MemoryStream ms = null;
            byte[] Bytes = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "", rimpe = "",
                           sAmbienteVal = "", sNumAutorizacion = "", sObligadoContabilidad = "", RegMicroEmp = "";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoGuiaRemision/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoGuiaRemision/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoGuiaRemision/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoGuiaRemision/obligadoContabilidad").InnerText;
                    try
                    {
                        rimpe = oDocument.SelectSingleNode("//infoTributaria/contribuyenteRimpe").InnerText;

                    }
                    catch (Exception)
                    {
                    }
                    bool Excep = false;

                    try
                    {
                        XmlNodeList IARegMicro = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                        foreach (XmlNode campoAdicional in IARegMicro)
                        {
                            if (campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas")
                            {
                                RegMicroEmp = "si";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Excep = true;
                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        try
                        {

                            string micoEmrp = oDocument.SelectSingleNode("//infoTributaria/regimenMicroempresas").InnerText;
                            if (!string.IsNullOrEmpty(micoEmrp))
                            {
                                Excep = false;
                                RegMicroEmp = "si";
                            }
                        }
                        catch (Exception)
                        {
                            Excep = true;
                        }

                    }

                    List<TableGuiRemision> TablaGuiaRemision = new List<TableGuiRemision>();

                    int registros = 0;
                    int cabeceraguiaregstros = 0;
                    int PagLimite1 = 25;
                    int MaxSoloPagina = 70;

                    float posDetalleTransportista = 0;
                    float posDetalleGuia = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();

                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;

                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();
                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;

                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);


                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));
                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {
                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {
                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);
                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    RUC.PaddingTop = 10f;
                    RUC.PaddingBottom = 6f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("G U I A  D E  R E M I S I Ó N ", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    Factura.PaddingBottom = 7f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    NumFactura.PaddingBottom = 7f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    lblNumAutorizacion.PaddingBottom = 10f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    NumAutorizacion.PaddingBottom = 7f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        NumAutorizacion.PaddingBottom = 10f;
                    }
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    FechaAutorizacion.PaddingBottom = 7f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        FechaAutorizacion.PaddingBottom = 7f;
                    }
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    Ambiente.PaddingBottom = 7f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        Ambiente.PaddingBottom = 10f;
                    }
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    Emision.PaddingBottom = 10f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        Emision.PaddingBottom = 15f;
                    }
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }

                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);

                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 9f;

                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 10f;

                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 5f;
                        DirSucursal.PaddingTop = 6f;

                        innerTableL.AddCell(DirSucursal);
                    }

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 3f;
                    ContribuyenteEspecial.PaddingTop = 7f;

                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    ObligadoContabilidad.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ObligadoContabilidad.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ObligadoContabilidad.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ObligadoContabilidad.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ObligadoContabilidad);

                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (RegMicroEmp.ToLower().Equals("si"))
                            {
                                PdfPCell RegimenMicroEmpresa = new PdfPCell(new Paragraph("Contribuyente Régimen Microempresas.", cabecera));
                                RegimenMicroEmpresa.Border = Rectangle.NO_BORDER;
                                RegimenMicroEmpresa.Padding = 5f;
                                RegimenMicroEmpresa.PaddingBottom = 3f;
                                RegimenMicroEmpresa.PaddingTop = 3f;
                                innerTableL.AddCell(RegimenMicroEmpresa);
                            }
                        }
                    }

                    XmlNodeList IA = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    bool agenteRetencionInfoA = false;

                    foreach (XmlNode campoAdicional in IA)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "Agente")
                        {
                            agenteRetencionInfoA = true;
                            PdfPCell AgenteRetencion = new PdfPCell(new Paragraph(campoAdicional.InnerText, cabecera));
                            AgenteRetencion.Border = Rectangle.NO_BORDER;
                            AgenteRetencion.Padding = 5f;
                            innerTableL.AddCell(AgenteRetencion);
                        }
                    }

                    try
                    {
                        string agente = oDocument.SelectSingleNode("//infoTributaria/agenteRetencion").InnerText;
                        if (!string.IsNullOrEmpty(agente))
                        {
                            if (!agenteRetencionInfoA)
                            {
                                agente = agente.TrimStart(new Char[] { '0' });
                                PdfPCell AgenteRetencion = new PdfPCell(new Paragraph("Agente de Retención Resolución No." + agente, cabecera));
                                AgenteRetencion.Border = Rectangle.NO_BORDER;
                                AgenteRetencion.Padding = 5f;
                                innerTableL.AddCell(AgenteRetencion);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        if (!string.IsNullOrEmpty(rimpe))
                        {
                            try
                            {
                                PdfPCell rimpe_ = new PdfPCell(new Paragraph(rimpe, cabecera));
                                rimpe_.Border = Rectangle.NO_BORDER;
                                rimpe_.Padding = 5f;
                                rimpe_.PaddingBottom = 3f;
                                rimpe_.PaddingTop = 3f;
                                innerTableL.AddCell(rimpe_);
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }

                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);

                    #endregion

                    #region Logo
                    BaseColor color = null;
                    iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpgPrueba);
                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    #region DetalleTransportista
                    PdfPTable tableDetalleTransportista = new PdfPTable(4);
                    tableDetalleTransportista.TotalWidth = 540f;
                    tableDetalleTransportista.WidthPercentage = 100;
                    float[] DetalleTransportistawidths = new float[] { 30f, 90f, 20f, 90f };
                    tableDetalleTransportista.SetWidths(DetalleTransportistawidths);

                    var lblRUCTransportista = new PdfPCell(new Paragraph("RUC/CI (Transportista):", detalle));
                    lblRUCTransportista.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(lblRUCTransportista);
                    var RUCTransportista = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/rucTransportista").InnerText, detalle));
                    RUCTransportista.Border = Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(RUCTransportista);

                    var lblPlaca = new PdfPCell(new Paragraph("Placa:", detalle));
                    lblPlaca.Border = Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(lblPlaca);
                    var Placa = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/placa").InnerText, detalle));
                    Placa.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleTransportista.AddCell(Placa);

                    var lblRazonSocial = new PdfPCell(new Paragraph("Razon Social / Nombres y Apellidos:", detalle));
                    lblRazonSocial.Border = Rectangle.LEFT_BORDER;
                    tableDetalleTransportista.AddCell(lblRazonSocial);
                    var RazonSocialTransportista = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/razonSocialTransportista").InnerText, detalle));
                    RazonSocialTransportista.Border = Rectangle.NO_BORDER;
                    tableDetalleTransportista.AddCell(RazonSocialTransportista);

                    var lblPuntoPartida = new PdfPCell(new Paragraph("Punto de partida:", detalle));
                    lblPuntoPartida.Border = Rectangle.NO_BORDER;
                    lblPuntoPartida.PaddingBottom = 5f;
                    tableDetalleTransportista.AddCell(lblPuntoPartida);
                    var PuntoPartida = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/dirPartida").InnerText, detalle));
                    PuntoPartida.Border = Rectangle.RIGHT_BORDER;
                    PuntoPartida.PaddingBottom = 5f;
                    tableDetalleTransportista.AddCell(PuntoPartida);

                    var lblFechaInicioTransporte = new PdfPCell(new Paragraph("Fecha Inicio de Transporte:", detalle));
                    lblFechaInicioTransporte.Border = Rectangle.LEFT_BORDER;
                    tableDetalleTransportista.AddCell(lblFechaInicioTransporte);
                    var FechaInicioTransporte = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/fechaIniTransporte").InnerText, detalle));
                    FechaInicioTransporte.Border = Rectangle.NO_BORDER;
                    tableDetalleTransportista.AddCell(FechaInicioTransporte);

                    var lblFechaFinTransporte = new PdfPCell(new Paragraph("Fecha Fin de Transporte:", detalle));
                    lblFechaFinTransporte.Border = Rectangle.NO_BORDER;
                    tableDetalleTransportista.AddCell(lblFechaFinTransporte);
                    var FechaFinTransporte = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/fechaFinTransporte").InnerText, detalle));
                    FechaFinTransporte.Border = Rectangle.RIGHT_BORDER;
                    tableDetalleTransportista.AddCell(FechaFinTransporte);

                    var lblDireccion = new PdfPCell(new Paragraph("Dirección:", detalle));
                    lblDireccion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleTransportista.AddCell(lblDireccion);

                    var Direccion = new PdfPCell(new Paragraph(direccionCliente == null ? "" : direccionCliente, detalle));
                    Direccion.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    Direccion.Colspan = 4;
                    tableDetalleTransportista.AddCell(Direccion);
                    #endregion

                    #region CabeceraGuia

                    XmlNodeList CabeceraGuiaDetalles;
                    CabeceraGuiaDetalles = oDocument.SelectNodes("//destinatarios/destinatario");
                    cabeceraguiaregstros = CabeceraGuiaDetalles.Count;

                    if (cabeceraguiaregstros > 0)
                    {

                        foreach (XmlNode Elemento in CabeceraGuiaDetalles)
                        {
                            string strComprobanteVenta = (Elemento["numDocSustento"] == null ? "" : Elemento["numDocSustento"].InnerText);
                            string strFechaEmision = (Elemento["fechaEmisionDocSustento"] == null ? "" : Elemento["fechaEmisionDocSustento"].InnerText);
                            string strNumeroAutorizacion = (Elemento["numAutDocSustento"] == null ? "" : Elemento["numAutDocSustento"].InnerText);
                            string strAduaneroUnico = (Elemento["docAduaneroUnico"] == null ? "" : Elemento["docAduaneroUnico"].InnerText);
                            string strMotivoTraslado = (Elemento["motivoTraslado"] == null ? "" : Elemento["motivoTraslado"].InnerText);
                            string strRUCDestinatario = (Elemento["identificacionDestinatario"] == null ? "" : Elemento["identificacionDestinatario"].InnerText);
                            string strRazonSocialDestinatario = (Elemento["razonSocialDestinatario"] == null ? "" : Elemento["razonSocialDestinatario"].InnerText);
                            string strCodEstablecimiento = (Elemento["codEstabDestino"] == null ? "" : Elemento["codEstabDestino"].InnerText);
                            string strDestino = (Elemento["dirDestinatario"] == null ? "" : Elemento["dirDestinatario"].InnerText);
                            string strRuta = (Elemento["ruta"] == null ? "" : Elemento["ruta"].InnerText);// Elemento["ruta"].InnerText;

                            PdfPTable tableCabeceraGuia = new PdfPTable(2);
                            tableCabeceraGuia.TotalWidth = 540f;
                            tableCabeceraGuia.WidthPercentage = 100;
                            tableCabeceraGuia.LockedWidth = true;
                            float[] DetalleGuiaWidths = new float[] { 80f, 300f };
                            tableCabeceraGuia.SetWidths(DetalleGuiaWidths);

                            var lblTOP = new PdfPCell(new Paragraph("", detalle));
                            lblTOP.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                            tableCabeceraGuia.AddCell(lblTOP);

                            var TOP = new PdfPCell(new Paragraph("", detalle));
                            TOP.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(TOP);

                            var lblComprobanteVenta = new PdfPCell(new Paragraph("Comprobante de venta:     Factura:", detalle));
                            lblComprobanteVenta.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                            tableCabeceraGuia.AddCell(lblComprobanteVenta);
                            var ComprobanteVenta = new PdfPCell(new Paragraph(strComprobanteVenta, detalle));
                            ComprobanteVenta.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(ComprobanteVenta);

                            var lblFechaEmision = new PdfPCell(new Paragraph("Fecha de emisión:", detalle));
                            lblFechaEmision.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblFechaEmision);
                            var FechaEmision = new PdfPCell(new Paragraph(strFechaEmision, detalle));
                            FechaEmision.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(FechaEmision);

                            var lblNumeroAutorizacion = new PdfPCell(new Paragraph("Número de autorización:", detalle));
                            lblNumeroAutorizacion.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblNumeroAutorizacion);
                            var NumeroAutorizacion = new PdfPCell(new Paragraph(strNumeroAutorizacion, detalle));
                            NumeroAutorizacion.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(NumeroAutorizacion);

                            var lblMotivoTraslado = new PdfPCell(new Paragraph("Motivo Traslado:", detalle));
                            lblMotivoTraslado.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblMotivoTraslado);
                            var MotivoTraslado = new PdfPCell(new Paragraph(strMotivoTraslado, detalle));
                            MotivoTraslado.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(MotivoTraslado);

                            var lblRUCDestinatario = new PdfPCell(new Paragraph("RUC / CI (Destinatario):", detalle));
                            lblRUCDestinatario.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblRUCDestinatario);
                            var RUCDestinatario = new PdfPCell(new Paragraph(strRUCDestinatario, detalle));
                            RUCDestinatario.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(RUCDestinatario);

                            var lblRazonSocialDestinatario = new PdfPCell(new Paragraph("Razón social / Nombres Apellidos:", detalle));
                            lblRazonSocialDestinatario.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblRazonSocialDestinatario);
                            var RazonSocialDestinatario = new PdfPCell(new Paragraph(strRazonSocialDestinatario, detalle));
                            RazonSocialDestinatario.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(RazonSocialDestinatario);

                            var lblDocumentoAduanero = new PdfPCell(new Paragraph("Documento Aduanero:", detalle));
                            lblDocumentoAduanero.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblDocumentoAduanero);
                            var DocumentoAduanero = new PdfPCell(new Paragraph(strAduaneroUnico, detalle));
                            DocumentoAduanero.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(DocumentoAduanero);

                            var lblCodEstablecimiento = new PdfPCell(new Paragraph("Código Establecimiento Destino:", detalle));
                            lblCodEstablecimiento.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblCodEstablecimiento);
                            var CodEstablecimiento = new PdfPCell(new Paragraph(strCodEstablecimiento, detalle));
                            CodEstablecimiento.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(CodEstablecimiento);

                            var lblDestino = new PdfPCell(new Paragraph("Destino (Punto de llegada):", detalle));
                            lblDestino.Border = Rectangle.LEFT_BORDER;
                            tableCabeceraGuia.AddCell(lblDestino);
                            var Destino = new PdfPCell(new Paragraph(strDestino, detalle));
                            Destino.Border = Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(Destino);

                            var lblRuta = new PdfPCell(new Paragraph("Ruta:", detalle));
                            lblRuta.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                            lblRuta.PaddingBottom = 5f;
                            tableCabeceraGuia.AddCell(lblRuta);
                            var Ruta = new PdfPCell(new Paragraph(strRuta, detalle));
                            Ruta.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                            Ruta.PaddingBottom = 5f;
                            tableCabeceraGuia.AddCell(Ruta);

                            var lblBOTTOM = new PdfPCell(new Paragraph("", detalle));
                            lblBOTTOM.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                            tableCabeceraGuia.AddCell(lblBOTTOM);

                            var BOTTOM = new PdfPCell(new Paragraph("", detalle));
                            BOTTOM.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                            tableCabeceraGuia.AddCell(BOTTOM);


                            TableGuiRemision GuiaCabeceraDetalle;
                            GuiaCabeceraDetalle = new TableGuiRemision();

                            GuiaCabeceraDetalle.Cabecera = tableCabeceraGuia;

                            XmlNodeList DetallesProducto;
                            DetallesProducto = Elemento.SelectNodes("detalles/detalle");
                            registros = DetallesProducto.Count;

                            PdfPTable tableDetalleFactura = new PdfPTable(7);

                            tableDetalleFactura.TotalWidth = 540f;
                            tableDetalleFactura.WidthPercentage = 100;
                            float[] DetalleFacturawidths = new float[] { 30f, 180f, 50f, 50f, 50f, 50f, 50f }; //, 30f};
                            tableDetalleFactura.SetWidths(DetalleFacturawidths);

                            var fontEncabezado = GetArial(7);

                            var encCant = new PdfPCell(new Paragraph("Cant.", fontEncabezado));
                            encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                            encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                            encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var encCodAuxiliar = new PdfPCell(new Paragraph("Cod. Auxiliar", fontEncabezado));
                            encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var detAdicional1 = new PdfPCell(new Paragraph("Det. Adicional 1", fontEncabezado));
                            encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var detAdicional2 = new PdfPCell(new Paragraph("Det. Adicional 2", fontEncabezado));
                            encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            var detAdicional3 = new PdfPCell(new Paragraph("Det. Adicional 3", fontEncabezado));
                            encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                            tableDetalleFactura.AddCell(encCant);
                            tableDetalleFactura.AddCell(encDescripcion);
                            tableDetalleFactura.AddCell(encCodPrincipal);
                            tableDetalleFactura.AddCell(encCodAuxiliar);
                            tableDetalleFactura.AddCell(detAdicional1);
                            tableDetalleFactura.AddCell(detAdicional2);
                            tableDetalleFactura.AddCell(detAdicional3);

                            PdfPCell CodPrincipal;
                            PdfPCell CodAuxiliar;
                            PdfPCell Cant;
                            PdfPCell Descripcion;
                            PdfPCell Det1;
                            PdfPCell Det2;
                            PdfPCell Det3;

                            foreach (XmlNode productoelemto in DetallesProducto)
                            {

                                CodPrincipal = new PdfPCell(new Phrase(productoelemto["codigoInterno"].InnerText, detalle));
                                CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                Cant = new PdfPCell(new Phrase(productoelemto["cantidad"].InnerText, detalle));
                                Cant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                Descripcion = new PdfPCell(new Phrase(productoelemto["descripcion"].InnerText, detalle));
                                CodAuxiliar = new PdfPCell(new Phrase(productoelemto["codigoAdicional"] == null ? "" : productoelemto["codigoAdicional"].InnerText, detalle));
                                CodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                                var DetallesAdicionales = productoelemto.SelectNodes("detallesAdicionales/detAdicional");
                                if (!(DetallesAdicionales[0] == null))
                                {
                                    Det1 = new PdfPCell(new Phrase(DetallesAdicionales[0].Attributes["valor"].Value, detalle));
                                    Det1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }
                                else
                                {
                                    Det1 = new PdfPCell(new Phrase("", detalle));
                                    Det1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }
                                if (!(DetallesAdicionales[1] == null))
                                {
                                    Det2 = new PdfPCell(new Phrase(DetallesAdicionales[1].Attributes["valor"].Value, detalle));
                                    Det2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }
                                else
                                {
                                    Det2 = new PdfPCell(new Phrase("", detalle));
                                    Det2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }
                                if (!(DetallesAdicionales[2] == null))
                                {
                                    Det3 = new PdfPCell(new Phrase(DetallesAdicionales[2].Attributes["valor"].Value, detalle));
                                    Det3.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }
                                else
                                {
                                    Det3 = new PdfPCell(new Phrase("", detalle));
                                    Det3.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                                }

                                tableDetalleFactura.AddCell(Cant);
                                tableDetalleFactura.AddCell(Descripcion);
                                tableDetalleFactura.AddCell(CodPrincipal);
                                tableDetalleFactura.AddCell(CodAuxiliar);
                                tableDetalleFactura.AddCell(Det1);
                                tableDetalleFactura.AddCell(Det2);
                                tableDetalleFactura.AddCell(Det3);
                            }

                            GuiaCabeceraDetalle.Detalles = tableDetalleFactura;
                            TablaGuiaRemision.Add(GuiaCabeceraDetalle);

                        }
                    }
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (!(campoAdicional.Attributes["nombre"].Value == "Agente"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas"))
                        {
                            lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;

                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion
                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);

                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;
                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);
                    var posicion = 15;
                    posDetalleTransportista = 796 - ContenedorD.TotalHeight - posicion;

                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;
                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;

                    if (DifTableL < posDetalleTransportista)
                    {
                        aumento = posDetalleTransportista - DifTableL + 10;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleTransportista) + 5;
                    }

                    tableDetalleTransportista.WriteSelectedRows(0, 5, 28, posDetalleTransportista - aumento, canvas);
                    posDetalleGuia = (posDetalleTransportista - 10) - tableDetalleTransportista.TotalHeight;

                    float inicialcab = posDetalleGuia;
                    float limite = 0;
                    int numpaginas = 1;

                    if (cabeceraguiaregstros > 0)
                    {

                        foreach (TableGuiRemision tablaguia in TablaGuiaRemision)
                        {
                            limite = limite + tablaguia.Cabecera.TotalHeight + 5;
                            if (limite > 520)
                            {
                                numpaginas = numpaginas + 1;
                            }

                            limite = limite + tablaguia.Detalles.TotalHeight + 10;
                            if (limite > 520)
                            {
                                numpaginas = numpaginas + 1;
                            }
                        }

                        oEvents.Contador = numpaginas;

                        limite = 0;

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));

                        foreach (TableGuiRemision tablaguia in TablaGuiaRemision)
                        {
                            tablaguia.Cabecera.WriteSelectedRows(0, 11, 28, inicialcab - aumento, canvas);
                            inicialcab = inicialcab - tablaguia.Cabecera.TotalHeight - 5;

                            limite = limite + tablaguia.Cabecera.TotalHeight + 5;
                            if (limite > 520)
                            {
                                documento.NewPage();
                            }


                            tablaguia.Detalles.WriteSelectedRows(0, PagLimite1, 28, inicialcab - aumento, canvas);
                            inicialcab = inicialcab - tablaguia.Detalles.TotalHeight - 10;

                            limite = limite + tablaguia.Detalles.TotalHeight + 10;
                            if (limite > 520)
                            {
                                documento.NewPage();
                            }
                        }
                    }
                    tableInfoAdicional.WriteSelectedRows(0, 17, 28, inicialcab - 10 - aumento, canvas);

                    writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Bytes;
        }

        public byte[] GeneraNotaCredito(string pDocumento_autorizado, string pRutaLogo, string direccion = "", string direccionCliente = "", string pCultura = "en-US")
        {
            return GeneraNotaCredito(null, pDocumento_autorizado, pRutaLogo, direccion, direccionCliente, pCultura);
        }

        /// <summary>
        /// Método para generación de nota de crédito RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraNotaCredito(string p_Documento_noAutorizado, string pDocumento_autorizado, string pRutaLogo, string direccion = "", string direccionCliente = "", string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            iTextSharp.text.Font detAdicional = GetArial(6);
            string IVAride = "";

            DateTime FechaEmisionValida = DateTime.Now;
            try
            {

                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    XmlNode xmlAutorizaciones;
                    XmlDocument oDocument_NoAutorizado = new XmlDocument();

                    Boolean isTransportista = false;

                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sContribuyenteEspecial = "", sRuc = "",
                           sAmbienteVal = "", sNumAutorizacion = "", sObligadoContabilidad = "", rimpe = "",
                           sComprobanteModificacion = "", RucTransportista = "", PuntoEmisionTrans = "", Regimen = "", RazonSocialTransportista = "", RegMicroEmp = "";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";


                    oDocument.LoadXml(pDocumento_autorizado);

                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    sAmbiente = oDocument.SelectSingleNode("//ambiente") == null ? sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN" : oDocument.SelectSingleNode("//ambiente").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = (oDocument.SelectSingleNode("//infoNotaCredito/contribuyenteEspecial") == null) ? "" : oDocument.SelectSingleNode("//infoNotaCredito/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoNotaCredito/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoNotaCredito/obligadoContabilidad").InnerText;

                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {
                        try
                        {
                            oDocument_NoAutorizado.LoadXml(p_Documento_noAutorizado);

                            RucTransportista = oDocument_NoAutorizado.DocumentElement.Attributes["rr"].Value;
                            PuntoEmisionTrans = oDocument_NoAutorizado.DocumentElement.Attributes["pt"].Value;
                            RazonSocialTransportista = oDocument_NoAutorizado.DocumentElement.Attributes["rt"].Value;
                            Regimen = oDocument_NoAutorizado.DocumentElement.Attributes["rg"].Value;
                            isTransportista = true;
                        }
                        catch (Exception e)
                        {
                            isTransportista = false;
                        }
                    }

                    bool Excep = false;
                    bool microEmpInfoAdicional = false;

                    try
                    {
                        XmlNodeList IARegMicro = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                        foreach (XmlNode campoAdicional in IARegMicro)
                        {
                            if (campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas")
                            {
                                RegMicroEmp = "si";
                                microEmpInfoAdicional = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Excep = true;
                    }


                    if (!Excep)
                    {
                        if (!microEmpInfoAdicional)
                        {
                            try
                            {
                                string micoEmpe = oDocument.SelectSingleNode("//infoTributaria/regimenMicroempresas").InnerText;
                                if (!string.IsNullOrEmpty(micoEmpe))
                                {

                                    RegMicroEmp = "si";

                                }
                            }
                            catch (Exception)
                            {
                                Excep = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(p_Documento_noAutorizado))
                    {
                        try
                        {
                            rimpe = oDocument_NoAutorizado.DocumentElement.Attributes["ri"].Value;
                        }
                        catch (Exception)
                        {
                        }

                    }

                    int registros = 200;
                    int PagLimite1 = 30;
                    int MaxPagina1 = 39;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);


                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;

                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();
                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeAltoVariable = 0D;
                    var porcentajeAnchoVariable = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;

                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);
                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));
                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {
                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {
                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);
                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    RUC.PaddingTop = 10f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("N O T A  D E  C R É D I T O ", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    Factura.PaddingBottom = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    NumFactura.PaddingBottom = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    lblNumAutorizacion.PaddingBottom = 10f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    NumAutorizacion.PaddingBottom = 10f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA DE AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    FechaAutorizacion.PaddingBottom = 10f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    Ambiente.PaddingBottom = 10f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    Emision.PaddingBottom = 10f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);
                    
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                RazonSocial.FixedHeight = 20f;
                            }
                        }
                        else
                        {
                            RazonSocial.FixedHeight = 20f;
                        }
                    }
                    else
                    {
                        RazonSocial.FixedHeight = 20f;
                    }
                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dirección Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 3f;

                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dirección Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 3f;
                        DirSucursal.PaddingTop = 3f;
                        if (!Excep)
                        {
                            if (!string.IsNullOrEmpty(RegMicroEmp))
                            {
                                if (!RegMicroEmp.ToLower().Equals("si"))
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }

                        innerTableL.AddCell(DirSucursal);
                    }

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 3f;
                    ContribuyenteEspecial.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ContribuyenteEspecial.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ContribuyenteEspecial.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ContribuyenteEspecial.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    ObligadoContabilidad.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ObligadoContabilidad.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ObligadoContabilidad.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ObligadoContabilidad.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ObligadoContabilidad);

                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (RegMicroEmp.ToLower().Equals("si"))
                            {
                                PdfPCell RegimenMicroEmpresa = new PdfPCell(new Paragraph("Contribuyente Régimen Microempresas.", cabecera));
                                RegimenMicroEmpresa.Border = Rectangle.NO_BORDER;
                                RegimenMicroEmpresa.Padding = 5f;
                                RegimenMicroEmpresa.PaddingBottom = 3f;
                                RegimenMicroEmpresa.PaddingTop = 3f;
                                innerTableL.AddCell(RegimenMicroEmpresa);
                            }
                        }
                    }

                    XmlNodeList IA = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    bool agenteRetencionInfoA = false;

                    foreach (XmlNode campoAdicional in IA)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "Agente")
                        {
                            PdfPCell AgenteRetencion = new PdfPCell(new Paragraph(campoAdicional.InnerText, cabecera));
                            AgenteRetencion.Border = Rectangle.NO_BORDER;
                            AgenteRetencion.Padding = 5f;
                            AgenteRetencion.PaddingBottom = 4f;
                            AgenteRetencion.PaddingTop = 4f;
                            innerTableL.AddCell(AgenteRetencion);
                            agenteRetencionInfoA = true;
                        }
                    }

                    try
                    {
                        string agente = oDocument.SelectSingleNode("//infoTributaria/agenteRetencion").InnerText;
                        if (!string.IsNullOrEmpty(agente))
                        {
                            if (!agenteRetencionInfoA)
                            {
                                agente = agente.TrimStart(new Char[] { '0' });
                                PdfPCell AgenteRetencion = new PdfPCell(new Paragraph("Agente de Retención Resolución No." + agente, cabecera));
                                AgenteRetencion.Border = Rectangle.NO_BORDER;
                                AgenteRetencion.Padding = 5f;
                                innerTableL.AddCell(AgenteRetencion);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        if (!string.IsNullOrEmpty(rimpe))
                        {
                            PdfPCell rimpe_ = new PdfPCell(new Paragraph(rimpe, cabecera));
                            rimpe_.Border = Rectangle.NO_BORDER;
                            rimpe_.Padding = 5f;
                            rimpe_.PaddingBottom = 4f;
                            rimpe_.PaddingTop = 4f;
                            innerTableL.AddCell(rimpe_);
                        }
                    }
                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);

                    #endregion

                    #region Logo
                    BaseColor color = null;
                    iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpgPrueba);
                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion
                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);
                    #region CabeceraNotaCredito
                    PdfPTable tableNotaCredito = new PdfPTable(4);
                    tableNotaCredito.TotalWidth = 540f;
                    tableNotaCredito.WidthPercentage = 100;
                    float[] DetalleNotaCreditoWidths = new float[] { 40f, 120f, 30f, 40f };
                    tableNotaCredito.SetWidths(DetalleNotaCreditoWidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/razonSocialComprador").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("Identificación:", detalle));//Identificación RUC / CI: Cambio solicitado por BB
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/identificacionComprador").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(RUCcliente);

                    FechaEmisionValida = DateTime.ParseExact(oDocument.SelectSingleNode("//infoNotaCredito/fechaEmision").InnerText, "dd/MM/yyyy", new CultureInfo(Cultura));

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                    tableNotaCredito.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.NO_BORDER;
                    tableNotaCredito.AddCell(FechaEmisionCliente);

                    var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    lblGuiaRemision.Border = Rectangle.NO_BORDER;
                    tableNotaCredito.AddCell(lblGuiaRemision);
                    var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    GuiaRemision.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(GuiaRemision);

                    var lblDireccion = new PdfPCell(new Paragraph("Dirección:", detalle));
                    lblDireccion.Border = Rectangle.LEFT_BORDER + Rectangle.NO_BORDER;
                    lblDireccion.PaddingBottom = 8f;
                    tableNotaCredito.AddCell(lblDireccion);
                    var Direccion = new PdfPCell(new Paragraph(direccionCliente == null ? "" : direccionCliente, detalle));
                    Direccion.Border = Rectangle.BOTTOM_BORDER;
                    Direccion.PaddingBottom = 8f;
                    tableNotaCredito.AddCell(Direccion);

                    var lblCellVacia = new PdfPCell(new Paragraph("", detalle));
                    lblCellVacia.Border = Rectangle.BOTTOM_BORDER;
                    tableNotaCredito.AddCell(lblCellVacia);
                    var cellVacia = new PdfPCell(new Paragraph("", detalle));
                    cellVacia.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(cellVacia);

                    var lblDocModifica = new PdfPCell(new Paragraph("Comprobante que se modifica:  ", detalle));
                    lblDocModifica.Border = Rectangle.LEFT_BORDER;
                    lblDocModifica.PaddingTop = 5f;
                    tableNotaCredito.AddCell(lblDocModifica);

                    sComprobanteModificacion = oDocument.SelectSingleNode("//infoNotaCredito/codDocModificado") == null ? "" : oDocument.SelectSingleNode("//infoNotaCredito/codDocModificado").InnerText;

                    switch (sComprobanteModificacion)
                    {
                        case "01":
                            sComprobanteModificacion = "FACTURA: ";
                            break;
                        case "04":
                            sComprobanteModificacion = "NOTA DE CRÉDITO: ";
                            break;
                        case "05":
                            sComprobanteModificacion = "NOTA DE DÉBITO: ";
                            break;
                        case "06":
                            sComprobanteModificacion = "GUÍA DE REMISIÓN: ";
                            break;
                        case "07":
                            sComprobanteModificacion = "COMPROBANTE DE RETENCIÓN: ";
                            break;
                        default:
                            break;
                    }

                    var DocModifica = new PdfPCell(new Paragraph(sComprobanteModificacion + oDocument.SelectSingleNode("//infoNotaCredito/numDocModificado").InnerText, detalle));
                    DocModifica.PaddingTop = 5f;
                    DocModifica.Colspan = 3;
                    DocModifica.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(DocModifica);

                    var lblFechaEmision = new PdfPCell(new Paragraph("Fecha emisión (comprobante a modificar): ", detalle));
                    lblFechaEmision.Border = Rectangle.LEFT_BORDER;
                    lblFechaEmision.PaddingBottom = 5f;
                    tableNotaCredito.AddCell(lblFechaEmision);

                    var FechaEmision = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/fechaEmisionDocSustento").InnerText, detalle));
                    FechaEmision.Colspan = 3;
                    FechaEmision.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(FechaEmision);

                    var lblRazonModificacion = new PdfPCell(new Paragraph("Razón de Modificación: ", detalle));
                    lblRazonModificacion.PaddingBottom = 5f;
                    lblRazonModificacion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableNotaCredito.AddCell(lblRazonModificacion);

                    var RazonModificacion = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/motivo").InnerText, detalle));
                    RazonModificacion.PaddingBottom = 5f;
                    RazonModificacion.Colspan = 3;
                    RazonModificacion.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableNotaCredito.AddCell(RazonModificacion);

                    #endregion

                    #region DetalleNotaCredito
                    PdfPTable tableDetalleNotaCredito = new PdfPTable(8); // 10

                    tableDetalleNotaCredito.TotalWidth = 540f;
                    tableDetalleNotaCredito.WidthPercentage = 100;
                    tableDetalleNotaCredito.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 30f, 120f, 20f, 25f, 25f, 25f, 26f, 28f };
                    tableDetalleNotaCredito.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCant = new PdfPCell(new Paragraph("Unidades", fontEncabezado));
                    encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                    encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional1 = new PdfPCell(new Paragraph("TM", fontEncabezado));
                    encDetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional2 = new PdfPCell(new Paragraph("Importe Bruto", fontEncabezado));
                    encDetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescuento = new PdfPCell(new Paragraph("Descuento", fontEncabezado));
                    encDescuento.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var encPrecioUnitario = new PdfPCell(new Paragraph("Precio Unitario", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var encPrecioTotal = new PdfPCell(new Paragraph("Importe Total", fontEncabezado));
                    encPrecioTotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleNotaCredito.AddCell(encCodPrincipal);
                    tableDetalleNotaCredito.AddCell(encDescripcion);
                    tableDetalleNotaCredito.AddCell(encCant);

                    tableDetalleNotaCredito.AddCell(encDetalleAdicional1);
                    tableDetalleNotaCredito.AddCell(encPrecioUnitario);

                    tableDetalleNotaCredito.AddCell(encDetalleAdicional2);
                    tableDetalleNotaCredito.AddCell(encDescuento);

                    tableDetalleNotaCredito.AddCell(encPrecioTotal);

                    PdfPCell CodPrincipal;
                    PdfPCell CodAuxiliar;
                    PdfPCell Cant;
                    PdfPCell Descripcion;
                    PdfPCell DetalleAdicional1;
                    PdfPCell DetalleAdicional2;
                    PdfPCell DetalleAdicional3;
                    PdfPCell PrecioUnitario;
                    PdfPCell Descuento;
                    PdfPCell PrecioTotal;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//detalles/detalle");
                    registros = Detalles.Count;
                    XmlNodeList DetallesAdicionales;
                    foreach (XmlNode Elemento in Detalles)
                    {
                        CodPrincipal = new PdfPCell(new Phrase(Elemento["codigoInterno"].InnerText, detalle));
                        CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Cant = new PdfPCell(new Phrase(Elemento["cantidad"].InnerText, detalle));
                        Cant.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        Descripcion = new PdfPCell(new Phrase(Elemento["descripcion"].InnerText, detalle));
                        DetallesAdicionales = Elemento.SelectNodes("detallesAdicionales/detAdicional");
                        if (!(DetallesAdicionales[0] == null))
                        {
                            DetalleAdicional1 = new PdfPCell(new Phrase(DetallesAdicionales[0].Attributes["valor"].Value, detalle));
                            DetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        else
                        {
                            DetalleAdicional1 = new PdfPCell(new Phrase("", detalle));
                            DetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        if (!(DetallesAdicionales[1] == null))
                        {
                            DetalleAdicional2 = new PdfPCell(new Phrase(DetallesAdicionales[1].Attributes["valor"].Value, detalle));
                            DetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }
                        else
                        {
                            DetalleAdicional2 = new PdfPCell(new Phrase("", detalle));
                            DetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        }

                        PrecioUnitario = new PdfPCell(new Phrase(Elemento["precioUnitario"].InnerText, detalle));
                        PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        Descuento = new PdfPCell(new Phrase(Elemento["descuento"].InnerText, detalle));
                        Descuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        PrecioTotal = new PdfPCell(new Phrase(Elemento["precioTotalSinImpuesto"].InnerText, detalle));
                        PrecioTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        tableDetalleNotaCredito.AddCell(CodPrincipal);
                        tableDetalleNotaCredito.AddCell(Descripcion);
                        tableDetalleNotaCredito.AddCell(Cant);
                        tableDetalleNotaCredito.AddCell(DetalleAdicional1);
                        tableDetalleNotaCredito.AddCell(PrecioUnitario);

                        tableDetalleNotaCredito.AddCell(DetalleAdicional2);
                        tableDetalleNotaCredito.AddCell(Descuento);
                        tableDetalleNotaCredito.AddCell(PrecioTotal);

                        XmlNodeList DetallesImpuestos;
                        DetallesImpuestos = oDocument.SelectNodes("//detalles/detalle/impuestos/impuesto");
                        if (IVAride == "")
                        {
                            foreach (XmlNode Elementoimp in DetallesImpuestos)
                            {
                                if (Elementoimp["codigo"].InnerText == "2" && Elementoimp["codigoPorcentaje"].InnerText == "2")
                                {
                                    IVAride = Elementoimp["tarifa"].InnerText;
                                    break;
                                }

                                if (Elementoimp["codigo"].InnerText == "2" && Elementoimp["codigoPorcentaje"].InnerText == "3")
                                {
                                    IVAride = Elementoimp["tarifa"].InnerText;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region FormaPagos
                    var tableFormaPago = new PdfPTable(2);
                    tableFormaPago.TotalWidth = 250f;
                    float[] FormaPagosWidths = new float[] { 65f, 170f };
                    tableFormaPago.SetWidths(FormaPagosWidths);

                    var lblFormaPago = new PdfPCell(new Paragraph("Forma de Pago", detalle));
                    lblFormaPago.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblFormaPago.Colspan = 2;
                    lblFormaPago.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblFormaPago.Padding = 5f;

                    var lblBottomfp = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottomfp.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottomfp.Padding = 2f;
                    var Bottomfp = new PdfPCell(new Paragraph("  ", detalle));
                    Bottomfp.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottomfp.Padding = 2f;

                    XmlNodeList FormaPagos;
                    FormaPagos = oDocument.SelectNodes("//pagos/pago");

                    PdfPCell lblFormaPagoDes;
                    PdfPCell MontoFp;
                    XmlNodeList InfoAdicionalaux;
                    InfoAdicionalaux = oDocument.SelectNodes("//infoAdicional/campoAdicional");
                    int banderapagos = 0;
                    foreach (XmlNode pago in FormaPagos)
                    {
                        if (banderapagos == 0)
                        {
                            tableFormaPago.AddCell(lblFormaPago);
                            banderapagos = 1;
                        }
                        //buscar descripcion de forma de pago

                        string datocodi = pago.ChildNodes[0].InnerText;
                        string descripcifromapg = "";


                        foreach (XmlNode campoAdicional in InfoAdicionalaux)
                        {
                            if ((campoAdicional.Attributes["nombre"].Value.ToString().Contains("[IF|" + datocodi)))
                            {
                                descripcifromapg = campoAdicional.InnerText.Split('|')[1];
                                break;
                            }
                        }

                        lblFormaPagoDes = new PdfPCell(new Paragraph(descripcifromapg, detAdicional));
                        lblFormaPagoDes.Border = Rectangle.LEFT_BORDER;
                        lblFormaPagoDes.Padding = 2f;

                        MontoFp = new PdfPCell(new Paragraph(pago.ChildNodes[1].InnerText, detAdicional));
                        MontoFp.Border = Rectangle.RIGHT_BORDER;
                        MontoFp.Padding = 2f;

                        tableFormaPago.AddCell(lblFormaPagoDes);
                        tableFormaPago.AddCell(MontoFp);
                    }
                    if (banderapagos == 1)
                    {
                        tableFormaPago.AddCell(lblBottomfp);
                        tableFormaPago.AddCell(Bottomfp);
                    }
                    #endregion


                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;
                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (!(campoAdicional.Attributes["nombre"].Value == "Agente"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas"
                            || campoAdicional.Attributes["nombre"].Value == "PuntodeEmision"
                            || campoAdicional.Attributes["nombre"].Value == "RUC"
                            || campoAdicional.Attributes["nombre"].Value == "RazonSocial"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente"))
                        {
                            lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;

                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    #region DatosTransportista
                    var tableInfoTransportista = new PdfPTable(4);
                    if (isTransportista)
                    {
                        tableInfoTransportista.TotalWidth = 250f;
                        float[] InfoTransportistaWidths = new float[] { 60f, 80f, 60f, 40f };
                        tableInfoTransportista.SetWidths(InfoTransportistaWidths);



                        var lblCodigo2 = new PdfPCell(new Paragraph("Punto de Emisión: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;

                        lblCodigo2.Padding = 2f;

                        var Codigo2 = new PdfPCell(new Paragraph(PuntoEmisionTrans, detAdicional));
                        Codigo2.Border = Rectangle.TOP_BORDER;
                        Codigo2.Padding = 2f;

                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        var Codigo3 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo3.Border = Rectangle.TOP_BORDER;
                        Codigo3.Padding = 2f;
                        var Codigo4 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo4.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;

                        tableInfoTransportista.AddCell(Codigo3);
                        tableInfoTransportista.AddCell(Codigo4);

                        lblCodigo2 = new PdfPCell(new Paragraph("RUC: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER;

                        lblCodigo2.Padding = 2f;

                        Codigo2 = new PdfPCell(new Paragraph(RucTransportista, detAdicional));
                        Codigo2.Border = Rectangle.RIGHT_BORDER;
                        Codigo2.BorderColor = BaseColor.WHITE;
                        Codigo2.Padding = 2f;

                        var lblCodigo3 = new PdfPCell(new Paragraph("Contribuyente: ", detAdicional));
                        lblCodigo3.Border = Rectangle.LEFT_BORDER;
                        lblCodigo3.BorderColor = BaseColor.WHITE;
                        lblCodigo3.Padding = 2f;

                        Codigo3 = new PdfPCell(new Paragraph(Regimen, detAdicional));
                        Codigo3.Border = Rectangle.RIGHT_BORDER;

                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        tableInfoTransportista.AddCell(lblCodigo3);
                        tableInfoTransportista.AddCell(Codigo3);


                        lblCodigo2 = new PdfPCell(new Paragraph("Razón Social: ", detAdicional));
                        lblCodigo2.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;

                        lblCodigo2.Padding = 2f;

                        Codigo2 = new PdfPCell(new Paragraph(RazonSocialTransportista, detAdicional));
                        Codigo2.Border = Rectangle.BOTTOM_BORDER;

                        Codigo2.Padding = 2f;

                        Codigo3 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo3.Border = Rectangle.BOTTOM_BORDER;
                        Codigo3.Padding = 2f;
                        Codigo4 = new PdfPCell(new Paragraph("", detAdicional));
                        Codigo4.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;


                        tableInfoTransportista.AddCell(lblCodigo2);
                        tableInfoTransportista.AddCell(Codigo2);
                        tableInfoTransportista.AddCell(Codigo3);
                        tableInfoTransportista.AddCell(Codigo4);
                    }

                    #endregion

                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    decimal dpropina = 0;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "propina")
                        {
                            dpropina = decimal.Parse(campoAdicional.InnerText, nfi);
                        }
                    }

                    #region Totales

                    var tableTotales = new PdfPTable(2);
                    tableTotales.TotalWidth = 180f;
                    float[] InfoTotales = new float[] { 130f, 50f };
                    tableTotales.SetWidths(InfoTotales);

                    XmlNodeList Impuestos;
                    Impuestos = oDocument.SelectNodes("//infoNotaCredito/totalConImpuestos/totalImpuesto");
                    decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0, dSubtotalExcento = 0, dIRBPNR = 0;
                    foreach (XmlNode Impuesto in Impuestos)
                    {
                        switch (Impuesto["codigo"].InnerText)
                        {
                            case "2":        // IVA
                                if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                                {
                                    dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2" || Impuesto["codigoPorcentaje"].InnerText == "3")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "6")    // No objeto de Impuesto
                                {
                                    dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "7")
                                {
                                    dSubtotalExcento = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                break;
                            case "3":       // ICE
                                dICE = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                break;
                            case "5":
                                dIRBPNR = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                break;
                        }
                    }

                    if (IVAride == "")
                    {
                        string iva12val = (System.Configuration.ConfigurationManager.AppSettings["IVA12"]);
                        DateTime Dfecha12 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHA12"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha12 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHA12"], "dd/MM/yyyy", new CultureInfo(Cultura));

                        string iva14val = (System.Configuration.ConfigurationManager.AppSettings["IVA14"]);
                        DateTime Dfecha14 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHA14"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha14 = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHA14"], "dd/MM/yyyy", new CultureInfo(Cultura));


                        string iva12desval = (System.Configuration.ConfigurationManager.AppSettings["IVADES12"]);
                        DateTime Dfecha12des = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["DFECHADES12"], "dd/MM/yyyy", new CultureInfo(Cultura));
                        DateTime Hfecha12des = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["HFECHADES12"], "dd/MM/yyyy", new CultureInfo(Cultura));

                        if (FechaEmisionValida >= Dfecha12 && FechaEmisionValida <= Hfecha12)
                        {
                            IVAride = iva12val;
                        }

                        if (FechaEmisionValida >= Dfecha14 && FechaEmisionValida <= Hfecha14)
                        {
                            IVAride = iva14val;
                        }

                        if (FechaEmisionValida >= Dfecha12des && FechaEmisionValida <= Hfecha12des)
                        {
                            IVAride = iva12desval;
                        }
                    }
                    IVAride = IVAride.Replace(".00", "");
                    var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL " + IVAride + "%", detalle));
                    lblSubTotal12.Padding = 2f;
                    var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12 == 0 ? "0.00" : dSubtotal12.ToString(nfi), detalle));
                    SubTotal12.Padding = 2f;
                    SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 0%", detalle));
                    lblSubTotal0.Padding = 2f;
                    var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0 == 0 ? "0.00" : dSubtotal0.ToString(nfi), detalle));
                    SubTotal0.Padding = 2f;
                    SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL No objeto de IVA", detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI == 0 ? "0.00" : dSubtotalNSI.ToString(nfi), detalle));
                    SubTotalNoSujetoIVA.Padding = 2f;
                    SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalExcentoIVA = new PdfPCell(new Paragraph("SUBTOTAL EXENTO IVA", detalle));
                    lblSubTotalExcentoIVA.Padding = 2f;
                    var SubTotalExcentoIVA = new PdfPCell(new Paragraph(dSubtotalExcento == 0 ? "0.00" : dSubtotalExcento.ToString(nfi), detalle));
                    SubTotalExcentoIVA.Padding = 2f;
                    SubTotalExcentoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTO", detalle));
                    lblSubTotalSinImpuestos.Padding = 2f;
                    var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/totalSinImpuestos").InnerText, detalle));
                    SubTotalSinImpuestos.Padding = 2f;
                    SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblDescuento = new PdfPCell(new Paragraph("TOTAL DESCUENTO", detalle));
                    lblDescuento.Padding = 2f;
                    var TotalDescuento = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/totalDescuento") == null ? "0.00" : oDocument.SelectSingleNode("//infoNotaCredito/totalDescuento").InnerText, detalle));
                    TotalDescuento.Padding = 2f;
                    TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVA12 = new PdfPCell(new Paragraph("IVA " + IVAride.Replace(".00", "") + "%", detalle));
                    lblIVA12.Padding = 2f;
                    var IVA12 = new PdfPCell(new Paragraph(dIVA12.ToString(nfi), detalle));
                    IVA12.Padding = 2f;
                    IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    var lblICE = new PdfPCell(new Paragraph("ICE:", detalle));
                    lblICE.Padding = 2f;
                    var ICE = new PdfPCell(new Paragraph(dICE == 0 ? "0.00" : dICE.ToString(nfi), detalle));
                    ICE.Padding = 2f;
                    ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    var lblIRBPNR = new PdfPCell(new Paragraph("IRBPNR:", detalle));
                    lblIRBPNR.Padding = 2f;
                    var IRBPNR = new PdfPCell(new Paragraph(dIRBPNR == 0 ? "0.00" : dIRBPNR.ToString(nfi), detalle));
                    IRBPNR.Padding = 2f;
                    IRBPNR.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                 
                    var lblValorTotal = new PdfPCell(new Paragraph("VALOR TOTAL", detalle));
                    lblValorTotal.Padding = 2f;
                    var ValorTotal = new PdfPCell(new Paragraph("$ " + oDocument.SelectSingleNode("//infoNotaCredito/valorModificacion").InnerText, detalle));
                    ValorTotal.Padding = 2f;
                    ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    decimal decTotal = decimal.Parse(oDocument.SelectSingleNode("//infoNotaCredito/valorModificacion").InnerText, new CultureInfo(Cultura));

                    var lblServicio = new PdfPCell(new Paragraph("SERVICIO 10%", detalle));
                    lblValorTotal.Padding = 2f;
                    var Servicio = new PdfPCell(new Paragraph(dpropina.ToString(nfi), detalle));
                    Servicio.Padding = 2f;
                    Servicio.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    tableTotales.AddCell(lblSubTotal12);
                    tableTotales.AddCell(SubTotal12);
                    tableTotales.AddCell(lblSubTotal0);
                    tableTotales.AddCell(SubTotal0);
                    tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                    tableTotales.AddCell(SubTotalNoSujetoIVA);
                    tableTotales.AddCell(lblSubTotalExcentoIVA);
                    tableTotales.AddCell(SubTotalExcentoIVA);
                    tableTotales.AddCell(lblSubTotalSinImpuestos);
                    tableTotales.AddCell(SubTotalSinImpuestos);
                    tableTotales.AddCell(lblDescuento);
                    tableTotales.AddCell(TotalDescuento);
                    tableTotales.AddCell(lblIVA12);
                    tableTotales.AddCell(IVA12);
                    tableTotales.AddCell(lblICE);
                    tableTotales.AddCell(ICE);
                    tableTotales.AddCell(lblValorTotal);

                    if (oDocument.SelectNodes("//compensaciones/compensacion").Count > 0)
                    {

                        XmlNodeList CompensacionesSolidaria;
                        CompensacionesSolidaria = oDocument.SelectNodes("//compensaciones/compensacion");
                        decimal tcompensa = 0;
                        foreach (XmlNode compensacion in CompensacionesSolidaria)
                        {
                            decimal valor = decimal.Round(decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura)), 2);
                            tcompensa = tcompensa + valor;
                        }

                        decimal nuevovalor = 0;
                        nuevovalor = decimal.Round(decimal.Parse(oDocument.SelectSingleNode("//infoNotaCredito/valorModificacion").InnerText, new CultureInfo(Cultura)), 2) + tcompensa;
                        ValorTotal = new PdfPCell(new Paragraph(nuevovalor.ToString(nfi), detalle));
                        ValorTotal.Padding = 2f;
                        ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        tableTotales.AddCell(ValorTotal);

                        foreach (XmlNode compensacion in CompensacionesSolidaria)
                        {
                            // PdfPCell lblCompensacionSolidaria;
                            PdfPCell CompensacionSolidaria;
                            //buscar descripcion de forma de pago

                            string datocodi = compensacion["codigo"].InnerText;
                            string descompensacion = "";
                            try
                            {
                                descompensacion = System.Configuration.ConfigurationManager.AppSettings["CS" + datocodi];//Descripcion de la compensacion parametrizada en el webconfig                                      
                            }
                            catch (Exception excomp) { }

                            var lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "%", detalle));
                            if (datocodi.Equals("1"))
                            {
                                lblCompensacionSolidaria = new PdfPCell(new Paragraph("(-) " + descompensacion + " " + compensacion["tarifa"].InnerText.Replace(".00", "") + "% IVA", detalle));
                            }
                            lblCompensacionSolidaria.Padding = 2f;

                            var ValorCompesa = decimal.Parse(compensacion["valor"].InnerText, new CultureInfo(Cultura));
                            CompensacionSolidaria = new PdfPCell(new Paragraph(decimal.Round(ValorCompesa, 2).ToString(nfi), detalle));
                            CompensacionSolidaria.Padding = 2f;
                            CompensacionSolidaria.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            if (ValorCompesa > 0)
                            {
                                tableTotales.AddCell(lblCompensacionSolidaria);
                                tableTotales.AddCell(CompensacionSolidaria);
                            }
                        }


                        var lblValorPagado = new PdfPCell(new Paragraph("VALOR A PAGAR", detalle));
                        lblValorTotal.Padding = 2f;
                        var ValorPagado = new PdfPCell(new Paragraph(decimal.Round(decTotal, 2).ToString(nfi), detalle));
                        ValorPagado.Padding = 2f;
                        ValorPagado.HorizontalAlignment = Rectangle.ALIGN_RIGHT;


                        tableTotales.AddCell(lblValorPagado);
                        tableTotales.AddCell(ValorPagado);

                    }
                    else
                    {
                        tableTotales.AddCell(ValorTotal);
                    }
                    if (dpropina > 0)
                    {
                        tableTotales.AddCell(lblServicio);
                        tableTotales.AddCell(Servicio);
                    }
                    #endregion

                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;

                    var aux = 10;
                    posDetalleCliente = 785 - ContenedorD.TotalHeight - aux;
                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;

                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;

                    if (DifTableL < posDetalleCliente)
                    {
                        aumento = posDetalleCliente - DifTableL + 20;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleCliente) + 5;
                    }
                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);
                    posDetalleFactura = (posDetalleCliente - 8) - tableNotaCredito.TotalHeight;

                    tableNotaCredito.WriteSelectedRows(0, 10, 28, posDetalleCliente - aumento, canvas);

                    int publicadas = 0;
                    Boolean FueSaltoPagina = false;
                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        var aux1 = 10;
                        tableDetalleNotaCredito.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - aux1 - tableNotaCredito.TotalHeight - aumento, canvas);
                       
                        if (aumento == 0)
                        {
                            posInfoAdicional = (posDetalleFactura) - (tableDetalleNotaCredito.TotalHeight + 15);
                        }
                        else
                        {
                            posInfoAdicional = (posDetalleFactura) - (tableDetalleNotaCredito.TotalHeight + 40);
                        }
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional - tableFormaPago.TotalHeight, canvas);
                        tableTotales.WriteSelectedRows(0, 15, 388, posInfoAdicional - tableFormaPago.TotalHeight, canvas);
                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        tableDetalleNotaCredito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight - aumento, canvas);
                        documento.NewPage();

                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806 - aumento, canvas);
                        tableTotales.WriteSelectedRows(0, 12, 408, 806 - aumento, canvas);
                        Console.WriteLine(tableDetalleNotaCredito.TotalHeight);  // 513
                    }
                    else
                    {
                        tableDetalleNotaCredito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight - aumento, canvas);
                        documento.NewPage();

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0, desde = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i <= Paginas; i++)
                        {
                            if (i > 0)
                            {
                                posInicial = 0;
                                tableDetalleNotaCredito.WriteSelectedRows(desde, hasta, 28, 806 - aumento, canvas);
                                publicadas = hasta - desde;
                                if (i != Paginas)
                                {
                                    desde = hasta + 1;
                                    hasta = desde + MaxSoloPagina;
                                    if (hasta > Detalles.Count)
                                    {
                                        hasta = Detalles.Count;
                                    }
                                    if ((hasta - desde) < MaxSoloPagina)
                                    {
                                        if (publicadas == MaxSoloPagina)
                                        {
                                            documento.NewPage();
                                            FueSaltoPagina = true;
                                        }
                                        else
                                        {
                                            if ((hasta - desde) <= 50)
                                            {
                                                FueSaltoPagina = false;
                                            }
                                            else
                                            {
                                                writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                                                documento.NewPage();
                                                FueSaltoPagina = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        documento.NewPage();
                                        FueSaltoPagina = true;
                                    }
                                }
                                else
                                {
                                    if ((hasta - desde) <= 50)
                                    {
                                        FueSaltoPagina = false;
                                    }
                                }
                            }
                            else
                            {
                                if (Detalles.Count <= 23)
                                { //entonces
                                    tableDetalleNotaCredito.WriteSelectedRows(0, 23, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight - aumento, canvas);
                                    publicadas = 23;
                                    desde = 24;
                                }
                                else if ((Detalles.Count >= 35))
                                {
                                    tableDetalleNotaCredito.WriteSelectedRows(0, 35, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight - aumento, canvas);
                                    publicadas = 35;
                                    desde = 36;
                                    documento.NewPage();
                                    hasta = desde + MaxSoloPagina;
                                    if (hasta > Detalles.Count)
                                    {
                                        hasta = Detalles.Count;
                                        FueSaltoPagina = true;
                                    }
                                }
                            }
                        }
                        if (FueSaltoPagina)
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                            tableTotales.WriteSelectedRows(0, 12, 400, 806, canvas);

                        }
                        else
                        {
                            posInicial = 810 - ((hasta - desde)) * 11;
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial - 10, canvas);
                            tableTotales.WriteSelectedRows(0, 12, 400, posInicial - 10, canvas);
                        }
                        Console.WriteLine(writer.PageNumber);
                    }

                    if (isTransportista)
                    {
                        float TotalHeight = (tableInfoAdicional.TotalHeight + tableFormaPago.TotalHeight) * 2;
                        tableInfoTransportista.WriteSelectedRows(0, 17, 28, posDetalleFactura - TotalHeight - 30 - aumento, canvas);
                    }

                    writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Bytes;
        }

        /// <summary>
        /// Método para generación de nota de débito RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraNotaDebito(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US", string direccion = "", string direccionCliente = "")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            iTextSharp.text.Font detAdicional = GetArial(6);

            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "",
                           sAmbienteVal = "", sNumAutorizacion,
                           sObligadoContabilidad = "",
                           sComprobanteModificacion = "", RegMicroEmp = "", rimpe = "";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";


                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = (oDocument.SelectSingleNode("//infoNotaDebito/contribuyenteEspecial") == null) ? "" : oDocument.SelectSingleNode("//infoNotaDebito/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoNotaDebito/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoNotaDebito/obligadoContabilidad").InnerText;
                    sComprobanteModificacion = oDocument.SelectSingleNode("//infoNotaDebito/codDocModificado") == null ? "" : oDocument.SelectSingleNode("//infoNotaDebito/codDocModificado").InnerText;

                    try
                    {
                        rimpe = oDocument.SelectSingleNode("//infoTributaria/contribuyenteRimpe").InnerText;
                    }
                    catch (Exception)
                    {
                    }


                    bool Excep = false;
                    bool microEmpInfoAdicional = false;
                    try
                    {
                        XmlNodeList IARegMicro = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                        foreach (XmlNode campoAdicional in IARegMicro)
                        {
                            if (campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas")
                            {
                                RegMicroEmp = "si";
                                microEmpInfoAdicional = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Excep = true;
                    }

                    if (!Excep)
                    {
                        if (!microEmpInfoAdicional)
                        {
                            try
                            {
                                string micoEmpe = oDocument.SelectSingleNode("//infoTributaria/regimenMicroempresas").InnerText;
                                if (!string.IsNullOrEmpty(micoEmpe))
                                {
                                    RegMicroEmp = "si";
                                }
                            }
                            catch (Exception)
                            {
                                Excep = true;
                            }
                        }
                    }


                    switch (sComprobanteModificacion)
                    {
                        case "01":
                            sComprobanteModificacion = "FACTURA: ";
                            break;
                        case "04":
                            sComprobanteModificacion = "NOTA DE CRÉDITO: ";
                            break;
                        case "05":
                            sComprobanteModificacion = "NOTA DE DÉBITO: ";
                            break;
                        case "06":
                            sComprobanteModificacion = "GUÍA DE REMISIÓN: ";
                            break;
                        case "07":
                            sComprobanteModificacion = "COMPROBANTE DE RETENCIÓN: ";
                            break;
                        default:
                            break;
                    }

                    int registros = 0;
                    int PagLimite1 = 30;
                    int MaxPagina1 = 39;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;

                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();
                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeAltoVariable = 0D;
                    var porcentajeAnchoVariable = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;

                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);


                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));



                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {
                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {
                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);
                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    RUC.PaddingTop = 10f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("N O T A  D E  D É B I T O ", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    lblNumAutorizacion.PaddingTop = 10f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    NumAutorizacion.PaddingTop = 10f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA DE AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    FechaAutorizacion.PaddingBottom = 10f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    Ambiente.PaddingBottom = 10f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    Emision.PaddingBottom = 10f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    Ambiente.PaddingBottom = 10f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);
                    
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                RazonSocial.FixedHeight = 20f;
                            }
                        }
                        else
                        {
                            RazonSocial.FixedHeight = 20f;
                        }
                    }
                    else
                    {
                        RazonSocial.FixedHeight = 20f;
                    }
                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dirección Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 3f;


                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dirección Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 3f;
                        DirSucursal.PaddingTop = 3f;
                        if (!Excep)
                        {
                            if (!string.IsNullOrEmpty(RegMicroEmp))
                            {
                                if (!RegMicroEmp.ToLower().Equals("si"))
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }

                        innerTableL.AddCell(DirSucursal);
                    }

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 3f;
                    ContribuyenteEspecial.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ContribuyenteEspecial.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ContribuyenteEspecial.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ContribuyenteEspecial.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    ObligadoContabilidad.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ObligadoContabilidad.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ObligadoContabilidad.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ObligadoContabilidad.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ObligadoContabilidad);


                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (RegMicroEmp.ToLower().Equals("si"))
                            {
                                PdfPCell RegimenMicroEmpresa = new PdfPCell(new Paragraph("Contribuyente Régimen Microempresas.", cabecera));
                                RegimenMicroEmpresa.Border = Rectangle.NO_BORDER;
                                RegimenMicroEmpresa.Padding = 5f;
                                RegimenMicroEmpresa.PaddingBottom = 3f;
                                RegimenMicroEmpresa.PaddingTop = 3f;
                                innerTableL.AddCell(RegimenMicroEmpresa);
                            }
                        }
                    }

                    bool agenteRetencionInfoA = false;
                    XmlNodeList IA = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    foreach (XmlNode campoAdicional in IA)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "Agente")
                        {
                            PdfPCell AgenteRetencion = new PdfPCell(new Paragraph(campoAdicional.InnerText, cabecera));
                            AgenteRetencion.Border = Rectangle.NO_BORDER;
                            AgenteRetencion.Padding = 5f;
                            AgenteRetencion.PaddingBottom = 4f;
                            AgenteRetencion.PaddingTop = 4f;
                            innerTableL.AddCell(AgenteRetencion);
                            agenteRetencionInfoA = true;
                        }
                    }

                    try
                    {
                        string agente = oDocument.SelectSingleNode("//infoTributaria/agenteRetencion").InnerText;
                        if (!string.IsNullOrEmpty(agente))
                        {
                            if (!agenteRetencionInfoA)
                            {
                                agente = agente.TrimStart(new Char[] { '0' });
                                PdfPCell AgenteRetencion = new PdfPCell(new Paragraph("Agente de Retención Resolución No." + agente, cabecera));
                                AgenteRetencion.Border = Rectangle.NO_BORDER;
                                AgenteRetencion.Padding = 5f;
                                innerTableL.AddCell(AgenteRetencion);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        if (!string.IsNullOrEmpty(rimpe))
                        {
                            try
                            {
                                PdfPCell rimpe_ = new PdfPCell(new Paragraph(rimpe, cabecera));
                                rimpe_.Border = Rectangle.NO_BORDER;
                                rimpe_.Padding = 5f;
                                rimpe_.PaddingBottom = 3f;
                                rimpe_.PaddingTop = 3f;
                                innerTableL.AddCell(rimpe_);
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }
                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);
                    
                    #endregion

                    #region Logo
                    BaseColor color = null;
                    iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpgPrueba);
                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion
                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);
                    #region CabeceraNotaDebito
                    PdfPTable tableNotaDebito = new PdfPTable(4);
                    tableNotaDebito.TotalWidth = 540f;
                    tableNotaDebito.WidthPercentage = 100;
                    float[] DetalleNotaCreditoWidths = new float[] { 30f, 120f, 30f, 40f };
                    tableNotaDebito.SetWidths(DetalleNotaCreditoWidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableNotaDebito.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/razonSocialComprador").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableNotaDebito.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("Identificación:", detalle));
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableNotaDebito.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/identificacionComprador").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableNotaDebito.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                    tableNotaDebito.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.RIGHT_BORDER;
                    FechaEmisionCliente.Colspan = 3;
                    tableNotaDebito.AddCell(FechaEmisionCliente);

                    var lblDireccion = new PdfPCell(new Paragraph("Dirección:", detalle));
                    lblDireccion.Border = Rectangle.LEFT_BORDER;
                    tableNotaDebito.AddCell(lblDireccion);

                    var Direccion = new PdfPCell(new Paragraph(direccionCliente == null ? "" : direccionCliente, detalle));
                    Direccion.Border = Rectangle.RIGHT_BORDER;
                    lblFechaEmisionCliente.PaddingBottom = 8f;
                    Direccion.Colspan = 4;
                    tableNotaDebito.AddCell(Direccion);

                    var lblDocModifica = new PdfPCell(new Paragraph("Comprobante que se modifica: ", detalle));
                    lblDocModifica.Border = Rectangle.LEFT_BORDER;
                    lblDocModifica.PaddingTop = 5f;
                    tableNotaDebito.AddCell(lblDocModifica);

                    var DocModifica = new PdfPCell(new Paragraph(sComprobanteModificacion + oDocument.SelectSingleNode("//infoNotaDebito/numDocModificado").InnerText, detalle));
                    DocModifica.PaddingTop = 5f;
                    DocModifica.Colspan = 3;
                    DocModifica.Border = Rectangle.RIGHT_BORDER;
                    tableNotaDebito.AddCell(DocModifica);

                    var lblFechaEmision = new PdfPCell(new Paragraph("Fecha de emisión (Comprobante a modificar): ", detalle));
                    lblFechaEmision.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblFechaEmision.PaddingBottom = 5f;
                    tableNotaDebito.AddCell(lblFechaEmision);

                    var FechaEmision = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/fechaEmisionDocSustento").InnerText, detalle));
                    FechaEmision.Colspan = 3;
                    FechaEmisionCliente.PaddingBottom = 5f;
                    FechaEmision.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableNotaDebito.AddCell(FechaEmision);

                    #endregion

                    #region DetalleNotaDebito
                    PdfPTable tableDetalleNotaDebito = new PdfPTable(2);

                    tableDetalleNotaDebito.TotalWidth = 540f;
                    tableDetalleNotaDebito.WidthPercentage = 100;
                    tableDetalleNotaDebito.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 90f, 40f };
                    tableDetalleNotaDebito.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    var encCodPrincipal = new PdfPCell(new Paragraph("Razón de la Modificación", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCodAuxiliar = new PdfPCell(new Paragraph("Valor de la Modificación", fontEncabezado));
                    encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;


                    tableDetalleNotaDebito.AddCell(encCodPrincipal);
                    tableDetalleNotaDebito.AddCell(encCodAuxiliar);

                    PdfPCell DetRazonModificacion;
                    PdfPCell DetValorModificacion;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//motivos/motivo");
                    registros = Detalles.Count;

                    foreach (XmlNode Elemento in Detalles)
                    {
                        DetRazonModificacion = new PdfPCell(new Phrase(Elemento["razon"].InnerText, detalle));
                        DetRazonModificacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        DetValorModificacion = new PdfPCell(new Phrase(Elemento["valor"].InnerText, detalle));
                        DetValorModificacion.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        tableDetalleNotaDebito.AddCell(DetRazonModificacion);
                        tableDetalleNotaDebito.AddCell(DetValorModificacion);

                    }
                    #endregion

                    #region FormaPagos
                    var tableFormaPago = new PdfPTable(2);
                    tableFormaPago.TotalWidth = 250f;
                    float[] FormaPagosWidths = new float[] { 175f, 75f };
                    tableFormaPago.SetWidths(FormaPagosWidths);

                    var lblFormaPago = new PdfPCell(new Paragraph("Forma de Pago", fontEncabezado));
                    lblFormaPago.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var lblFPvalor = new PdfPCell(new Paragraph("Valor", fontEncabezado));
                    lblFPvalor.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblFPvalor.Padding = 2f;

                    var lblBottomfp = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottomfp.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottomfp.Padding = 2f;
                    var Bottomfp = new PdfPCell(new Paragraph("  ", detalle));
                    Bottomfp.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottomfp.Padding = 2f;

                    XmlNodeList FormaPagos;
                    FormaPagos = oDocument.SelectNodes("//pagos/pago");

                    int banderapagos = 0;
                    XmlNodeList InfoAdicionalaux;
                    InfoAdicionalaux = oDocument.SelectNodes("//infoAdicional/campoAdicional");
                    foreach (XmlNode pago in FormaPagos)
                    {
                        //buscar descripcion de forma de pago

                        string datocodi = pago["formaPago"].InnerText;
                        string descripcifromapg = "";

                        if (banderapagos == 0)
                        {
                            tableFormaPago.AddCell(lblFormaPago);
                            tableFormaPago.AddCell(lblFPvalor);
                            
                            banderapagos = 1;
                        }

                        try
                        {
                            descripcifromapg = System.Configuration.ConfigurationManager.AppSettings["F" + datocodi];//Descripcion de la forma de pago parametrizada en el webconfig                        
                        }
                        catch (Exception exfpg) { }

                        var lblFormaPagoDes = new PdfPCell(new Paragraph(descripcifromapg.ToUpper(), detalle));
                        lblFormaPagoDes.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;//Rectangle.LEFT_BORDER;
                        lblFormaPagoDes.HorizontalAlignment = Rectangle.ALIGN_LEFT;

                        var MontoFp = new PdfPCell(new Paragraph(decimal.Round(decimal.Parse(pago["total"].InnerText, new CultureInfo(Cultura)), 2).ToString(nfi), detalle));
                        MontoFp.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER; ;
                        MontoFp.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        tableFormaPago.AddCell(lblFormaPagoDes);
                        tableFormaPago.AddCell(MontoFp);
                    }

                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (!(campoAdicional.Attributes["nombre"].Value == "Agente"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas"))
                        {
                            lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;

                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    #region Totales

                    var tableTotales = new PdfPTable(2);
                    tableTotales.TotalWidth = 160f;
                    float[] InfoTotales = new float[] { 105f, 55f };
                    tableTotales.SetWidths(InfoTotales);

                    XmlNodeList Impuestos;
                    Impuestos = oDocument.SelectNodes("//infoNotaDebito/impuestos/impuesto");
                    decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0;
                    foreach (XmlNode Impuesto in Impuestos)
                    {
                        switch (Impuesto["codigo"].InnerText)
                        {
                            case "2":        // IVA
                                if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                                {
                                    dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // No objeto de Impuesto
                                {
                                    dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                break;
                            case "3":       // ICE
                                dICE = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                break;
                        }
                    }
                    var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL 12.00%:", detalle));
                    lblSubTotal12.Padding = 2f;
                    var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12.ToString(nfi), detalle));
                    SubTotal12.Padding = 2f;
                    SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 00.00%:", detalle));
                    lblSubTotal0.Padding = 2f;
                    var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0.ToString(nfi), detalle));
                    SubTotal0.Padding = 2f;
                    SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL No sujeto de IVA:", detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI.ToString(nfi), detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS:", detalle));
                    lblSubTotalSinImpuestos.Padding = 2f;
                    var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/totalSinImpuestos").InnerText, detalle));
                    SubTotalSinImpuestos.Padding = 2f;
                    SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblDescuento = new PdfPCell(new Paragraph("DESCUENTO:", detalle));
                    lblDescuento.Padding = 2f;
                    var TotalDescuento = new PdfPCell(new Paragraph((oDocument.SelectSingleNode("//infoNotaDebito/totalDescuento") == null ? "0.00" : oDocument.SelectSingleNode("//infoNotaDebito/totalDescuento").InnerText), detalle));
                    TotalDescuento.Padding = 2f;
                    TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblICE = new PdfPCell(new Paragraph("ICE:", detalle));
                    lblICE.Padding = 2f;
                    var ICE = new PdfPCell(new Paragraph(dICE.ToString(nfi), detalle));
                    ICE.Padding = 2f;
                    ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVA12 = new PdfPCell(new Paragraph("IVA 12.00%:", detalle));
                    lblIVA12.Padding = 2f;
                    var IVA12 = new PdfPCell(new Paragraph(dIVA12.ToString(nfi), detalle));
                    IVA12.Padding = 2f;
                    IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblValorTotal = new PdfPCell(new Paragraph("VALOR TOTAL:", detalle));
                    lblValorTotal.Padding = 2f;
                    var ValorTotal = new PdfPCell(new Paragraph("$ " + oDocument.SelectSingleNode("//infoNotaDebito/valorTotal").InnerText, detalle));
                    ValorTotal.Padding = 2f;
                    ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    tableTotales.AddCell(lblSubTotal12);
                    tableTotales.AddCell(SubTotal12);
                    tableTotales.AddCell(lblSubTotal0);
                    tableTotales.AddCell(SubTotal0);
                    tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                    tableTotales.AddCell(SubTotalNoSujetoIVA);
                    tableTotales.AddCell(lblSubTotalSinImpuestos);
                    tableTotales.AddCell(SubTotalSinImpuestos);
                    tableTotales.AddCell(lblDescuento);
                    tableTotales.AddCell(TotalDescuento);
                    tableTotales.AddCell(lblIVA12);
                    tableTotales.AddCell(IVA12);
                    tableTotales.AddCell(lblValorTotal);
                    tableTotales.AddCell(ValorTotal);

                    #endregion

                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;

                    posDetalleCliente = 785 - ContenedorD.TotalHeight - 5;
                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;

                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;

                    if (DifTableL < posDetalleCliente)
                    {
                        aumento = posDetalleCliente - DifTableL + 10;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleCliente) + 5;
                    }
                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);
                    posDetalleFactura = (posDetalleCliente - 8) - tableNotaDebito.TotalHeight;

                    tableNotaDebito.WriteSelectedRows(0, 10, 28, posDetalleCliente - aumento, canvas);

                    Boolean FueSaltoPagina = false;

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        tableDetalleNotaDebito.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight - aumento, canvas);
                        posDetalleFactura = (posDetalleFactura - 10) - tableDetalleNotaDebito.TotalHeight;
                        posInfoAdicional = (posDetalleFactura - 10) - tableFormaPago.TotalHeight;
                        tableFormaPago.WriteSelectedRows(0, 17, 28, posDetalleFactura - aumento, canvas);
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional - aumento, canvas);
                        tableTotales.WriteSelectedRows(0, 10, 408, posDetalleFactura - aumento, canvas);
                        Console.WriteLine(tableDetalleNotaDebito.TotalHeight);  // 403

                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));

                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0, desde = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = 23;
                        faltantes = registros - MaxPagina1 + 1;
                        faltantes = Detalles.Count;
                        int publicadas = 0;
                        for (int i = 0; i <= Paginas; i++)
                        {
                            if (i > 0)
                            {
                                posInicial = 0;
                                tableDetalleNotaDebito.WriteSelectedRows(desde, hasta, 28, 806 - aumento, canvas);
                                publicadas = hasta - desde;
                                if (i != Paginas)
                                {
                                    desde = hasta + 1;
                                    hasta = desde + MaxSoloPagina;
                                    if (hasta > Detalles.Count)
                                    {
                                        hasta = Detalles.Count;
                                    }
                                    if ((hasta - desde) < MaxSoloPagina)
                                    {
                                        if (publicadas == MaxSoloPagina)
                                        {
                                            documento.NewPage();
                                            FueSaltoPagina = true;
                                        }
                                        else
                                        {
                                            if ((hasta - desde) <= 50)
                                            {
                                                FueSaltoPagina = false;
                                            }
                                            else
                                            {
                                                writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                                                documento.NewPage();
                                                FueSaltoPagina = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        documento.NewPage();
                                        FueSaltoPagina = true;
                                    }
                                }
                                else
                                {
                                    if ((hasta - desde) <= 50)
                                    {
                                        FueSaltoPagina = false;
                                    }
                                }
                            }
                            else
                            {
                                if (Detalles.Count <= 23)
                                { //entonces
                                    tableDetalleNotaDebito.WriteSelectedRows(0, 23, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight - aumento, canvas);
                                    publicadas = 23;
                                    desde = 24;
                                }
                                else if ((Detalles.Count >= 35))
                                {
                                    tableDetalleNotaDebito.WriteSelectedRows(0, 35, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight - aumento, canvas);
                                    publicadas = 35;
                                    desde = 36;
                                    documento.NewPage();
                                    hasta = desde + MaxSoloPagina;
                                    if (hasta > Detalles.Count)
                                    {
                                        hasta = Detalles.Count;
                                    }
                                }
                            }
                        }

                        if (FueSaltoPagina)
                        {
                            tableFormaPago.WriteSelectedRows(0, 17, 28, 806, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806 - tableFormaPago.TotalHeight, canvas);
                            tableTotales.WriteSelectedRows(0, 10, 408, 806, canvas);
                        }
                        else
                        {
                            posInicial = 810 - ((hasta - desde)) * 11;
                            tableFormaPago.WriteSelectedRows(0, 17, 28, posInicial - 10 - aumento, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial - 10 - tableFormaPago.TotalHeight - aumento, canvas);
                            tableTotales.WriteSelectedRows(0, 10, 408, posInicial - 10 - aumento, canvas);
                        }

                    }
                    writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }

            }

            catch (Exception ex)
            {

            }
            return Bytes;
        }
        /// <summary>
        /// Método para generación de comprobante de retención RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraCompR2(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US", string direccion = "", string direccionCliente = "")
        {
            MemoryStream ms = null;
            byte[] BytesAux = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "", rimpe = "",
                           sAmbienteVal = "", sEjercicioFiscal = "", sNumAutorizacion = "", sObligadoContabilidad = "", RegMicroEmp = "";
                    Cultura = pCultura;
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);
                    //version = oDocument.SelectSingleNode("//version").InnerText;
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    sAmbiente = oDocument.SelectSingleNode("//ambiente") == null ? sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN" : oDocument.SelectSingleNode("//ambiente").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial").InnerText;
                    sEjercicioFiscal = oDocument.SelectSingleNode("//infoCompRetencion/periodoFiscal").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad").InnerText;

                    bool Excep = false;
                    bool microEmpInfoAdicional = false;

                    try
                    {
                        XmlNodeList IARegMicro = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                        foreach (XmlNode campoAdicional in IARegMicro)
                        {
                            if (campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas")
                            {
                                RegMicroEmp = "si";
                                microEmpInfoAdicional = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Excep = true;
                    }

                    if (!Excep)
                    {
                        if (!microEmpInfoAdicional)
                        {
                            try
                            {
                                string micoEmpe = oDocument.SelectSingleNode("//infoTributaria/regimenMicroempresas").InnerText;
                                if (!string.IsNullOrEmpty(micoEmpe))
                                {

                                    RegMicroEmp = "si";

                                }
                            }
                            catch (Exception)
                            {
                                Excep = true;
                            }
                        }
                    }


                    int registros = 0;
                    int PagLimite1 = 35;
                    int MaxPagina1 = 45;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    int NumRegistrosxPagina = 0;
                    Boolean PrimeraPagina = true;
                    float LimitePagina = 0;

                    List<TableDetalleComprobante> ListDetalle = new List<TableDetalleComprobante>();

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                    iTextSharp.text.Font detAdicional = GetArial(6);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;

                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();
                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;

                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);


                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));
                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {

                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {

                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);
                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("C O M P R O B A N T E  D E  R E T E N C I Ó N", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA DE AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);

                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 4f;
                    RazonSocial.FixedHeight = 18f;
                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dirección Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 3f;
                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dirección Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 3f;
                        DirSucursal.PaddingTop = 3f;
                        innerTableL.AddCell(DirSucursal);
                    }


                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 3f;
                    ContribuyenteEspecial.PaddingTop = 3f;
                    ContribuyenteEspecial.FixedHeight = 18f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    ObligadoContabilidad.PaddingTop = 3f;
                    ObligadoContabilidad.FixedHeight = 18f;
                    innerTableL.AddCell(ObligadoContabilidad);

                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 250f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);

                    #endregion

                    #region Logo
                    BaseColor color = null;
                    iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpgPrueba);

                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);

                    #region DetalleCliente
                    PdfPTable tableDetalleCliente = new PdfPTable(4);
                    tableDetalleCliente.TotalWidth = 540f;
                    tableDetalleCliente.WidthPercentage = 100;
                    float[] DetalleClientewidths = new float[] { 30f, 120f, 30f, 40f };
                    tableDetalleCliente.SetWidths(DetalleClientewidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/razonSocialSujetoRetenido").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("Identificación:", detalle)); // Cambio soliciato por BB Identificación
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/identificacionSujetoRetenido").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(FechaEmisionCliente);
                    var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    lblGuiaRemision.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblGuiaRemision);
                    var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    GuiaRemision.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(GuiaRemision);
                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(8);

                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    tableDetalleFactura.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    PdfPTable tableDetalleComprobanteRetencionPagina = new PdfPTable(8);
                    tableDetalleComprobanteRetencionPagina.TotalWidth = 540f;
                    tableDetalleComprobanteRetencionPagina.WidthPercentage = 100;
                    tableDetalleComprobanteRetencionPagina.LockedWidth = true;
                    float[] DetalleComprobanteRetencionWidthsPagina = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                    tableDetalleComprobanteRetencionPagina.SetWidths(DetalleComprobanteRetencionWidthsPagina);


                    var fontEncabezado = GetArial(7);
                    var encComprobante = new PdfPCell(new Paragraph("Comprobante", fontEncabezado));
                    encComprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encNumero = new PdfPCell(new Paragraph("Número", fontEncabezado));
                    encNumero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encFechaEmision = new PdfPCell(new Paragraph("Fecha Emisión", fontEncabezado));
                    encFechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encEjercicioFiscal = new PdfPCell(new Paragraph("Ejercicio Fiscal", fontEncabezado));
                    encEjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encBaseImponible = new PdfPCell(new Paragraph("Base Imponible para la Retención", fontEncabezado));
                    encBaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encImpuesto = new PdfPCell(new Paragraph("IMPUESTO", fontEncabezado));
                    encImpuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPorcentajeRetencion = new PdfPCell(new Paragraph("Porcentaje Retención", fontEncabezado));
                    encPorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Valor Retenido", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encComprobante);
                    tableDetalleFactura.AddCell(encNumero);
                    tableDetalleFactura.AddCell(encFechaEmision);
                    tableDetalleFactura.AddCell(encEjercicioFiscal);
                    tableDetalleFactura.AddCell(encBaseImponible);
                    tableDetalleFactura.AddCell(encImpuesto);
                    tableDetalleFactura.AddCell(encPorcentajeRetencion);
                    tableDetalleFactura.AddCell(encPrecioUnitario);

                    PdfPCell Comprobante = null;
                    PdfPCell Numero;
                    PdfPCell FechaEmision;
                    PdfPCell EjercicioFiscal;
                    PdfPCell BaseImponible;
                    PdfPCell Impuesto;
                    PdfPCell PorcentajeRetencion;
                    PdfPCell PrecioUnitario;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//docsSustento/docSustento");
                    registros = Detalles.Count;

                    Dictionary<int, string> dictionary = new Dictionary<int, string>();
                    dictionary.Add(1, "Factura");
                    dictionary.Add(2, "Nota o boleta de venta");
                    dictionary.Add(3, "Liquidación de compra de Bienes o Prestación de servicios");
                    dictionary.Add(4, "Nota de crédito");
                    dictionary.Add(5, "Nota de débito");
                    dictionary.Add(6, "Guías de Remisión");
                    dictionary.Add(7, "Comprobante de Retención");
                    dictionary.Add(8, "Boletos o entradas a espectáculos públicos");
                    dictionary.Add(9, "Tiquetes o vales emitidos por máquinas registradoras");
                    dictionary.Add(11, "Pasajes expedidos por empresas de aviación");
                    dictionary.Add(12, "Documentos emitidos por instituciones financieras");
                    dictionary.Add(15, "Comprobante de venta emitido en el Exterior");
                    dictionary.Add(16, "Formulario Único de Exportación (FUE) o Declaración Aduanera Única(DAU) o Declaración Andina de Valor(DAV)");
                    dictionary.Add(18, "Documentos autorizados utilizados en ventasexcepto N/C  N/D ");
                    dictionary.Add(19, "Comprobantes de Pago de Cuotas o Aportes");
                    dictionary.Add(20, "Documentos por Servicios Administrativos emitidos por Inst.del Estado");
                    dictionary.Add(21, "Carta de Porte Aéreo");
                    dictionary.Add(22, "RECAP");
                    dictionary.Add(23, "Nota de Crédito TC");
                    dictionary.Add(24, "Nota de Débito TC");
                    dictionary.Add(41, "Comprobante de venta emitido por reembolso");
                    dictionary.Add(42, "Documento agente de retención Presuntiva");
                    dictionary.Add(43, "Liquidación para Explotación y Exploracion de Hidrocarburos");
                    dictionary.Add(44, "Comprobante de Contribuciones y Aportes");
                    dictionary.Add(45, "Liquidación por reclamos de aseguradoras");
                    dictionary.Add(47, "Nota de Crédito por Reembolso Emitida por Intermediario");
                    dictionary.Add(48, "Nota de Débito por Reembolso Emitida por Intermediario");
                    dictionary.Add(49, "Proveedor Directo de Exportador Bajo Régimen Especial");
                    dictionary.Add(50, "A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(51, "N/C A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(52, "N/D A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(294, "Liquidación de compra de Bienes Muebles Usados");
                    dictionary.Add(344, "Liquidación de compra de vehículos usados");

                    Decimal TotalRet = 0;

                    //seccion para los campos de cabecera

                    NumRegistrosxPagina = 1;

                    foreach (XmlNode Elemento in Detalles)
                    {
                        XmlNodeList Detalles1;
                        Detalles1 = Elemento.SelectNodes("retenciones/retencion");
                        registros = Detalles1.Count;


                        foreach (XmlNode Elemento1 in Detalles1)
                        {
                            NumRegistrosxPagina = NumRegistrosxPagina + 1;
                            tableDetalleComprobanteRetencionPagina = new PdfPTable(8);
                            if (Elemento["codDocSustento"] != null)
                            {
                                if (dictionary.ContainsKey(int.Parse(Elemento["codDocSustento"].InnerText)))
                                {
                                    Comprobante = new PdfPCell(new Phrase(dictionary[int.Parse(Elemento["codDocSustento"].InnerText)], detalle));
                                }
                                else
                                {
                                    Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                                }
                            }
                            else
                            {
                                Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                            }
                            CultureInfo culture = new CultureInfo("en-US");

                            Comprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            Numero = new PdfPCell(new Phrase(Elemento["numDocSustento"] == null ? "" : Elemento["numDocSustento"].InnerText, detalle));
                            Numero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            FechaEmision = new PdfPCell(new Phrase(Elemento["fechaEmisionDocSustento"] == null ? "" : Elemento["fechaEmisionDocSustento"].InnerText, detalle));
                            FechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            EjercicioFiscal = new PdfPCell(new Phrase(sEjercicioFiscal, detalle));
                            EjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            String basImpon = Elemento1["baseImponible"].InnerText;
                            Double bI = Convert.ToDouble(basImpon, CultureInfo.CreateSpecificCulture("en-US"));
                            BaseImponible = new PdfPCell(new Phrase(bI.ToString("N2"), detalle));

                            BaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            switch (Elemento1["codigo"].InnerText)
                            {
                                case "1":
                                    Impuesto = new PdfPCell(new Phrase("RENTA", detalle));
                                    break;
                                case "2":
                                    Impuesto = new PdfPCell(new Phrase("IVA", detalle));
                                    break;
                                case "6":
                                    Impuesto = new PdfPCell(new Phrase("ISD", detalle));
                                    break;
                                default:
                                    Impuesto = new PdfPCell(new Phrase("", detalle));
                                    break;
                            }
                            Impuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            PorcentajeRetencion = new PdfPCell(new Phrase(Elemento1["porcentajeRetener"].InnerText, detalle));
                            PorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            String precUnit = Elemento1["valorRetenido"].InnerText;
                            Double pU = Convert.ToDouble(precUnit, CultureInfo.CreateSpecificCulture("en-US"));
                            PrecioUnitario = new PdfPCell(new Phrase(pU.ToString("N2"), detalle));
                            PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            tableDetalleComprobanteRetencionPagina.AddCell(Comprobante);
                            tableDetalleComprobanteRetencionPagina.AddCell(Numero);
                            tableDetalleComprobanteRetencionPagina.AddCell(FechaEmision);
                            tableDetalleComprobanteRetencionPagina.AddCell(EjercicioFiscal);
                            tableDetalleComprobanteRetencionPagina.AddCell(BaseImponible);
                            tableDetalleComprobanteRetencionPagina.AddCell(Impuesto);
                            tableDetalleComprobanteRetencionPagina.AddCell(PorcentajeRetencion);
                            tableDetalleComprobanteRetencionPagina.AddCell(PrecioUnitario);

                            if ((tableDetalleComprobanteRetencionPagina.TotalHeight + LimitePagina) > 520 && PrimeraPagina)
                            {
                                PrimeraPagina = false;
                                ListDetalle.Add(new TableDetalleComprobante() { desde = 0, hasta = NumRegistrosxPagina, Detalles = tableDetalleFactura });
                                tableDetalleFactura = new PdfPTable(8);
                                tableDetalleFactura.TotalWidth = 540f;
                                tableDetalleFactura.WidthPercentage = 100;
                                tableDetalleFactura.LockedWidth = true;
                                float[] DetalleComprobanteRetencionWidthsPaginas = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                                tableDetalleFactura.SetWidths(DetalleComprobanteRetencionWidthsPaginas);
                                NumRegistrosxPagina = 1;

                            }

                            if ((tableDetalleComprobanteRetencionPagina.TotalHeight + LimitePagina) > (520 + (posDetalleCliente - 10 - tableDetalleCliente.TotalHeight)))
                            {
                                ListDetalle.Add(new TableDetalleComprobante() { desde = 0, hasta = NumRegistrosxPagina, Detalles = tableDetalleFactura });
                                tableDetalleFactura = new PdfPTable(8);
                                tableDetalleFactura.TotalWidth = 540f;
                                tableDetalleFactura.WidthPercentage = 100;
                                tableDetalleFactura.LockedWidth = true;
                                float[] DetalleComprobanteRetencionWidthsPaginas = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                                tableDetalleFactura.SetWidths(DetalleComprobanteRetencionWidthsPaginas);
                                NumRegistrosxPagina = 1;
                            }

                            TotalRet += decimal.Parse(Elemento1["valorRetenido"].InnerText, new CultureInfo(Cultura));

                            tableDetalleFactura.AddCell(Comprobante);
                            tableDetalleFactura.AddCell(Numero);
                            tableDetalleFactura.AddCell(FechaEmision);
                            tableDetalleFactura.AddCell(EjercicioFiscal);
                            tableDetalleFactura.AddCell(BaseImponible);
                            tableDetalleFactura.AddCell(Impuesto);
                            tableDetalleFactura.AddCell(PorcentajeRetencion);
                            tableDetalleFactura.AddCell(PrecioUnitario);
                        }
                    }

                    if (tableDetalleFactura.TotalHeight > 0)
                    {
                        ListDetalle.Add(new TableDetalleComprobante()
                        {
                            desde = 0,
                            hasta = NumRegistrosxPagina,
                            Detalles = tableDetalleFactura
                        });
                    }

                    #endregion

                    #region Total

                    PdfPTable cTotal = new PdfPTable(1);
                    PdfPCell Total = new PdfPCell(new Paragraph("Total:         " + "$ " + string.Format("{0:#,0.00}", TotalRet)));
                    Total.Border = Rectangle.NO_BORDER;
                    Total.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Total.PaddingTop = 5f;
                    cTotal.AddCell(Total);
                    cTotal.TotalWidth = 540f;
                    cTotal.WidthPercentage = 100;
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 200f;
                    float[] InfoAdicionalWidths = new float[] { 40f, 130f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);


                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 5f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 5f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                        lblCodigo.Border = Rectangle.LEFT_BORDER;
                        lblCodigo.Padding = 2f;

                        Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                        Codigo.Border = Rectangle.RIGHT_BORDER;
                        Codigo.Padding = 2f;

                        tableInfoAdicional.AddCell(lblCodigo);
                        tableInfoAdicional.AddCell(Codigo);
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;

                    posDetalleCliente = 785 - tableR.TotalHeight;
                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;

                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;

                    if (DifTableL < posDetalleCliente)
                    {
                        aumento = posDetalleCliente - DifTableL + 10;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleCliente) + 5;
                    }

                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);
                    posDetalleFactura = (posDetalleCliente - 8) - tableDetalleCliente.TotalHeight;
                    tableDetalleCliente.WriteSelectedRows(0, 5, 28, posDetalleCliente - aumento, canvas);

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        tableDetalleFactura.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        posInfoAdicional = (posDetalleFactura - 10) - tableDetalleFactura.TotalHeight;
                        cTotal.WriteSelectedRows(0, 1, 20, posInfoAdicional - aumento, canvas);
                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInfoAdicional - cTotal.TotalHeight - aumento, canvas);

                    }

                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        documento.NewPage();
                        cTotal.WriteSelectedRows(0, 1, 20, 806 - aumento, canvas);
                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806 - cTotal.TotalHeight - aumento, canvas);
                    }
                    else
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        documento.NewPage();

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806 - aumento, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina)
                            {
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120)
                        {
                            cTotal.WriteSelectedRows(0, 1, 20, posInicial + 10 - aumento, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInicial + 10 - cTotal.TotalHeight - aumento, canvas);
                        }
                        else
                        {
                            cTotal.WriteSelectedRows(0, 1, 20, 806 - aumento, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806 - cTotal.TotalHeight - aumento, canvas);
                        }
                    }
                    writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    BytesAux = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
            }

            return BytesAux;
        }

        public byte[] GeneraComp(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US", string direccion = "", string direccionCliente = "")
        {
            MemoryStream ms = null;
            byte[] BytesAux = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "", rimpe = "",
                           sAmbienteVal = "", sEjercicioFiscal = "", sNumAutorizacion = "", sObligadoContabilidad = "", RegMicroEmp = "";
                    Cultura = pCultura;
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    sAmbiente = oDocument.SelectSingleNode("//ambiente") == null ? sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN" : oDocument.SelectSingleNode("//ambiente").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (direccion == null || direccion == "") ? "" : direccion;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial").InnerText;
                    sEjercicioFiscal = oDocument.SelectSingleNode("//infoCompRetencion/periodoFiscal").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad") == null ? "NO" : oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad").InnerText;
                    try
                    {
                        rimpe = oDocument.SelectSingleNode("//infoTributaria/contribuyenteRimpe").InnerText;
                    }
                    catch (Exception)
                    {
                    }

                    bool Excep = false;
                    bool microEmpInfoAdicional = false;

                    try
                    {
                        XmlNodeList IARegMicro = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                        foreach (XmlNode campoAdicional in IARegMicro)
                        {
                            if (campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas")
                            {
                                RegMicroEmp = "si";
                                microEmpInfoAdicional = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Excep = true;
                    }

                    if (!Excep)
                    {
                        if (!microEmpInfoAdicional)
                        {
                            try
                            {
                                string micoEmpe = oDocument.SelectSingleNode("//infoTributaria/regimenMicroempresas").InnerText;
                                if (!string.IsNullOrEmpty(micoEmpe))
                                {
                                    RegMicroEmp = "si";
                                }
                            }
                            catch (Exception)
                            {
                                Excep = true;
                            }
                        }
                    }

                    int registros = 0;
                    int PagLimite1 = 35;
                    int MaxPagina1 = 45;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                    iTextSharp.text.Font detAdicional = GetArial(6);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;

                    StreamReader s = new StreamReader(pRutaLogo);
                    System.Drawing.Image jpg1 = System.Drawing.Image.FromStream(s.BaseStream);
                    s.Close();
                    var altoDbl = Convert.ToDouble(jpg1.Height);
                    var anchoDbl = Convert.ToDouble(jpg1.Width);
                    var altoMax = 130D;
                    var anchoMax = 250D;
                    var porcentajeAlto = 0D;
                    var porcentajeAncho = 0D;
                    var porcentajeIncremento = 0D;
                    var porcentajeDecremento = 0D;
                    var nuevoAlto = 0;
                    var nuevoAncho = 0;

                    if ((altoDbl < altoMax) && (anchoDbl < anchoMax))
                    {
                        porcentajeAlto = (altoMax / altoDbl) - 1D;
                        porcentajeAncho = (anchoMax / anchoDbl) - 1D;
                        porcentajeIncremento = (porcentajeAlto < porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32((altoDbl * porcentajeIncremento) + altoDbl);
                        nuevoAncho = Convert.ToInt32((anchoDbl * porcentajeIncremento) + anchoDbl);
                    }
                    else if ((altoMax < altoDbl) && (anchoMax < anchoDbl))
                    {
                        porcentajeAlto = (1D - (altoMax / altoDbl));
                        porcentajeAncho = (1D - (anchoMax / anchoDbl));
                        porcentajeDecremento = (porcentajeAlto > porcentajeAncho) ? porcentajeAlto : porcentajeAncho;
                        nuevoAlto = Convert.ToInt32(altoDbl - (altoDbl * porcentajeDecremento));
                        nuevoAncho = Convert.ToInt32(anchoDbl - (anchoDbl * porcentajeDecremento));
                    }
                    else if ((altoMax < altoDbl) && (anchoDbl < anchoMax))// 152 y 150
                    {

                        porcentajeDecremento = (altoMax / altoDbl);
                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);
                    }
                    else if ((altoDbl < altoMax) && (anchoMax < anchoDbl))// 100 y 300
                    {

                        //decrementamos el amcho
                        porcentajeDecremento = (anchoMax / anchoDbl);

                        nuevoAlto = Convert.ToInt32(altoDbl * porcentajeDecremento);
                        nuevoAncho = Convert.ToInt32(anchoDbl * porcentajeDecremento);

                    }
                    jpg1 = ImagenLogo.ImageResize(jpg1, nuevoAlto, nuevoAncho);
                    #region Tabla 1
                    var tablaLayout = new PdfPTable(2)
                    {
                        TotalWidth = 540f,
                        LockedWidth = true
                    };

                    tablaLayout.SetWidths(new float[] { 270f, 270f });
                    var tablaEnlazada = new PdfPTable(1);
                    #endregion
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("C O M P R O B A N T E  D E  R E T E N C I Ó N", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA DE AUTORIZACIÓN: " + sFechaAutorizacion.ToString(), cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    innerTableD.AddCell(FechaAutorizacion);
                   
                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    if ((sMatriz.Length >= 166) && (sSucursal.Length >= 102 && sSucursal.Length <= 164) || (sSucursal.Length >= 165) && (sMatriz.Length >= 102 && sMatriz.Length < 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    else if ((sMatriz.Length >= 166) && (sSucursal.Length >= 165) || (sSucursal.Length >= 165) && (sMatriz.Length >= 166))
                    {
                        ClaveAcceso.PaddingBottom = 15f;
                    }
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);
                    var ContenedorD = new PdfPTable(1)
                    {
                        TotalWidth = 278f
                    };
                    AgregarCeldaTabla(ContenedorD, innerTableD);
                    
                    #endregion

                    #region TablaIzquierda

                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.PaddingBottom = 3f;
                    RazonSocial.PaddingTop = 4f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                RazonSocial.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            RazonSocial.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        RazonSocial.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(RazonSocial);

                    if (sMatriz.Length >= 200)
                    {
                        sMatriz = sMatriz.Substring(0, 200);
                    }

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dirección Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    DirMatriz.PaddingBottom = 3f;
                    DirMatriz.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                    innerTableL.AddCell(DirMatriz);

                    if (sSucursal.Length >= 200)
                    {
                        sSucursal = sSucursal.Substring(0, 200);
                    }

                    if (!string.IsNullOrEmpty(sSucursal))
                    {
                        PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dirección Sucursal: " + sSucursal, cabecera));
                        DirSucursal.Border = Rectangle.NO_BORDER;
                        DirSucursal.Padding = 5f;
                        DirSucursal.PaddingBottom = 3f;
                        DirSucursal.PaddingTop = 3f;
                        if (!Excep)
                        {
                            if (!string.IsNullOrEmpty(RegMicroEmp))
                            {
                                if (!RegMicroEmp.ToLower().Equals("si"))
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                        innerTableL.AddCell(DirSucursal);
                    }

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.PaddingBottom = 3f;
                    ContribuyenteEspecial.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ContribuyenteEspecial.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ContribuyenteEspecial.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ContribuyenteEspecial.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.PaddingBottom = 3f;
                    ObligadoContabilidad.PaddingTop = 3f;
                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (!RegMicroEmp.ToLower().Equals("si"))
                            {
                                ObligadoContabilidad.FixedHeight = 18f;
                            }
                        }
                        else
                        {
                            ObligadoContabilidad.FixedHeight = 18f;
                        }
                    }
                    else
                    {
                        ObligadoContabilidad.FixedHeight = 18f;
                    }
                    innerTableL.AddCell(ObligadoContabilidad);

                    if (!Excep)
                    {
                        if (!string.IsNullOrEmpty(RegMicroEmp))
                        {
                            if (RegMicroEmp.ToLower().Equals("si"))
                            {
                                PdfPCell RegimenMicroEmpresa = new PdfPCell(new Paragraph("Contribuyente Régimen Microempresas.", cabecera));
                                RegimenMicroEmpresa.Border = Rectangle.NO_BORDER;
                                RegimenMicroEmpresa.Padding = 5f;
                                RegimenMicroEmpresa.PaddingBottom = 3f;
                                RegimenMicroEmpresa.PaddingTop = 3f;
                                innerTableL.AddCell(RegimenMicroEmpresa);
                            }
                        }
                    }

                    bool agenteRetencionInfoA = false;
                    XmlNodeList IA = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    foreach (XmlNode campoAdicional in IA)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "Agente")
                        {
                            iTextSharp.text.Font cabeceraAgente = GetArial(7);

                            PdfPCell AgenteRetencion = new PdfPCell(new Paragraph(campoAdicional.InnerText, cabeceraAgente));
                            AgenteRetencion.Border = Rectangle.NO_BORDER;
                            AgenteRetencion.Padding = 5f;
                            AgenteRetencion.PaddingBottom = 4f;
                            AgenteRetencion.PaddingTop = 4f;
                            innerTableL.AddCell(AgenteRetencion);
                            agenteRetencionInfoA = true;
                        }
                    }

                    try
                    {
                        string agente = oDocument.SelectSingleNode("//infoTributaria/agenteRetencion").InnerText;
                        if (!string.IsNullOrEmpty(agente))
                        {
                            if (!agenteRetencionInfoA)
                            {
                                agente = agente.TrimStart(new Char[] { '0' });
                                PdfPCell AgenteRetencion = new PdfPCell(new Paragraph("Agente de Retención Resolución No." + agente, cabecera));
                                AgenteRetencion.Border = Rectangle.NO_BORDER;
                                AgenteRetencion.Padding = 5f;
                                innerTableL.AddCell(AgenteRetencion);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (string.IsNullOrEmpty(RegMicroEmp))
                    {
                        if (!string.IsNullOrEmpty(rimpe))
                        {
                            try
                            {
                                PdfPCell rimpe_ = new PdfPCell(new Paragraph(rimpe, cabecera));
                                rimpe_.Border = Rectangle.NO_BORDER;
                                rimpe_.Padding = 5f;
                                rimpe_.PaddingBottom = 3f;
                                rimpe_.PaddingTop = 3f;
                                innerTableL.AddCell(rimpe_);
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }
                    var ContenedorL = new PdfPTable(1)
                    {
                        TotalWidth = 250f
                    };
                    AgregarCeldaTabla(ContenedorL, innerTableL);
                   
                    #endregion

                    #region Logo
                    BaseColor color = null;
                    Image jpgPrueba = Image.GetInstance(jpg1, color);
                    jpg1.Dispose();
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpgPrueba);

                    logo.Border = Rectangle.NO_BORDER;
                    logo.HorizontalAlignment = Element.ALIGN_CENTER;
                    logo.Padding = 4f;
                    logo.FixedHeight = 130f;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion
                    AgregarCeldaTablaEnlazada(tablaEnlazada, tableLOGO);
                    AgregarCeldaTablaEnlazada(tablaEnlazada, ContenedorL);
                    AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada);
                    AgregarCeldaTablaLayout(tablaLayout, ContenedorD);
                    #region DetalleCliente
                    PdfPTable tableDetalleCliente = new PdfPTable(4);
                    tableDetalleCliente.TotalWidth = 540f;
                    tableDetalleCliente.WidthPercentage = 100;
                    float[] DetalleClientewidths = new float[] { 30f, 120f, 30f, 40f };
                    tableDetalleCliente.SetWidths(DetalleClientewidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/razonSocialSujetoRetenido").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("Identificación:", detalle)); // Cambio soliciato por BB Identificación
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/identificacionSujetoRetenido").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                    tableDetalleCliente.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.RIGHT_BORDER;
                    FechaEmisionCliente.Colspan = 4;
                    tableDetalleCliente.AddCell(FechaEmisionCliente);

                    var lblDireccion = new PdfPCell(new Paragraph("Dirección:", detalle));
                    lblDireccion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblDireccion);

                    var Direccion = new PdfPCell(new Paragraph(direccionCliente == null ? "" : direccionCliente, detalle));
                    Direccion.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    Direccion.Colspan = 4;
                    tableDetalleCliente.AddCell(Direccion);

                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(8);

                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    tableDetalleFactura.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    var encComprobante = new PdfPCell(new Paragraph("Comprobante", fontEncabezado));
                    encComprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encNumero = new PdfPCell(new Paragraph("Número", fontEncabezado));
                    encNumero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encFechaEmision = new PdfPCell(new Paragraph("Fecha Emisión", fontEncabezado));
                    encFechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encEjercicioFiscal = new PdfPCell(new Paragraph("Ejercicio Fiscal", fontEncabezado));
                    encEjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encBaseImponible = new PdfPCell(new Paragraph("Base Imponible para la Retención", fontEncabezado));
                    encBaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encImpuesto = new PdfPCell(new Paragraph("IMPUESTO", fontEncabezado));
                    encImpuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPorcentajeRetencion = new PdfPCell(new Paragraph("Porcentaje Retención", fontEncabezado));
                    encPorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Valor Retenido", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encComprobante);
                    tableDetalleFactura.AddCell(encNumero);
                    tableDetalleFactura.AddCell(encFechaEmision);
                    tableDetalleFactura.AddCell(encEjercicioFiscal);
                    tableDetalleFactura.AddCell(encBaseImponible);
                    tableDetalleFactura.AddCell(encImpuesto);
                    tableDetalleFactura.AddCell(encPorcentajeRetencion);
                    tableDetalleFactura.AddCell(encPrecioUnitario);

                    PdfPCell Comprobante = null;
                    PdfPCell Numero;
                    PdfPCell FechaEmision;
                    PdfPCell EjercicioFiscal;
                    PdfPCell BaseImponible;
                    PdfPCell Impuesto;
                    PdfPCell PorcentajeRetencion;
                    PdfPCell PrecioUnitario;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//impuestos/impuesto");
                    registros = Detalles.Count;

                    Dictionary<int, string> dictionary = new Dictionary<int, string>();
                    dictionary.Add(1, "Factura");
                    dictionary.Add(2, "Nota o boleta de venta");
                    dictionary.Add(3, "Liquidación de compra de Bienes o Prestación de servicios");
                    dictionary.Add(4, "Nota de crédito");
                    dictionary.Add(5, "Nota de débito");
                    dictionary.Add(6, "Guías de Remisión");
                    dictionary.Add(7, "Comprobante de Retención");
                    dictionary.Add(8, "Boletos o entradas a espectáculos públicos");
                    dictionary.Add(9, "Tiquetes o vales emitidos por máquinas registradoras");
                    dictionary.Add(11, "Pasajes expedidos por empresas de aviación");
                    dictionary.Add(12, "Documentos emitidos por instituciones financieras");
                    dictionary.Add(15, "Comprobante de venta emitido en el Exterior");
                    dictionary.Add(16, "Formulario Único de Exportación (FUE) o Declaración Aduanera Única(DAU) o Declaración Andina de Valor(DAV)");
                    dictionary.Add(18, "Documentos autorizados utilizados en ventasexcepto N/C  N/D ");
                    dictionary.Add(19, "Comprobantes de Pago de Cuotas o Aportes");
                    dictionary.Add(20, "Documentos por Servicios Administrativos emitidos por Inst.del Estado");
                    dictionary.Add(21, "Carta de Porte Aéreo");
                    dictionary.Add(22, "RECAP");
                    dictionary.Add(23, "Nota de Crédito TC");
                    dictionary.Add(24, "Nota de Débito TC");
                    dictionary.Add(41, "Comprobante de venta emitido por reembolso");
                    dictionary.Add(42, "Documento agente de retención Presuntiva");
                    dictionary.Add(43, "Liquidación para Explotación y Exploracion de Hidrocarburos");
                    dictionary.Add(44, "Comprobante de Contribuciones y Aportes");
                    dictionary.Add(45, "Liquidación por reclamos de aseguradoras");
                    dictionary.Add(47, "Nota de Crédito por Reembolso Emitida por Intermediario");
                    dictionary.Add(48, "Nota de Débito por Reembolso Emitida por Intermediario");
                    dictionary.Add(49, "Proveedor Directo de Exportador Bajo Régimen Especial");
                    dictionary.Add(50, "A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(51, "N/C A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(52, "N/D A Inst. Estado y Empr. Públicas que percibe ingreso exento de Imp.Renta");
                    dictionary.Add(294, "Liquidación de compra de Bienes Muebles Usados");
                    dictionary.Add(344, "Liquidación de compra de vehículos usados");

                    Decimal TotalRet = 0;


                    foreach (XmlNode Elemento in Detalles)
                    {

                        if (Elemento["codDocSustento"] != null)
                        {
                            if (dictionary.ContainsKey(int.Parse(Elemento["codDocSustento"].InnerText)))
                            {
                                Comprobante = new PdfPCell(new Phrase(dictionary[int.Parse(Elemento["codDocSustento"].InnerText)], detalle));
                            }
                            else
                            {
                                Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                            }
                        }
                        else
                        {
                            Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                        }
                        CultureInfo culture = new CultureInfo("en-US");

                        Comprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Numero = new PdfPCell(new Phrase(Elemento["numDocSustento"] == null ? "" : Elemento["numDocSustento"].InnerText, detalle));
                        Numero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        FechaEmision = new PdfPCell(new Phrase(Elemento["fechaEmisionDocSustento"] == null ? "" : Elemento["fechaEmisionDocSustento"].InnerText, detalle));
                        FechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        EjercicioFiscal = new PdfPCell(new Phrase(sEjercicioFiscal, detalle));
                        EjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                        String basImpon = Elemento["baseImponible"].InnerText;
                        Double bI = Convert.ToDouble(basImpon, CultureInfo.CreateSpecificCulture("en-US"));
                        BaseImponible = new PdfPCell(new Phrase(bI.ToString("N2"), detalle));

                        BaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        switch (Elemento["codigo"].InnerText)
                        {
                            case "1":
                                Impuesto = new PdfPCell(new Phrase("RENTA", detalle));
                                break;
                            case "2":
                                Impuesto = new PdfPCell(new Phrase("IVA", detalle));
                                break;
                            case "6":
                                Impuesto = new PdfPCell(new Phrase("ISD", detalle));
                                break;
                            default:
                                Impuesto = new PdfPCell(new Phrase("", detalle));
                                break;
                        }
                        Impuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        PorcentajeRetencion = new PdfPCell(new Phrase(Elemento["porcentajeRetener"].InnerText, detalle));
                        PorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                        String precUnit = Elemento["valorRetenido"].InnerText;
                        Double pU = Convert.ToDouble(precUnit, CultureInfo.CreateSpecificCulture("en-US"));
                        PrecioUnitario = new PdfPCell(new Phrase(pU.ToString("N2"), detalle));
                        PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                        TotalRet += decimal.Parse(Elemento["valorRetenido"].InnerText, new CultureInfo(Cultura));

                        tableDetalleFactura.AddCell(Comprobante);
                        tableDetalleFactura.AddCell(Numero);
                        tableDetalleFactura.AddCell(FechaEmision);
                        tableDetalleFactura.AddCell(EjercicioFiscal);
                        tableDetalleFactura.AddCell(BaseImponible);
                        tableDetalleFactura.AddCell(Impuesto);
                        tableDetalleFactura.AddCell(PorcentajeRetencion);
                        tableDetalleFactura.AddCell(PrecioUnitario);
                    }
                    #endregion
                    #region Total

                    PdfPTable cTotal = new PdfPTable(1);
                    PdfPCell Total = new PdfPCell(new Paragraph("Total:         " + "$ " + string.Format("{0:#,0.00}", TotalRet)));
                    Total.Border = Rectangle.NO_BORDER;
                    Total.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Total.PaddingTop = 5f;
                    cTotal.AddCell(Total);
                    cTotal.TotalWidth = 540f;
                    cTotal.WidthPercentage = 100;
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 200f;
                    float[] InfoAdicionalWidths = new float[] { 40f, 130f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);


                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 5f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 5f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (!(campoAdicional.Attributes["nombre"].Value == "Agente"
                            || campoAdicional.Attributes["nombre"].Value == "Contribuyente Régimen Microempresas"))
                        {
                            lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;

                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    var tablaLHeight = ContenedorL.TotalHeight;
                    var tablaLogoHeight = tableLOGO.TotalHeight;
                    var espacioMas = tablaLHeight - tablaLogoHeight;
                    var posicionYTablaR = documento.PageSize.Height - espacioMas;
                    var posicion1 = 5;
                    posDetalleCliente = 785 - ContenedorD.TotalHeight - posicion1;
                    var aumento = float.Parse("0");
                    var TotalTL = ContenedorL.TotalHeight;

                    var PosYTableL = 790 - tableLOGO.TotalHeight - 10;
                    var DifTableL = PosYTableL - ContenedorL.TotalHeight;

                    if (DifTableL < posDetalleCliente)
                    {
                        aumento = posDetalleCliente - DifTableL + 10;
                    }
                    else
                    {
                        PosYTableL = PosYTableL - (DifTableL - posDetalleCliente) + 5;
                    }
                    tablaLayout.WriteSelectedRows(0, 1, 30, 791, canvas);
                    posDetalleFactura = (posDetalleCliente - 8) - tableDetalleCliente.TotalHeight;

                    tableDetalleCliente.WriteSelectedRows(0, 5, 28, posDetalleCliente - aumento, canvas);

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        tableDetalleFactura.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        posInfoAdicional = (posDetalleFactura - 10) - tableDetalleFactura.TotalHeight;
                        cTotal.WriteSelectedRows(0, 1, 20, posInfoAdicional - aumento, canvas);
                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInfoAdicional - cTotal.TotalHeight - aumento, canvas);
                    }

                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        documento.NewPage();
                        cTotal.WriteSelectedRows(0, 1, 20, 806 - aumento, canvas);
                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806 - cTotal.TotalHeight - aumento, canvas);
                    }
                    else
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight - aumento, canvas);
                        documento.NewPage();

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806 - aumento, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina)
                            {
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120)
                        {
                            cTotal.WriteSelectedRows(0, 1, 20, posInicial + 10 - aumento, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInicial + 10 - cTotal.TotalHeight - aumento, canvas);
                        }
                        else
                        {
                            cTotal.WriteSelectedRows(0, 1, 20, 806 - aumento, canvas);
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806 - cTotal.TotalHeight - aumento, canvas);
                        }
                    }
                    writer.PageEvent = new Controller.ITextEvents(pRutaLogo);
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    BytesAux = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
            }

            return BytesAux;
        }

        public byte[] GeneraComprobanteRetencion(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US", string direccion = "", string direccionCliente = "")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            XmlNodeList Detalles2;
            XmlDocument oDocument1 = new XmlDocument();
            oDocument1.LoadXml(pDocumento_autorizado);
            oDocument1.LoadXml(oDocument1.SelectSingleNode("//comprobante").InnerText);
            int version = 0;
            Detalles2 = oDocument1.SelectNodes("//retenciones/retencion");
            version = Detalles2.Count;
            if (version > 0)
            {
                Bytes = GeneraCompR2(pDocumento_autorizado, pRutaLogo, pCultura, direccion);
            }
            else
            {
                Bytes = GeneraComp(pDocumento_autorizado, pRutaLogo, pCultura, direccion, direccionCliente);
            }

            return Bytes;
        }

        public byte[] GeneraLiquidacionCompra(string autorizacionXml, string logoRuta, string direccion = "", string direccionCliente = "")
        {
            var proveedorFormato = new CultureInfo("en-US");

            return GeneraLiquidacionCompra(autorizacionXml, logoRuta, proveedorFormato, direccion, direccionCliente);
        }

        public byte[] GeneraLiquidacionCompra(string autorizacionXml, string logoRuta, IFormatProvider proveedorFormato, string direccion = "", string direccionCliente = "")
        {
            byte[] retorno = null;

            using (var ms = new MemoryStream())
            {
                var escritor = new LiquidacionCompraEscritor(ms, proveedorFormato);

                if (direccion.Length >= 200)
                {
                    direccion = direccion.Substring(0, 200);
                }

                escritor.Escribir(autorizacionXml, logoRuta, direccion, direccionCliente);

                ms.Position = 0L;
                retorno = ms.ToArray();
            }

            return retorno;
        }

        /// <summary>
        /// Método privado para obtener el tipo de letra 
        /// </summary>
        /// <param name="pFontSize">Tamaño de la fuente</param>
        /// <returns></returns>
        private static iTextSharp.text.Font GetArial(int pFontSize)
        {
            var fontName = "Arial";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, pFontSize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        }
        /// <summary>
        /// Método privado para obtener el tipo de letra
        /// </summary>
        /// <param name="pFontSize">Tamaño de la fuente</param>
        /// <returns></returns>
        private static iTextSharp.text.Font GetArialBlack(int pFontSize)
        {
            var fontName = "Arial";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, pFontSize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        }

        #region Contrato

        public byte[] GeneraContrato(DataTable InfoCliente, DataTable InfoSipecom, DataTable InfoGeneral, string RutaLogoSipecom, string RutaLogoCliente, string RucSipecom, string idContrato)
        {
            byte[] retorno = null;
            var util = new General();
            var infoCliente = util.LeerDataTableCliente(InfoCliente);
            var infoSipecom = util.LeerDataTableCliente(InfoSipecom);
            var infoGeneral = LeerDataTableInfoGeneral(InfoGeneral);
            var listDocumentos = ObtenerDocumentos(InfoGeneral);

            if (idContrato == "" || idContrato == null)
            {
                if (infoCliente != null)
                {
                    infoCliente.FechaInicioPlan = util.ObtenerFechaDDMMYYYY(infoCliente.FechaInicioPlan);
                    infoCliente.FechaFinPlan = util.ObtenerFechaDDMMYYYY(infoCliente.FechaFinPlan);
                }
            }
            infoCliente.FechaCompletaInicioPlan = util.ObtenerFechaFormateada(infoCliente.FechaInicioPlan);

            using (var ms = new MemoryStream())
            {
                var escritor = new ContratoEscritor(ms);

                escritor.Escribir(infoCliente, infoSipecom, infoGeneral, RutaLogoSipecom, RutaLogoCliente, listDocumentos);

                ms.Position = 0L;
                retorno = ms.ToArray();
            }

            return retorno;
        }

        private InfoContrato LeerDataTableInfoGeneral(DataTable InfoGeneral)
        {
            var objCliente = new InfoContrato();
            foreach (DataRow row in InfoGeneral.Rows)
            {
                var idtabla = row["tabla"].ToString();
                switch (idtabla)
                {
                    case "1020":
                        ObtenerInfoGeneralSipecom(row, objCliente);
                        break;
                    default:
                        break;
                }

            }

            return objCliente;
        }

        private InfoContrato ObtenerInfoGeneralSipecom(DataRow row, InfoContrato objCliente)
        {
            var codigo = row["codigo"].ToString();
            switch (codigo)
            {
                case "EmailContb":
                    objCliente.EmailContable = row["detalle"].ToString();
                    break;
                case "EmailSoprt":
                    objCliente.EmailSoporte = row["detalle"].ToString();
                    break;
                case "UrlSipe":
                    objCliente.UrlSipecom = row["detalle"].ToString();
                    break;
                default:
                    break;
            }

            return objCliente;
        }

        private List<string> ObtenerDocumentos(DataTable InfoCliente)
        {
            var list = new List<string>();


            foreach (DataRow item in InfoCliente.Rows)
            {
                if (item["Tabla"].ToString() == "1001")
                {
                    list.Add(item["detalle"].ToString());
                }
            }
            return list;
        }

        #endregion

        #region Proteccion de Datos

        public byte[] GeneraPoliticaProteccionDatos(string politica)
        {
            byte[] retorno = null;

            using (var ms = new MemoryStream())
            {
                var escritor = new ProteccionDatosEscritor(ms);

                escritor.Escribir(politica);

                ms.Position = 0L;
                retorno = ms.ToArray();
            }

            return retorno;
        }

        #endregion
    }
}

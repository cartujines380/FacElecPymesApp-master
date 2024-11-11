using Infraestructura.Transversal.eComp.PDF.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infraestructura.Transversal.eComp.PDF
{
    internal class LiquidacionCompraEscritor
    {
        #region Campos

        private readonly Stream m_flujo;
        private readonly IFormatProvider m_proveedorFormato;

        private PdfWriter m_writer;
        private Document m_documento;
        private Font m_fuenteArialOcho;
        private Font m_fuenteArialSiete;
        private Font m_fuenteArialSeis;

        #endregion

        #region Constructores

        internal LiquidacionCompraEscritor(Stream flujo)
            : this(flujo, CultureInfo.InvariantCulture)
        {
        }

        internal LiquidacionCompraEscritor(Stream flujo, IFormatProvider proveedorFormato)
        {
            if (flujo == null)
            {
                throw new ArgumentNullException(nameof(flujo));
            }

            if (proveedorFormato == null)
            {
                throw new ArgumentNullException(nameof(proveedorFormato));
            }

            m_flujo = flujo;
            m_proveedorFormato = proveedorFormato;

            InicializarFuentes();
        }

        #endregion

        #region Metodos internos

        internal void Escribir(string autorizacionXml, string logoRuta, string direccion = "", string direccionCliente = "")
        {
            var autorizacionEle = XElement.Parse(autorizacionXml);

            var comprobanteXml = autorizacionEle.GetElementValue("comprobante");

            var comprobanteEle = XElement.Parse(comprobanteXml);

            m_documento = new Document();

            m_writer = PdfWriter.GetInstance(m_documento, m_flujo);

            m_documento.Open();

            var tablaLogo = CrearTablaLogo(logoRuta);
            var tablaEstablecimiento = CrearTablaEstablecimiento(comprobanteEle, direccion);
            var tablaEncabezado = CrearTablaEncabezado(autorizacionEle, comprobanteEle);
            var tablaProveedor = CrearTablaProveedor(comprobanteEle, direccionCliente);
            var tablaDetalle = CrearTablaDetalle(comprobanteEle);
            var tablaFormasPago = CrearTablaFormasPago(comprobanteEle);
            var tablaInformacionAdicional = CrearTablaInformacionAdicional(comprobanteEle);
            var tablaTotales = CrearTablaTotales(comprobanteEle);
            var tablaReembolso = CrearTablaReembolso(comprobanteEle);

            var tablaLayout = new PdfPTable(2)
            {
                TotalWidth = 540f,
                LockedWidth = true
            };


            tablaLayout.SetWidths(new float[] { 270f, 270f });

            var tablaEnlazada1 = new PdfPTable(1);

            AgregarCeldaTablaEnlazada(tablaEnlazada1, tablaLogo);
            AgregarCeldaTablaEnlazada(tablaEnlazada1, tablaEstablecimiento);

            var tablaVaciaAdicional = CrearTablaVacia(250f, 8f);

            var tablaEnlazada2 = new PdfPTable(1);
            AgregarCeldaTablaEnlazada(tablaEnlazada2, tablaFormasPago);
            AgregarCeldaTablaEnlazada(tablaEnlazada2, tablaVaciaAdicional);
            AgregarCeldaTablaEnlazada(tablaEnlazada2, tablaInformacionAdicional);

            var tablaVaciaTotales = CrearTablaVacia(98f);

            var tablaEnlazada3 = new PdfPTable(2)
            {
                TotalWidth = 278f
            };

            tablaEnlazada3.SetWidths(new float[] { 98f, 180f });

            AgregarCeldaTablaEnlazada(tablaEnlazada3, tablaVaciaTotales);
            AgregarCeldaTablaEnlazada(tablaEnlazada3, tablaTotales);

            AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada1);
            AgregarCeldaTablaLayout(tablaLayout, tablaEncabezado);
            AgregarCeldaTablaLayout(tablaLayout, tablaProveedor, 2);
            AgregarCeldaTablaLayout(tablaLayout, tablaDetalle, 2);
            if (tablaReembolso != null) AgregarCeldaTablaLayout(tablaLayout, tablaReembolso, 2);
            AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada2);
            AgregarCeldaTablaLayout(tablaLayout, tablaEnlazada3);

            m_documento.Add(tablaLayout);

            m_writer.CloseStream = false;
            m_documento.Close();
        }

        #endregion

        #region Metodos privados

        #region Metodos de inicializacion

        private void InicializarFuentes()
        {
            m_fuenteArialOcho = FontFactory.GetFont("Arial", 8f, Font.NORMAL, BaseColor.BLACK);
            m_fuenteArialSiete = FontFactory.GetFont("Arial", 7f, Font.NORMAL, BaseColor.BLACK);
            m_fuenteArialSeis = FontFactory.GetFont("Arial", 6f, Font.NORMAL, BaseColor.BLACK);
        }

        #endregion

        #region Metodos de celdas

        private PdfPCell AgregarCeldaTexto(PdfPTable tabla, string texto)
        {
            return tabla.AddCell(new PdfPCell(new Paragraph(texto, m_fuenteArialOcho))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f
            });
        }

        private PdfPCell AgregarCeldaTextoProveedor(PdfPTable tabla, string texto)
        {
            return tabla.AddCell(new PdfPCell(new Paragraph(texto, m_fuenteArialSiete))
            {
                Border = Rectangle.NO_BORDER,
            });
        }

        private PdfPCell AgregarCeldaEncabezado(PdfPTable tabla, string texto)
        {
            return tabla.AddCell(new PdfPCell(new Paragraph(texto, m_fuenteArialSiete))
            {
                HorizontalAlignment = Rectangle.ALIGN_CENTER
            });
        }

        private PdfPCell AgregarCeldaDetalleEncabezado(PdfPTable tabla, string texto)
        {
            return AgregarCeldaDetalleEncabezado(tabla, texto, Rectangle.ALIGN_CENTER);
        }

        private PdfPCell AgregarCeldaDetalleEncabezado(PdfPTable tabla, string texto, int alineacionHorizontal)
        {
            return tabla.AddCell(new PdfPCell(new Paragraph(texto, m_fuenteArialSiete))
            {
                HorizontalAlignment = alineacionHorizontal
            });
        }

        private PdfPCell AgregarCeldaDetalle(PdfPTable tabla, string texto)
        {
            return AgregarCeldaDetalle(tabla, texto, Rectangle.ALIGN_CENTER);
        }

        private PdfPCell AgregarCeldaDetalle(PdfPTable tabla, string texto, int alineacionHorizontal)
        {
            return tabla.AddCell(new PdfPCell(new Phrase(texto, m_fuenteArialSiete))
            {
                HorizontalAlignment = alineacionHorizontal
            });
        }

        private PdfPCell AgregarCeldaTextoAdicional(PdfPTable tabla, string texto)
        {
            return tabla.AddCell(new PdfPCell(new Paragraph(texto, m_fuenteArialSeis))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 2f
            });
        }

        private PdfPCell AgregarCeldaCodigoBarra(PdfPTable tabla, string codigo)
        {
            var codigoImg = BarcodeHelper.GetBarcode128(m_writer.DirectContent, codigo, false, Barcode.CODE128);

            return tabla.AddCell(new PdfPCell(codigoImg)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_CENTER
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

        private PdfPCell AgregarCeldaTablaLayout(PdfPTable tabla, PdfPTable tablaAgregar)
        {
            return tabla.AddCell(new PdfPCell(tablaAgregar)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 4f
            });
        }

        private PdfPCell AgregarCeldaTablaLayout(PdfPTable tabla, PdfPTable tablaAgregar, int colSpan)
        {
            return tabla.AddCell(new PdfPCell(tablaAgregar)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 4f,
                Colspan = colSpan
            });
        }

        #endregion

        #region Metodos de tablas

        private PdfPTable CrearTablaLogo(string logoRuta)
        {
            StreamReader s = new StreamReader(logoRuta);
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
            BaseColor color = null;
            iTextSharp.text.Image jpgPrueba = iTextSharp.text.Image.GetInstance(jpg1, color);
            jpg1.Dispose();
            var logoCelda = new PdfPCell(jpgPrueba)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Rectangle.ALIGN_CENTER,
                Padding = 4f,
                FixedHeight = 130f
            };

            var retorno = new PdfPTable(1);

            retorno.AddCell(logoCelda);
            retorno.TotalWidth = 250f;

            return retorno;
        }

        private PdfPTable CrearTablaEncabezado(XElement autorizacionEle, XElement comprobanteEle)
        {
            var autorizacionNumero = autorizacionEle.GetElementValue("numeroAutorizacion");
            var autorizacionFechaStr = autorizacionEle.GetElementValue("fechaAutorizacion");

            var infoTributariaEle = comprobanteEle.Element("infoTributaria");

            var ambienteStr = infoTributariaEle.GetElementValue("ambiente");
            var ambiente = (ambienteStr == "1") ? "PRUEBAS" : "PRODUCCIÓN";
            var tipoEmisionStr = infoTributariaEle.GetElementValue("tipoEmision");
            var tipoEmision = (tipoEmisionStr == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
            var ruc = infoTributariaEle.GetElementValue("ruc");
            var claveAcceso = infoTributariaEle.GetElementValue("claveAcceso");
            var establecimientoCodigo = infoTributariaEle.GetElementValue("estab");
            var puntoEmisionCodigo = infoTributariaEle.GetElementValue("ptoEmi");
            var secuencial = infoTributariaEle.GetElementValue("secuencial");
            var comprobanteNumero = string.Format("{0}-{1}-{2}", establecimientoCodigo, puntoEmisionCodigo, secuencial);

            var tablaInterna = new PdfPTable(1);

            AgregarCeldaTexto(tablaInterna, string.Format("R.U.C.: {0}", ruc));
            AgregarCeldaTexto(tablaInterna, "LIQUIDACIÓN DE COMPRA DE BIENES Y PRESTACIÓN DE SERVICIOS");
            AgregarCeldaTexto(tablaInterna, string.Format("No. {0}", comprobanteNumero));
            AgregarCeldaTexto(tablaInterna, "NÚMERO DE AUTORIZACIÓN:");
            AgregarCeldaTexto(tablaInterna, autorizacionNumero);
            AgregarCeldaTexto(tablaInterna, string.Format("FECHA Y HORA DE AUTORIZACIÓN: {0}", autorizacionFechaStr));
            AgregarCeldaTexto(tablaInterna, string.Format("AMBIENTE: {0}", ambiente));
            AgregarCeldaTexto(tablaInterna, string.Format("EMISIÓN: {0}", tipoEmision));
            AgregarCeldaTexto(tablaInterna, "CLAVE DE ACCESO:");
            AgregarCeldaCodigoBarra(tablaInterna, claveAcceso);

            var retorno = new PdfPTable(1)
            {

                TotalWidth = 278f
            };

            AgregarCeldaTabla(retorno, tablaInterna);

            return retorno;
        }

        private PdfPTable CrearTablaEstablecimiento(XElement comprobanteEle, string direccion = "")
        {
            var infoTributariaEle = comprobanteEle.Element("infoTributaria");

            var razonSocial = infoTributariaEle.GetElementValue("razonSocial");
            var dirMatriz = infoTributariaEle.GetElementValue("dirMatriz");
            var rimpe = infoTributariaEle.GetElementValue("contribuyenteRimpe") ?? string.Empty;

            var infoLiqCompraEle = comprobanteEle.Element("infoLiquidacionCompra");

            var dirEstablecimiento = (infoLiqCompraEle.GetElementValue("dirEstablecimiento") ?? string.Empty);
            var obligadoContabilidad = (infoLiqCompraEle.GetElementValue("obligadoContabilidad") ?? "NO");
            var contribuyenteEspecial = (infoLiqCompraEle.GetElementValue("contribuyenteEspecial") ?? string.Empty);

            var tablaInterna = new PdfPTable(1);

            if (dirMatriz.Length >= 200)
            {
                dirMatriz = dirMatriz.Substring(0, 200);
            }

            AgregarCeldaTexto(tablaInterna, razonSocial).FixedHeight = 20f;
            AgregarCeldaTexto(tablaInterna, string.Format("Dirección Matriz: {0}", dirMatriz));
            if (!string.IsNullOrEmpty(direccion))
            {
                AgregarCeldaTexto(tablaInterna, string.Format("Dirección Sucursal: {0}", direccion));
            }
            AgregarCeldaTexto(tablaInterna, string.Format("Contribuyente Especial Nro: {0}", contribuyenteEspecial)).FixedHeight = 20f;
            AgregarCeldaTexto(tablaInterna, string.Format("OBLIGADO A LLEVAR CONTABILIDAD: {0}", obligadoContabilidad)).FixedHeight = 20f;

            var informacionesAdicionales = comprobanteEle
                .Element("infoAdicional")?
                .Elements("campoAdicional")
                .Select(ia => new
                {
                    Nombre = ia.GetAttributeValue("nombre"),
                    Valor = ia.Value
                });
            bool AgenteInfoA = false;
            bool MicroEmp = false;
            if (informacionesAdicionales != null)
            {
                foreach (var infoAdicional in informacionesAdicionales)
                {
                    if (infoAdicional.Nombre == "Agente")
                    {
                        AgenteInfoA = true;
                    }
                    if (infoAdicional.Nombre == "Contribuyente Régimen Microempresas")
                    {
                        MicroEmp = true;
                    }

                    if (infoAdicional.Nombre == "Agente" || infoAdicional.Nombre == "Contribuyente Régimen Microempresas")
                    {
                        AgregarCeldaTexto(tablaInterna, infoAdicional.Valor).FixedHeight = 20f;
                    }
                }
            }

            if (!AgenteInfoA)
            {
                string regAgente = infoTributariaEle.GetElementValue("agenteRetencion") ?? string.Empty;
                if (!string.IsNullOrEmpty(regAgente))
                {
                    regAgente = regAgente.TrimStart(new Char[] { '0' });
                    AgregarCeldaTexto(tablaInterna, "Agente de Retención Resolución " + regAgente).FixedHeight = 20f;
                }
            }

            if (!MicroEmp)
            {
                string regAgente = infoTributariaEle.GetElementValue("regimenMicroempresas") ?? string.Empty;
                if (!string.IsNullOrEmpty(regAgente))
                {
                    AgregarCeldaTexto(tablaInterna, "Contribuyente Régimen Microempresas").FixedHeight = 20f;

                }
            }

            if (!string.IsNullOrEmpty(rimpe))
            {
                AgregarCeldaTexto(tablaInterna, rimpe).FixedHeight = 20f;
            }

            var retorno = new PdfPTable(1)
            {
                TotalWidth = 250f
            };

            AgregarCeldaTabla(retorno, tablaInterna);

            return retorno;
        }

        private PdfPTable CrearTablaProveedor(XElement comprobanteEle, string direccionCliente)
        {
            var infoLiqCompraEle = comprobanteEle.Element("infoLiquidacionCompra");

            var fechaEmisionStr = infoLiqCompraEle.GetElementValue("fechaEmision");
            var razonSocialProveedor = infoLiqCompraEle.GetElementValue("razonSocialProveedor");
            var identificacionProveedor = infoLiqCompraEle.GetElementValue("identificacionProveedor");
            var _direccionComprador = infoLiqCompraEle.GetElementValue("direccionProveedor");
            var direccionComprador = (_direccionComprador == null || _direccionComprador == "") ? direccionCliente : _direccionComprador;
            var tablaInterna = new PdfPTable(4)
            {
                TotalWidth = 540f
            };

            tablaInterna.SetWidths(new float[] { 74f, 294f, 74f, 98f });

            //Primera fila
            AgregarCeldaTextoProveedor(tablaInterna, "Razón Social / Nombres y Apellidos:");
            AgregarCeldaTextoProveedor(tablaInterna, razonSocialProveedor);
            AgregarCeldaTextoProveedor(tablaInterna, "Identificación:");
            AgregarCeldaTextoProveedor(tablaInterna, identificacionProveedor);
            //Segunda fila
            AgregarCeldaTextoProveedor(tablaInterna, "Fecha Emisión:");
            AgregarCeldaTextoProveedor(tablaInterna, fechaEmisionStr);
            AgregarCeldaTextoProveedor(tablaInterna, string.Empty);
            AgregarCeldaTextoProveedor(tablaInterna, string.Empty);
            //Tercera fila 
            AgregarCeldaTextoProveedor(tablaInterna, "Dirección:");
            AgregarCeldaTextoProveedor(tablaInterna, direccionComprador);
            AgregarCeldaTextoProveedor(tablaInterna, string.Empty);
            AgregarCeldaTextoProveedor(tablaInterna, string.Empty);
            var retorno = new PdfPTable(1);

            retorno.AddCell(new PdfPCell(tablaInterna)
            {
                Padding = 0f,
                Border = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER
            });

            return retorno;
        }

        private PdfPTable CrearTablaDetalle(XElement comprobanteEle)
        {
            var detalles = comprobanteEle
                .Element("detalles")
                .Elements("detalle")
                .Select(d => new
                {
                    CodigoPrincipal = d.GetElementValue("codigoPrincipal"),
                    CodigoAuxiliar = d.GetElementValue("codigoAuxiliar"),
                    Descripcion = d.GetElementValue("descripcion"),
                    UnidadMedida = d.GetElementValue("unidadMedida"),
                    CantidadStr = d.GetElementValue("cantidad"),
                    PrecioUnitarioStr = d.GetElementValue("precioUnitario"),
                    DescuentoStr = d.GetElementValue("descuento"),
                    PrecioTotalSinImpuestoStr = d.GetElementValue("precioTotalSinImpuesto"),
                    Adicionales = d.Element("detallesAdicionales")?
                        .Elements("detAdicional")
                        .Select(da => new
                        {
                            Nombre = da.GetAttributeValue("nombre"),
                            Valor = da.GetAttributeValue("valor")
                        }),
                    Impuestos = d.Element("impuestos")?
                        .Elements("impuesto")
                        .Select(da => new
                        {
                            Codigo = da.GetElementValue("codigo"),
                            PorcentajeCodigo = da.GetElementValue("codigoPorcentaje"),
                            TarifaStr = da.GetElementValue("tarifa"),
                            BaseImponibleStr = da.GetElementValue("baseImponible"),
                            ValorStr = da.GetElementValue("valor")
                        })
                });


            var retorno = new PdfPTable(8)
            {
                TotalWidth = 540f
            };

            retorno.SetWidths(new float[] { 54f, 217f, 36f, 45f, 45f, 45f, 47f, 51f });

            //Primera fila encabezado
            AgregarCeldaDetalleEncabezado(retorno, "Cod. Principal");
            AgregarCeldaDetalleEncabezado(retorno, "Descripción");
            AgregarCeldaDetalleEncabezado(retorno, "Unidades");
            AgregarCeldaDetalleEncabezado(retorno, "TM");
            AgregarCeldaDetalleEncabezado(retorno, "Precio Unitario");
            AgregarCeldaDetalleEncabezado(retorno, "Importe Bruto");
            AgregarCeldaDetalleEncabezado(retorno, "Descuento");
            AgregarCeldaDetalleEncabezado(retorno, "Importe Total");

            foreach (var detalle in detalles)
            {
                var cantidad = ADecimal(detalle.CantidadStr);
                var cantidadStr = ACadena(cantidad, 2);
                var precioUnitario = ADecimal(detalle.PrecioUnitarioStr);
                var precioUnitarioStr = ACadena(precioUnitario, 2);

                AgregarCeldaDetalle(retorno, detalle.CodigoPrincipal);
                AgregarCeldaDetalle(retorno, detalle.Descripcion, Rectangle.ALIGN_LEFT);
                AgregarCeldaDetalle(retorno, cantidadStr, Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, string.Empty);
                AgregarCeldaDetalle(retorno, precioUnitarioStr, Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, string.Empty);
                AgregarCeldaDetalle(retorno, detalle.DescuentoStr, Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, detalle.PrecioTotalSinImpuestoStr, Rectangle.ALIGN_RIGHT);
            }

            return retorno;
        }

        private string ObtenerDescripcionFormaPago(string formaPagoCodigo)
        {
            var clave = string.Format("F{0}", formaPagoCodigo);

            var valor = ConfigurationManager.AppSettings[clave];

            return string.IsNullOrEmpty(valor) ? string.Empty : valor;
        }

        private PdfPTable CrearTablaFormasPago(XElement comprobanteEle)
        {
            var formasPago = comprobanteEle
                .Element("infoLiquidacionCompra")
                .Element("pagos")?
                .Elements("pago")
                .Select(fp => new
                {
                    Codigo = fp.GetElementValue("formaPago"),
                    TotalStr = fp.GetElementValue("total"),
                    PlazoStr = fp.GetElementValue("plazo"),
                    UnidadTiempo = fp.GetElementValue("unidadTiempo")
                });

            var retorno = new PdfPTable(4)
            {
                TotalWidth = 250f
            };

            retorno.SetWidths(new float[] { 109f, 47f, 47f, 47f });

            AgregarCeldaDetalleEncabezado(retorno, "Forma de Pago");
            AgregarCeldaDetalleEncabezado(retorno, "Valor");
            AgregarCeldaDetalleEncabezado(retorno, "Plazo");
            AgregarCeldaDetalleEncabezado(retorno, "Tiempo");

            if (formasPago != null)
            {
                foreach (var formaPago in formasPago)
                {
                    var formaPagoDesc = ObtenerDescripcionFormaPago(formaPago.Codigo);

                    AgregarCeldaDetalle(retorno, formaPagoDesc, Rectangle.ALIGN_LEFT);
                    AgregarCeldaDetalle(retorno, formaPago.TotalStr, Rectangle.ALIGN_RIGHT);
                    AgregarCeldaDetalle(retorno, formaPago.PlazoStr, Rectangle.ALIGN_RIGHT);
                    AgregarCeldaDetalle(retorno, formaPago.UnidadTiempo, Rectangle.ALIGN_RIGHT);
                }
            }

            return retorno;
        }

        private PdfPTable CrearTablaInformacionAdicional(XElement comprobanteEle)
        {
            var informacionesAdicionales = comprobanteEle
                .Element("infoAdicional")?
                .Elements("campoAdicional")
                .Select(ia => new
                {
                    Nombre = ia.GetAttributeValue("nombre"),
                    Valor = ia.Value
                });

            var tablaInterna = new PdfPTable(2)
            {
                TotalWidth = 250f
            };

            tablaInterna.SetWidths(new float[] { 70f, 180f });

            tablaInterna.AddCell(new PdfPCell(new Paragraph("Información Adicional", m_fuenteArialSiete))
            {
                HorizontalAlignment = Rectangle.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER,
                Colspan = 2
            });

            if (informacionesAdicionales != null)
            {
                foreach (var infoAdicional in informacionesAdicionales)
                {
                    if (!(infoAdicional.Nombre == "Agente"
                        || infoAdicional.Nombre == "Contribuyente Régimen Microempresas"))
                    {
                        AgregarCeldaTextoAdicional(tablaInterna, infoAdicional.Nombre);
                        AgregarCeldaTextoAdicional(tablaInterna, infoAdicional.Valor);
                    }
                }
            }

            var retorno = new PdfPTable(1);

            retorno.AddCell(new PdfPCell(tablaInterna)
            {
                Padding = 0f,
                Border = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER
            });

            return retorno;
        }

        private PdfPTable CrearTablaTotales(XElement comprobanteEle)
        {
            var ceroStr = ACadena(0.00m, 2);

            var infoLiqCompraEle = comprobanteEle.Element("infoLiquidacionCompra");

            var totalesImpuestos = infoLiqCompraEle
                .Element("totalConImpuestos")
                .Elements("totalImpuesto")
                .Select(ti => new
                {
                    Codigo = ti.GetElementValue("codigo"),
                    PorcentajeCodigo = ti.GetElementValue("codigoPorcentaje"),
                    DescuentoAdicionalStr = ti.GetElementValue("descuentoAdicional"),
                    BaseImponibleStr = ti.GetElementValue("baseImponible"),
                    TarifaStr = ti.GetElementValue("tarifa"),
                    ValorStr = ti.GetElementValue("valor")
                });

            var totalImpuestoDocePorCiento = totalesImpuestos
                .Where(ti =>
                    (ti.Codigo == "2")
                    && ((ti.PorcentajeCodigo == "2") || (ti.PorcentajeCodigo == "3"))
                )
                .FirstOrDefault();

            var subtotalDocePorcientoStr = (totalImpuestoDocePorCiento?.BaseImponibleStr ?? ceroStr);
            var ivaDocePorCientoStr = (totalImpuestoDocePorCiento?.ValorStr ?? ceroStr);
            var ivaDocePorCientoEtiqueta = ((totalImpuestoDocePorCiento?.PorcentajeCodigo == "2") ? "12%" : "14%");

            var subtotalCeroPorcientoStr = (totalesImpuestos
                .Where(ti => (ti.Codigo == "2") && (ti.PorcentajeCodigo == "0"))
                .FirstOrDefault()?
                .BaseImponibleStr ?? ceroStr);

            var subtotalNoObjetoIvaStr = (totalesImpuestos
                .Where(ti => (ti.Codigo == "2") && (ti.PorcentajeCodigo == "6"))
                .FirstOrDefault()?
                .BaseImponibleStr ?? ceroStr);

            var subtotalExentoIvaStr = (totalesImpuestos
                .Where(ti => (ti.Codigo == "2") && (ti.PorcentajeCodigo == "7"))
                .FirstOrDefault()?
                .BaseImponibleStr ?? ceroStr);

            var ice = (totalesImpuestos
               .Where(ti => (ti.Codigo == "3"))
               .FirstOrDefault()?
               .ValorStr ?? ceroStr);

            var totalSinImpuestosStr = (infoLiqCompraEle.GetElementValue("totalSinImpuestos") ?? ceroStr);
            var totalDescuentoStr = (infoLiqCompraEle.GetElementValue("totalDescuento") ?? ceroStr);
            var totalImporteStr = (infoLiqCompraEle.GetElementValue("importeTotal") ?? ceroStr);

            var retorno = new PdfPTable(2)
            {
                TotalWidth = 180f
            };

            retorno.SetWidths(new float[] { 125f, 55f });

            AgregarCeldaDetalleEncabezado(retorno, string.Format("SUBTOTAL {0}", ivaDocePorCientoEtiqueta), Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, subtotalDocePorcientoStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "SUBTOTAL 0%", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, subtotalCeroPorcientoStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "SUBTOTAL NO OBJETO DE IVA", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, subtotalNoObjetoIvaStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "SUBTOTAL EXENTO DE IVA", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, subtotalExentoIvaStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "SUBTOTAL SIN IMPUESTOS", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, totalSinImpuestosStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "TOTAL DESCUENTO", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, totalDescuentoStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, string.Format("IVA {0}", ivaDocePorCientoEtiqueta), Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, ivaDocePorCientoStr, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "ICE", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, ice, Rectangle.ALIGN_RIGHT);
            AgregarCeldaDetalleEncabezado(retorno, "VALOR TOTAL", Rectangle.ALIGN_LEFT);
            AgregarCeldaDetalleEncabezado(retorno, totalImporteStr, Rectangle.ALIGN_RIGHT);

            return retorno;
        }

        private PdfPTable CrearTablaVacia(float totalWidth)
        {
            var retorno = new PdfPTable(1)
            {
                TotalWidth = totalWidth
            };

            retorno.AddCell(new PdfPCell(new Paragraph(string.Empty, m_fuenteArialOcho))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0f
            });

            return retorno;
        }

        private PdfPTable CrearTablaVacia(float totalWidth, float totalHeight)
        {
            var retorno = new PdfPTable(1)
            {
                TotalWidth = totalWidth
            };

            retorno.AddCell(new PdfPCell(new Paragraph(string.Empty, m_fuenteArialOcho))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0f,
                FixedHeight = totalHeight
            });

            return retorno;
        }

        private PdfPTable CrearTablaReembolso(XElement comprobanteEle)
        {

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

            var detalles = comprobanteEle
                .Element("reembolsos")?
                .Elements("reembolsoDetalle")
                .Select(d => new
                {
                    IdentificacionProveedorReembolso = d.GetElementValue("identificacionProveedorReembolso"),
                    CodDocReembolso = dictionary[int.Parse(d.GetElementValue("codDocReembolso").ToString())],
                    EstabDocReembolso = d.GetElementValue("estabDocReembolso"),
                    PtoEmiDocReembolso = d.GetElementValue("ptoEmiDocReembolso"),
                    SecuencialDocReembolso = d.GetElementValue("secuencialDocReembolso"),
                    FechaEmisionDocReembolso = d.GetElementValue("fechaEmisionDocReembolso"),
                    Impuestos = d.Element("detalleImpuestos")?
                        .Elements("detalleImpuesto")
                        .Select(da => new
                        {
                            Codigo = da.GetElementValue("codigo"),
                            PorcentajeCodigo = da.GetElementValue("codigoPorcentaje"),
                            TarifaStr = da.GetElementValue("tarifa"),
                            BaseImponibleStr = da.GetElementValue("baseImponibleReembolso"),
                            ValorStr = da.GetElementValue("impuestoReembolso")
                        })
                });

            if (detalles == null) return null;

            var retorno = new PdfPTable(12)
            {
                TotalWidth = 540f
            };

            retorno.SetWidths(new float[] { 50f, 50f, 50f, 30f, 30f, 30f, 30f, 25f, 25f, 25f, 25f, 25f });

            //Primera fila encabezado
            AgregarCeldaDetalleEncabezado(retorno, "Iden. Proveedor");
            AgregarCeldaDetalleEncabezado(retorno, "Documento");
            AgregarCeldaDetalleEncabezado(retorno, "No. Documento");
            AgregarCeldaDetalleEncabezado(retorno, "Fecha Emisión");
            AgregarCeldaDetalleEncabezado(retorno, "Base Imponible tarifa 0%");
            AgregarCeldaDetalleEncabezado(retorno, "Base Imponible tarifa IVA diferente de 0%");
            AgregarCeldaDetalleEncabezado(retorno, "Base Imponible no objeto de IVA");
            AgregarCeldaDetalleEncabezado(retorno, "Base imponible exenta de IVA");
            AgregarCeldaDetalleEncabezado(retorno, "Total Bases Imponibles");
            AgregarCeldaDetalleEncabezado(retorno, "Monto ICE");
            AgregarCeldaDetalleEncabezado(retorno, "Monto IVA");
            AgregarCeldaDetalleEncabezado(retorno, "Total");

            foreach (var detalle in detalles)
            {
                var numDoc = detalle.PtoEmiDocReembolso + "-" + detalle.EstabDocReembolso + "-" + detalle.SecuencialDocReembolso;
                var BaseImp_0 = 0M;
                var BaseImp_12 = 0M;
                var BaseNoObjeto = 0M;
                var BaseExento = 0M;
                var TotalBases = 0M;
                var MontoIva = 0M;
                var MontoIce = 0M;
                var Tot_Reem = 0M;

                foreach (var imp in detalle.Impuestos)
                {
                    var valor = ADecimal(imp.BaseImponibleStr) == null ? 0M : ADecimal(imp.BaseImponibleStr).Value;

                    switch (imp.PorcentajeCodigo)
                    {
                        case "0":
                            BaseImp_0 += valor;
                            break;
                        case "2":
                            BaseImp_12 += valor;
                            break;
                        case "6":
                            BaseNoObjeto += valor;
                            break;
                        case "7":
                            BaseExento += valor;
                            break;
                    }

                    MontoIva += ADecimal(imp.ValorStr) == null ? 0M : ADecimal(imp.ValorStr).Value;
                    if (imp.Codigo.Equals("3")) MontoIce += ADecimal(imp.ValorStr) == null ? 0M : ADecimal(imp.ValorStr).Value;
                }

                TotalBases = BaseImp_0 + BaseImp_12 + BaseNoObjeto + BaseExento;
                Tot_Reem = TotalBases + MontoIva;

                AgregarCeldaDetalle(retorno, detalle.IdentificacionProveedorReembolso);
                AgregarCeldaDetalle(retorno, detalle.CodDocReembolso);
                AgregarCeldaDetalle(retorno, numDoc);
                AgregarCeldaDetalle(retorno, detalle.FechaEmisionDocReembolso);
                AgregarCeldaDetalle(retorno, ACadena(BaseImp_0, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(BaseImp_12, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(BaseNoObjeto, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(BaseExento, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(TotalBases, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(MontoIce, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(MontoIva, 2), Rectangle.ALIGN_RIGHT);
                AgregarCeldaDetalle(retorno, ACadena(Tot_Reem, 2), Rectangle.ALIGN_RIGHT);
            }

            return retorno;
        }

        #endregion

        #region Metodos de conversion

        private decimal? ADecimal(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            var aux = 0m;

            if (decimal.TryParse(valor, NumberStyles.Any, m_proveedorFormato, out aux))
            {
                return aux;
            }

            return null;
        }

        private string ACadena(decimal? valor, int cantidadDecimales)
        {
            if (!valor.HasValue)
            {
                return string.Empty;
            }

            var formatoMoneda = "F" + cantidadDecimales.ToString(CultureInfo.InvariantCulture);

            return valor.Value.ToString(formatoMoneda, m_proveedorFormato);
        }

        #endregion

        #endregion

    }
}

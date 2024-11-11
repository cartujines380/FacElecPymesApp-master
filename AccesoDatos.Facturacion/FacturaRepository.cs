using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.AccesoDatos.EntLib;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public class FacturaRepository : IFacturaRepository
    {
        #region Campos

        private readonly Database m_database;
        private bool m_desechado = false;

        #endregion

        #region Constructores

        public FacturaRepository(IDatabaseProvider databaseProvider)
        {
            if (databaseProvider == null)
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }

            m_database = databaseProvider.GetDatabase();

            if (m_database == null)
            {
                throw new ArgumentNullException(nameof(databaseProvider));
            }
        }

        #endregion

        #region Metodos privados

        private void Leer(IDataReader reader, Sucursal catalago)
        {
            catalago.CodEstablecimiento = reader.ColumnToString("Establecimiento");
        }

        private void Leer(IDataReader reader, PuntoEmision catalago)
        {
            catalago.CodPuntoEmision = reader.ColumnToString("PtoEmision");
        }

        private void Leer(IDataReader reader, Secuencial catalago)
        {
            catalago.CodSecuencial = reader.ColumnToInt32("Secuencial") ?? 0;
        }

        private int Leer(IDataReader reader)
        {
            var total = reader.ColumnToInt32("Total") ?? 0;

            return total;
        }

        private void Leer2(IDataReader reader, ObtenerInfoRIMPEResponse rimpe)
        {
            rimpe.TipoRIMPE = reader.ColumnToString("tipoRIMPE");
            rimpe.Detalle = reader.ColumnToString("detalle");
        }

        private XDocument GenerarXmlSecuenciales(ObtenerSecuencialRequest request)
        {
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("d",
                    new XAttribute("ac", request.Tipo == "C" ? "C3" : "C2"),
                    new XAttribute("usuario", request.Usuario),
                    new XAttribute("ru", request.EmpresaRuc),
                    new XAttribute("es", request.Establecimiento),
                    new XAttribute("pe", request.PuntoEmision),
                    new XAttribute("tipodoc", request.DocumentoTipo)
                )
            );
        }

        private XDocument CrearComprobante(Factura factura, string usuario)
        {
            var ivaDoceCodigoPorcentaje = factura.ObtenerIvaDoceCodigoPorcentaje();

            var totalConImpuestoICE = factura.Detalle.GroupBy(row => row.IceCodigo)
                .Select(group => new
                {
                    IceCodigo = group.Key,
                    IceValor = group.Select(row => row.IceValor).FirstOrDefault(),
                    BaseImponibleIceSum = group.Sum(row => row.BaseImponible),
                    ImpuestosValorIceSum = group.Sum(row => row.ImpuestosValorIce)
                });

            AgregarInformacionAdicionalRIMPE(factura);

            var facturaEle = new XElement("f",
                new XAttribute("rs", factura.RazonSocial),
                null,
                new XAttribute("r", factura.EmpresaRuc),
                new XAttribute("tdoc", "01"),
                new XAttribute("e", factura.EstablecimientoNumero),
                new XAttribute("pe", factura.PuntoEmisionNumero),
                new XAttribute("s", string.Empty),
                new XAttribute("dm", factura.MatrizDireccion),
                new XAttribute("fe", UtilFormato.ACadena(factura.FechaEmision)),
                new XAttribute("rm", factura.GuiaRemision),
                null,
                string.IsNullOrEmpty(factura.ContribuyenteEspecialNumero) ? null : new XAttribute("cce", factura.ContribuyenteEspecialNumero),
                string.IsNullOrEmpty(factura.ObligadoContabilidad) ? null : new XAttribute("oc", factura.ObligadoContabilidad),
                new XAttribute("tic", factura.TipoIdentificacionCodigo),
                new XAttribute("rsc", factura.RazonSocialProveedor),
                new XAttribute("ic", factura.Identificacion),
                new XAttribute("tsi", UtilFormato.ACadena(factura.SubtotalSinImpuestos)),
                new XAttribute("td", UtilFormato.ACadena(factura.TotalDescuento)),
                new XAttribute("p", UtilFormato.ACadena(0M)),
                new XAttribute("it", UtilFormato.ACadena(factura.Total)),
                new XAttribute("m", factura.Moneda),
                null,
                string.IsNullOrEmpty(factura.ProveedorDireccion) ? null : new XAttribute("dp", factura.ProveedorDireccion),
                string.IsNullOrEmpty(factura.CompradorDireccion) ? null : new XAttribute("dc", factura.CompradorDireccion),
                new XAttribute("us", usuario),
                factura.EsRimpe ? new XAttribute("ri", "CONTRIBUYENTE RÉGIMEN RIMPE") : null,
                factura.EsExportacion ? new XAttribute("ecet", "EXPORTADOR") : null,
                factura.EsExportacion ? new XAttribute("eitf", factura.DefinicionTermino) : null,
                factura.EsExportacion ? new XAttribute("elit", factura.LugarConvenio) : null,
                factura.EsExportacion ? new XAttribute("epor", factura.PaisOrigen) : null,
                factura.EsExportacion ? new XAttribute("epem", factura.PuertoEmbarque) : null,
                factura.EsExportacion ? new XAttribute("epud", factura.PuertoDestino) : null,
                factura.EsExportacion ? new XAttribute("epde", factura.PaisDestino) : null,
                factura.EsExportacion ? new XAttribute("epad", factura.PaisAdquisicion) : null,
                factura.EsExportacion ? new XAttribute("eiti", factura.DefTerminoSinImpuesto) : null,
                factura.EsExportacion ? new XAttribute("efin", factura.FleteInternacional) : null,
                factura.EsExportacion ? new XAttribute("esin", factura.SeguroInternacional) : null,
                factura.EsExportacion ? new XAttribute("egad", factura.GastosAduaneros) : null,
                factura.EsExportacion ? new XAttribute("egto", factura.GastosTransporte) : null,
                factura.EsReembolso ? new XAttribute("rcdr", "41") : null,
                factura.EsReembolso ? new XAttribute("rtcr", UtilFormato.ACadena(factura.Reembolso.Total)) : null,
                factura.EsReembolso ? new XAttribute("rtbr", UtilFormato.ACadena(factura.Reembolso.TotalBaseImponible)) : null,
                factura.EsReembolso ? new XAttribute("rtir", UtilFormato.ACadena(factura.Reembolso.TotalImpuesto)) : null,
                factura.EsReembolso
                    ? factura.Reembolso.Detalle
                        .Select(dr => new XElement("fr",
                            new XAttribute("rdtipr", dr.Proveedor.TipoIdentificacionCodigo),
                            new XAttribute("rdipr", dr.Proveedor.Identificacion),
                            new XAttribute("rdppr", dr.Proveedor.PaisPagoCodigo),
                            new XAttribute("rdtpr", dr.Proveedor.Tipo),
                            new XAttribute("rdcdr", dr.Documento.Codigo),
                            new XAttribute("rdedr", dr.Documento.EstablecimientoCodigo),
                            new XAttribute("rdpdr", dr.Documento.PuntoEmisionCodigo),
                            new XAttribute("rdsdr", dr.Documento.Secuencial),
                            new XAttribute("rdfdr", dr.Documento.FechaEmisionStr),
                            new XAttribute("rdndr", dr.Documento.NumeroAutorizacion),
                            new XElement("fri",
                                new XAttribute("cr", dr.Impuesto.Codigo),
                                new XAttribute("cp", dr.Impuesto.PorcentajeCodigo),
                                new XAttribute("tr", UtilFormato.Truncate((dr.Impuesto.PorcentajeValor ?? 0M) * 100M, 0)),
                                new XAttribute("bi", UtilFormato.ACadena(dr.Impuesto.BaseImponible)),
                                new XAttribute("vi", UtilFormato.ACadena(dr.Impuesto.ImpuestosValor)) // Valor del impuesto del reembolso
                            )
                        )
                    )
                   : null,
                factura.SubtotalCeroPorciento != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "0"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalCeroPorciento)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                factura.SubtotalDocePorciento != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", ivaDoceCodigoPorcentaje),
                        new XAttribute("bi", ivaDoceCodigoPorcentaje.Equals("2")
                            ? UtilFormato.ACadena(factura.SubtotalDocePorcientoConIce)
                            : UtilFormato.ACadena(factura.SubtotalDocePorciento)
                        ),
                        new XAttribute("t", factura.ObtenerIvasManuales()),
                        new XAttribute("v", UtilFormato.ACadena(factura.IvaDocePorCiento))// Valor
                    )
                    : null,
                factura.SubtotalNoObjetoIva != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "6"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalNoObjetoIva)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                factura.SubtotalExentoIva != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "7"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalExentoIva)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                totalConImpuestoICE
                    .Where(x => x.IceValor != null && x.IceValor.HasValue && !x.IceCodigo.Equals("0"))
                    .Select(tc => new XElement("tc",
                        new XAttribute("c", "3"),
                        new XAttribute("cp", tc.IceCodigo),
                        new XAttribute("bi", tc.BaseImponibleIceSum),
                        new XAttribute("t", UtilFormato.Truncate((tc.IceValor ?? 0M) * 100M, 0)),
                        new XAttribute("v", tc.ImpuestosValorIceSum)

                     )),
                factura.FormasPago
                    .Where(fp => !fp.EsVacio)
                    .Select(fp => new XElement("fp",
                        new XAttribute("cf", fp.FormaPagoCodigo),
                        new XAttribute("to", fp.MontoStr),
                        string.IsNullOrEmpty(fp.TiempoCodigo) ? null : new XAttribute("pf", fp.Plazo),
                        string.IsNullOrEmpty(fp.TiempoCodigo) ? null : new XAttribute("uf", fp.TiempoCodigo) // Unidad de Tiempo Forma de Pago
                    )),
                factura.Detalle
                    .Where(d => !d.EsVacio)
                    .Select(d => new XElement("d",
                        new XAttribute("cp", d.ProductoCodigo),
                        null,
                        new XAttribute("de", d.Descripcion),
                        new XAttribute("c", d.CantidadStr),
                        new XAttribute("pu", d.PrecioUnitarioStr),
                        new XAttribute("d", d.DescuentoStr),
                        new XAttribute("ptsi", UtilFormato.ACadena(d.BaseImponible)),
                        null,
                        new XElement("i",
                            new XAttribute("c", "2"),
                            new XAttribute("cp", d.ImpuestoModelo.Codigo),
                            new XAttribute("t", UtilFormato.Truncate((d.ImpuestoModelo.Valor ?? 0M) * 100M, 0)),
                            new XAttribute("bi", d.ImpuestoModelo.Codigo.Equals("2")
                                ? UtilFormato.ACadena(d.BaseImponibleConImpuestoICE)
                                : UtilFormato.ACadena(d.BaseImponible)
                            ),
                            new XAttribute("v", d.ImpuestoModelo.Codigo.Equals("2")
                                ? UtilFormato.ACadena(d.ImpuestosValorConImpuestoICE)
                                : UtilFormato.ACadena(d.ImpuestosValor)
                            ) // Valor
                        ),
                        (string.IsNullOrEmpty(d.IceValorStr) || d.IceCodigo.Equals("0"))
                            ? null
                            : new XElement("i",
                                new XAttribute("c", "3"),
                                new XAttribute("cp", d.IceCodigo),
                                new XAttribute("t", UtilFormato.Truncate((d.IceValor ?? 0M) * 100M, 0)),
                                new XAttribute("bi", UtilFormato.ACadena(d.BaseImponible)),
                                new XAttribute("v", UtilFormato.ACadena(d.ImpuestosValorIce)) // Valor
                               )
                    )
                )
            );

            var infoAdicional = factura.Adicionales
                    .Where(a => !a.EsVacio)
                    .Select(a => new XElement("ia",
                        new XAttribute("n", a.Codigo),
                        new XAttribute("v", a.Valor)
                     ));

            var valorEmail = "valorEmail|";

            foreach (var item in infoAdicional)
            {
                var nombre = item.FirstAttribute.Value;
                var valor = item.LastAttribute.Value;

                if (nombre == "email" | nombre == "Email")
                {
                    if (!valorEmail.Contains(valor))
                    {
                        facturaEle.Add(new XElement(item));
                        valorEmail += valor + "|";
                    }
                }
                else
                    facturaEle.Add(new XElement(item));
            }

            var secRequest = new ObtenerSecuencialRequest()
            {
                Usuario = usuario,
                EmpresaRuc = factura.EmpresaRuc,
                DocumentoTipo = "01",
                Establecimiento = factura.EstablecimientoNumero,
                PuntoEmision = factura.PuntoEmisionNumero
            };

            var secInt = ObtenerSecuencial(secRequest);

            factura.Secuencial = secInt.ToString().PadLeft(9, '0');

            facturaEle.SetAttributeValue("s", factura.Secuencial);

            return new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    facturaEle
                   );
        }

        private XDocument CrearComprobanteTransportista(Factura factura, string usuario)
        {
            var ivaDoceCodigoPorcentaje = factura.ObtenerIvaDoceCodigoPorcentaje();

            var totalConImpuestoICE = factura.Detalle.GroupBy(row => row.IceCodigo)
                .Select(group => new
                {
                    IceCodigo = group.Key,
                    IceValor = group.Select(row => row.IceValor).FirstOrDefault(),
                    BaseImponibleIceSum = group.Sum(row => row.BaseImponible),
                    ImpuestosValorIceSum = group.Sum(row => row.ImpuestosValorIce)
                });

            var facturaEle = new XElement("f",
                new XAttribute("rs", factura.RazonSocial),
                new XAttribute("r", factura.EmpresaRuc),
                new XAttribute("tdoc", "01"),
                new XAttribute("e", factura.EstablecimientoNumero),
                new XAttribute("pe", factura.PuntoEmisionNumero),
                new XAttribute("s", string.Empty),
                new XAttribute("dm", factura.MatrizDireccion != null ? factura.MatrizDireccion : ""),
                new XAttribute("rg", factura.Regimen),
                new XAttribute("rm", factura.GuiaRemision != null ? factura.GuiaRemision : ""),

                new XAttribute("rt", factura.RazonSocialTransporista),
                new XAttribute("rr", factura.RucTransportista),
                new XAttribute("pt", factura.PuntoEmisionTransportista),
                new XAttribute("fe", UtilFormato.ACadena(factura.FechaEmision)),

                string.IsNullOrEmpty(factura.ContribuyenteEspecialNumero) ? null : new XAttribute("cce", factura.ContribuyenteEspecialNumero),
                string.IsNullOrEmpty(factura.ObligadoContabilidad) ? null : new XAttribute("oc", factura.ObligadoContabilidad),
                new XAttribute("tic", factura.TipoIdentificacionCodigo),
                new XAttribute("rsc", factura.RazonSocialProveedor),
                new XAttribute("ic", factura.Identificacion),
                new XAttribute("tsi", UtilFormato.ACadena(factura.SubtotalSinImpuestos)),
                new XAttribute("td", UtilFormato.ACadena(factura.TotalDescuento)),
                new XAttribute("p", UtilFormato.ACadena(0M)),
                new XAttribute("it", UtilFormato.ACadena(factura.Total)),
                new XAttribute("m", factura.Moneda),
                new XAttribute("us", usuario),

                string.IsNullOrEmpty(factura.CompradorDireccion) ? null : new XAttribute("dc", factura.CompradorDireccion),


                factura.SubtotalCeroPorciento != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "0"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalCeroPorciento)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                factura.SubtotalDocePorciento != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", ivaDoceCodigoPorcentaje),
                        new XAttribute("bi", ivaDoceCodigoPorcentaje.Equals("2")
                            ? UtilFormato.ACadena(factura.SubtotalDocePorcientoConIce)
                            : UtilFormato.ACadena(factura.SubtotalDocePorciento)
                        ),
                        new XAttribute("t", factura.ObtenerIvasManuales()),
                        new XAttribute("v", UtilFormato.ACadena(factura.IvaDocePorCiento))// Valor
                    )
                    : null,
                factura.SubtotalNoObjetoIva != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "6"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalNoObjetoIva)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                factura.SubtotalExentoIva != 0M
                    ? new XElement("tc",
                        new XAttribute("c", "2"),
                        new XAttribute("cp", "7"),
                        new XAttribute("bi", UtilFormato.ACadena(factura.SubtotalExentoIva)),
                        new XAttribute("t", "0"),
                        new XAttribute("v", "0.00")// Valor
                      )
                    : null,
                totalConImpuestoICE
                    .Where(x => x.IceValor != null && x.IceValor.HasValue && !x.IceCodigo.Equals("0"))
                    .Select(tc => new XElement("tc",
                        new XAttribute("c", "3"),
                        new XAttribute("cp", tc.IceCodigo),
                        new XAttribute("bi", tc.BaseImponibleIceSum),
                        new XAttribute("t", UtilFormato.Truncate((tc.IceValor ?? 0M) * 100M, 0)),
                        new XAttribute("v", tc.ImpuestosValorIceSum)

                     )),
                new XAttribute("ectt", "TRANSPORTISTA"),
                factura.FormasPago
                    .Where(fp => !fp.EsVacio)
                    .Select(fp => new XElement("fp",
                        new XAttribute("cf", fp.FormaPagoCodigo),
                        new XAttribute("to", fp.MontoStr),
                        string.IsNullOrEmpty(fp.TiempoCodigo) ? null : new XAttribute("pf", fp.Plazo),
                        string.IsNullOrEmpty(fp.TiempoCodigo) ? null : new XAttribute("uf", fp.TiempoCodigo) // Unidad de Tiempo Forma de Pago
                    )),
                factura.Detalle
                    .Where(d => !d.EsVacio)
                    .Select(d => new XElement("d",
                        new XAttribute("cp", d.ProductoCodigo),
                        null,
                        new XAttribute("de", d.Descripcion),
                        new XAttribute("c", d.CantidadStr),
                        new XAttribute("pu", d.PrecioUnitarioStr),
                        new XAttribute("d", d.DescuentoStr),
                        new XAttribute("ptsi", UtilFormato.ACadena(d.BaseImponible)),
                        null,
                        new XElement("i",
                            new XAttribute("c", "2"),
                            new XAttribute("cp", d.ImpuestoModelo.Codigo),
                            new XAttribute("t", UtilFormato.Truncate((d.ImpuestoModelo.Valor ?? 0M) * 100M, 0)),
                            new XAttribute("bi", d.ImpuestoModelo.Codigo.Equals("2")
                                ? UtilFormato.ACadena(d.BaseImponibleConImpuestoICE)
                                : UtilFormato.ACadena(d.BaseImponible)
                            ),
                            new XAttribute("v", d.ImpuestoModelo.Codigo.Equals("2")
                                ? UtilFormato.ACadena(d.ImpuestosValorConImpuestoICE)
                                : UtilFormato.ACadena(d.ImpuestosValor)
                            ) // Valor
                        ),
                        (string.IsNullOrEmpty(d.IceValorStr) || d.IceCodigo.Equals("0"))
                            ? null
                            : new XElement("i",
                                new XAttribute("c", "3"),
                                new XAttribute("cp", d.IceCodigo),
                                new XAttribute("t", UtilFormato.Truncate((d.IceValor ?? 0M) * 100M, 0)),
                                new XAttribute("bi", UtilFormato.ACadena(d.BaseImponible)),
                                new XAttribute("v", UtilFormato.ACadena(d.ImpuestosValorIce)) // Valor
                               )
                    )
                ),
                string.IsNullOrEmpty(factura.ReceptorEmail)
                    ? null
                    : new XElement("tn",
                        new XAttribute("v", factura.ReceptorEmail != null ? factura.ReceptorEmail : "")
                    )
            );

            var infoAdicional = factura.Adicionales
                    .Where(a => !a.EsVacio)
                    .Select(a => new XElement("ia",
                        new XAttribute("n", a.Codigo),
                        new XAttribute("v", a.Valor)
                     ));

            var valorEmail = "valorEmail|";

            foreach (var item in infoAdicional)
            {
                var nombre = item.FirstAttribute.Value;
                var valor = item.LastAttribute.Value;

                if (nombre == "email" | nombre == "Email")
                {
                    if (!valorEmail.Contains(valor))
                    {
                        facturaEle.Add(new XElement(item));
                        valorEmail += valor + "|";
                    }
                }
                else
                {
                    facturaEle.Add(new XElement(item));
                }
            }

            var secRequest = new ObtenerSecuencialRequest()
            {
                Usuario = usuario,
                EmpresaRuc = factura.EmpresaRuc,
                DocumentoTipo = "01",
                Establecimiento = factura.EstablecimientoNumero,
                PuntoEmision = factura.PuntoEmisionNumero
            };

            IngrsarInfoAdicionalDatosTransportistas(factura, facturaEle);
            AgregarInformacionAdicionalRIMPE(factura);

            var secInt = ObtenerSecuencial(secRequest);

            factura.Secuencial = secInt.ToString().PadLeft(9, '0');

            facturaEle.SetAttributeValue("s", factura.Secuencial);

            return new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    facturaEle
                   );
        }

        private void IngrsarInfoAdicionalDatosTransportistas(Factura factModelo, XElement facturaEle)
        {
            facturaEle.Add(new XElement(!ExisteInfoAdicionalFactura(facturaEle, "PuntodeEmision") ?
                new XElement("ia",
                        new XAttribute("n", "PuntodeEmision"),
                        new XAttribute("v", factModelo.PuntoEmisionTransportista)
                        ) : null));

            facturaEle.Add(new XElement(!ExisteInfoAdicionalFactura(facturaEle, "RUC") ?
               new XElement("ia",
                       new XAttribute("n", "RUC"),
                       new XAttribute("v", factModelo.RucTransportista)
                       ) : null));

            facturaEle.Add(new XElement(!ExisteInfoAdicionalFactura(facturaEle, "RazonSocial") ?
              new XElement("ia",
                      new XAttribute("n", "RazonSocial"),
                      new XAttribute("v", factModelo.RazonSocialTransporista)
                      ) : null));

            facturaEle.Add(new XElement(!ExisteInfoAdicionalFactura(facturaEle, "Contribuyente") ?
              new XElement("ia",
                      new XAttribute("n", "Contribuyente"),
                      new XAttribute("v", factModelo.Regimen)
                      ) : null));
        }

        private bool ExisteInfoAdicionalFactura(XElement retorno, string nombre)
        {
            var elementoIA = retorno.Descendants(XName.Get("ia"));
            if (elementoIA.Count() > 0)
            {
                foreach (var item in elementoIA)
                {
                    var atributosIAn = item.Attributes("n");

                    foreach (var attr in atributosIAn)
                    {
                        if (attr.Value.ToString().ToLower().Equals(nombre.ToLower()))
                            return true;
                    }
                }
            }

            return false;
        }

        private void AgregarInformacionAdicionalRIMPE(Factura modelo)
        {
            var resultado = ObtenerInfoRIMPE(modelo.EmpresaRuc);
            var agregarInfo = false;
            var detalle = "";
            if (resultado != null)
            {
                var tipoRIMPE = resultado.TipoRIMPE;
                detalle = resultado.Detalle;
                if (tipoRIMPE != null && !tipoRIMPE.Equals("0"))
                {
                    agregarInfo = true;
                }
            }

            if (agregarInfo)
            {
                modelo.Adicionales.Add(new InformacionAdicional()
                {
                    Codigo = "RIMPE",
                    Valor = detalle,
                    ValorStr = detalle
                });
            }
        }

        private ObtenerInfoRIMPEResponse ObtenerInfoRIMPE(string Ruc)
        {
            var retorno = new ObtenerInfoRIMPEResponse();
            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_CONSESTABLECIMIENTO_RIMPE]"))
            {
                m_database.AddInParameter(cmd, "PI_Ruc", DbType.String, Ruc);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {

                        Leer2(reader, retorno);
                    }
                }
            }

            return retorno;
        }


        #endregion

        #region IFacturaRepository

        public Factura Add(Factura entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Factura entity)
        {
            throw new NotImplementedException();
        }

        public Factura Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Factura> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Factura> GetPaged(int pageIndex, int pageCount, bool ascending)
        {
            throw new NotImplementedException();
        }

        public void Update(Factura entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sucursal> ObtenerEstablecimiento(string ruc, string esTransportista)
        {
            var retorno = new List<Sucursal>();

            var sp = esTransportista == "Si" ? "[Comprobantes].[CMP_CONS_ESTABLECIMIENTO_TRANSP]" : "[Comprobantes].[CMP_CONS_ESTABLECIMIENTO]";

            using (var cmd = m_database.GetStoredProcCommand(sp))
            {
                m_database.AddInParameter(cmd, "Usuario", DbType.String, "usrmtraframe");
                m_database.AddInParameter(cmd, "Ruc", DbType.String, ruc);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var sucursal = new Sucursal();
                        Leer(reader, sucursal);

                        retorno.Add(sucursal);
                    }
                }
            }

            return retorno;
        }

        public IEnumerable<PuntoEmision> ObtenerPtoEmision(string ruc, string establecimiento, string esTransportista)
        {
            var retorno = new List<PuntoEmision>();

            var sp = esTransportista == "Si" ? "[Comprobantes].[CMP_CONS_PUNTO_EMISION_POR_ESTABL_TRANSP]" : "[Comprobantes].[CMP_CONS_PUNTO_EMISION_POR_ESTABL]";

            using (var cmd = m_database.GetStoredProcCommand(sp))
            {
                m_database.AddInParameter(cmd, "Usuario", DbType.String, "usrmtraframe");
                m_database.AddInParameter(cmd, "Ruc", DbType.String, ruc);
                m_database.AddInParameter(cmd, "Establ", DbType.String, establecimiento);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var ptoEmision = new PuntoEmision();
                        Leer(reader, ptoEmision);

                        retorno.Add(ptoEmision);
                    }
                }
            }

            return retorno;
        }

        public int ObtenerSecuencial(ObtenerSecuencialRequest request)
        {
            var retorno = 0;

            var xmlDatos = GenerarXmlSecuenciales(request);

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_MANT_SECUENCIALES]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlDatos", DbType.Xml, xmlDatos.ToString());

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var secuencial = new Secuencial();
                        Leer(reader, secuencial);

                        retorno = secuencial.CodSecuencial;
                    }
                }
            }

            return retorno;
        }

        public void GuardarFactura(Factura factura, string usuario)
        {
            var comprobanteXml = factura.Estransportista
                ? CrearComprobanteTransportista(factura, usuario).ToString()
                : CrearComprobante(factura, usuario).ToString();

            using (var cmd = m_database.GetStoredProcCommand("[Documento].[Doc_PROC_INGRESADOCUMENTO]"))
            {
                m_database.AddInParameter(cmd, "PI_XmlDatos", DbType.Xml, comprobanteXml);

                m_database.ExecuteNonQuery(cmd);
            }
        }

        public int ObtenerTotalComprobante(string ruc, string estado)
        {
            var retorno = 0;

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_TOTAL_ESTADO_COMPROBANTES]"))
            {
                m_database.AddInParameter(cmd, "RUC", DbType.String, ruc);
                m_database.AddInParameter(cmd, "ESTADO", DbType.String, estado);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno = Leer(reader);
                    }
                }
            }

            return retorno;
        }

        public int ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc)
        {
            var retorno = 0;

            using (var cmd = m_database.GetStoredProcCommand("[Comprobantes].[CMP_TOTAL_COMPROBANTES]"))
            {
                m_database.AddInParameter(cmd, "RUC", DbType.String, ruc);
                m_database.AddInParameter(cmd, "ESTADO", DbType.String, estado);
                m_database.AddInParameter(cmd, "TIPO_DOC", DbType.String, tipoDoc);

                using (var reader = m_database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        retorno = Leer(reader);
                    }
                }
            }

            return retorno;
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                //Desechar
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        ~FacturaRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

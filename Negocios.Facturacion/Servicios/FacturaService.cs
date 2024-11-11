using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using Sipecom.FactElec.Pymes.Negocios.Facturacion.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class FacturaService : IFacturaService
    {
        #region Campos

        private readonly IEstablecimientoRepository m_establecimientoRepository;
        private readonly IFacturaRepository m_facturaRepository;
        private readonly ICatalogoRepository m_catalogoRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public FacturaService(
            IEstablecimientoRepository establecimientoRepository,
            IFacturaRepository facturaRepository,
            ICatalogoRepository catalogoRepository
        )
        {
            m_establecimientoRepository = establecimientoRepository ?? throw new ArgumentNullException(nameof(establecimientoRepository));
            m_facturaRepository = facturaRepository ?? throw new ArgumentNullException(nameof(facturaRepository));
            m_catalogoRepository = catalogoRepository ?? throw new ArgumentNullException(nameof(catalogoRepository));
        }

        #endregion

        #region Metodos privados

        private void AgregarRegimenMicroEmpresaInfo(Factura factura)
        {
            var regimenes = m_establecimientoRepository.ObtenerRegimenMicroempresaData(
                factura.EmpresaRuc,
                factura.FechaEmision.Value
            );

            if (regimenes.Any())
            {
                factura.Adicionales.Add(new InformacionAdicional()
                {
                    Codigo = "Contribuyente Régimen Microempresas",
                    Valor = "Contribuyente Régimen Microempresas"
                });
            }
        }

        private void AgregarRimpeInfo(Factura factura)
        {
            var rimpes = m_establecimientoRepository.ObtenerRimpeData(factura.EmpresaRuc);

            factura.EsRimpe = rimpes.Any();

            var rimpeNoDefault = rimpes
                .Where(r => !(r.Tipo == "0"))
                .FirstOrDefault();

            if (rimpeNoDefault != null)
            {
                factura.Adicionales.Add(new InformacionAdicional()
                {
                    Codigo = "RIMPE",
                    Valor = rimpeNoDefault.Detalle
                });
            }
        }

        private void CompletarFacturaDetalle(FacturaDetalle detalle, IEnumerable<ImpuestoData> impuestos, IEnumerable<ImpuestoRetencion> impuestosRetencionFuente)
        {
            if (!string.IsNullOrEmpty(detalle.PorcentajeIvaCodigo) && impuestos != null)
            {
                var impuestosIva = impuestos.Where(x => x.Codigo == detalle.PorcentajeIvaCodigo && x.ImpuestoRetener == 2016);

                if (impuestosIva.Any())
                {
                    var impuestoIva = impuestosIva.First();
                    detalle.PorcentajeIvaValor = UtilFormato.Truncate(impuestoIva.Valor, 2);
                }
            }

            if (!string.IsNullOrEmpty(detalle.IceCodigo) && impuestos != null)
            {
                var impuestosIce = impuestos.Where(x => x.Codigo == detalle.IceCodigo && x.ImpuestoRetener == 2019);

                if (impuestosIce.Any())
                {
                    var impuestoIce = impuestosIce.First();
                    detalle.IceValor = UtilFormato.Truncate(impuestoIce.Valor, 2);

                    if (impuestoIce.Descripcion.Equals("No grava ICE"))
                        detalle.AplicaIce = "Si";
                    else
                        detalle.AplicaIce = "No";
                }
            }

            if (impuestosRetencionFuente.Any())
            {
                var impuestoRetencionFuente = impuestosRetencionFuente.First();
                detalle.PorcentajeRetencionValor = impuestoRetencionFuente.Porcentaje;
            }
        }

        private IEnumerable<ImpuestoData> ObtenerImpuestosIvaIce()
        {
            var retorno = m_catalogoRepository.ObtenerImpuestos();
            return retorno;
        }

        private IEnumerable<ImpuestoRetencion> ObtenerImpuestosRetencionFuenteModelo(int impuestoRetenerId, string tipoRetencion)
        {
            var retorno = m_catalogoRepository.ObtenerImpuestosRetencionFuente(impuestoRetenerId, tipoRetencion);

            return retorno;
        }

        private Factura CrearFactura(Factura factura)
        {
            var retorno = new Factura();

            retorno.EmpresaRuc = factura.EmpresaRuc;
            retorno.EstablecimientoNumero = factura.EstablecimientoNumero;
            retorno.PuntoEmisionNumero = factura.PuntoEmisionNumero;
            retorno.FechaEmision = factura.FechaEmision;
            retorno.TipoIdentificacionCodigo = factura.TipoIdentificacionCodigo;
            retorno.RazonSocial = factura.RazonSocial;
            retorno.Email = factura.Email;
            retorno.Identificacion = factura.Identificacion;
            retorno.RazonSocialProveedor = factura.RazonSocialProveedor;
            retorno.EsReembolso = factura.EsReembolso;
            retorno.EsExportacion = factura.EsExportacion;
            retorno.Estransportista = factura.Estransportista;
            retorno.DefinicionTermino = QuitarEspaciosEnBlanco(factura.DefinicionTermino);
            retorno.DefTerminoSinImpuesto = QuitarEspaciosEnBlanco(factura.DefTerminoSinImpuesto);
            retorno.PuertoEmbarque = QuitarEspaciosEnBlanco(factura.PuertoEmbarque);
            retorno.PaisAdquisicion = factura.PaisAdquisicionCodigo != null ? factura.PaisAdquisicionCodigo.PadLeft(3, '0') : "";
            retorno.PaisOrigen = factura.PaisOrigenCodigo != null ? factura.PaisOrigenCodigo.PadLeft(3, '0') : "";
            retorno.FleteInternacional = factura.FleteInternacional;
            retorno.LugarConvenio = QuitarEspaciosEnBlanco(factura.LugarConvenio);
            retorno.SeguroInternacional = factura.SeguroInternacional;
            retorno.PuertoDestino = QuitarEspaciosEnBlanco(factura.PuertoDestino);
            retorno.GastosAduaneros = factura.GastosAduaneros;
            retorno.PaisDestino = factura.PaisDestinoCodigo != null ? factura.PaisDestinoCodigo.PadLeft(3, '0') : "";
            retorno.GastosTransporte = factura.GastosTransporte;
            retorno.GuiaRemision = factura.GuiaRemision;

            var impuestos = ObtenerImpuestosIvaIce();
            var impuestosRetencionFuente = ObtenerImpuestosRetencionFuenteModelo(4, "F");

            foreach (var detalle in factura.Detalle)
            {
                var detalleNew = new FacturaDetalle
                {
                    ProductoCodigo = detalle.ProductoCodigo,
                    Descripcion = detalle.Descripcion,
                    Cantidad = detalle.Cantidad,
                    Descuento = detalle.Descuento,
                    PrecioUnitario = detalle.PrecioUnitario,
                    PorcentajeIvaCodigo = detalle.PorcentajeIvaCodigo,
                    IceCodigo = detalle.IceCodigo
                };

                CompletarFacturaDetalle(detalleNew, impuestos, impuestosRetencionFuente);
                retorno.AgregarDetalle(detalleNew);
            }

            if (factura.FormasPago != null)
            {
                foreach (var formaPago in factura.FormasPago)
                {
                    var formaPagoNew = new FormaPago
                    {
                        FormaPagoCodigo = formaPago.FormaPagoCodigo,
                        Monto = formaPago.Monto,
                        TiempoCodigo = formaPago.TiempoCodigo,
                        Plazo = formaPago.Plazo
                    };

                    retorno.AgregarFormaPago(formaPagoNew);
                }
            }

            foreach(var infoAdicional in factura.Adicionales)
            {
                var infoAdicionalNew = new InformacionAdicional
                {
                    Codigo = infoAdicional.Codigo,
                    Valor = infoAdicional.Valor
                };

                retorno.AgregarAdicional(infoAdicionalNew);
            }

            foreach (var infoReembolso in factura.Reembolso.Detalle)
            {
                var infoReembolsoNew = new FacturaReembolsoDetalle
                {
                    Detalle = infoReembolso.Detalle,
                    Documento = infoReembolso.Documento,
                    Impuesto = infoReembolso.Impuesto,
                    Proveedor = infoReembolso.Proveedor
                };

                retorno.Reembolso.AgregarDetalle(infoReembolsoNew);
            }

            retorno.ActualizaTotal();

            return retorno;
        }

        private string QuitarEspaciosEnBlanco(string cadena)
        {
            var _cadena = "";
            if (cadena != null)
            {
                _cadena = Regex.Replace(cadena, @"\s+", String.Empty);
            }
            return _cadena;
        }

        #endregion

        #region IFacturaService

        public IEnumerable<Sucursal> ObtenerEstablecimiento(string ruc, string esTransportista)
        {
            var result = m_facturaRepository.ObtenerEstablecimiento(ruc, esTransportista);
            return result;
        }

        public IEnumerable<PuntoEmision> ObtenerPtoEmision(string ruc, string establecimiento, string esTransportista)
        {
            var result = m_facturaRepository.ObtenerPtoEmision(ruc, establecimiento, esTransportista);
            return result;
        }

        public int ObtenerSecuencial(ObtenerSecuencialRequest request)
        {
            var result = m_facturaRepository.ObtenerSecuencial(request);
            return result;
        }

        public GuardarFacturaResponse GuardarFactura(GuardarFacturaRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Factura == null)
            {
                throw new ArgumentNullException(nameof(request.Factura));
            }

            if (string.IsNullOrEmpty(request.Usuario))
            {
                throw new ArgumentNullException(nameof(request.Usuario));
            }

            var response = new GuardarFacturaResponse();

            var facturaNew = CrearFactura(request.Factura);
            var usuario = request.Usuario;

            var establecimiento = facturaNew.Estransportista
                    ? m_establecimientoRepository.ObtenerDataEstablecimientoTransportistaPorUsuarioEmpresa(usuario, facturaNew.EmpresaRuc)
                    : m_establecimientoRepository.ObtenerDataEstablecimientoPorUsuarioEmpresa(usuario, facturaNew.EmpresaRuc);

            if (establecimiento != null)
            {
                facturaNew.MatrizDireccion = establecimiento.MatrizDireccion;
                facturaNew.ContribuyenteEspecialNumero = establecimiento.EsContribuyenteEspecial;
                facturaNew.ObligadoContabilidad = (establecimiento.ObligadoContabilidad ?? false) ? "SI" : "NO";

                if (facturaNew.Estransportista)
                {
                    facturaNew.Regimen = establecimiento.Regimen.Trim();
                    facturaNew.RazonSocialTransporista = establecimiento.RazonSocialTransportista.Trim();
                    facturaNew.RucTransportista = establecimiento.RucTransportista.Trim();
                    facturaNew.PuntoEmisionTransportista = establecimiento.PuntoEmisionTransportista.Trim();
                }
            }

            facturaNew.Moneda = "DOLAR";

            var validacion = facturaNew.Validar(true);

            if (!validacion.EsValido)
            {
                foreach (var err in validacion.Errores)
                {
                    response.AgregarValidacion(err);
                }

                return response;
            }

            AgregarRegimenMicroEmpresaInfo(facturaNew);
            AgregarRimpeInfo(facturaNew);

            m_facturaRepository.GuardarFactura(facturaNew, usuario);

            return response;
        }

        public int ObtenerTotalComprobante(string ruc, string estado)
        {
            var result = m_facturaRepository.ObtenerTotalComprobante(ruc, estado);
            return result;
        }

        public int ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc)
        {
            var result = m_facturaRepository.ObtenerTotalComprobantePorTipo(ruc, estado, tipoDoc);
            return result;
        }

        #endregion

        #region IDispose

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                m_establecimientoRepository.Dispose();
                m_facturaRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

        ~FacturaService()
        {
            Dispose(false);
        }

        #endregion
    }
}

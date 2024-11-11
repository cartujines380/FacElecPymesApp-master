using System;
using System.Collections.Generic;
using System.Linq;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class Factura : Entity
    {
        #region Constantes

        private const string TIPO_IDENT_CONSUMIDOR_FINAL_CODIGO = "07";

        private const decimal TOTAL_FACTURA_CONSUMIDOR_FINAL_MAXIMO = 200M;

        #endregion

        #region Propiedades

        public string GuiaRemision { get; set; }

        public string Secuencial { get; set; }

        public string EmpresaRuc { get; set; }

        public string EstablecimientoNumero { get; set; }

        public string PuntoEmisionNumero { get; set; }

        public string MatrizDireccion { get; set; }

        public DateTime? FechaEmision { get; set; }

        public string ContribuyenteEspecialNumero { get; set; }

        public string ObligadoContabilidad { get; set; }

        public string TipoIdentificacionCodigo { get; set; }

        public string RazonSocial { get; set; }

        public string Email { get; set; }

        public string Identificacion { get; set; }

        public string CompradorDireccion { get; set; }

        public string RazonSocialCliente { get; set; }

        public string RazonSocialProveedor { get; set; }

        public string Moneda { get; set; }

        public string ProveedorDireccion { get; set; }

        public string ReceptorEmail { get; set; }

        public decimal SubtotalSinImpuestos { get; set; }

        public decimal TotalDescuento { get; set; }

        public decimal Total { get; set; }

        public decimal IvaDocePorCiento { get; set; }

        public decimal Ice { get; set; }

        public decimal SubtotalCeroPorciento { get; set; }

        public decimal SubtotalDocePorciento { get; set; }

        public decimal SubtotalDocePorcientoConIce { get; set; }

        public decimal SubtotalNoObjetoIva { get; set; }

        public decimal SubtotalExentoIva { get; set; }

        public decimal ImpuestoRenta { get; set; }

        public string Regimen { get; set; }

        public string RucTransportista { get; set; }

        public string PuntoEmisionTransportista { get; set; }

        public string RazonSocialTransporista { get; set; }

        public ICollection<FacturaDetalle> Detalle { get; set; }

        public ICollection<FormaPago> FormasPago { get; set; }

        public ICollection<InformacionAdicional> Adicionales { get; set; }

        public string Numero
        {
            get
            {
                var retorno = new List<string>();

                if (!string.IsNullOrEmpty(EstablecimientoNumero))
                    retorno.Add(EstablecimientoNumero);

                if (!string.IsNullOrEmpty(PuntoEmisionNumero))
                    retorno.Add(PuntoEmisionNumero);

                if (!string.IsNullOrEmpty(Secuencial))
                    retorno.Add(Secuencial);

                return string.Join("-", retorno.ToArray());
            }
        }

        public bool EsReembolso { get; set; }

        public bool EsExportacion { get; set; }

        public bool Estransportista { get; set; }

        public FacturaReembolso Reembolso { get; set; }

        public ICollection<FacturaReembolso> ReembolsoDetalle { get; }


        public string DefinicionTermino { get; set; }

        public string DefTerminoSinImpuesto { get; set; }

        public string PuertoEmbarque { get; set; }

        public string PaisAdquisicion { get; set; }

        public string PaisAdquisicionCodigo { get; set; }

        public string PaisOrigen { get; set; }

        public string PaisOrigenCodigo { get; set; }

        public string LugarConvenio { get; set; }

        public string PuertoDestino { get; set; }

        public string PaisDestino { get; set; }

        public string PaisDestinoCodigo { get; set; }

        public decimal FleteInternacional { get; set; }

        public decimal SeguroInternacional { get; set; }

        public decimal GastosAduaneros { get; set; }

        public decimal GastosTransporte { get; set; }

        public bool EstaActualizadoTotal { get; set; }

        public bool EsRimpe { get; set; }

        #endregion

        #region Contructores

        public Factura()
        {
            Detalle = new List<FacturaDetalle>();
            FormasPago = new List<FormaPago>();
            Adicionales = new List<InformacionAdicional>();
            EsReembolso = false;
            Reembolso = new FacturaReembolso();
            ReembolsoDetalle = new List<FacturaReembolso>();
        }

        #endregion

        #region Metodos privados

        private decimal Redondear(decimal valor)
        {
            return UtilFormato.Truncate(valor, 3).Value;
        }

        private decimal Truncate(decimal valor)
        {
            return UtilFormato.TruncateDecimal(valor, 2).Value;
        }

        private decimal CalcularFormasPagoTotal()
        {
            return FormasPago.Sum(fp => Redondear(fp.Monto ?? 0M));
        }

        private decimal CalcularReembolsoTotal()
        {
            return ReembolsoDetalle.Sum(rm => Redondear(rm.TotalImpuesto ?? 0M));
        }

        #endregion

        #region Metodos publicos

        public FacturaDetalle ObtenerDetalle(int detalleId)
        {
            var detalles = Detalle.Where(fd => fd.Id == detalleId);

            if (!detalles.Any())
                return null;

            return detalles.First();
        }

        public void AgregarDetalle(FacturaDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException("detalle");

            Detalle.Add(detalle);

            CalcularTotales();
        }

        public void ModificarDetalle(FacturaDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException("detalle");

            var detalleAct = ObtenerDetalle(detalle.Id);

            if (detalleAct == null)
                return;

            detalleAct.ProductoCodigo = detalle.ProductoCodigo;
            detalleAct.Descripcion = detalle.Descripcion;
            detalleAct.Cantidad = detalle.Cantidad;
            detalleAct.Descuento = detalle.Descuento;
            detalleAct.PrecioUnitario = detalle.PrecioUnitario;
            detalleAct.PorcentajeIvaCodigo = detalle.PorcentajeIvaCodigo;
            detalleAct.PorcentajeIvaValor = detalle.PorcentajeIvaValor;
            detalleAct.IceCodigo = detalle.IceCodigo;
            detalleAct.IceValor = detalle.IceValor;
            detalleAct.PorcentajeRetencionCodigo = detalle.PorcentajeRetencionCodigo;
            detalleAct.PorcentajeRetencionValor = detalle.PorcentajeRetencionValor;

            CalcularTotales();
        }

        public void EliminarDetalle(int detalleId)
        {
            var detalle = ObtenerDetalle(detalleId);

            if (detalle == null)
                return;

            Detalle.Remove(detalle);

            CalcularTotales();
        }

        public FormaPago ObtenerFormaPago(int formaPagoId)
        {
            var formasPago = FormasPago.Where(fp => fp.Id == formaPagoId);

            if (!formasPago.Any())
                return null;

            return formasPago.First();
        }

        public void AgregarFormaPago(FormaPago formaPago)
        {
            if (formaPago == null)
                throw new ArgumentNullException("formaPago");

            FormasPago.Add(formaPago);
        }

        public void ModificarFormaPago(FormaPago formaPago)
        {
            var formaPagoAct = ObtenerFormaPago(formaPago.Id);

            if (formaPagoAct == null)
                return;

            formaPagoAct.FormaPagoCodigo = formaPago.FormaPagoCodigo;
            formaPagoAct.Monto = formaPago.Monto;
            formaPagoAct.TiempoCodigo = formaPago.TiempoCodigo;
            formaPagoAct.Plazo = formaPago.Plazo;
        }

        public void EliminarFormaPago(int formaPagoId)
        {
            var formaPago = ObtenerFormaPago(formaPagoId);

            if (formaPago == null)
            {
                return;
            }

            FormasPago.Remove(formaPago);
        }

        public InformacionAdicional ObtenerAdicional(int adicionalId)
        {
            var adicionales = Adicionales.Where(fp => fp.Id == adicionalId);

            if (!adicionales.Any())
                return null;

            return adicionales.First();
        }

        public void AgregarAdicional(InformacionAdicional adicional)
        {
            if (adicional == null)
                throw new ArgumentNullException("adicional");

            Adicionales.Add(adicional);
        }

        public void ModificarAdicional(InformacionAdicional adicional)
        {
            var adicionalAct = ObtenerAdicional(adicional.Id);

            if (adicionalAct == null)
                return;

            adicionalAct.Codigo = adicional.Codigo;
            adicionalAct.Valor = adicional.Valor;
        }

        public void EliminarAdicional(int adicionalId)
        {
            var adicional = ObtenerAdicional(adicionalId);

            if (adicional == null)
            {
                return;
            }

            Adicionales.Remove(adicional);
        }

        public void CalcularTotales()
        {
            SubtotalDocePorciento = 0M;
            SubtotalDocePorcientoConIce = 0M;
            IvaDocePorCiento = 0M;
            Ice = 0M;
            SubtotalCeroPorciento = 0M;
            SubtotalNoObjetoIva = 0M;
            SubtotalExentoIva = 0M;
            SubtotalSinImpuestos = 0M;
            TotalDescuento = 0M;
            Total = 0M;
            ImpuestoRenta = 0M;

            var IvaDocePorCientoTem = 0M;
            var TotalTem = 0M;

            foreach (var detalle in Detalle)
            {
                var impuesto = detalle.ImpuestoModelo;

                Ice = Redondear(Ice + (detalle.ImpuestosValorIce ?? 0M));

                if (impuesto.EsDocePorCiento() || impuesto.EsCatorcePorCiento())
                {
                    SubtotalDocePorciento = Redondear(SubtotalDocePorciento + (detalle.BaseImponible ?? 0M));
                    SubtotalDocePorcientoConIce = Redondear(SubtotalDocePorcientoConIce + (detalle.BaseImponibleConImpuestoICE ?? 0M));
                    IvaDocePorCientoTem += (detalle.ImpuestosValorConImpuestoICE ?? 0M);
                    TotalTem += (detalle.PrecioTotal ?? 0M);
                }
                else if (impuesto.EsCeroPorCiento())
                {
                    SubtotalCeroPorciento = Redondear(SubtotalCeroPorciento + detalle.BaseImponible ?? 0M);
                    TotalTem += (detalle.PrecioTotal ?? 0M);
                }
                else if (impuesto.EsNoObjetoIva())
                {
                    SubtotalNoObjetoIva = Redondear(SubtotalNoObjetoIva + (detalle.BaseImponible ?? 0M));
                    TotalTem += (detalle.PrecioTotal ?? 0M);
                }
                else if (impuesto.EsExentoIva())
                {
                    SubtotalExentoIva = Redondear(SubtotalExentoIva + (detalle.BaseImponible ?? 0M));
                    TotalTem += (detalle.PrecioTotal ?? 0M);
                }

                if ((detalle.Descuento ?? 0M) > 0M)
                    TotalDescuento = Redondear(TotalDescuento + (detalle.Descuento ?? 0M));

                if ((detalle.PorcentajeRetencionValor ?? 0M) > 0M)
                    ImpuestoRenta = Redondear(ImpuestoRenta + (detalle.TarifaRetencion ?? 0M));

                SubtotalSinImpuestos = Redondear((SubtotalSinImpuestos + (detalle.BaseImponible ?? 0M)));
            }

            IvaDocePorCiento = Redondear(IvaDocePorCientoTem);
            Total = Redondear(TotalTem);
        }

        public void ActualizaTotal()
        {
            Total = Total + FleteInternacional + SeguroInternacional + GastosAduaneros + GastosTransporte;
            EstaActualizadoTotal = true;
        }



        public ResultadoValidacion Validar()
        {
            return Validar(false);
        }

        public ResultadoValidacion Validar(bool omitirVacios)
        {
            var resultado = new ResultadoValidacion();
            
            if(resultado.Errores.Any())
                resultado = new ResultadoValidacion();

            if (string.IsNullOrEmpty(EmpresaRuc))
                resultado.AnadirError("La empresa es requerida");

            if (string.IsNullOrEmpty(RazonSocial))
                resultado.AnadirError("La razon social es requerida");

            if (string.IsNullOrEmpty(Identificacion))
                resultado.AnadirError("El número de identificación es requerido");
            else
            {
                if ((TipoIdentificacionCodigo == "04") && (Identificacion.Length != 13))
                    resultado.AnadirError("La longitud del RUC no es el adecuado");

                if ((TipoIdentificacionCodigo == "05") && (Identificacion.Length != 10))
                    resultado.AnadirError("La longitud de la Cédula no es la adecuada");
            }

            if (string.IsNullOrEmpty(EstablecimientoNumero))
                resultado.AnadirError("El establecimiento de la factura es requerida");

            if (string.IsNullOrEmpty(PuntoEmisionNumero))
                resultado.AnadirError("El punto de emisión de la factura es requerida");

            if (Detalle.Any())
            {
                var contDetalle = 0;

                foreach (var liqCompDet in Detalle)
                {
                    contDetalle += 1;

                    if (omitirVacios && liqCompDet.EsVacio)
                        continue;

                    var validacionDetalle = liqCompDet.Validar();

                    if (!validacionDetalle.EsValido)
                    {
                        foreach (var errorDetalle in validacionDetalle.Errores)
                            resultado.AnadirError(string.Format("Detalle [{0}]: {1}", contDetalle, errorDetalle));
                    }
                }
            }
            else
                resultado.AnadirError("La factura requiere al menos un detalle");

            if (FormasPago.Any())
            {
                var contFormaPago = 0;

                foreach (var formaPago in FormasPago)
                {
                    contFormaPago += 1;

                    if (omitirVacios && formaPago.EsVacio)
                        continue;

                    var validacionFormaPago = formaPago.Validar();

                    if (!validacionFormaPago.EsValido)
                    {
                        foreach (var errorFormaPago in validacionFormaPago.Errores)
                            resultado.AnadirError(string.Format("Forma de pago [{0}]: {1}", contFormaPago, errorFormaPago));
                    }
                }
            }
            else
                resultado.AnadirError("La factura requiere al menos una forma de pago");

            if (Adicionales.Any())
            {
                var contAdicional = 0;

                foreach (var adicional in this.Adicionales)
                {
                    contAdicional += 1;

                    if (omitirVacios && adicional.EsVacio)
                        continue;

                    var validacionAdicional = adicional.Validar();

                    if (!validacionAdicional.EsValido)
                    {
                        foreach (var errorAdicional in validacionAdicional.Errores)
                            resultado.AnadirError(string.Format("Adicional [{0}]: {1}", contAdicional, errorAdicional));
                    }
                }
            }

            if (
                (Total > TOTAL_FACTURA_CONSUMIDOR_FINAL_MAXIMO)
                && (TipoIdentificacionCodigo == TIPO_IDENT_CONSUMIDOR_FINAL_CODIGO)
            )
            {
                var mensajeError = string.Format("La factura supera los {0} no puede ser emitida para consumidor final", TOTAL_FACTURA_CONSUMIDOR_FINAL_MAXIMO);

                resultado.AnadirError(mensajeError);
            }

            var formasPagoTotal = CalcularFormasPagoTotal();

            Total = UtilFormato.ADecimal(UtilFormato.ACadena(Total)).Value;

            if (formasPagoTotal != Total)
                resultado.AnadirError("La suma de las formas de pago debe ser igual al valor total de la factura");

            if (EsReembolso)
            {
                var ReembolsoTotal = Reembolso.RetornaTotales();

                var validacionReembolso = Reembolso.Validar();

                if (!validacionReembolso.EsValido)
                {
                    foreach (var errorReembolso in validacionReembolso.Errores)
                        resultado.AnadirError(errorReembolso);
                }
                if (ReembolsoTotal != Total)
                    resultado.AnadirError("Total de Reembolsos no es igual al valor de la factura");
            }

            return resultado;
        }

        public string ObtenerIvasManuales()
        {
            var result = string.Empty;

            foreach (var detalle in Detalle)
            {
                var impuesto = detalle.ImpuestoModelo;

                if (impuesto.EsDocePorCiento() || impuesto.EsCatorcePorCiento())
                {
                    if (impuesto.EsDocePorCiento())
                        result = "12";
                    else if (impuesto.EsCatorcePorCiento())
                        result = "14";
                    else
                        result = "0";

                    break;
                }
            }

            return result;
        }

        public string ObtenerIvaDoceCodigoPorcentaje()
        {
            var ivasManuales = ObtenerIvasManuales();

            return (ivasManuales == "12") ? "2" : "3"; // 2 (12%), 3 (14%)
        }

        #endregion
    }
}
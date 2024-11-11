using System;
using System.Collections.Generic;
using System.Linq;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class FacturaReembolso
    {
        private int m_secuencial;

        public string Codigo { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalBaseImponible { get; set; }

        public decimal? TotalImpuesto { get; set; }

        public ICollection<FacturaReembolsoDetalle> Detalle { get; set; }

        public FacturaReembolso()
        {
            Detalle = new List<FacturaReembolsoDetalle>();
            m_secuencial = 0;
        }

        private int ProximoSecuencial()
        {
            m_secuencial -= 1;

            return m_secuencial;
        }

        private decimal Redondear(decimal valor)
        {
            return UtilFormato.Truncate(valor, 2).Value;
        }

        public FacturaReembolsoDetalle ObtenerDetalle(int detalleId)
        {
            var detalles = Detalle.Where(fd => fd.Id == detalleId);

            if (!detalles.Any())
                return null;

            return detalles.First();
        }

        public FacturaReembolsoDetalle AgregarDetalle(FacturaReembolsoDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));

            var TipIdent = detalle.Proveedor.TipoIdentificacionCodigo;
            var LongIdent = detalle.Proveedor.Identificacion.Length;
            if (TipIdent.Equals("04") && (LongIdent != 13))
                throw new Exception("Error en la identificacion RUC");
            if (TipIdent.Equals("05") && (LongIdent != 10))
                throw new Exception("Error en la identificacion Cedula");
            var detalleNew = new FacturaReembolsoDetalle();

            detalleNew.ChangeCurrentIdentity(ProximoSecuencial());
            detalleNew.EstablecerProveedor(detalle.Proveedor);
            detalleNew.EstablecerDocumento(detalle.Documento);
            detalleNew.EstablecerImpuesto(detalle.Impuesto);

            this.Detalle.Add(detalleNew);

            CalcularTotales();

            return detalleNew;
        }

        public void ModificarDetalle(FacturaReembolsoDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));

            var detalleAct = ObtenerDetalle(detalle.Id);

            if (detalleAct == null)
                return;

            detalleAct.EstablecerProveedor(detalle.Proveedor);
            detalleAct.EstablecerDocumento(detalle.Documento);
            detalleAct.EstablecerImpuesto(detalle.Impuesto);

            CalcularTotales();
        }

        public void EliminarDetalle(int detalleId)
        {
            var detalle = ObtenerDetalle(detalleId);

            if (detalle == null)
                return;

            this.Detalle.Remove(detalle);

            CalcularTotales();
        }

        public decimal RetornaTotales()
        {
            TotalBaseImponible = 0M;
            TotalImpuesto = 0M;
            Total = 0M;

            foreach (var detalle in Detalle)
            {
                TotalBaseImponible = Redondear(TotalBaseImponible + detalle.Impuesto.BaseImponible ?? 0M);
                TotalImpuesto = Redondear(TotalImpuesto + detalle.Impuesto.ImpuestosValor ?? 0M);
                Total = Redondear(Total + detalle.Impuesto.ValorTotal ?? 0M);
            }
            return Total;
        }

        public void CalcularTotales()
        {
            TotalBaseImponible = 0M;
            TotalImpuesto = 0M;
            Total = 0M;

            foreach (var detalle in Detalle)
            {
                TotalBaseImponible = Redondear(TotalBaseImponible + detalle.Impuesto.BaseImponible ?? 0M);
                TotalImpuesto = Redondear(TotalImpuesto + detalle.Impuesto.ImpuestosValor ?? 0M);
                Total = Redondear(Total + detalle.Impuesto.ValorTotal ?? 0M);
            }
        }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            if (Detalle.Any())
            {
                var contDetalle = 0;

                foreach (var liqCompRemblDet in Detalle)
                {
                    contDetalle += 1;

                    var validacionDetalle = liqCompRemblDet.Validar();

                    if (!validacionDetalle.EsValido)
                    {
                        foreach (var errorDetalle in validacionDetalle.Errores)
                            resultado.AnadirError(string.Format("Detalle reembolso [{0}]: {1}", contDetalle, errorDetalle));
                    }
                }
            }
            else
                resultado.AnadirError("El reembolso requiere al menos un detalle");

            return resultado;
        }
    }

}

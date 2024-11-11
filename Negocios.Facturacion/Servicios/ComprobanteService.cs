using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class ComprobanteService : IComprobanteService
    {
        #region Campos

        private readonly IComprobanteRepository m_comprobanteRepository;

        private bool m_desechado = false;

        private const string Factura = "01";
        private const string FacturaExportacion = "99";
        private const string FacturaReembolso = "98";
        private const string FacturaTransportista = "77";
        private const string NotaCredito = "04";
        private const string NotaDebito = "05";
        private const string GuiaRemision = "06";
        private const string ComprobantesRetencion = "07";
        private const string LiquidacionCompra = "03";

        #endregion

        #region Constructores 

        public ComprobanteService(IComprobanteRepository comprobanteRepository)
        {
            m_comprobanteRepository = comprobanteRepository ?? throw new ArgumentNullException(nameof(comprobanteRepository));
        }

        #endregion

        #region IComprobanteService

        public IEnumerable<Comprobante> ObtenerComprobantes(FiltroModel request)
        {
            var cadena = ObtenerCodigoTipo(request);
            request.Tipos = cadena;

            var result = m_comprobanteRepository.ObtenerComprobantes(request);

            return result;
        }

        public Comprobante ObtenerComprobantesXML(InfoComprobanteModel request)
        {
            var result = m_comprobanteRepository.ObtenerComprobantesXML(request);

            return result;
        }

        private string ObtenerCodigoTipo(FiltroModel request)
        {
            string cadena = string.Empty;
            int x = 0;

            if (request != null)
            {
                if (request.ListTipo != null)
                {
                    foreach (string item in request.ListTipo)
                    {
                        string coidgo = TiposComprobantes(item);
                        cadena = coidgo + "|" + cadena;
                        x++;
                    }
                }
            }
          
            return cadena;
        }

        private string TiposComprobantes(string item)
        {
            string codigoTipo = string.Empty;
            switch (item)
            {
                case "Factura":
                    codigoTipo = Factura;
                    break;
                case "Factura Exportación":
                    codigoTipo = FacturaExportacion;
                    break;
                case "Factura Reembolso":
                    codigoTipo = FacturaReembolso;
                    break;
                case "Factura Transportista":
                    codigoTipo = FacturaTransportista;
                    break;
                case "Nota de Crédito":
                    codigoTipo = NotaCredito;
                    break;
                case "Nota de Débito":
                    codigoTipo = NotaDebito;
                    break;
                case "Guía de Remisión":
                    codigoTipo = GuiaRemision;
                    break;
                case "Comprobantes de Retención":
                    codigoTipo = ComprobantesRetencion;
                    break;
                case "Liquidación de Compra":
                    codigoTipo = LiquidacionCompra;
                    break;
                default:
                    codigoTipo = Factura + "|" + FacturaExportacion + "|" + FacturaReembolso + "|" + FacturaTransportista + "|" + NotaCredito + "|" + NotaDebito + "|" + GuiaRemision + "|" + ComprobantesRetencion + "|" + LiquidacionCompra;
                    break;
            }

            return codigoTipo;
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
                m_comprobanteRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ComprobanteService()
        {
            Dispose(false);
        }

        #endregion
    }
}

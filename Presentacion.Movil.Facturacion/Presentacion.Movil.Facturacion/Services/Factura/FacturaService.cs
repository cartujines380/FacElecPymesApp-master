using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Factura;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Factura
{
    public class FacturaService : IFacturaService
    {
        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/factura";

        #endregion

        #region Constructor

        public FacturaService(
           IRequestProvider requestProvider,
           ISettingsService settingsService
       )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region IFacturaService

        public async Task<ObservableCollection<NumeroFacturaModel>> ObtenerCodigoEstablecimiento(string ruc, bool esTransportista)
        {
            var retorno = new ObservableCollection<NumeroFacturaModel>();

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerestablecimiento/" + ruc + "/" + esTransportista);
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerestablecimiento", ruc, esTransportista.ToString());

                var codestablecimiento = await m_requestProvider.GetAsync<IEnumerable<NumeroFacturaModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<NumeroFacturaModel>(codestablecimiento);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<ObservableCollection<NumeroFacturaModel>> ObtenerCodigoPuntoEmision(string ruc, string codEstablecimiento, bool esTransportista)
        {
            var retorno = new ObservableCollection<NumeroFacturaModel>();

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerpuntoemision/" + ruc + "/" + codEstablecimiento + "/" + esTransportista);
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerpuntoemision", ruc, codEstablecimiento, esTransportista.ToString());

                var codPtoEmision = await m_requestProvider.GetAsync<IEnumerable<NumeroFacturaModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<NumeroFacturaModel>(codPtoEmision);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<int> ObtenerSecuencial(string ruc, string codEstablecimiento, string codPtoEmision)
        {
            var retorno = 0;

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenersecuencial");
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenersecuencial");

                var reques = new ObtenerSecuencialRequest
                {
                   DocumentoTipo = "01",
                   EmpresaRuc = ruc,
                   Establecimiento = codEstablecimiento,
                   PuntoEmision = codPtoEmision,
                   Tipo = "C"
                };

                retorno = await m_requestProvider.PostAsync<ObtenerSecuencialRequest, int>(uri, reques, m_settingsService.AuthAccessToken);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<GuardarFacturaResponse> GuardarFactura(GuardarFacturaRequest request)
        {
            var retorno = new GuardarFacturaResponse();
            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "guardar");

                retorno = await m_requestProvider.PostAsync<GuardarFacturaRequest, GuardarFacturaResponse>(uri, request, m_settingsService.AuthAccessToken);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<int> ObtenerTotalComprobante(string ruc, string estado)
        {
            var retorno = 0;

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenertotalcomprobante/" + ruc + "/" + estado);
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenertotalcomprobante", ruc, estado);

                retorno = await m_requestProvider.GetAsync<int>(uri, m_settingsService.AuthAccessToken);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<int> ObtenerTotalComprobantePorTipo(string ruc, string estado, string tipoDoc)
        {
            var retorno = 0;

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenertotalcomprobanteportipo/" + ruc + "/" + estado + "/" + tipoDoc);
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenertotalcomprobanteportipo", ruc, estado, tipoDoc);

                retorno = await m_requestProvider.GetAsync<int>(uri, m_settingsService.AuthAccessToken);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        #endregion
    }
}

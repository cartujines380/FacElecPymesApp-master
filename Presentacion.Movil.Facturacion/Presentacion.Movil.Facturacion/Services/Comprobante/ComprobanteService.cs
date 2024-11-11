using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Comprobante
{
    public class ComprobanteService : IComprobanteService
    {
        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/Comprobantes";

        #endregion

        #region Constructor

        public ComprobanteService(IRequestProvider requestProvider,
           ISettingsService settingsService)
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region IComprobanteService

        public async Task<ObservableCollection<ComprobanteModel>> ConsultarComprobante(FiltroModel request)
        {
            var retorno = new ObservableCollection<ComprobanteModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenercomprobantes");

                var comprobante = await m_requestProvider.PostAsync<FiltroModel, IEnumerable<ComprobanteModel>>(uri, request, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<ComprobanteModel>(comprobante);
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

        public async Task<ComprobanteModel> ConsultarComprobanteXML(InfoComprobanteModel request)
        {
            var retorno = new ComprobanteModel();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenercomprobantesxml");

                retorno = await m_requestProvider.PostAsync<InfoComprobanteModel, ComprobanteModel>(uri, request, m_settingsService.AuthAccessToken);

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

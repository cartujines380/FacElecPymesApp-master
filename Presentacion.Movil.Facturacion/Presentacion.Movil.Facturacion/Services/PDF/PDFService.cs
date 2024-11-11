using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.PDF
{
    public class PDFService : IPDFService
    {
        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/PDF";

        #endregion

        #region Constructor

        public PDFService(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region IPDFService

        public async Task<PDFModel> GenerarPDF(ComprobanteModel infoComp)
        {
            var retorno = new PDFModel();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.PDFEndpointBase, URL_BASE, "GenerarPDF");

                var pdf = await m_requestProvider.PostAsync<ComprobanteModel, PDFModel>(uri, infoComp, m_settingsService.AuthAccessToken);
                retorno.PDFbtye = pdf.PDFbtye;
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

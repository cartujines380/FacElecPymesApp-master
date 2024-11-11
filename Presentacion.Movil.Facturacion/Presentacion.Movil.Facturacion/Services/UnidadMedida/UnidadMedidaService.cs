using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.UnidadMedida;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.UnidadMedida
{
    public class UnidadMedidaService : IUnidadMedidaService
    {

        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/unidadmedida";

        #endregion

        #region Constructor

        public UnidadMedidaService(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region IUnidadMedidaService

        public async Task<ObservableCollection<UnidadMedidaModel>> ObtenerUnidadMedidaPorArticulo(string ruc, string codArticulo)
        {
            var retorno = new ObservableCollection<UnidadMedidaModel>();

            try
            {
                //var uri = string.Format("{0}/{1}/{2}", m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerunidadmedidaporarticulo/" + ruc + "/" + codArticulo);
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerunidadmedidaporarticulo", ruc, codArticulo);

                var catalogos = await m_requestProvider.GetAsync<IEnumerable<UnidadMedidaModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<UnidadMedidaModel>(catalogos);

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

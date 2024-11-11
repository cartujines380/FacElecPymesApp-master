using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Articulo
{
    public class ArticuloService : IArticuloService
    {

        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/articulo";

        #endregion

        #region Constructor

        public ArticuloService(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region ICatalogoService

        public async Task<ObservableCollection<ArticuloModel>> ObtenerArticulosPorEmpresa(string criterioBusquedad, string ruc, bool esTransportista)
        {
            var retorno = new ObservableCollection<ArticuloModel>();

            try
            {
                var _criterioBusquedad = criterioBusquedad == "" ? " " : criterioBusquedad;
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerarticuloporempresa", _criterioBusquedad, ruc, esTransportista.ToString());

                var articulos = await m_requestProvider.GetAsync<IEnumerable<ArticuloModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<ArticuloModel>(articulos);

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

        public async Task<ArticuloModel> GuardarArticulo(ArticuloModel articulo)
        {
            var retorno = new ArticuloModel();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "guardararticulo");

                retorno = await m_requestProvider.PostAsync<ArticuloModel, ArticuloModel>(uri, articulo, m_settingsService.AuthAccessToken);

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

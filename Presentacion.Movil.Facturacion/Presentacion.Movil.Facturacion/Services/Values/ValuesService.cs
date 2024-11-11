using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Values
{
    public class ValuesService : IValuesService
    {
        private const string API_URL_BASE = "api/values";

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;

        public ValuesService(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task<ObservableCollection<string>> GetValuesAsync()
        {
            var retorno = new ObservableCollection<string>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, $"{API_URL_BASE}");

                var values = await m_requestProvider.GetAsync<IEnumerable<string>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<string>(values);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error al obtener values: [{0}]", ex.Message));
            }

            return retorno;
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings m_serializerSettings;
        private readonly IRequestSettings m_requestSettings;

        public RequestProvider(IRequestSettings requestSettings)
        {
            m_requestSettings = requestSettings ?? throw new ArgumentNullException(nameof(requestSettings));

            m_serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            m_serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            try
            {
                var httpClient = CreateHttpClient(token);
                var response = await httpClient.GetAsync(uri);

                await HandleResponse(response);

                var serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, m_serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                // Opcional: Realizar otras acciones de manejo de excepciones aquí
                return default(TResult); // O un valor predeterminado adecuado para TResult en caso de excepción
            }
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string token = "", string header = "")
        {
            try
            {
                return await PostAsync<object, TResult>(uri, null, token, header);
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                // Opcional: Realizar otras acciones de manejo de excepciones aquí
                return default(TResult); // O un valor predeterminado adecuado para TResult en caso de excepción
            }
        }

        public async Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string token = "", string header = "")
        {
            try
            {
                var httpClient = CreateHttpClient(token);

                if (!string.IsNullOrEmpty(header))
                {
                    AddHeaderParameter(httpClient, header);
                }

                StringContent content = null;

                if (data == null)
                {
                    content = new StringContent(string.Empty);
                }
                else
                {
                    content = new StringContent(JsonConvert.SerializeObject(data));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);

                var serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, m_serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                // Opcional: Realizar otras acciones de manejo de excepciones aquí
                return default(TResult); // O un valor predeterminado adecuado para TResult en caso de excepción
            }
            
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            try
            {
                var httpClient = CreateHttpClient(string.Empty);

                if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                {
                    AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
                }

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);

                var serialized = await response.Content.ReadAsStringAsync();

                TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, m_serializerSettings));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo una excepción: {ex.Message}");
                // Opcional: Realizar otras acciones de manejo de excepciones aquí
                return default(TResult); // O un valor predeterminado adecuado para TResult en caso de excepción
            }
            
        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            var httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, m_serializerSettings));

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            var httpClient = CreateHttpClient(token);

            await httpClient.DeleteAsync(uri);
        }

        private HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };

            return handler;
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = m_requestSettings.ValidateSslCertificate
                ? new HttpClient()
                : new HttpClient(GetInsecureHandler());

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}

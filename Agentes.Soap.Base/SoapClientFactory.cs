using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace Sipecom.FactElec.Pymes.Agentes.Soap.Base
{
    public abstract class SoapClientFactory<TClient, TChannel> : ISoapClientFactory<TClient>
        where TClient : class
        where TChannel : class
    {
        #region Campos

        private readonly IEndpointConfiguration m_configuration;

        #endregion

        #region Constructores

        protected SoapClientFactory(IEndpointConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;

            m_configuration.Initialize();
        }

        #endregion

        #region Metodos protegidos

        protected virtual Binding GetBindingForEndpoint(IEndpointConfiguration configuration)
        {
            var binding = new BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;
            binding.OpenTimeout = TimeSpan.FromMilliseconds(configuration.OpenTimeout);
            binding.ReceiveTimeout = TimeSpan.FromMilliseconds(configuration.ReceiveTimeout);
            binding.SendTimeout = TimeSpan.FromMilliseconds(configuration.SendTimeout);
            binding.CloseTimeout = TimeSpan.FromMilliseconds(configuration.CloseTimeout);

            return binding;
        }

        protected virtual EndpointAddress GetEndpointAddress(string relativeAddress, IEndpointConfiguration configuration)
        {
            var baseUri = new Uri(configuration.BaseAddress);
            var uri = new Uri(baseUri, relativeAddress);

            return new EndpointAddress(uri.AbsoluteUri);
        }

        #endregion

        #region Metodos abstractos

        protected abstract string RelativeAddress { get; }

        protected abstract TClient CreateClientIntern(Binding binding, EndpointAddress endpointAddress);

        #endregion

        #region ISoapClientFactory

        public TClient CreateClient()
        {
            var binding = GetBindingForEndpoint(m_configuration);
            var endpointAddress = GetEndpointAddress(RelativeAddress, m_configuration);

            var client = CreateClientIntern(binding, endpointAddress);

            var clientBase = client as ClientBase<TChannel>;

            if (clientBase != null)
            {
                clientBase.Endpoint.EndpointBehaviors.Add(new EndpointLoggingBehavior());
            }

            return client;
        }

        #endregion
    }
}

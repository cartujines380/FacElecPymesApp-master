using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using Sipecom.FactElec.Pymes.AccesoDatos.Base.Recursos;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Base
{
    public class AppSettingsConnectionStringResolver : IConnectionStringResolver
    {
        #region Constantes

        public const string CONNECTION_STRING_DEFAULT = "Default";

        public const string SECTION_KEY = "ConnectionStrings";
        public const string CONNECTION_STRING_KEY = "ConnectionString";
        public const string PROVIDER_NAME_KEY = "ProviderName";

        #endregion

        #region Campos

        private readonly IConfiguration m_configuration;

        #endregion

        #region Constructores

        public AppSettingsConnectionStringResolver(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_configuration = configuration;
        }

        #endregion

        #region IConnectionStringResolver

        public ConnectionStringItem GetConnectionString()
        {
            return GetConnectionString(CONNECTION_STRING_DEFAULT);
        }

        public ConnectionStringItem GetConnectionString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var connStrings = GetAllConnectionStrings();

            var connStrItems = connStrings.Where(cs => cs.Name == name);

            if (connStrItems.Any())
            {
                return connStrItems.First();
            }

            var message = string.Format(Mensajes.exc_GetConnStrInvalid, name);

            throw new InvalidOperationException(message);
        }

        public IEnumerable<ConnectionStringItem> GetAllConnectionStrings()
        {
            var retorno = new List<ConnectionStringItem>();
            var consStrSections = m_configuration.GetSection(SECTION_KEY);

            if (consStrSections == null)
            {
                return retorno;
            }

            var connStrSections = consStrSections.GetChildren();

            foreach (var connStrSection in connStrSections)
            {
                var connStrSect = connStrSection.GetSection(CONNECTION_STRING_KEY);
                var providerNameSect = connStrSection.GetSection(PROVIDER_NAME_KEY);

                var connStrItem = new ConnectionStringItem()
                {
                    Name = connStrSection.Key,
                    ConnectionString = connStrSect?.Value ?? string.Empty,
                    ProviderName = providerNameSect?.Value ?? string.Empty
                };

                retorno.Add(connStrItem);
            }

            return retorno;
        }

        #endregion
    }
}

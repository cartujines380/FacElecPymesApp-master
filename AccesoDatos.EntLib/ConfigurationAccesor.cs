using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Sipecom.FactElec.Pymes.AccesoDatos.Base;


namespace Sipecom.FactElec.Pymes.AccesoDatos.EntLib
{
    public class ConfigurationAccesor : IConfigurationAccesor
    {
        #region Constantes

        private const string CONNECTION_STRING_DEFAULT = "Default";
        private const string DATA_CONFIGURATION_SECTION = "dataConfiguration";
        private const string CONNECTION_STRINGS_SECTION = "connectionStrings";

        #endregion

        #region Campos

        private readonly IConnectionStringResolver m_connectionStringResolver;

        private ConnectionStringsSection m_connectionStringsSection;
        private DatabaseSettings m_databaseSettings;

        #endregion

        #region Constructores

        public ConfigurationAccesor(IConnectionStringResolver connectionStringResolver)
        {
            if (connectionStringResolver == null)
            {
                throw new ArgumentNullException(nameof(connectionStringResolver));
            }

            m_connectionStringResolver = connectionStringResolver;
        }

        #endregion

        #region Metodos privados

        private IEnumerable<ConnectionStringSettings> CreateConnectionStringSettings()
        {
            return m_connectionStringResolver.GetAllConnectionStrings()
                .Select(cs =>
                    new ConnectionStringSettings(
                        cs.Name,
                        cs.ConnectionString,
                        cs.ProviderName
                    )
                );
        }

        private ConnectionStringsSection GetConnectionStringsSection()
        {
            if (m_connectionStringsSection != null)
            {
                return m_connectionStringsSection;
            }

            var connStrSettings = CreateConnectionStringSettings();

            m_connectionStringsSection = new ConnectionStringsSection();

            foreach (var connStrSeting in connStrSettings)
            {
                m_connectionStringsSection.ConnectionStrings.Add(connStrSeting);
            }

            return m_connectionStringsSection;
        }

        private DatabaseSettings GetDatabaseSettings()
        {
            if (m_databaseSettings != null)
            {
                return m_databaseSettings;
            }

            m_databaseSettings = new DatabaseSettings
            {
                DefaultDatabase = CONNECTION_STRING_DEFAULT
            };

            m_databaseSettings.ProviderMappings.Add(new DbProviderMapping(DbProviderMapping.DefaultSqlProviderName, typeof(SqlDatabase)));

            return m_databaseSettings;
        }

        #endregion

        #region IConfigurationAccesor

        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection retorno = null;

            switch (sectionName)
            {
                case DATA_CONFIGURATION_SECTION:
                    retorno = GetDatabaseSettings();
                    break;

                case CONNECTION_STRINGS_SECTION:
                    retorno = GetConnectionStringsSection();
                    break;
            }

            return retorno;
        }

        #endregion
    }
}

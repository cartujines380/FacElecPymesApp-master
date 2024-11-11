using System;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sipecom.FactElec.Pymes.AccesoDatos.EntLib
{
    public class DatabaseProvider : IDatabaseProvider
    {
        #region Campos

        private readonly IConfigurationAccesor m_configurationAccesor;
        private readonly DatabaseProviderFactory m_dataBasefactory;

        #endregion

        #region Constructores

        public DatabaseProvider(IConfigurationAccesor configurationAccesor)
        {
            if (configurationAccesor == null)
            {
                throw new ArgumentNullException(nameof(configurationAccesor));
            }

            m_configurationAccesor = configurationAccesor;
            m_dataBasefactory = new DatabaseProviderFactory(s => m_configurationAccesor.GetSection(s));
        }

        #endregion

        #region IDatabaseProvider

        public Database GetDatabase()
        {
            return m_dataBasefactory.CreateDefault();
        }

        public Database GetDatabase(string name)
        {
            return m_dataBasefactory.Create(name);
        }

        #endregion
    }
}

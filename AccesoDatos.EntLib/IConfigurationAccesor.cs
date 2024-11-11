using System;
using System.Configuration;

namespace Sipecom.FactElec.Pymes.AccesoDatos.EntLib
{
    public interface IConfigurationAccesor
    {
        ConfigurationSection GetSection(string sectionName);
    }
}

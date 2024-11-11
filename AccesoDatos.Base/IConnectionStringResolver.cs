using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Base
{
    public interface IConnectionStringResolver
    {
        ConnectionStringItem GetConnectionString();

        ConnectionStringItem GetConnectionString(string name);

        IEnumerable<ConnectionStringItem> GetAllConnectionStrings();
    }
}

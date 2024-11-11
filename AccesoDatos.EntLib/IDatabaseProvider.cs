using System;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sipecom.FactElec.Pymes.AccesoDatos.EntLib
{
    public interface IDatabaseProvider
    {
        Database GetDatabase();

        Database GetDatabase(string name);
    }
}

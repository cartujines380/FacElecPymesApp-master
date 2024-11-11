using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);

        ILogger CreateLogger(string name);
    }
}

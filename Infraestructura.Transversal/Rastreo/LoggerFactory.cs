using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo
{
    public static class LoggerFactory
    {
        #region Campos

        static ILoggerFactory m_factory = null;

        #endregion

        #region Metodos publicos

        public static void SetCurrent(ILoggerFactory factory)
        {
            m_factory = factory;
        }

        public static ILogger CreateLogger<T>() where T : class
        {
            return (m_factory == null) ? null : m_factory.CreateLogger(typeof(T));
        }

        public static ILogger CreateLogger(Type type)
        {
            return (m_factory == null) ? null : m_factory.CreateLogger(type);
        }

        public static ILogger CreateLogger(string name)
        {
            return (m_factory == null) ? null : m_factory.CreateLogger(name);
        }

        #endregion
    }
}

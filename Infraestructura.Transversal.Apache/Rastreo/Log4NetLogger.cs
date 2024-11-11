using System;
using System.Globalization;
using log4net;
using log4net.Util;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Rastreo
{
    public class Log4NetLogger : ILogger
    {
        #region Campos

        private readonly ILog m_log;

        #endregion

        #region Constructores

        public Log4NetLogger(ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            m_log = log;
        }

        #endregion

        #region Metodos privados

        private object CreateMessage(string format, params object[] args)
        {
            return new SystemStringFormat(CultureInfo.InvariantCulture, format, args);
        }

        #endregion 

        #region ILogger

        #region Debug

        public bool IsDebugEnabled
        {
            get
            {
                return m_log.IsDebugEnabled;
            }
        }

        public void Debug(string message)
        {
            m_log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            m_log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            m_log.DebugFormat(format, args);
        }

        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            var message = CreateMessage(format, args);

            m_log.Debug(message, exception);
        }

        #endregion

        #region Info

        public bool IsInfoEnabled
        {
            get
            {
                return m_log.IsInfoEnabled;
            }
        }

        public void Info(string message)
        {
            m_log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            m_log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            m_log.InfoFormat(format, args);
        }

        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            var message = CreateMessage(format, args);

            m_log.Info(message, exception);
        }

        #endregion

        #region Warn

        public bool IsWarnEnabled
        {
            get
            {
                return m_log.IsWarnEnabled;
            }
        }

        public void Warn(string message)
        {
            m_log.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            m_log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            m_log.WarnFormat(format, args);
        }

        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            var message = CreateMessage(format, args);

            m_log.Warn(message, exception);
        }

        #endregion

        #region Error

        public bool IsErrorEnabled
        {
            get
            {
                return m_log.IsErrorEnabled;
            }
        }

        public void Error(string message)
        {
            m_log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            m_log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            m_log.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            var message = CreateMessage(format, args);

            m_log.Error(message, exception);
        }

        #endregion

        #region Fatal

        public bool IsFatalEnabled
        {
            get
            {
                return m_log.IsFatalEnabled;
            }
        }

        public void Fatal(string message)
        {
            m_log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            m_log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            m_log.FatalFormat(format, args);
        }

        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            var message = CreateMessage(format, args);

            m_log.Fatal(message, exception);
        }

        #endregion

        #endregion
    }
}

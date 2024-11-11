using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo
{
    public interface ILogger
    {
        #region Debug

        bool IsDebugEnabled { get; }

        void Debug(string message);

        void Debug(string message, Exception exception);

        void DebugFormat(string format, params object[] args);

        void DebugFormat(string format, Exception exception, params object[] args);

        #endregion

        #region Info

        bool IsInfoEnabled { get; }

        void Info(string message);

        void Info(string message, Exception exception);

        void InfoFormat(string format, params object[] args);

        void InfoFormat(string format, Exception exception, params object[] args);

        #endregion

        #region Warn

        bool IsWarnEnabled { get; }

        void Warn(string message);

        void Warn(string message, Exception exception);

        void WarnFormat(string format, params object[] args);

        void WarnFormat(string format, Exception exception, params object[] args);

        #endregion

        #region Error

        bool IsErrorEnabled { get; }

        void Error(string message);

        void Error(string message, Exception exception);

        void ErrorFormat(string format, params object[] args);

        void ErrorFormat(string format, Exception exception, params object[] args);

        #endregion

        #region Fatal

        bool IsFatalEnabled { get; }

        void Fatal(string message);

        void Fatal(string message, Exception exception);

        void FatalFormat(string format, params object[] args);

        void FatalFormat(string format, Exception exception, params object[] args);

        #endregion
    }
}

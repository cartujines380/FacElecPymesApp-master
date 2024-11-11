using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Repository;
using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Rastreo;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Rastreo
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        #region Constantes

        public const string CONFIG_FILE_NAME_DEFAULT = "log4net.config";

        #endregion

        #region Campos

        private readonly string m_configFileName;
        private readonly bool m_watch;

        private ILoggerRepository m_repository;

        #endregion

        #region Constructores

        public Log4NetLoggerFactory() : this(CONFIG_FILE_NAME_DEFAULT)
        {

        }

        public Log4NetLoggerFactory(string configFileName) : this(configFileName, false)
        {

        }

        public Log4NetLoggerFactory(string configFileName, bool watch)
        {
            if (string.IsNullOrEmpty(configFileName))
            {
                throw new ArgumentNullException("configFileName");
            }

            m_configFileName = configFileName;
            m_watch = watch;

            Initialize();
        }

        #endregion

        #region Metodos privados

        private void Initialize()
        {
            var assembly = GetLoggingAssembly();

            CreateRepository(assembly);

            Configure(assembly);
        }

        private static Assembly GetLoggingAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        private string GetFilePath(Assembly assembly, string fileName)
        {
            var newFileName = (Path.IsPathRooted(fileName))
                ? fileName
                : Path.Combine(Path.GetDirectoryName(assembly.Location), fileName);

            return Path.GetFullPath(newFileName);
        }

        private void CreateRepository(Assembly assembly)
        {
            var repositoryType = typeof(log4net.Repository.Hierarchy.Hierarchy);

            m_repository = LogManager.CreateRepository(assembly, repositoryType);
        }

        private void Configure(Assembly assembly)
        {
            var filePath = GetFilePath(assembly, m_configFileName);

            var configFile = new FileInfo(filePath);

            if (m_watch)
            {
                XmlConfigurator.ConfigureAndWatch(m_repository, configFile);
            }
            else
            {
                XmlConfigurator.Configure(m_repository, configFile);
            }
        }

        #endregion

        #region Metodos publicos

        public Log4NetLogger CreateLogger(Type type)
        {
            var log = LogManager.GetLogger(m_repository.Name, type);

            return new Log4NetLogger(log);
        }

        public Log4NetLogger CreateLogger(string name)
        {
            var log = LogManager.GetLogger(m_repository.Name, name);

            return new Log4NetLogger(log);
        }

        #endregion

        #region ILoggerFactory

        ILogger ILoggerFactory.CreateLogger(Type type)
        {
            return CreateLogger(type);
        }

        ILogger ILoggerFactory.CreateLogger(string name)
        {
            return CreateLogger(name);
        }

        #endregion
    }
}

namespace TodoApiDTO.Logging
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Concurrent;
    using System.IO;

    internal class Log4NetProvider : ILoggerProvider
    {
        private readonly FileInfo log4NetConfigFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProvider(FileInfo log4NetConfigFile)
        {
            this.log4NetConfigFile = log4NetConfigFile;
        }

        public static void TryRegister(ILoggerFactory loggerFactory)
        {
            var fileInfo = new FileInfo("log4net.xml");
            if (fileInfo.Exists)
            {
                loggerFactory.AddProvider(new Log4NetProvider(fileInfo));
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            loggers.Clear();
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, this.log4NetConfigFile);
        }
    }
}
using System.Collections.Concurrent;

namespace BasicCleanArchitectureTemplate.Web.Logging
{
    public class LogProvider : ILoggerProvider
    {
        private readonly string configFile;

        private readonly ConcurrentDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>();

        public LogProvider(string configFile)
        {
            this.configFile = configFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            loggers.Clear();
        }

        private ILogger CreateLoggerImplementation(string name)
        {
            return new Logger(name, new FileInfo(configFile));
        }
    }
}

namespace BasicCleanArchitectureTemplate.Web.Logging
{
    public static class LogExtensions
    {
        public static ILoggingBuilder AddLogger(this ILoggingBuilder factory)
        {
            factory.AddProvider(new LogProvider("log4net.config"));
            return factory;
        }
    }
}

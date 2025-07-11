using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Logging;

public class DefaultPluginLoggerConfigurator : IPluginLoggerConfigurator
{
    public void ConfigureLogging(IServiceCollection services, string pluginName)
    {
        services.AddSingleton(GetLoggerFactory(pluginName));
        services.AddLogging();
    }

    public ILoggerFactory GetLoggerFactory(string pluginName)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter(pluginName, LogLevel.Trace)
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddDebug();
        });

        return loggerFactory;
    }
}

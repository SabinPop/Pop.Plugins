using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pop.Plugins.Abstractions;
using Pop.Plugins.Abstractions.Settings;

namespace Pop.Plugins.Logging;

public class PluginLoggingConfigurator : IPluginLoggingConfigurator
{
    public void ConfigureLogging(IPluginServiceCollection services, PluginLoggingSettings settings, string pluginAssembly)
    {
        if (!settings.Enabled)
        {
            return;
        }

        var logLevel = Enum.TryParse<LogLevel>(settings.MinimumLevel, true, out var level)
            ? level
            : LogLevel.Trace;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(logLevel)
                .AddFilter(pluginAssembly, logLevel)
                .AddConsole()
                .AddDebug();
        });

        services.AddSingleton(loggerFactory);
        services.AddLogging();
    }
}

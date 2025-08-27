using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pop.Plugins.DependencyInjection.Abstractions;
using Pop.Plugins.Logging.Abstractions;

namespace Pop.Plugins.Logging;

/// <summary>
/// Default implementation of <see cref="IPluginLoggingConfigurator"/> for configuring plugin logging.
/// </summary>
public class PluginLoggingConfigurator : IPluginLoggingConfigurator
{
    /// <summary>
    /// Configures logging for the specified plugin.
    /// </summary>
    /// <param name="services">The plugin's service collection.</param>
    /// <param name="settings">The logging settings to apply.</param>
    /// <param name="pluginAssembly">The name of the plugin assembly for log filtering.</param>
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

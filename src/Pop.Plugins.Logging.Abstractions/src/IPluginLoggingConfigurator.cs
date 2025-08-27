using Pop.Plugins.DependencyInjection.Abstractions;

namespace Pop.Plugins.Logging.Abstractions;

/// <summary>
/// Provides methods to configure logging for a plugin.
/// </summary>
public interface IPluginLoggingConfigurator
{
    /// <summary>
    /// Configures logging for the specified plugin.
    /// </summary>
    /// <param name="services">The plugin's service collection.</param>
    /// <param name="settings">The logging settings to apply.</param>
    /// <param name="pluginAssembly">The name of the plugin assembly for log filtering.</param>
    void ConfigureLogging(IPluginServiceCollection services, PluginLoggingSettings settings, string pluginAssembly);
}

using Pop.Plugins.Abstractions.Settings;

namespace Pop.Plugins.Abstractions;

public interface IPluginLoggingConfigurator
{
    void ConfigureLogging(IPluginServiceCollection services, PluginLoggingSettings settings, string pluginAssembly);
}


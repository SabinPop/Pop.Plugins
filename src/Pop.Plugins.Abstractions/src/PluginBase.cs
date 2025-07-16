using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions.Settings;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public abstract class PluginBase: IPlugin
{
    protected PluginBase(IPluginLoggingConfigurator pluginLoggingConfigurator,
        PluginSettings pluginSettings)
    {
        Assembly = GetType().Assembly;
        Name = Assembly.FullName!;
        Services = new PluginServiceCollection();
        PluginLoggingConfigurator = pluginLoggingConfigurator;
        PluginSettings = pluginSettings;
    }

    public string Name { get; private set; }

    public Assembly Assembly { get; private set; }

    public PluginSettings PluginSettings { get; private set; }

    public IPluginServiceCollection Services { get; }

    public IServiceProvider? ServiceProvider { get; set; }

    public IPluginLoggingConfigurator PluginLoggingConfigurator { get; private set; }

    public virtual void ConfigureHostServices(IServiceCollection services)
    {

    }

    public virtual void ConfigurePluginServices()
    {
        PluginLoggingConfigurator.ConfigureLogging(Services, PluginSettings.Logging, Name);
    }
}

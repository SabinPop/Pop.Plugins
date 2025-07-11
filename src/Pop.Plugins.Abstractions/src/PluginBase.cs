using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public abstract class PluginBase: IPlugin
{
    protected PluginBase(IPluginLoggerConfigurator pluginLoggerConfigurator)
    {
        Assembly = GetType().Assembly;
        Name = Assembly.FullName!;
        Services = new ServiceCollection();
        pluginLoggerConfigurator.ConfigureLogging(Services, Name);
    }

    public string Name { get; private set; }

    public Assembly Assembly { get; private set; }

    public IServiceCollection Services { get; }

    public IServiceProvider? ServiceProvider { get; set; }

    public virtual void ConfigureHostServices(IServiceCollection services)
    {

    }

    public virtual void ConfigurePluginServices()
    {

    }
}

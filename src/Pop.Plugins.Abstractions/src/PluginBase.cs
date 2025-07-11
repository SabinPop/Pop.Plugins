using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public abstract class PluginBase: IPlugin
{
    protected PluginBase(IPluginLoggerConfigurator pluginLoggerConfigurator)
    {
        PluginAssembly = GetType().Assembly;
        Name = PluginAssembly.FullName!;
        PluginServices = new ServiceCollection();
        pluginLoggerConfigurator.ConfigureLogging(PluginServices, Name);
    }

    public string Name { get; private set; }

    public Assembly PluginAssembly { get; private set; }

    public IServiceCollection PluginServices { get; }

    public IServiceProvider? PluginServiceProvider { get; set; }

    public abstract void ConfigureHostServices(IServiceCollection services);

    public abstract void ConfigureModuleServices();
}

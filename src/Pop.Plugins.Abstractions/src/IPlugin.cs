using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public interface IPlugin
{
    string Name { get; }

    Assembly Assembly { get; }

    IPluginServiceCollection Services { get; }

    // remove setter in future versions
    // IServiceProvider is set by the plugin manager after the plugin is loaded
    // but it can be removed from IPlugin interface and keep it
    // somewhere else 
    IServiceProvider? ServiceProvider { get; set; }

    void ConfigureHostServices(IServiceCollection services);

    void ConfigurePluginServices();
}
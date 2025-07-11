using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public interface IPlugin
{
    string Name { get; }

    Assembly PluginAssembly { get; }

    IServiceCollection PluginServices { get; }

    IServiceProvider? PluginServiceProvider { get; }

    void ConfigureHostServices(IServiceCollection services);

    void ConfigureModuleServices();
}
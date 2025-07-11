using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

public interface IPlugin
{
    string Name { get; }

    Assembly Assembly { get; }

    IServiceCollection Services { get; }

    IServiceProvider? ServiceProvider { get; }

    void ConfigureHostServices(IServiceCollection services);

    void ConfigurePluginServices();
}
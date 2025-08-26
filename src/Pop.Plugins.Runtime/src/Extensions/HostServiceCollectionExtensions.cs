using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime.Extensions;

public static class HostServiceCollectionExtensions
{
    public static IServiceCollection AddPluginManager(
        this IServiceCollection services,
        Action<PluginManagerOptions> setup,
        Action<IPluginManager>? configure = null)
    {
        var options = new PluginManagerOptions();
        setup(options);

        services.TryAddSingleton<IPluginManager>(sp =>
        {
            var pluginManager = new PluginManager(options.PluginsFolder, services);

            configure?.Invoke(pluginManager);

            if (options.AutoLoadPlugins)
            {
                if (options.PluginFilter is not null)
                {
                    foreach (var dll in Directory.GetFiles(options.PluginsFolder, "*.dll")
                                                 .Where(options.PluginFilter))
                    {
                        pluginManager.LoadPlugin(dll);
                    }
                }
                else
                {
                    pluginManager.LoadPlugins();
                }
            }

            return pluginManager;
        });

        return services;
    }
}

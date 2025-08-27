using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime.Extensions;

/// <summary>
/// Extension methods for registering and configuring the plugin manager in the host application's service collection.
/// </summary>
public static class HostServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures the <see cref="IPluginManager"/> in the host application's service collection.
    /// </summary>
    /// <param name="services">The host application's service collection.</param>
    /// <param name="setup">An action to configure <see cref="PluginManagerOptions"/>.</param>
    /// <param name="configure">An optional action to further configure the <see cref="IPluginManager"/>.</param>
    /// <returns>The updated service collection.</returns>
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

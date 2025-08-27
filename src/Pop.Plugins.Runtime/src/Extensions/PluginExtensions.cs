using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime.Extensions;

/// <summary>
/// Extension methods for working with plugins.
/// </summary>
public static class PluginExtensions
{
    /// <summary>
    /// Builds the service provider for the specified plugin if it has not already been built.
    /// </summary>
    /// <param name="plugin">The plugin instance for which to build the service provider.</param>
    /// <returns>The built <see cref="IServiceProvider"/> for the plugin.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the plugin's service collection is null or empty.</exception>
    public static IServiceProvider BuildServiceProvider(this IPlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        // Check if the plugin already has a service provider
        if (plugin.ServiceProvider != null)
        {
            // If the service provider is already built, return it
            return plugin.ServiceProvider;
        }

        // Ensure the plugin has a service collection
        if (plugin.Services is null || plugin.Services.Count == 0)
        {
            throw new InvalidOperationException("Plugin's service collection is null or empty.");
        }

        var pluginServiceProvider = plugin.Services.BuildServiceProvider();
        plugin.ServiceProvider = pluginServiceProvider;

        return pluginServiceProvider;
    }
}

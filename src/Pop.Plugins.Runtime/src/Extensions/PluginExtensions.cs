using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime.Extensions;

public static class PluginExtensions
{
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pop.Plugins.Abstractions.Settings;
using Pop.Plugins.DependencyInjection.Abstractions;

namespace Pop.Plugins.Runtime.Extensions;

/// <summary>
/// Extension methods for working with plugin service collections.
/// </summary>
public static class PluginServiceCollectionExtensions
{
    /// <summary>
    /// Adds plugin settings and options to the plugin's service collection.
    /// </summary>
    /// <param name="services">The plugin's service collection.</param>
    /// <param name="pluginSettings">The settings to inject into the plugin's service collection.</param>
    /// <returns>The updated plugin service collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> or <paramref name="pluginSettings"/> is null.</exception>
    public static IPluginServiceCollection AddPluginOptions(this IPluginServiceCollection services, PluginSettings pluginSettings)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(pluginSettings);

        // Inject PluginSettings via IOptions<PluginSettings>
        services.AddSingleton(pluginSettings);
        services.AddSingleton<IOptions<PluginSettings>>(new OptionsWrapper<PluginSettings>(pluginSettings));

        return services;
    }

    //public static IServiceProvider BuildServiceProvider(this IPluginServiceCollection services)
    //{
    //    ArgumentNullException.ThrowIfNull(services);

    //    // Build the service provider from the collection
    //    return (services as IServiceCollection).BuildServiceProvider();
    //}
}

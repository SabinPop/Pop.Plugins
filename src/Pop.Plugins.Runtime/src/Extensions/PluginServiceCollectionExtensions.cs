using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pop.Plugins.Abstractions;
using Pop.Plugins.Abstractions.Settings;

namespace Pop.Plugins.Runtime.Extensions;

public static class PluginServiceCollectionExtensions
{
    public static IPluginServiceCollection AddPluginOptions(this IPluginServiceCollection services, PluginSettings pluginSettings)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(pluginSettings);

        // Inject PluginSettings via IOptions<PluginSettings>
        services.AddSingleton(pluginSettings);
        services.AddSingleton<IOptions<PluginSettings>>(new OptionsWrapper<PluginSettings>(pluginSettings));
        
        return services;
    }

    public static IServiceProvider BuildServiceProvider(this IPluginServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Build the service provider from the collection
        return (services as IServiceCollection).BuildServiceProvider();
    }
}

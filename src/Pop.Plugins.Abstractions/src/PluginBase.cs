using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions.Settings;
using Pop.Plugins.DependencyInjection.Abstractions;
using Pop.Plugins.Logging.Abstractions;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

/// <summary>
/// Base implementation of <see cref="IPlugin"/> that provides common functionality for plugins.
/// </summary>
public abstract class PluginBase : IPlugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginBase"/> class.
    /// </summary>
    /// <param name="pluginLoggingConfigurator">The logging configurator for plugin-specific logging.</param>
    /// <param name="pluginSettings">The settings for this plugin instance.</param>
    protected PluginBase(IPluginLoggingConfigurator pluginLoggingConfigurator,
        PluginSettings pluginSettings)
    {
        Assembly = GetType().Assembly;
        Name = Assembly.FullName!;
        Services = new PluginServiceCollection();
        PluginLoggingConfigurator = pluginLoggingConfigurator;
        PluginSettings = pluginSettings;
    }

    /// <summary>
    /// Gets the name of the plugin, typically derived from the assembly name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the assembly containing the plugin implementation.
    /// </summary>
    public Assembly Assembly { get; private set; }

    /// <summary>
    /// Gets the settings configured for this plugin.
    /// </summary>
    public PluginSettings PluginSettings { get; private set; }

    /// <summary>
    /// Gets the service collection specific to this plugin.
    /// Used for registering plugin-specific services.
    /// </summary>
    public IPluginServiceCollection Services { get; }

    /// <summary>
    /// Gets or sets the service provider built from the plugin's service collection.
    /// This is typically set by the plugin manager after loading the plugin.
    /// </summary>
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets the logging configurator used to set up plugin-specific logging.
    /// </summary>
    public IPluginLoggingConfigurator PluginLoggingConfigurator { get; private set; }

    /// <summary>
    /// Configures services that should be registered in the host application's service collection.
    /// </summary>
    /// <param name="services">The host application's service collection.</param>
    public virtual void ConfigureHostServices(IServiceCollection services)
    {

    }

    /// <summary>
    /// Configures services specific to this plugin.
    /// Called during plugin initialization to set up plugin-specific services.
    /// </summary>
    public virtual void ConfigurePluginServices()
    {
        PluginLoggingConfigurator.ConfigureLogging(Services, PluginSettings.Logging, Name);
    }
}

using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.DependencyInjection.Abstractions;
using System.Reflection;

namespace Pop.Plugins.Abstractions;

/// <summary>
/// Represents a plugin that can be loaded and managed at runtime.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Gets the name of the plugin, typically derived from the assembly name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the assembly containing the plugin implementation.
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    /// Gets the service collection specific to this plugin.
    /// Used for registering plugin-specific services.
    /// </summary>
    IPluginServiceCollection Services { get; }

    // remove setter in future versions
    // IServiceProvider is set by the plugin manager after the plugin is loaded
    // but it can be removed from IPlugin interface and keep it
    // somewhere else 
    /// <summary>
    /// Gets or sets the service provider built from the plugin's service collection.
    /// This is typically set by the plugin manager after loading the plugin.
    /// </summary>
    IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Configures services that should be registered in the host application's service collection.
    /// </summary>
    /// <param name="services">The host application's service collection.</param>
    void ConfigureHostServices(IServiceCollection services);

    /// <summary>
    /// Configures services specific to this plugin.
    /// Called during plugin initialization to set up plugin-specific services.
    /// </summary>
    void ConfigurePluginServices();
}
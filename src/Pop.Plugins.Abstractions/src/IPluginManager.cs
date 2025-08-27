using Microsoft.Extensions.DependencyInjection;

namespace Pop.Plugins.Abstractions;

/// <summary>
/// Manages the loading, unloading, and configuration of plugins at runtime.
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// Gets the folder path where plugin assemblies (DLLs) are located.
    /// </summary>
    string PluginsFolder { get; }

    /// <summary>
    /// Gets a read-only list of currently loaded plugins.
    /// </summary>
    IReadOnlyList<IPlugin> Plugins { get; }

    /// <summary>
    /// Loads all plugin assemblies from the configured plugins folder.
    /// </summary>
    void LoadPlugins();

    /// <summary>
    /// Loads a specific plugin assembly from the given path.
    /// </summary>
    /// <param name="dllPath">The path to the plugin assembly file.</param>
    void LoadPlugin(string dllPath);

    /// <summary>
    /// Unloads a specific plugin assembly and releases its resources.
    /// </summary>
    /// <param name="dllPath">The path to the plugin assembly file that was previously loaded.</param>
    void UnloadPlugin(string dllPath);

    /// <summary>
    /// Registers services that should be shared across all plugins.
    /// </summary>
    /// <param name="registration">The action to configure shared services.</param>
    void RegisterSharedServices(Action<IServiceCollection> registration);
}

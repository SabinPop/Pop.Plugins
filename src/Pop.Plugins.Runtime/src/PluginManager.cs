using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;
using Pop.Plugins.Abstractions.Settings;
using Pop.Plugins.Logging;
using Pop.Plugins.Logging.Abstractions;
using Pop.Plugins.Runtime.Extensions;

namespace Pop.Plugins.Runtime;

/// <summary>
/// Manages the loading, unloading, and configuration of plugins at runtime.
/// </summary>
internal class PluginManager : IPluginManager
{
    private readonly IServiceCollection _sharedServices;
    private readonly List<IPlugin> _plugins = [];
    private readonly List<Action<IServiceCollection>> _sharedRegistrations = [];
    private readonly Dictionary<string, (IPlugin Plugin, PluginLoadContext Context)> _loadedPlugins = [];

    /// <summary>
    /// Gets the folder path where plugin assemblies (DLLs) are located.
    /// </summary>
    public string PluginsFolder { get; private set; }

    /// <summary>
    /// Gets a read-only list of currently loaded plugins.
    /// </summary>
    public IReadOnlyList<IPlugin> Plugins => _plugins;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManager"/> class.
    /// </summary>
    /// <param name="pluginsFolder">The folder path containing plugin assemblies.</param>
    /// <param name="sharedServices">The host application's shared service collection.</param>
    public PluginManager(string pluginsFolder, IServiceCollection sharedServices)
    {
        PluginsFolder = pluginsFolder;
        _sharedServices = sharedServices;
    }

    /// <summary>
    /// Registers services that should be shared across all plugins.
    /// </summary>
    /// <param name="registration">The action to configure shared services.</param>
    public void RegisterSharedServices(Action<IServiceCollection> registration)
        => _sharedRegistrations.Add(registration);

    /// <summary>
    /// Loads all plugin assemblies from the configured plugins folder.
    /// </summary>
    public void LoadPlugins()
    {
        foreach (var dll in Directory.GetFiles(PluginsFolder, "*.dll"))
        {
            LoadPlugin(dll);
        }
    }

    /// <summary>
    /// Loads a specific plugin assembly from the given path.
    /// </summary>
    /// <param name="dllPath">The path to the plugin assembly file.</param>
    public void LoadPlugin(string dllPath)
    {
        if (_loadedPlugins.ContainsKey(dllPath))
        {
            return;
        }

        var loadContext = new PluginLoadContext(dllPath);
        var pluginAssembly = loadContext.LoadFromAssemblyPath(dllPath);
        Type[] pluginAssemblyTypes = pluginAssembly.GetTypes();

        var type = pluginAssemblyTypes
            .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface);

        if (type is null)
        {
            return;
        }

        // Get the plugin implementation of IPluginLoggingConfigurator
        // If not found, use the default implementation
        var pluginLoggingConfigurator = pluginAssemblyTypes
            .FirstOrDefault(t => typeof(IPluginLoggingConfigurator).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface)
            ?? typeof(PluginLoggingConfigurator);

        var pluginSettingsType = pluginAssemblyTypes
            .FirstOrDefault(t => typeof(PluginSettings).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface)
            ?? typeof(PluginSettings);

        var pluginSettings = PluginConfigurationLoader.LoadSettings(dllPath, pluginSettingsType);

        object[] args = [(IPluginLoggingConfigurator)Activator.CreateInstance(pluginLoggingConfigurator)!];
        var plugin = (IPlugin)Activator.CreateInstance(type, args)!;

        plugin.Services.AddPluginOptions(pluginSettings);
        plugin.ConfigurePluginServices();
        plugin.ConfigureHostServices(_sharedServices);

        foreach (var registration in _sharedRegistrations)
        {
            registration(plugin.Services);
        }

        plugin.BuildServiceProvider();

        _plugins.Add(plugin);
        _loadedPlugins[dllPath] = (plugin, loadContext);
    }

    /// <summary>
    /// Unloads a specific plugin assembly and releases its resources.
    /// </summary>
    /// <param name="dllPath">The path to the plugin assembly file that was previously loaded.</param>
    public void UnloadPlugin(string dllPath)
    {
        if (!_loadedPlugins.TryGetValue(dllPath, out var tuple))
        {
            return;
        }

        (var plugin, var pluginContext) = tuple;
        _plugins.Remove(plugin);
        _loadedPlugins.Remove(dllPath);
        pluginContext.Unload();

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
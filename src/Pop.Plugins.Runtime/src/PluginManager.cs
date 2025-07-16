using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;
using Pop.Plugins.Abstractions.Settings;
using Pop.Plugins.Logging;
using Pop.Plugins.Runtime.Extensions;

namespace Pop.Plugins.Runtime;

internal class PluginManager : IPluginManager
{
    private readonly IServiceCollection _sharedServices;
    private readonly List<IPlugin> _plugins = [];
    private readonly List<Action<IServiceCollection>> _sharedRegistrations = [];
    private readonly Dictionary<string, (IPlugin Plugin, PluginLoadContext Context)> _loadedPlugins = [];

    public string PluginsFolder { get; private set; }
    public IReadOnlyList<IPlugin> Plugins => _plugins;

    public PluginManager(string pluginsFolder, IServiceCollection sharedServices)
    {
        PluginsFolder = pluginsFolder;
        _sharedServices = sharedServices;
    }

    public void RegisterSharedServices(Action<IServiceCollection> registration)
        => _sharedRegistrations.Add(registration);

    public void LoadModules()
    {
        foreach (var dll in Directory.GetFiles(PluginsFolder, "*.dll"))
        {
            LoadModule(dll);
        }
    }

    public void LoadModule(string dllPath)
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

        // Get the module implementation of IPluginLoggerConfigurator
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

    public void UnloadModule(string dllPath)
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
using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;
using Pop.Plugins.Logging;

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
        var type = pluginAssembly
            .GetTypes()
            .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface);

        if (type is null)
        {
            return;
        }

        // Get the module implementation of IPluginLoggerConfigurator
        // If not found, use the default implementation
        var pluginLoggerConfigurator = pluginAssembly
            .GetTypes()
            .FirstOrDefault(t => typeof(IPluginLoggerConfigurator).IsAssignableFrom(t)
                        && !t.IsAbstract
                        && !t.IsInterface);

        if (pluginLoggerConfigurator is null)
        {
            pluginLoggerConfigurator = typeof(DefaultPluginLoggerConfigurator);
        }

        object[] args = [(IPluginLoggerConfigurator)Activator.CreateInstance(pluginLoggerConfigurator)!];
        var plugin = (IPlugin)Activator.CreateInstance(type, args)!;
        plugin.ConfigurePluginServices();
        plugin.ConfigureHostServices(_sharedServices);

        foreach (var registration in _sharedRegistrations)
        {
            registration(plugin.Services);
        }

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
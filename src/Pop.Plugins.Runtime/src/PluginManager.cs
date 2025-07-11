using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime;

internal class PluginManager : IPluginManager
{
    private readonly IServiceCollection _sharedServices;
    private readonly List<IPlugin> _plugins = [];
    private readonly List<Action<IServiceCollection>> _sharedRegistrations = [];
    private readonly Dictionary<string, (IPlugin Plugin, PluginLoadContext Context)> _loaded = [];

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
        if (_loaded.ContainsKey(dllPath))
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

        var plugin = (IPlugin)Activator.CreateInstance(type)!;
        plugin.ConfigureModuleServices();
        plugin.ConfigureHostServices(_sharedServices);

        foreach (var reg in _sharedRegistrations)
        {
            reg(plugin.PluginServices);
        }

        _plugins.Add(plugin);
        _loaded[dllPath] = (plugin, loadContext);
    }

    public void UnloadModule(string dllPath)
    {
        if (!_loaded.TryGetValue(dllPath, out var tuple))
        {
            return;
        }

        _plugins.Remove(tuple.Plugin);
        _loaded.Remove(dllPath);

        tuple.Context.Unload();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
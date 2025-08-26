using Microsoft.Extensions.DependencyInjection;

namespace Pop.Plugins.Abstractions;

public interface IPluginManager
{
    string PluginsFolder { get; }

    IReadOnlyList<IPlugin> Plugins { get; }

    void LoadPlugins();

    void LoadPlugin(string dllPath);

    void UnloadPlugin(string dllPath);

    void RegisterSharedServices(Action<IServiceCollection> registration);
}

using Microsoft.Extensions.DependencyInjection;

namespace Pop.Plugins.Abstractions;

public interface IPluginManager
{
    string PluginsFolder { get; }

    IReadOnlyList<IPlugin> Plugins { get; }

    void LoadModules();

    void LoadModule(string dllPath);

    void UnloadModule(string dllPath);

    void RegisterSharedServices(Action<IServiceCollection> registration);
}

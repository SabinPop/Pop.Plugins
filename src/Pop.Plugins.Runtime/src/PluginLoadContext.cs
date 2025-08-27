using System.Reflection;
using System.Runtime.Loader;

namespace Pop.Plugins.Runtime;

/// <summary>
/// Provides a collectible <see cref="AssemblyLoadContext"/> for loading plugin assemblies and their dependencies in isolation.
/// </summary>
internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginLoadContext"/> class for the specified plugin path.
    /// </summary>
    /// <param name="pluginPath">The file path to the plugin assembly.</param>
    public PluginLoadContext(string pluginPath)
        : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    /// <summary>
    /// Loads the assembly with the specified name, resolving dependencies using the plugin's path.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly to load.</param>
    /// <returns>The loaded <see cref="Assembly"/>, or null if not found.</returns>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var path = _resolver.ResolveAssemblyToPath(assemblyName);
        if (path is null)
        {
            return null;
        }

        return LoadFromAssemblyPath(path);
    }
}

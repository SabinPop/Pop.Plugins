using System.Reflection;
using System.Runtime.Loader;

namespace Pop.Plugins.Runtime;

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string modulePath)
        : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(modulePath);
    }

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

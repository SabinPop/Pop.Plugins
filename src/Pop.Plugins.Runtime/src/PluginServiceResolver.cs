using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime;

/// <summary>
/// Resolves services from loaded plugins and the global service provider, with caching support.
/// </summary>
internal class PluginServiceResolver : IPluginServiceResolver, IDisposable
{
    private readonly IServiceProvider _globalServiceProvider;
    private readonly IEnumerable<IPlugin> _plugins;
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginServiceResolver"/> class.
    /// </summary>
    /// <param name="globalServiceProvider">The global service provider for the host application.</param>
    /// <param name="plugins">The collection of loaded plugins.</param>
    public PluginServiceResolver(IServiceProvider globalServiceProvider, IEnumerable<IPlugin> plugins)
    {
        _globalServiceProvider = globalServiceProvider;
        _plugins = plugins;
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    /// <summary>
    /// Attempts to resolve a service of the specified type from plugins or the global service provider.
    /// </summary>
    /// <param name="typeOfService">The type of service to resolve.</param>
    /// <returns>The resolved service instance, or null if not found.</returns>
    public object? TryResolve(Type typeOfService)
    {
        if (_memoryCache.TryGetValue(typeOfService, out var cachedService))
        {
            return cachedService;
        }

        foreach (var plugin in _plugins)
        {
            var service = plugin.ServiceProvider!.GetService(typeOfService);
            if (service != null)
            {
                _memoryCache.Set(typeOfService, service);
                return service;
            }
        }

        var globalService = _globalServiceProvider.GetService(typeOfService);
        if (globalService != null)
        {
            _memoryCache.Set(typeOfService, globalService);
            return globalService;
        }

        return null;
    }

    /// <summary>
    /// Attempts to resolve a service of the specified generic type from plugins or the global service provider.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <returns>The resolved service instance, or null if not found.</returns>
    public TService? TryResolve<TService>() where TService : class
    {
        ArgumentNullException.ThrowIfNull(typeof(TService));

        if (_memoryCache.TryGetValue(typeof(TService), out var cachedService))
        {
            return cachedService as TService;
        }

        foreach (var plugin in _plugins)
        {
            var service = plugin.ServiceProvider!.GetService<TService>();
            if (service != null)
            {
                _memoryCache.Set(typeof(TService), service);
                return service;
            }
        }

        var globalService = _globalServiceProvider.GetService<TService>();
        if (globalService != null)
        {
            _memoryCache.Set(typeof(TService), globalService);
            return globalService;
        }
        return null;
    }

    /// <summary>
    /// Disposes the memory cache used for service resolution.
    /// </summary>
    public void Dispose()
    {
        _memoryCache.Dispose();
    }
}

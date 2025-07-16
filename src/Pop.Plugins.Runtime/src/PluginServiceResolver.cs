using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Pop.Plugins.Abstractions;

namespace Pop.Plugins.Runtime;

internal class PluginServiceResolver : IPluginServiceResolver, IDisposable
{
    private readonly IServiceProvider _globalServiceProvider;
    private readonly IEnumerable<IPlugin> _plugins;
    private readonly IMemoryCache _memoryCache;

    public PluginServiceResolver(IServiceProvider globalServiceProvider, IEnumerable<IPlugin> plugins)
    {
        _globalServiceProvider = globalServiceProvider;
        _plugins = plugins;
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

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

    public void Dispose()
    {
        _memoryCache.Dispose();
    }
}

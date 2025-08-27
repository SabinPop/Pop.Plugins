namespace Pop.Plugins.Abstractions;

/// <summary>
/// Resolves services from loaded plugins and the global service provider.
/// </summary>
public interface IPluginServiceResolver
{
    /// <summary>
    /// Attempts to resolve a service of the specified type from plugins or the global service provider.
    /// </summary>
    /// <param name="typeOfService">The type of service to resolve.</param>
    /// <returns>The resolved service instance, or null if not found.</returns>
    object? TryResolve(Type typeOfService);

    /// <summary>
    /// Attempts to resolve a service of the specified generic type from plugins or the global service provider.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <returns>The resolved service instance, or null if not found.</returns>
    TService? TryResolve<TService>() where TService : class;
}

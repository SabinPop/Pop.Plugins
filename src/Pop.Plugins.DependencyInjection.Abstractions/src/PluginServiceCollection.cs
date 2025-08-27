using Microsoft.Extensions.DependencyInjection;

namespace Pop.Plugins.DependencyInjection.Abstractions;

/// <summary>
/// Default implementation for <see cref="IPluginServiceCollection"/>.
/// Provides a service collection for registering plugin-specific services.
/// </summary>
public class PluginServiceCollection : ServiceCollection, IPluginServiceCollection
{

}
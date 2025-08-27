using Microsoft.Extensions.DependencyInjection;

namespace Pop.Plugins.DependencyInjection.Abstractions;

/// <summary>
/// Represents a service collection for plugins, extending <see cref="IServiceCollection"/>.
/// </summary>
public interface IPluginServiceCollection : IServiceCollection
{

}
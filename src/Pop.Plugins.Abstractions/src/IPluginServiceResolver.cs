namespace Pop.Plugins.Abstractions;

public interface IPluginServiceResolver
{
    object? TryResolve(Type typeOfService);

    TService? TryResolve<TService>() where TService : class;
}

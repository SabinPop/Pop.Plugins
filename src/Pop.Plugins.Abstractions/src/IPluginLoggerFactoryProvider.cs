using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Pop.Plugins.Abstractions;

public interface IPluginLoggerConfigurator
{
    void ConfigureLogging(IServiceCollection services, string pluginName);

    ILoggerFactory GetLoggerFactory(string pluginName);
}


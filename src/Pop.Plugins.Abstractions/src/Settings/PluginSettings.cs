using Pop.Plugins.Logging.Abstractions;

namespace Pop.Plugins.Abstractions.Settings;

/// <summary>
/// Represents configuration settings for a plugin.
/// </summary>
public class PluginSettings
{
    /// <summary>
    /// Gets or sets the logging settings for the plugin.
    /// </summary>
    public PluginLoggingSettings Logging { get; set; } = new();
}

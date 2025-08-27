namespace Pop.Plugins.Logging.Abstractions;

/// <summary>
/// Represents logging configuration settings for a plugin.
/// </summary>
public class PluginLoggingSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether logging is enabled for the plugin.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the minimum log level for the plugin (e.g., "Information", "Debug").
    /// </summary>
    public string? MinimumLevel { get; set; } = "Information";
}

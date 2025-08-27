namespace Pop.Plugins.Runtime;

/// <summary>
/// Configuration options for the plugin manager.
/// </summary>
public class PluginManagerOptions
{
    /// <summary>
    /// Gets or sets the folder path where plugin assemblies (DLLs) are located.
    /// </summary>
    public string PluginsFolder { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional filter to determine which DLLs should be loaded as plugins.
    /// Returns true if the DLL should be loaded.
    /// </summary>
    public Func<string, bool>? PluginFilter { get; set; }

    /// <summary>
    /// Gets or sets whether plugins should be automatically loaded during initialization.
    /// </summary>
    public bool AutoLoadPlugins { get; set; } = true;
}

namespace Pop.Plugins.Abstractions.Settings;

public class PluginLoggingSettings
{
    public bool Enabled { get; set; } = true;

    public string? MinimumLevel { get; set; } = "Trace";
}
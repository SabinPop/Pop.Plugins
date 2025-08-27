using Pop.Plugins.Abstractions.Settings;
using System.Text.Json;

namespace Pop.Plugins.Runtime;

/// <summary>
/// Provides methods for loading plugin configuration settings from JSON files.
/// </summary>
public static class PluginConfigurationLoader
{
    /// <summary>
    /// Loads plugin settings from a JSON file located next to the plugin assembly.
    /// If the settings file does not exist, returns a default instance of <see cref="PluginSettings"/>.
    /// </summary>
    /// <param name="pluginPath">The file path to the plugin assembly.</param>
    /// <param name="pluginSettingsType">The type of the plugin settings to deserialize.</param>
    /// <returns>An instance of <see cref="PluginSettings"/> loaded from the JSON file, or a default instance if the file does not exist.</returns>
    public static PluginSettings LoadSettings(string pluginPath, Type pluginSettingsType)
    {
        var pluginFileName = Path.GetFileNameWithoutExtension(pluginPath);
        var settingsPath = Path.Combine(Path.GetDirectoryName(pluginPath)!, $"{pluginFileName}.settings.json");
        if (!File.Exists(settingsPath))
        {
            return (PluginSettings)Activator.CreateInstance(pluginSettingsType)!; // return default config
        }

        var json = File.ReadAllText(settingsPath);
        var settings = JsonSerializer.Deserialize(json, pluginSettingsType);
        return (PluginSettings)settings!;
    }
}
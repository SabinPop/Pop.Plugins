using Pop.Plugins.Abstractions.Settings;
using System.Text.Json;

namespace Pop.Plugins.Runtime;

public static class PluginConfigurationLoader
{
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
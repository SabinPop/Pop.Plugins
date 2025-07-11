namespace Pop.Plugins.Runtime;

public class PluginManagerOptions
{
    /// <summary>
    /// Folderul din care sunt încărcate pluginurile (fișiere .dll).
    /// </summary>
    public string PluginsFolder { get; set; } = string.Empty;

    /// <summary>
    /// Filtru opțional pentru a selecta ce DLL-uri se încarcă.
    /// Returnează true dacă DLL-ul trebuie încărcat.
    /// </summary>
    public Func<string, bool>? PluginFilter { get; set; }

    /// <summary>
    /// Dacă este true, încarcă automat pluginurile la inițializare.
    /// </summary>
    public bool AutoLoadPlugins { get; set; } = true;
}

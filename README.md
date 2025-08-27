# Pop.Plugins

**Pop.Plugins** is a lightweight and extensible plugin framework for .NET 8+ that enables runtime discovery and loading of modular plugins (DLLs) into your .NET applications — including Console apps, Web APIs, or Background Services.

---

## 📦 Packages & Dependencies

| Package                                                                                                           | Description                                                       |
|-------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------|
| [`Pop.Plugins.Abstractions`](src/Pop.Plugins.Abstractions/src/README.md)                                          | Interfaces and base types required to implement plugins           |
| [`Pop.Plugins.DependencyInjection.Abstractions`](src/Pop.Plugins.DependencyInjection.Abstractions/src/README.md)  | Abstractions for plugin DI container                              |
| [`Pop.Plugins.Logging`](src/Pop.Plugin.Logging/src/README.md)                                                     | Logging configuration helpers for plugin-specific log categories  |
| [`Pop.Plugins.Logging.Abstractions`](src/Pop.Plugins.Logging.Abstractions/src/README.md)                          | Logging abstractions for plugin logging                           |
| [`Pop.Plugins.Runtime`](src/Pop.Plugins.Runtime/src/README.md)                                                    | Plugin loader and runtime manager for loading/unloading plugins   |

**Required NuGet packages:**
- Microsoft.Extensions.Caching.Memory (for service resolution caching)
- Microsoft.Extensions.DependencyInjection (for DI)
- Microsoft.Extensions.Options (for options/configuration)

---

## 📝 Documentation
- All public and internal APIs are documented with XML comments (in English).
- See source code for inline documentation of types, methods, parameters, and return values.

---

## 🚀 Getting Started
### 1. Register the PluginManager in your host application:

```csharp
builder.Services.AddPluginManager(options =>
{
    options.PluginsFolder = Path.Combine(AppContext.BaseDirectory, "Plugins");
    options.PluginFilter = dllPath => Path.GetFileName(dllPath).Contains("Plugin.");
    options.AutoLoadPlugins = true;
},
configure: pluginManager =>
{
    pluginManager.RegisterSharedServices(services =>
    {
        services.AddSingleton<IMyService, MyService>();
    });
});
```

### 2. Create a Plugin
Create a class library project and reference `Pop.Plugins.Abstractions`. Then implement your plugin:

```csharp
public class MyPlugin : PluginBase
{
    public MyPlugin(IPluginLoggingConfigurator pluginLoggerConfigurator, PluginSettings pluginSettings)
      : base(pluginLoggerConfigurator, pluginSettings)
    {
    }

    public override void ConfigurePluginServices()
    {
        Services.AddSingleton<IMyPluginService, MyPluginService>();
    }

    public override void ConfigureHostServices(IServiceCollection services)
    {
        services.AddTransient<IMyPluginService, MyPluginService>();
    }
}
```

### 3. Plugin Configuration
- Place a JSON settings file next to your plugin DLL (e.g., `MyPlugin.settings.json`).
- The framework will automatically load settings using `PluginConfigurationLoader`.

### 4. Deploy the Plugin
- Build your plugin project
- Copy the resulting `.dll` and `.settings.json` into the plugins/ folder of your host application
- On next run, it will be automatically loaded

---

## 🔄 Features
- Automatic loading of plugin configuration from JSON
- Custom `AssemblyLoadContext` per plugin for isolation
- Optional shared service registration
- Per-plugin dependency injection container
- Plugin-scoped logging (`ILogger<MyService>` uses plugin category)
- Plugin unload support (experimental)
- Service resolution with caching (`PluginServiceResolver`)

---

## 🧩 Plugin Development Guide
- Document your public APIs with XML comments
- Implement your plugin by inheriting from `PluginBase` or `IPlugin`
- Register host/shared services in `ConfigureHostServices`
- Register plugin-specific services in `ConfigurePluginServices`
- Use `PluginSettings` for configuration and `PluginLoggingSettings` for logging

---

## 📚 API Reference
- All types, methods, and parameters are documented in source code with XML comments
- See source for details on:
  - `IPlugin`
  - `IPluginManager`
  - `PluginBase`
  - `PluginConfigurationLoader`
  - `PluginLoadContext`
  - `PluginManager`
  - `PluginServiceResolver`

---

## 🤝 Contribution Guidelines
- Ensure all new code is covered by XML documentation
- Follow .NET coding conventions and document all public/internal APIs
- Fork the repository and create a feature branch
- Submit pull requests with clear descriptions

---

## 📄 License
MIT License

---

## 💬 Support / Contact
For issues, feature requests, or questions, open a GitHub issue or contact the maintainer.


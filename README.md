# Pop.Plugins

**Pop.Plugins** is a lightweight and extensible plugin framework for .NET 8+ that enables runtime discovery and loading of modular plugins (DLLs) into your .NET applications — including Console apps, Web APIs, or Background Services.

---

## 📦 Packages

| Package                   | Description                                                       |
|---------------------------|-------------------------------------------------------------------|
| `Pop.Plugins.Abstractions` | Interfaces and base types required to implement plugins            |
| `Pop.Plugins.Logging`      | Logging configuration helpers for plugin-specific log categories   |
| `Pop.Plugins.Runtime`      | Plugin loader and runtime manager for loading/unloading plugins    |

---

## 🚀 Installation

Add the following packages to your project via NuGet (when published) or project reference:

```bash
dotnet add package Pop.Plugins.Abstractions
dotnet add package Pop.Plugins.Runtime
dotnet add package Pop.Plugins.Logging
```

## 🔧 Getting Started
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

## 2. Create a Plugin
### Create a class library project and reference Pop.Plugins.Abstractions. Then implement your plugin:

```csharp
public class MyPlugin : BasePlugin
{
    public MyPlugin(IPluginLoggerConfigurator pluginLoggerConfigurator)
      : base(pluginLoggerConfigurator)
    {
      
    }

    public override void ConfigureModuleServices()
    {
        PluginServices.AddSingleton<IMyPluginService, MyPluginService>();
    }

    public override void ConfigureHostServices(IServiceCollection services)
    {
        services.AddTransient<IMyPluginService, MyPluginService>();
    }
}
```

## 3. Deploy the Plugin
- Build your plugin project
- Copy the resulting .dll into the plugins/ folder of your host application
- On next run, it will be automatically loaded

 ## 🔄 Features
- Custom AssemblyLoadContext per plugin
- Per-plugin dependency injection container
- Plugin-scoped logging (ILogger<MyService> uses plugin category)
- Optional shared service registration
- Plugin unload support (experimental)




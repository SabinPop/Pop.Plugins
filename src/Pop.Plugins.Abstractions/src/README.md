# Pop.Plugins.Abstractions

This project defines the core interfaces and base types required to implement plugins in the Pop.Plugins framework.

## Purpose
- Provides the `IPlugin` interface and related abstractions for plugin development.
- Enables plugins to register their own services and interact with the host application.

## Usage
- Reference this project in your plugin implementations and host applications.
- Implement the `IPlugin` interface or inherit from `PluginBase` to create plugins.

## Features
- Plugin lifecycle management interfaces
- Dependency injection abstractions for plugins
- Settings and configuration abstractions

## Integration
- Used by both plugin projects and host applications to ensure compatibility and extensibility.

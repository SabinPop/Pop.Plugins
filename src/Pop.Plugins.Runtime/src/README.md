# Pop.Plugins.Runtime

This project handles plugin loading, unloading, and runtime management for the Pop.Plugins framework.

## Purpose
- Discovers, loads, and manages plugins at runtime in .NET applications.
- Provides plugin isolation, configuration loading, and service resolution.

## Usage
- Reference this project in your host application to enable runtime plugin management.
- Use `PluginManager` to load/unload plugins and manage their lifecycles.

## Features
- Plugin discovery and loading from DLLs
- Custom `AssemblyLoadContext` for plugin isolation
- Plugin configuration loading from JSON
- Service resolution and caching
- Plugin unload support (experimental)

## Integration
- Works with `Pop.Plugins.Abstractions` and other Pop.Plugins packages for a complete plugin system.

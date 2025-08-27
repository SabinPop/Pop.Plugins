# Pop.Plugins.DependencyInjection.Abstractions

This project provides abstractions for plugin dependency injection containers in the Pop.Plugins framework.

## Purpose
- Defines interfaces and types for plugin-specific service registration and resolution.
- Enables plugins to manage their own DI containers independently of the host.

## Usage
- Reference this project in plugins to register and resolve services using DI abstractions.
- Use `IPluginServiceCollection` and related types for service management.

## Features
- Service collection abstraction for plugins
- Integration with Microsoft.Extensions.DependencyInjection.Abstractions

## Integration
- Used by plugin projects and the host to ensure consistent DI behavior across plugins.

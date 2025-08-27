# Pop.Plugins.Logging

This project provides logging configuration helpers for plugins in the Pop.Plugins framework.

## Purpose
- Enables plugin-scoped logging using Microsoft.Extensions.Logging.
- Provides default and customizable logging configurators for plugins.

## Usage
- Reference this project in plugins or host applications to enable logging for plugins.
- Use `PluginLoggingConfigurator` to configure logging for your plugin services.

## Features
- Logging configurator for plugin assemblies
- Support for minimum log level and log filtering
- Integration with DI and logging abstractions

## Integration
- Works with `Pop.Plugins.Abstractions` and `Pop.Plugins.Logging.Abstractions` for consistent logging across plugins.

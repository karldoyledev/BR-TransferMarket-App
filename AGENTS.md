# AGENTS.md — AI Coding Agent Guidelines

## Project: BR TransferMarket App

Cross-platform (.NET MAUI / C#) mobile app for the Brazilian football transfer market.

## Conventions

- **Language**: C# 13 (.NET 10)
- **Architecture**: MVVM — Models in `Models/`, ViewModels in `ViewModels/`, XAML Views in `Views/`
- **Naming**: PascalCase for classes/properties, camelCase for private fields prefixed with `_`
- **XAML**: Pages with `x:DataType` for compiled bindings, converters in `Converters/`
- **DI**: Register services and ViewModels in `MauiProgram.cs` via `builder.Services`
- **Nullability**: Nullable reference types enabled — avoid `!` suppression unless necessary

## Rules

1. Keep ViewModels focused — one responsibility per ViewModel
2. Use `BaseViewModel` for `INotifyPropertyChanged` boilerplate
3. Prefer compiled bindings (`x:DataType`) in XAML
4. Put reusable value converters in `Converters/`
5. Navigation via Shell routing (`Shell.Current.GoToAsync`)
6. No business logic in code-behind — delegate to ViewModels or Services
7. Document public APIs with XML doc comments (`///`)

## Current Phase

🚧 **Phase 1** — Scaffolding + authentication shell complete. Login flow (mock auth, session, navigation), unit/integration test suite, and a Windows-runnable MAUI app are in place. Next up: transfer market dashboard, player/club browsing, and Appium UI tests.
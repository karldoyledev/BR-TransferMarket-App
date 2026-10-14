# BR TransferMarket App

Cross-platform mobile application for tracking and exploring the Brazilian football transfer market — player valuations, rumors, completed deals, and market analytics.

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | .NET MAUI (C#) |
| Target | iOS + Android + Windows |
| Architecture | MVVM (Model-View-ViewModel) |
| .NET Version | 10.0 |

## Main Objective

Build a mobile-first transfer market experience where users can:

- Browse player profiles with stats, valuation, and contract info
- Track transfer rumors and confirmed deals
- Filter by club, position, nationality, market value
- Save/watch players and clubs
- (Future) Real-time notifications for breaking transfer news

## Getting Started

```bash
# Prerequisites: .NET 10 SDK + MAUI workload
dotnet workload install maui

# Build everything (Core, app, tests)
dotnet build BR-TransferMarket-App.slnx

# Run on Windows
dotnet build TransferMarketApp/TransferMarketApp.csproj -f net10.0-windows10.0.19041.0
# then launch TransferMarketApp/bin/Debug/net10.0-windows10.0.19041.0/win-x64/TransferMarketApp.exe

# Note: the Android target requires the Android SDK (see https://aka.ms/dotnet-android-install-sdk)
```

Demo login: `admin` / `password`.

## Project Structure

```
BR-TransferMarket-App/
├── TransferMarketApp/           # .NET MAUI app (Shell, Views, platform services)
├── TransferMarketApp.Core/      # Platform-agnostic Models, ViewModels, Services
├── TransferMarketApp.Tests/     # xUnit unit + integration tests
├── TransferMarketApp.UITests/   # Appium UI tests (scaffold — no tests yet)
└── BR-TransferMarket-App.slnx   # Solution
```

## Status

🚧 **Phase 1 — Core scaffolding & authentication shell** (login flow, session, navigation).
Next: transfer market dashboard, player/club browsing, and UI tests.

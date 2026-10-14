using Microsoft.Extensions.DependencyInjection;
using TransferMarketApp.Core.Services;
using TransferMarketApp.Core.ViewModels;

namespace TransferMarketApp.Core;

/// <summary>
/// Registers the platform-agnostic core services in any IServiceCollection.
/// Called by the MAUI app (and by integration tests) so the same wiring
/// is exercised everywhere.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ISessionService, SessionService>();

        // ViewModels
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<MainViewModel>();

        return services;
    }
}
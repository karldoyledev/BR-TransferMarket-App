using Microsoft.Extensions.DependencyInjection;
using TransferMarketApp.Core;
using TransferMarketApp.Core.Services;
using TransferMarketApp.Core.ViewModels;
using TransferMarketApp.Tests.Fakes;

namespace TransferMarketApp.Tests.Integration;

/// <summary>
/// Verifies the DI wiring registered by AddCoreServices resolves every
/// consumer with the correct runtime dependencies (integration test).
/// </summary>
public class DependencyInjectionTests
{
    [Fact]
    public void AddCoreServices_ResolvesLoginViewModel_WithFakes()
    {
        var services = new ServiceCollection();
        services.AddSingleton<INavigationService, FakeNavigationService>();
        services.AddSingleton<IDialogService, FakeDialogService>();
        services.AddCoreServices();

        using var provider = services.BuildServiceProvider();

        var vm = provider.GetRequiredService<LoginViewModel>();
        Assert.NotNull(vm);
    }

    [Fact]
    public void AddCoreServices_RegistersAuthAndSession_AsSingletons()
    {
        var services = new ServiceCollection();
        services.AddCoreServices();

        using var provider = services.BuildServiceProvider();

        var auth1 = provider.GetRequiredService<IAuthService>();
        var auth2 = provider.GetRequiredService<IAuthService>();
        var session1 = provider.GetRequiredService<ISessionService>();
        var session2 = provider.GetRequiredService<ISessionService>();

        Assert.Same(auth1, auth2);       // singleton
        Assert.Same(session1, session2); // singleton
        Assert.IsType<AuthService>(auth1);
        Assert.IsType<SessionService>(session1);
    }

    [Fact]
    public void AddCoreServices_ResolvesMainViewModel()
    {
        var services = new ServiceCollection();
        services.AddCoreServices();

        using var provider = services.BuildServiceProvider();

        var vm = provider.GetRequiredService<MainViewModel>();
        Assert.NotNull(vm);
    }
}
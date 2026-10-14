using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Tests.Integration;

/// <summary>
/// Integration test using the real AuthService + SessionService together
/// to verify the full happy-path auth flow (minus navigation/UI).
/// </summary>
public class AuthFlowTests
{
    [Fact]
    public async Task FullFlow_LoginDemoUser_EstablishesSession()
    {
        var auth = new AuthService();
        var session = new SessionService();

        var user = await auth.LoginAsync(AuthService.DemoUsername, AuthService.DemoPassword);
        session.StartSession(user, rememberMe: false);

        Assert.True(session.IsAuthenticated);
        Assert.Same(user, session.CurrentUser);

        session.EndSession();
        Assert.False(session.IsAuthenticated);
    }

    [Fact]
    public async Task FullFlow_InvalidCredentials_Throws_AndSessionStaysEmpty()
    {
        var auth = new AuthService();
        var session = new SessionService();

        await Assert.ThrowsAsync<AuthenticationException>(() => auth.LoginAsync("nope", "nope"));

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.CurrentUser);
    }
}
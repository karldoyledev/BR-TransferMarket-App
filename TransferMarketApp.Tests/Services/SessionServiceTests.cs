using TransferMarketApp.Core.Models;
using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Tests.Services;

public class SessionServiceTests
{
    private readonly SessionService _sut = new();
    private static User DemoUser => new() { Username = "admin", DisplayName = "Admin", Token = "abc" };

    [Fact]
    public void DefaultState_IsNotAuthenticated()
    {
        Assert.Null(_sut.CurrentUser);
        Assert.False(_sut.IsAuthenticated);
        Assert.False(_sut.RememberMe);
    }

    [Fact]
    public void StartSession_SetsCurrentUser_AndAuthenticated()
    {
        var user = DemoUser;
        _sut.StartSession(user, rememberMe: false);

        Assert.True(_sut.IsAuthenticated);
        Assert.Same(user, _sut.CurrentUser);
    }

    [Fact]
    public void StartSession_WithRememberMe_StoresFlag()
    {
        _sut.StartSession(DemoUser, rememberMe: true);

        Assert.True(_sut.RememberMe);
    }

    [Fact]
    public void EndSession_ClearsUser_AndFlag()
    {
        _sut.StartSession(DemoUser, rememberMe: true);
        _sut.EndSession();

        Assert.False(_sut.IsAuthenticated);
        Assert.Null(_sut.CurrentUser);
        Assert.False(_sut.RememberMe);
    }

    [Fact]
    public void StartSession_NullUser_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.StartSession(null!, false));
    }
}
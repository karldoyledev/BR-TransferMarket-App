using TransferMarketApp.Core.Models;
using TransferMarketApp.Core.Services;
using TransferMarketApp.Core.ViewModels;

namespace TransferMarketApp.Tests.ViewModels;

public class MainViewModelTests
{
    [Fact]
    public void WelcomeMessage_WhenAuthenticated_UsesDisplayName()
    {
        var session = new SessionService();
        session.StartSession(new User { Username = "admin", DisplayName = "Admin" }, false);

        var vm = new MainViewModel(session);

        Assert.Equal("Welcome, Admin!", vm.WelcomeMessage);
    }

    [Fact]
    public void WelcomeMessage_WhenLoggedOut_FallsBackToUser()
    {
        var vm = new MainViewModel(new SessionService());

        Assert.Equal("Welcome, User!", vm.WelcomeMessage);
    }
}
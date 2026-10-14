using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Core.ViewModels;

/// <summary>
/// ViewModel for the post-login landing page.
/// Placeholder for the transfer market dashboard.
/// </summary>
public sealed class MainViewModel : BaseViewModel
{
    private readonly ISessionService _sessionService;

    public MainViewModel(ISessionService sessionService)
    {
        _sessionService = sessionService;
        Title = "Transfer Market";
    }

    public string WelcomeMessage => $"Welcome, {_sessionService.CurrentUser?.DisplayName ?? "User"}!";
}
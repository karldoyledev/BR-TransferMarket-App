using TransferMarketApp.Core.Models;

namespace TransferMarketApp.Core.Services;

/// <summary>
/// In-memory session service. Swap with SecureStorage-backed
/// persistence when "remember me" needs to survive app restarts.
/// </summary>
public sealed class SessionService : ISessionService
{
    public User? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser is not null;
    public bool RememberMe { get; private set; }

    public void StartSession(User user, bool rememberMe)
    {
        ArgumentNullException.ThrowIfNull(user);
        CurrentUser = user;
        RememberMe = rememberMe;
    }

    public void EndSession()
    {
        CurrentUser = null;
        RememberMe = false;
    }
}
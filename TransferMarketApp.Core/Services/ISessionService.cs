using TransferMarketApp.Core.Models;

namespace TransferMarketApp.Core.Services;

/// <summary>
/// Tracks the current authenticated session.
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// The currently authenticated user, or null when logged out.
    /// </summary>
    User? CurrentUser { get; }

    /// <summary>
    /// True when a user is signed in.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// True when the user opted into "remember me".
    /// </summary>
    bool RememberMe { get; }

    /// <summary>
    /// Establishes a session for the given user.
    /// </summary>
    void StartSession(User user, bool rememberMe);

    /// <summary>
    /// Ends the current session.
    /// </summary>
    void EndSession();
}
using TransferMarketApp.Core.Models;

namespace TransferMarketApp.Core.Services;

/// <summary>
/// Authenticates users. Local mock implementation for now —
/// swap with a real REST client / third-party provider later.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Validates credentials and returns an authenticated <see cref="User"/>.
    /// </summary>
    /// <exception cref="AuthenticationException">Credentials were rejected.</exception>
    Task<User> LoginAsync(string username, string password, CancellationToken ct = default);

    /// <summary>
    /// Clears the current session.
    /// </summary>
    Task LogoutAsync(CancellationToken ct = default);
}

/// <summary>
/// Thrown when authentication fails (e.g. invalid credentials).
/// </summary>
public sealed class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message) { }
}
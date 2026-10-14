namespace TransferMarketApp.Core.Models;

/// <summary>
/// An authenticated user identity.
/// </summary>
public sealed class User
{
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Email { get; init; }

    /// <summary>
    /// Identity token issued after successful authentication.
    /// Local-only for now; swap for a real JWT later.
    /// </summary>
    public string Token { get; init; } = string.Empty;
}
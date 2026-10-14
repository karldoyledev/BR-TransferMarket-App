using TransferMarketApp.Core.Models;
using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Tests.Fakes;

/// <summary>
/// Configurable fake of <see cref="IAuthService"/> for fast, deterministic unit tests.
/// </summary>
public sealed class FakeAuthService : IAuthService
{
    private readonly bool _shouldSucceed;
    private readonly string? _failureMessage;

    public int LoginCallCount { get; private set; }
    public Task<User> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        LoginCallCount++;
        if (!_shouldSucceed)
            return Task.FromException<User>(new AuthenticationException(_failureMessage ?? "Invalid username or password."));

        var user = new User
        {
            Username = username,
            DisplayName = "Test User",
            Token = Guid.NewGuid().ToString("N"),
        };
        return Task.FromResult(user);
    }

    public Task LogoutAsync(CancellationToken ct = default) => Task.CompletedTask;

    public static FakeAuthService Success() => new(true, null);
    public static FakeAuthService Failure(string? message = null) => new(false, message);
    private FakeAuthService(bool shouldSucceed, string? failureMessage)
    {
        _shouldSucceed = shouldSucceed;
        _failureMessage = failureMessage;
    }
}
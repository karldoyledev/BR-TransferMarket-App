using TransferMarketApp.Core.Models;

namespace TransferMarketApp.Core.Services;

/// <summary>
/// Mock authentication service. Accepts the demo credentials,
/// with a short simulated network delay to exercise the loading state.
///
/// Replace with real authentication logic (e.g. an API call) later —
/// the public contract (<see cref="IAuthService"/>) stays identical.
/// </summary>
public sealed class AuthService : IAuthService
{
    // Demo account so "real" credentials still work out of the box.
    public const string DemoUsername = "admin";
    public const string DemoPassword = "password";

    /// <summary>Simulated network latency for the mock.</summary>
    private static readonly TimeSpan SimulatedLatency = TimeSpan.FromMilliseconds(1200);

    public async Task<User> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        // Simulate network latency
        await Task.Delay(SimulatedLatency, ct);

        // Local mock: require a known username/password combination.
        // Swapping in a real backend only changes this method's internals.
        if (username != DemoUsername || password != DemoPassword)
        {
            throw new AuthenticationException("Invalid username or password.");
        }

        return new User
        {
            Username = username,
            DisplayName = "Admin",
            Token = Guid.NewGuid().ToString("N"), // stand-in for a JWT
        };
    }

    public Task LogoutAsync(CancellationToken ct = default)
    {
        // Nothing to persist in the mock — real impl clears tokens/state.
        return Task.CompletedTask;
    }
}
using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Tests.Fakes;

/// <summary>
/// Records navigation attempts so tests can assert the target route.
/// </summary>
public sealed class FakeNavigationService : INavigationService
{
    public List<string> NavigatedRoutes { get; } = new();
    public int NavigateCallCount => NavigatedRoutes.Count;

    public Task NavigateToRootAsync(string route, CancellationToken ct = default)
    {
        NavigatedRoutes.Add(route);
        return Task.CompletedTask;
    }
}
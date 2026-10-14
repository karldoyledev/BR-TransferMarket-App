namespace TransferMarketApp.Core.Services;

/// <summary>
/// Abstraction over UI navigation so ViewModels stay free of
/// Shell/framework dependencies and remain unit-testable.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the given absolute route, replacing the navigation stack.
    /// </summary>
    Task NavigateToRootAsync(string route, CancellationToken ct = default);
}
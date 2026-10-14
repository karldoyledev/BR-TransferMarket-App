using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Services;

/// <summary>
/// Shell-backed implementation of <see cref="INavigationService"/>.
/// </summary>
public sealed class NavigationService : INavigationService
{
    public async Task NavigateToRootAsync(string route, CancellationToken ct = default)
    {
        // Absolute navigation replaces the stack — user can't navigate "back" to login.
        await Shell.Current.GoToAsync($"//{route}");
    }
}
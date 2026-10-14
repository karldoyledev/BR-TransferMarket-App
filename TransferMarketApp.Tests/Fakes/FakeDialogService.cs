using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Tests.Fakes;

/// <summary>
/// Records alert dialogs so tests can assert titles/messages.
/// </summary>
public sealed class FakeDialogService : IDialogService
{
    public List<(string Title, string Message, string Cancel)> Alerts { get; } = new();

    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        Alerts.Add((title, message, cancel));
        return Task.CompletedTask;
    }
}
using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Services;

/// <summary>
/// Shell-backed implementation of <see cref="IDialogService"/>.
/// </summary>
public sealed class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return Shell.Current.DisplayAlertAsync(title, message, cancel);
    }
}
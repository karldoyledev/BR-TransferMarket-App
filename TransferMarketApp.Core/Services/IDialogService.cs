namespace TransferMarketApp.Core.Services;

/// <summary>
/// Abstraction over dialog/alerts so ViewModels stay free of
/// Shell/framework dependencies and remain unit-testable.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows a simple alert dialog with a single dismiss button.
    /// </summary>
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
}
using System.Windows.Input;

namespace TransferMarketApp.Core.Commands;

/// <summary>
/// Framework-free <see cref="ICommand"/> implementation for ViewModels.
/// Keeps the core library free of MAUI dependencies so it can be unit-tested.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => _execute(parameter);

    public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
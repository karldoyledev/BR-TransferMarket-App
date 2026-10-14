using System.Windows.Input;
using TransferMarketApp.Core.Commands;
using TransferMarketApp.Core.Services;

namespace TransferMarketApp.Core.ViewModels;

/// <summary>
/// ViewModel for the login screen.
/// Handles input validation, the login flow, session creation,
/// and navigation — with zero Shell/framework dependencies.
/// </summary>
public sealed class LoginViewModel : BaseViewModel
{
    public const int MinUsernameLength = 3;
    public const int MinPasswordLength = 6;

    private readonly IAuthService _authService;
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public LoginViewModel(
        IAuthService authService,
        ISessionService sessionService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _authService = authService;
        _sessionService = sessionService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        Title = "Sign In";

        LoginCommand = new RelayCommand(
            async _ => await LoginAsync(),
            canExecute: () => !IsBusy);
        ForgotPasswordCommand = new RelayCommand(async _ => await OnForgotPasswordAsync());
        SignUpCommand = new RelayCommand(async _ => await OnSignUpAsync());
    }

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value))
            {
                ClearError();
                OnPropertyChanged(nameof(CanSubmit));
            }
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                ClearError();
                OnPropertyChanged(nameof(CanSubmit));
            }
        }
    }

    private bool _rememberMe;
    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Whether the fields are non-empty (minimal pre-submit gate).
    /// Full validation still happens in <see cref="LoginAsync"/>.
    /// </summary>
    public bool CanSubmit => !string.IsNullOrWhiteSpace(Username) &&
                             !string.IsNullOrWhiteSpace(Password);

    public ICommand LoginCommand { get; }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand SignUpCommand { get; }

    private void ClearError()
    {
        if (ErrorMessage is not null)
            ErrorMessage = null;
    }

    /// <summary>
    /// Core login flow: validates input, invokes auth, starts a session,
    /// navigates to the main page. Exposed publicly so unit tests can await it directly.
    /// </summary>
    public async Task LoginAsync()
    {
        // Field validation
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Please enter your username.";
            return;
        }

        if (Username.Length < MinUsernameLength)
        {
            ErrorMessage = $"Username must be at least {MinUsernameLength} characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter your password.";
            return;
        }

        if (Password.Length < MinPasswordLength)
        {
            ErrorMessage = $"Password must be at least {MinPasswordLength} characters.";
            return;
        }

        IsBusy = true;
        ((RelayCommand)LoginCommand).ChangeCanExecute();
        ErrorMessage = null;

        try
        {
            var user = await _authService.LoginAsync(Username, Password);

            _sessionService.StartSession(user, RememberMe);
            await _navigationService.NavigateToRootAsync(Routes.MainPage);
        }
        catch (AuthenticationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsBusy = false;
            ((RelayCommand)LoginCommand).ChangeCanExecute();
        }
    }

    private async Task OnForgotPasswordAsync()
    {
        await _dialogService.ShowAlertAsync(
            "Forgot Password",
            "Password recovery is not available yet. Use admin / password for the demo.");
    }

    private async Task OnSignUpAsync()
    {
        await _dialogService.ShowAlertAsync(
            "Sign Up",
            "Account creation is not available yet. Use admin / password for the demo.");
    }
}
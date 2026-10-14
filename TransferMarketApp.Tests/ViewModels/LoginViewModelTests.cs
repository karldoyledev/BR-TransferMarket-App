using TransferMarketApp.Core;
using TransferMarketApp.Core.Services;
using TransferMarketApp.Core.ViewModels;
using TransferMarketApp.Tests.Fakes;

namespace TransferMarketApp.Tests.ViewModels;

public class LoginViewModelTests
{
    private readonly FakeAuthService _auth;
    private readonly SessionService _session;
    private readonly FakeNavigationService _navigation;
    private readonly FakeDialogService _dialog;
    private readonly LoginViewModel _vm;

    public LoginViewModelTests()
    {
        _auth = FakeAuthService.Success();
        _session = new SessionService();
        _navigation = new FakeNavigationService();
        _dialog = new FakeDialogService();

        _vm = new LoginViewModel(_auth, _session, _navigation, _dialog)
        {
            Username = "admin",
            Password = "password",
        };
    }

    // ----- CanSubmit gate -----

    [Fact]
    public void CanSubmit_EmptyFields_IsFalse()
    {
        var vm = new LoginViewModel(_auth, _session, _navigation, _dialog);

        Assert.False(vm.CanSubmit);
    }

    [Fact]
    public void CanSubmit_FilledFields_IsTrue()
    {
        Assert.True(_vm.CanSubmit);
    }

    // ----- Validation -----

    [Theory]
    [InlineData("", "password", "Please enter your username.")]
    [InlineData("ab", "password", "Username must be at least 3 characters.")]
    [InlineData("admin", "", "Please enter your password.")]
    [InlineData("admin", "12345", "Password must be at least 6 characters.")]
    public async Task LoginAsync_InvalidInput_SetsErrorMessage(
        string username, string password, string expectedError)
    {
        _vm.Username = username;
        _vm.Password = password;

        await _vm.LoginAsync();

        Assert.Equal(expectedError, _vm.ErrorMessage);
        Assert.True(_vm.HasError);
        Assert.Equal(0, _auth.LoginCallCount);
        Assert.Empty(_navigation.NavigatedRoutes);
    }

    // ----- Happy path -----

    [Fact]
    public async Task LoginAsync_ValidCredentials_StartsSession()
    {
        await _vm.LoginAsync();

        Assert.True(_session.IsAuthenticated);
        Assert.Equal("admin", _session.CurrentUser?.Username);
        Assert.Null(_vm.ErrorMessage);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_NavigatesToMainPage()
    {
        await _vm.LoginAsync();

        Assert.Equal([Routes.MainPage], _navigation.NavigatedRoutes);
    }

    [Fact]
    public async Task LoginAsync_RememberMeChecked_FlowsToSession()
    {
        _vm.RememberMe = true;
        await _vm.LoginAsync();

        Assert.True(_session.RememberMe);
    }

    // ----- Failure path -----

    [Fact]
    public async Task LoginAsync_InvalidCredentials_SetsError_NoSession_NoNavigation()
    {
        var auth = FakeAuthService.Failure("Invalid username or password.");
        var vm = new LoginViewModel(auth, _session, _navigation, _dialog)
        {
            Username = "admin",
            Password = "wrongpassword",
        };

        await vm.LoginAsync();

        Assert.Equal("Invalid username or password.", vm.ErrorMessage);
        Assert.False(_session.IsAuthenticated);
        Assert.Empty(_navigation.NavigatedRoutes);
    }

    // ----- Busy state -----

    [Fact]
    public async Task LoginAsync_SetsBusy_DuringLogin_AndClearsAfter()
    {
        // Use the real AuthService: its simulated network latency makes the
        // in-flight busy state observable (the fakes complete synchronously).
        var auth = new AuthService();
        var vm = new LoginViewModel(auth, _session, _navigation, _dialog)
        {
            Username = AuthService.DemoUsername,
            Password = AuthService.DemoPassword,
        };

        var task = vm.LoginAsync();
        Assert.True(vm.IsBusy);
        await task;

        Assert.False(vm.IsBusy);
    }

    // ----- Error clearing on input -----

    [Fact]
    public async Task ChangingUsername_ClearsError()
    {
        _vm.Username = "";
        await _vm.LoginAsync();          // sets "Please enter your username."
        Assert.True(_vm.HasError);

        _vm.Username = "admin";          // property change clears the error
        Assert.False(_vm.HasError);
        Assert.Null(_vm.ErrorMessage);
    }

    // ----- Dialogs -----

    [Fact]
    public void ForgotPasswordCommand_ShowsAlert()
    {
        _vm.ForgotPasswordCommand.Execute(null);

        Assert.Single(_dialog.Alerts);
        Assert.Equal("Forgot Password", _dialog.Alerts[0].Title);
        Assert.Equal("Password recovery is not available yet. Use admin / password for the demo.", _dialog.Alerts[0].Message);
    }

    [Fact]
    public void SignUpCommand_ShowsAlert()
    {
        _vm.SignUpCommand.Execute(null);

        Assert.Single(_dialog.Alerts);
        Assert.Equal("Sign Up", _dialog.Alerts[0].Title);
        Assert.Equal("Account creation is not available yet. Use admin / password for the demo.", _dialog.Alerts[0].Message);
    }

    [Fact]
    public void LoginCommand_CanExecute_ReflectsBusyState()
    {
        Assert.True(_vm.LoginCommand.CanExecute(null));
        _vm.IsBusy = true;
        Assert.False(_vm.LoginCommand.CanExecute(null));
        _vm.IsBusy = false;
        Assert.True(_vm.LoginCommand.CanExecute(null));
    }
}
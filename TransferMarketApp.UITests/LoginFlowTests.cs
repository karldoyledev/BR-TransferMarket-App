using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using TransferMarketApp.UITests.Appium;

namespace TransferMarketApp.UITests;

/// <summary>
/// End-to-end login flow tests driven through Appium (UiAutomator2) on Android.
///
/// Requirements before running (see README.md in this project):
///  - an Android emulator/device visible to `adb devices`
///  - an Appium 2 server with the uiautomator2 (v4) driver installed
///  - a Debug Android build of the TransferMarketApp
///
/// Element lookup uses the AutomationId declared in LoginPage.xaml/MainPage.xaml.
/// On Android, .NET MAUI maps AutomationId to the view's *resource-id* (not
/// content-desc), so locators target `resource-id`, e.g.
/// `com.companyname.transfermarketapp:id/UsernameEntry`. Each test class shares
/// one fresh Appium session via <see cref="AppiumFixture"/>.
/// </summary>
[Collection("Appium")]
public sealed class LoginFlowTests : IClassFixture<AppiumFixture>
{
    private readonly AndroidDriver _driver;

    public LoginFlowTests(AppiumFixture fixture) => _driver = fixture.Driver;

    [Fact]
    [Trait("Category", "UI")]
    public void DemoCredentials_Login_ReachesMainPage()
    {
        var username = _driver.FindElement(ResId("UsernameEntry"));
        username.Click();
        username.SendKeys("admin");

        var password = _driver.FindElement(ResId("PasswordEntry"));
        password.Click();
        password.SendKeys("password");

        TryHideKeyboard();

        _driver.FindElement(ResId("LoginButton")).Click();

        // Mock auth has a ~1.2s simulated delay, then Shell navigates to MainPage,
        // whose Label binds to MainViewModel.WelcomeMessage ("Welcome, Admin!").
        var welcome = _driver.FindElement(By.XPath("//*[@text='Welcome, Admin!']"));
        Assert.True(welcome.Displayed);
    }

    private void TryHideKeyboard()
    {
        try
        {
            _driver.HideKeyboard();
        }
        catch (WebDriverException)
        {
            // Keyboard wasn't up / already hidden — not a failure.
        }
    }

    /// <summary>
    /// Locates a control by the AutomationId MAUI assigned in XAML. On Android
    /// that surfaces as the view's fully-qualified resource-id.
    /// </summary>
    private static By ResId(string automationId)
        => By.XPath($"//*[@resource-id='{AppiumSession.AppPackage}:id/{automationId}']");
}

/// <summary>
/// Runs in its own Appium session (own fixture) so a failed login leaves the
/// previous tests' navigation state behind.
/// </summary>
[Collection("Appium")]
public sealed class InvalidCredentialsTests : IClassFixture<AppiumFixture>
{
    private readonly AndroidDriver _driver;

    public InvalidCredentialsTests(AppiumFixture fixture) => _driver = fixture.Driver;

    [Fact]
    [Trait("Category", "UI")]
    public void WrongPassword_ShowsError_AndStaysOnLogin()
    {
        var username = _driver.FindElement(By.XPath("//*[@resource-id='com.companyname.transfermarketapp:id/UsernameEntry']"));
        username.Click();
        username.SendKeys("admin");

        var password = _driver.FindElement(By.XPath("//*[@resource-id='com.companyname.transfermarketapp:id/PasswordEntry']"));
        password.Click();
        password.SendKeys("wrongpassword");

        TryHideKeyboard();

        _driver.FindElement(By.XPath("//*[@resource-id='com.companyname.transfermarketapp:id/LoginButton']")).Click();

        // AuthService rejects non-demo credentials with this exact message,
        // which binds to LoginViewModel.ErrorMessage / the ErrorLabel.
        var error = _driver.FindElement(By.XPath("//*[@text='Invalid username or password.']"));
        Assert.True(error.Displayed);
    }

    private void TryHideKeyboard()
    {
        try
        {
            _driver.HideKeyboard();
        }
        catch (WebDriverException)
        {
            // Keyboard wasn't up / already hidden — not a failure.
        }
    }
}

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace TransferMarketApp.UITests.Appium;

/// <summary>
/// Marks the UI test suite as a single collection and disables xUnit's parallel
/// execution: Appium sessions drive a physical emulator/device, so tests must
/// run one at a time.
/// </summary>
[CollectionDefinition("Appium", DisableParallelization = true)]
public sealed class AppiumCollection;

/// <summary>
/// Owns one Appium driver (one freshly installed app + session) for the lifetime
/// of a test class and quits it afterwards. xUnit creates a new fixture per
/// class, so each test class gets its own clean app launch.
/// </summary>
public sealed class AppiumFixture : IDisposable
{
    public AndroidDriver Driver { get; }

    public AppiumFixture() => Driver = AppiumSession.CreateAndroidDriver();

    public void Dispose() => Driver?.Quit();
}

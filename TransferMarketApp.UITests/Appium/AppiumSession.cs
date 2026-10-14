using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace TransferMarketApp.UITests.Appium;

/// <summary>
/// Creates the Appium Android (UiAutomator2) session for the MAUI app.
///
/// All settings can be overridden via environment variables so the same tests
/// run unchanged on dev machines and CI:
///   TM_APPIUM_URL  - Appium server base URL (default http://127.0.0.1:4723)
///   TM_APK_PATH    - absolute path to the built .apk
///                    (default: auto-located Debug build under TransferMarketApp/bin)
///   TM_DEVICE_NAME - display name of the emulator/device (default "Android Emulator")
/// </summary>
public static class AppiumSession
{
    public const string DefaultAppiumUrl = "http://127.0.0.1:4723";

    /// <summary>Matches the ApplicationId in TransferMarketApp.csproj.</summary>
    public const string AppPackage = "com.companyname.transfermarketapp";

    /// <summary>
    /// Path to the freshly built Debug APK. UI tests must ship with a new build
    /// so they (a) exercise current code and (b) launch into a fresh login screen.
    /// </summary>
    public static string LocateApkPath()
    {
        var fromEnv = Environment.GetEnvironmentVariable("TM_APK_PATH");
        if (!string.IsNullOrWhiteSpace(fromEnv))
            return fromEnv;

        // Walk up from the test-host directory to the repo root, then
        // resolve the standard MAUI Android output path. NOTE: use the
        // Release build — Debug builds are "Fast Deployment" APKs that
        // don't bundle the .NET assemblies and abort when installed
        // standalone (which is exactly how Appium installs them).
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null &&
               !File.Exists(Path.Combine(dir.FullName, "BR-TransferMarket-App.slnx")))
        {
            dir = dir.Parent;
        }

        if (dir is null)
            throw new FileNotFoundException(
                "Could not locate the repository root. Set TM_APK_PATH to the built .apk explicitly.");

        var apk = Path.Combine(
            dir.FullName,
            "TransferMarketApp", "bin", "Release", "net10.0-android",
            $"{AppPackage}-Signed.apk");

        if (!File.Exists(apk))
            throw new FileNotFoundException(
                $"APK not found at '{apk}'. Build it first: " +
                "dotnet build TransferMarketApp/TransferMarketApp.csproj -f net10.0-android -c Release (or set TM_APK_PATH).");

        return apk;
    }

    public static AndroidDriver CreateAndroidDriver()
    {
        // v5 AppiumOptions: automationName/deviceName/app are dedicated properties;
        // "platformName" has no property so it goes through the additional option bag.
        var options = new AppiumOptions
        {
            AutomationName = "UiAutomator2",
            DeviceName = GetSetting("TM_DEVICE_NAME", "Android Emulator"),
            App = LocateApkPath(),
        };
        options.AddAdditionalAppiumOption(MobileCapabilityType.PlatformName, "Android");

        // Cold starts on emulators can exceed UiAutomator2's defaults (20s) — give
        // app launch/adb commands generous timeouts on first session.
        options.AddAdditionalAppiumOption("appLaunchTimeout", 90000);
        options.AddAdditionalAppiumOption("appWaitDuration", 60000);
        options.AddAdditionalAppiumOption("adbExecTimeout", 90000);

        var serverUrl = new Uri(GetSetting("TM_APPIUM_URL", DefaultAppiumUrl));

        var driver = new AndroidDriver(serverUrl, options, TimeSpan.FromSeconds(120));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        return driver;
    }

    private static string GetSetting(string name, string defaultValue)
        => Environment.GetEnvironmentVariable(name) ?? defaultValue;
}

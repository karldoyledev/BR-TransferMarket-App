# TransferMarketApp.UITests

Appium UI tests for the BR TransferMarket MAUI app, driven end-to-end through a real
Android emulator/device.

## Current state

- `Appium.WebDriver 5.1.0` (.NET Appium client) + xUnit — referenced, no tests previously.
- `Appium/AppiumSession.cs` — capability builder + APK auto-discovery (all overridable via env vars).
- `Appium/AppiumFixture.cs` — one Appium session per test class, sequential (non-parallel) collection.
- `LoginFlowTests.cs` — demo-credentials login → MainPage smoke test, plus an invalid-credentials failure test.

- Element lookup targets the `AutomationId`s declared in the app's XAML. On Android,
  .NET MAUI maps `AutomationId` to the view's **resource-id** (e.g.
  `com.companyname.transfermarketapp:id/UsernameEntry`) — *not* `content-desc`.
  Locators therefore use XPath on `@resource-id`; `MobileBy.AccessibilityId(...)`
  will **not** match.

## Compatibility / version matrix

The pinned client **5.1.0** targets Appium **2.x**:

| Appium server | uiautomator2 driver | Appium.WebDriver (client) |
|---|---|---|
| 2.x | 4.x (e.g. `uiautomator2@4`) | 5.x (this project) — ✅ |
| 3.x | 5.x / 6.x (Appium 3 only) | 8.x / 9.x |

> ⚠️ `appium driver install uiautomator2` (no version) installs the latest driver, which now
> requires Appium 3. For this project pin the 4.x driver. If you prefer Appium 3, bump the
> Appium.WebDriver package.

## Android setup (one-time)

1. **JDK 17+** — required to build the Android APK.
   ```powershell
   winget install EclipseAdoptium.Temurin.17.JDK
   ```
   Set `JAVA_HOME` to the install dir and add `%JAVA_HOME%\bin` to `PATH`.

2. **Android SDK** — Android Studio (recommended), or command-line tools:
   ```powershell
   winget install Google.AndroidStudio
   # OR just the cmdline tools + platform tools:
   winget install Google.PlatformTools
   ```
   Set `ANDROID_HOME` (e.g. `C:\Users\<you>\AppData\Local\Android\Sdk`) and add
   `%ANDROID_HOME%\platform-tools` to `PATH`.

3. **Accept licenses + install emulator bits** (with cmdline-tools):
   ```powershell
   sdkmanager --licenses
   sdkmanager "platform-tools" "platforms;android-35" "emulator" "system-images;android-35;google_apis;x86_64"
   avdmanager create avd -n tm_pixel -k "system-images;android-35;google_apis;x86_64" -d pixel_6
   ```

4. **Start the emulator** and confirm connectivity:
   ```powershell
   $ANDROID_HOME\emulator\emulator.exe -avd tm_pixel
   adb devices          # device must show as "device" (emulator-5554 ... device)
   ```

## Appium setup (one-time)

```powershell
npm install -g appium@2
appium driver install uiautomator2@4
appium driver doctor uiautomator2   # validates SDK/JAVA_HOME/adb prerequisites
```

## Build the APK

> ⚠️ **Use a Release build.** MAUI Android Debug APKs are "Fast Deployment" builds —
> they don't bundle the .NET assemblies and **abort on launch when installed
> standalone** (which is how Appium installs them). `dotnet test` below defaults
> to the Release APK.

```powershell
dotnet build TransferMarketApp/TransferMarketApp.csproj -f net10.0-android -c Release
# → TransferMarketApp/bin/Release/net10.0-android/com.companyname.transfermarketapp-Signed.apk
```

## Run the tests

Prerequisites, all in one checklist:

```powershell
# 1. Emulator running (or `adb devices` shows a physical device)
$ANDROID_HOME\emulator\emulator.exe -avd tm_pixel

# 2. Appium server running in a terminal
appium --port 4723
```

Then run in another terminal (APK path auto-detects, or set `TM_APK_PATH`):

```powershell
dotnet test TransferMarketApp.UITests/TransferMarketApp.UITests.csproj
```

Verified green stack: Appium 2.19 + uiautomator2 driver 4.2.9 + Android API 36
emulator + Release APK — 2/2 tests passing in ~30s (one fresh app install per test class).

### Configuration (all optional)

| Env var | Default | Purpose |
|---|---|---|
| `TM_APPIUM_URL` | `http://127.0.0.1:4723` | Appium server endpoint |
| `TM_APK_PATH` | auto-detected Debug APK | Path to the `.apk` under test |
| `TM_DEVICE_NAME` | `Android Emulator` | adb device label |

## Troubleshooting

- **Session creation fails / timeouts** → run `appium driver doctor uiautomator2`, confirm
  `adb devices`, and that the emulator finished booting (`adb shell getprop sys.boot_completed`).
- **Element not found by id** → the APK may be stale (rebuild Release), or the `AutomationId`
  mapping changed. Inspect the live UI hierarchy with [Appium Inspector][1] or
  `adb shell uiautomator dump` and look for `resource-id="...:id/UsernameEntry"`.
- **App installs but never starts ("activity never started")** → you almost certainly installed
  a Fast-Deployment **Debug** APK. Rebuild with `-c Release` (see above).
- **Wrong `content-desc`** → verify the app package (`com.companyname.transfermarketapp`) and, if
  you switch to explicit `appPackage`/`appActivity` capabilities, find the launcher activity with:
  `aapt dump badging <apk> | findstr launchable-activity` (MAUI activities are `crc64…MainActivity`).

## Other platforms

- **Windows (WinUI 3)** — possible via `appium driver install windows` (WinAppDriver), but support
  for WinUI 3 binaries is flaky; treat as experimental. `app` = path to
  `TransferMarketApp.exe`.
- **iOS** — requires a Mac with Xcode (`appium driver install xcuitest`); not runnable on Windows.

[1]: https://github.com/appium/appium-inspector

using Microsoft.Playwright;
using Serilog;
using SwagLabs.Pages;
using System.Diagnostics;
using Serilog.Core;

namespace SwagLabs
{
    public class BaseTest
    {
        protected static readonly Dictionary<string, string> Users = new()
        {
            ["StandardUser"] = "standard_user",
            ["LockedOutUser"] = "locked_out_user",
            ["ProblemUser"] = "problem_user",
            ["PerformanceGlitchUser"] = "performance_glitch_user",
            ["ErrorUser"] = "error_user",
            ["VisualUser"] = "visual_user",
        };

        protected IPlaywright? PlaywrightInstance;
        protected IBrowser? Browser;
        protected IBrowserContext? BrowserContext;
        protected IPage? PageInstance;
        protected string UserLogin;
        protected ILogger Logger;

        protected async Task<LoginPage> NavigateToLoginPageAsync(string url = "https://www.saucedemo.com/")
        {
            await PageInstance.GotoAsync(url);
            return await LoginPage.InitAsync(PageInstance, Logger);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetupAsync()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/log-.txt", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            Logger = Log.Logger;
            Logger.Information("=== Test Suite Started ===");

            PlaywrightInstance = await Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });

            UserLogin = Users["StandardUser"];
            //Pozostałe loginy na potrzeby testów negatywnych
            //UserLogin = Users["LockedOutUser"];
            //UserLogin = Users["ProblemUser"];
            //UserLogin = Users["PerformanceGlitchUser"];
            //UserLogin = Users["ErrorUser"];
            //UserLogin = Users["VisualUser"];

            Logger.Information($"OneTimeSetup completed - Browser initialized with user: {UserLogin}");
        }

        [SetUp]
        public async Task Setup()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            Logger?.Information($"=== Starting test: {testName} ===");

            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }

            Logger?.Debug($"Test context and page initialized for {testName}");
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            if (testResult == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Logger?.Error("Test {TestName} FAILED", testName);

                // Opcjonalnie: zrób screenshot
                if (PageInstance != null)
                {
                    var screenshotPath = $"logs/Screenshots/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
                    await PageInstance.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                    Logger?.Information("Screenshot saved to {ScreenshotPath}", screenshotPath);
                }
            }
            else
            {
                Logger?.Information("Test {TestName} finished with status: {TestResult}", testName, testResult);
            }

            if (BrowserContext != null)
            {
                await BrowserContext.CloseAsync();
                BrowserContext = null;
                PageInstance = null;
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            Logger?.Information("=== Test Suite Completed ===");

            if (Browser != null)
            {
                await Browser.CloseAsync();
                Browser = null;
            }

            PlaywrightInstance?.Dispose();
            PlaywrightInstance = null;

            Log.CloseAndFlush();
        }
    }
}

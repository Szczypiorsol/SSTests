using Microsoft.Playwright;
using System.Diagnostics;

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

        [OneTimeSetUp]
        public async Task OneTimeSetupAsync()
        {
            PlaywrightInstance = await Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });

            UserLogin = Users["StandardUser"];
            //UserLogin = Users["LockedOutUser"];
            //UserLogin = Users["ProblemUser"];
            //UserLogin = Users["PerformanceGlitchUser"];
            //UserLogin = Users["ErrorUser"];
            //UserLogin = Users["VisualUser"];
        }

        [SetUp]
        public async Task Setup()
        {
            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }
        }

        [TearDown]
        public async Task TearDownAsync()
        {
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
            if (Browser != null)
            {
                await Browser.CloseAsync();
                Browser = null;
            }

            PlaywrightInstance?.Dispose();
            PlaywrightInstance = null;
        }
    }
}

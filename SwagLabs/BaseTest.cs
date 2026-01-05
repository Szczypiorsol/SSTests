using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SwagLabs
{
    public class BaseTest : PageTest
    {
        private IPage? _page;
        private IPlaywright? _playwright;
        private IBrowser? _browser;

        protected IPage PageInstance => _page ?? Page;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            if (Debugger.IsAttached)
            {
                _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
                _page = await _browser.NewPageAsync();
            }
        }

        [SetUp]
        public async Task Setup()
        {
            if (Debugger.IsAttached)
            {
                await _page.PauseAsync();
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
        }

    }
}

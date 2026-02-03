using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage _page;
        protected bool _isInitialized = false;
        protected readonly string _pageName;
        protected readonly int _defaultTimeout;
        protected readonly ILogger _logger;

        private readonly Button _burgerButton;
        private readonly Button _logoutButton;

        public Button BurgerButton => _burgerButton;
        public Button LogoutButton => _logoutButton;

        public BasePage(IPage page, string pageName, ILogger logger, int defaultTimeout = 1000)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _pageName = pageName;
            _defaultTimeout = defaultTimeout;
            _page.SetDefaultTimeout(_defaultTimeout);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _burgerButton = new Button(_page, GetBy.CssSelector, "div.bm-burger-button");
            _logoutButton = new Button(_page, GetBy.TestId, "logout-sidebar-link");
        }

        public abstract Task InitAsync();

        protected void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"[{_pageName}] is not initialized. Call InitAsync() before using the page.");
            }
        }

        public async Task RefreshAsync()
        {
            _logger?.Information($"Refreshing [{_pageName}]...");
            EnsureInitialized();
            await _page.ReloadAsync();
            _logger?.Information($"[{_pageName}] refreshed successfully.");
        }

        public async Task<LoginPage> LogoutAsync()
        {
            if(_pageName == "LoginPage")
            {
                throw new InvalidOperationException("You are already on the Login Page.");
            }

            _logger.Information($"Logging out from [{_pageName}]...");
            await BurgerButton.ClickAsync();
            await LogoutButton.ClickAsync();
            _logger.Information("Logout successful. Navigating to Login Page...");
            return await LoginPage.InitAsync(_page, _logger);
        }
    }
}

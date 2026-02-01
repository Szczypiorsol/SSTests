using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage _page;
        protected bool _isInitialized = false;
        protected readonly string _pageName;
        protected readonly int _defaultTimeout;
        protected readonly Button _burgerButton;
        protected readonly Button _logoutButton;

        public BasePage(IPage page, string pageName, int defaultTimeout = 1000)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _pageName = pageName;
            _defaultTimeout = defaultTimeout;
            _page.SetDefaultTimeout(_defaultTimeout);

            _burgerButton = new Button(_page, GetBy.CssSelector, "div.bm-burger-button", $"{_pageName}_[BurgerButton]");
            _logoutButton = new Button(_page, GetBy.TestId, "logout-sidebar-link", $"{_pageName}_[LogoutButton]");
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
            EnsureInitialized();
            await _page.ReloadAsync();
        }

        public async Task<LoginPage> LogoutAsync()
        {
            if(_pageName == "LoginPage")
            {
                throw new InvalidOperationException("You are already on the Login Page.");
            }

            await _burgerButton.CheckIsVisibleAsync();
            await _burgerButton.ClickAsync();
            await _logoutButton.CheckIsVisibleAsync();
            await _logoutButton.ClickAsync();
            return await LoginPage.InitAsync(_page);
        }
    }
}

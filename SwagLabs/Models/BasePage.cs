using Microsoft.Playwright;

namespace SwagLabs.Models
{
    public abstract class BasePage
    {
        protected readonly IPage _page;
        protected bool _isInitialized = false;
        protected readonly string _pageName;
        protected readonly int _defaultTimeout;

        public BasePage(IPage page, string pageName, int defaultTimeout = 300)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _pageName = pageName;
            _defaultTimeout = defaultTimeout;
            _page.SetDefaultTimeout(_defaultTimeout);
        }

        public abstract Task InitAsync();

        protected void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"{_pageName} is not initialized. Call InitAsync() before using the page.");
            }
        }
    }
}

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwagLabs.Models
{
    public abstract class BasePage(IPage page)
    {
        protected readonly IPage _page = page;
        protected bool _isInitialized = false;

        public abstract Task InitAsync();

        protected void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("Page is not initialized. Call InitAsync() before using the page.");
            }
        }
    }
}

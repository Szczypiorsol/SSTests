using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CheckoutCompletePage : BasePage
    {
        private readonly TextBox _thankYouMessageTextBox;
        private readonly Button _backHomeButton;

        public CheckoutCompletePage(IPage page, int defaultTimeout = 300) : base(page, "[CheckoutCompletePage]", defaultTimeout)
        {
            _thankYouMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h2.complete-header", $"{_pageName}_[ThankYouMessageTextBox]");
            _backHomeButton = new Button(_page, GetBy.Role, "Back Home", $"{_pageName}_[BackHomeButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _thankYouMessageTextBox.CheckIsVisibleAsync();
                await _backHomeButton.CheckIsVisibleAsync();
            }
            catch (Exception ex) when (ex is AssertionException || ex is PlaywrightException)
            {
                throw new AssertionException($"{_pageName} did not load correctly.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"{_pageName} did not load within {_defaultTimeout} miliseconds.", ex);
            }

            _isInitialized = true;
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page)
        {
            CheckoutCompletePage checkoutCompletePage = new(page);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task AssertThankYouMessageAsync(string expectedText)
        {
            EnsureInitialized();
            await _thankYouMessageTextBox.AssertTextAsync(expectedText);
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            EnsureInitialized();
            try
            {
                await _backHomeButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click back home button within {_defaultTimeout} miliseconds.", ex);
            }
            return await ProductsPage.InitAsync(_page);
        }
    }
}
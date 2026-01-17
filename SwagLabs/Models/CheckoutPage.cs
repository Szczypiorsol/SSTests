using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CheckoutPage : BasePage
    {
        private readonly TextBox _firstNameTextBox;
        private readonly TextBox _lastNameTextBox;
        private readonly TextBox _postalCodeTextBox;
        private readonly Button _cancelButton;
        private readonly Button _continueButton;

        public CheckoutPage(IPage page, int defaultTimeout = 300) : base(page, "[CheckoutPage]", defaultTimeout)
        {
            _firstNameTextBox = new TextBox(_page, GetBy.Placeholder, "First Name", $"{_pageName}_[FirstNameTextBox]");
            _lastNameTextBox = new TextBox(_page, GetBy.Placeholder, "Last Name", $"{_pageName}_[LastNameTextBox]");
            _postalCodeTextBox = new TextBox(_page, GetBy.Placeholder, "Zip/Postal Code", $"{_pageName}_[PostalCodeTextBox]");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel", $"{_pageName}_[CancelButton]");
            _continueButton = new Button(_page, GetBy.Role, "Continue", $"{_pageName}_[ContinueButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _firstNameTextBox.CheckIsVisibleAsync();
                await _lastNameTextBox.CheckIsVisibleAsync();
                await _postalCodeTextBox.CheckIsVisibleAsync();
                await _cancelButton.CheckIsVisibleAsync();
                await _continueButton.CheckIsVisibleAsync();
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

        public static async Task<CheckoutPage> InitAsync(IPage page)
        {
            CheckoutPage checkoutPage = new(page);
            await checkoutPage.InitAsync();
            return checkoutPage;
        }

        public async Task<CheckoutPage> FillCheckoutInformationAsync(string firstName, string lastName, string postalCode)
        {
            EnsureInitialized();
            await _firstNameTextBox.EnterTextAsync(firstName);
            await _lastNameTextBox.EnterTextAsync(lastName);
            await _postalCodeTextBox.EnterTextAsync(postalCode);
            return await InitAsync(_page);
        }

        public async Task<CheckoutOverviewPage> ClickContinueAsync()
        {
            EnsureInitialized();
            try
            {
                await _continueButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click continue button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutOverviewPage.InitAsync(_page);
        }

        public async Task<CartPage> ClickCancelAsync()
        {
            EnsureInitialized();
            try
            {
                await _cancelButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click cancel button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CartPage.InitAsync(_page);
        }
    }
}
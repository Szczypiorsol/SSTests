using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CheckoutOverviewPage : BasePage
    {
        private readonly ListControl _overviewItemList;
        private readonly TextBox _paymentInformationTextBox;
        private readonly TextBox _shippingInformationTextBox;
        private readonly TextBox _summarySubtotalTextBox;
        private readonly TextBox _summaryTaxTextBox;
        private readonly TextBox _summaryTotalTextBox;
        private readonly Button _cancelButton;
        private readonly Button _finishButton;

        public CheckoutOverviewPage(IPage page, int defaultTimeout = 300) : base(page, "[CheckoutOverviewPage]", defaultTimeout)
        {
            _overviewItemList = new ListControl(
                _page, 
                GetBy.CssSelector, 
                "div.cart_list",
                $"{_pageName}_[ProductsList]", 
                GetBy.CssSelector, 
                "div.cart_item",
                $"{_pageName}_[Product]"
                );
            _paymentInformationTextBox = new TextBox(_page, GetBy.TestId, "payment-info-value", $"{_pageName}_[PaymentInformationTextBox]");
            _shippingInformationTextBox = new TextBox(
                _page, 
                GetBy.TestId, 
                "shipping-info-value",
                $"{_pageName}_[ShippingInformationTextBox]"
                );
            _summarySubtotalTextBox = new TextBox(
                _page, 
                GetBy.CssSelector, 
                "div.summary_subtotal_label",
                $"{_pageName}_[SummarySubtotalTextBox]"
                );
            _summaryTaxTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_tax_label", $"{_pageName}_[SummaryTaxTextBox]");
            _summaryTotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_total_label", $"{_pageName}_[SummaryTotalTextBox]");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel", $"{_pageName}_[CancelButton]");
            _finishButton = new Button(_page, GetBy.Role, "Finish", $"{_pageName}_[FinishButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _overviewItemList.CheckIsVisibleAsync();
                await _paymentInformationTextBox.CheckIsVisibleAsync();
                await _shippingInformationTextBox.CheckIsVisibleAsync();
                await _summarySubtotalTextBox.CheckIsVisibleAsync();
                await _summaryTaxTextBox.CheckIsVisibleAsync();
                await _summaryTotalTextBox.CheckIsVisibleAsync();
                await _cancelButton.CheckIsVisibleAsync();
                await _finishButton.CheckIsVisibleAsync();
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

        public static async Task<CheckoutOverviewPage> InitAsync(IPage page)
        {
            CheckoutOverviewPage checkoutOverviewPage = new(page);
            await checkoutOverviewPage.InitAsync();
            return checkoutOverviewPage;
        }

        public async Task AssertOverviewItemsCountAsync(int expectedCount)
        {
            EnsureInitialized();
            await _overviewItemList.AssertItemCountAsync(expectedCount);
        }

        public async Task AssertOverviewItemAtAsync(int index, string expectedName, string expectedPrice)
        {
            EnsureInitialized();
            await _overviewItemList.AssertItemElementTextAsync(expectedName, index, GetBy.CssSelector, "div.inventory_item_name", "Name");
            await _overviewItemList.AssertItemElementTextAsync(expectedPrice, index, GetBy.CssSelector, "div.inventory_item_price", "Price");
        }

        public async Task AssertPaymentInformationAsync(string expectedPaymentInformation)
        {
            EnsureInitialized();
            await _paymentInformationTextBox.AssertTextAsync(expectedPaymentInformation);
        }

        public async Task AssertShippingInformationAsync(string expectedShippingInformation)
        {
            EnsureInitialized();
            await _shippingInformationTextBox.AssertTextAsync(expectedShippingInformation);
        }

        public async Task AssertSummarySubtotalAsync(string expectedSummarySubtotal)
        {
            EnsureInitialized();
            await _summarySubtotalTextBox.AssertTextAsync(expectedSummarySubtotal);
        }

        public async Task AssertSummaryTaxAsync(string expectedSummaryTax)
        {
            EnsureInitialized();
            await _summaryTaxTextBox.AssertTextAsync(expectedSummaryTax);
        }

        public async Task AssertSummaryTotalAsync(string expectedSummaryTotal)
        {
            EnsureInitialized();
            await _summaryTotalTextBox.AssertTextAsync(expectedSummaryTotal);
        }

        public async Task<CheckoutPage> ClickCancelAsync()
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
            return await CheckoutPage.InitAsync(_page);
        }

        public async Task<CheckoutCompletePage> ClickFinishAsync()
        {
            EnsureInitialized();
            try
            {
                await _finishButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click finish button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutCompletePage.InitAsync(_page);
        }
    }
}
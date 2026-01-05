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

        public CheckoutOverviewPage(IPage page) : base(page)
        {
            _overviewItemList = new ListControl(_page, GetBy.CssSelector, "div.cart_list", GetBy.CssSelector, "div.cart_item");
            _paymentInformationTextBox = new TextBox(_page, GetBy.TestId, "payment-info-value");
            _shippingInformationTextBox = new TextBox(_page, GetBy.TestId, "shipping-info-value");
            _summarySubtotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_subtotal_label");
            _summaryTaxTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_tax_label");
            _summaryTotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_total_label");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel");
            _finishButton = new Button(_page, GetBy.Role, "Finish");
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
            catch (PlaywrightException ex)
            {
                throw new Exception("Checkout Overview Page did not load correctly.", ex);
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
            await _overviewItemList.AssertItemElementTextAsync(expectedName, index, GetBy.CssSelector, "div.inventory_item_name");
            await _overviewItemList.AssertItemElementTextAsync(expectedPrice, index, GetBy.CssSelector, "div.inventory_item_price");
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
            await _cancelButton.ClickAsync();
            return await CheckoutPage.InitAsync(_page);
        }

        public async Task<CheckoutCompletePage> ClickFinishAsync()
        {
            EnsureInitialized();
            await _finishButton.ClickAsync();
            return await CheckoutCompletePage.InitAsync(_page);
        }
    }
}
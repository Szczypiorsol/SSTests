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
            await _overviewItemList.CheckIsVisibleAsync();
            await _paymentInformationTextBox.CheckIsVisibleAsync();
            await _shippingInformationTextBox.CheckIsVisibleAsync();
            await _summarySubtotalTextBox.CheckIsVisibleAsync();
            await _summaryTaxTextBox.CheckIsVisibleAsync();
            await _summaryTotalTextBox.CheckIsVisibleAsync();
            await _cancelButton.CheckIsVisibleAsync();
            await _finishButton.CheckIsVisibleAsync();
            _isInitialized = true;
        }

        public static async Task<CheckoutOverviewPage> InitAsync(IPage page)
        {
            CheckoutOverviewPage checkoutOverviewPage = new(page);
            await checkoutOverviewPage.InitAsync();
            return checkoutOverviewPage;
        }

        public async Task<int> GetOverviewItemsCountAsync()
        {
            EnsureInitialized();
            return await _overviewItemList.GetItemCountAsync();
        }

        public async Task<string> GetPaymentInformationAsync()
        {
            EnsureInitialized();
            return await _paymentInformationTextBox.GetTextAsync();
        }

        public async Task<string> GetShippingInformationAsync()
        {
            EnsureInitialized();
            return await _shippingInformationTextBox.GetTextAsync();
        }

        public async Task<string> GetSummarySubtotalAsync()
        {
            EnsureInitialized();
            return await _summarySubtotalTextBox.GetTextAsync();
        }

        public async Task<string> GetSummaryTaxAsync()
        {
            EnsureInitialized();
            return await _summaryTaxTextBox.GetTextAsync();
        }

        public async Task<string> GetSummaryTotalAsync()
        {
            EnsureInitialized();
            return await _summaryTotalTextBox.GetTextAsync();
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
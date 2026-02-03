using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public class CheckoutOverviewPage : BasePage
    {
        private readonly ListControl _productsItemList;
        private readonly TextBox _paymentInformationTextBox;
        private readonly TextBox _shippingInformationTextBox;
        private readonly TextBox _summarySubtotalTextBox;
        private readonly TextBox _summaryTaxTextBox;
        private readonly TextBox _summaryTotalTextBox;
        private readonly Button _cancelButton;
        private readonly Button _finishButton;

        public ListControl ProductsItemList => _productsItemList;
        public TextBox PaymentInformationTextBox => _paymentInformationTextBox;
        public TextBox ShippingInformationTextBox => _shippingInformationTextBox;
        public TextBox SummarySubtotalTextBox => _summarySubtotalTextBox;
        public TextBox SummaryTaxTextBox => _summaryTaxTextBox;
        public TextBox SummaryTotalTextBox => _summaryTotalTextBox;
        public Button CancelButton => _cancelButton;
        public Button FinishButton => _finishButton;

        public CheckoutOverviewPage(IPage page, ILogger logger) : base(page, "CheckoutOverviewPage", logger)
        {
            _productsItemList = new ListControl(_page, GetBy.CssSelector, "div.cart_list",GetBy.CssSelector, "div.cart_item");
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
            _logger?.Information("Initializing [CheckoutOverviewPage]...");
            await ProductsItemList.WaitToBeVisibleAsync();
            await PaymentInformationTextBox.WaitToBeVisibleAsync();
            await ShippingInformationTextBox.WaitToBeVisibleAsync();
            await SummarySubtotalTextBox.WaitToBeVisibleAsync();
            await SummaryTaxTextBox.WaitToBeVisibleAsync();
            await SummaryTotalTextBox.WaitToBeVisibleAsync();
            await CancelButton.WaitToBeVisibleAsync();
            await FinishButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[CheckoutOverviewPage] initialized successfully.");
        }

        public static async Task<CheckoutOverviewPage> InitAsync(IPage page, ILogger logger)
        {
            CheckoutOverviewPage checkoutOverviewPage = new(page, logger);
            await checkoutOverviewPage.InitAsync();
            return checkoutOverviewPage;
        }

        public ILocator GetProductNameLocator(int index)
        {
            return _productsItemList.GetItemElementLocator(index, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int index)
        {
            return _productsItemList.GetItemElementLocator(index, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<CheckoutPage> ClickCancelAsync()
        {
            _logger?.Information("Clicking [Cancel] button on [CheckoutOverviewPage]...");
            EnsureInitialized();
            await CancelButton.ClickAsync();
            _logger.Information("[Cancel] button clicked.");
            return await CheckoutPage.InitAsync(_page, _logger);
        }

        public async Task<CheckoutCompletePage> ClickFinishAsync()
        {
            _logger?.Information("Clicking [Finish] button on [CheckoutOverviewPage]...");
            EnsureInitialized();
            await _finishButton.ClickAsync();
            _logger.Information("[Finish] button clicked.");
            return await CheckoutCompletePage.InitAsync(_page, _logger);
        }
    }
}
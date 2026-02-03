using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public class CheckoutCompletePage : BasePage
    {
        private readonly TextBox _thankYouMessageTextBox;
        private readonly Button _backHomeButton;

        public TextBox ThankYouMessageTextBox => _thankYouMessageTextBox;
        public Button BackHomeButton => _backHomeButton;

        public CheckoutCompletePage(IPage page, ILogger logger) : base(page, "CheckoutCompletePage", logger)
        {
            _thankYouMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h2.complete-header");
            _backHomeButton = new Button(_page, GetBy.Role, "Back Home");
        }

        public override async Task InitAsync()
        {
            _logger?.Information("Initializing [CheckoutCompletePage]...");
            await ThankYouMessageTextBox.WaitToBeVisibleAsync();
            await BackHomeButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[CheckoutCompletePage] initialized successfully.");
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page, ILogger logger)
        {
            CheckoutCompletePage checkoutCompletePage = new(page, logger);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            _logger.Information("Clicking [Back Home] button on [CheckoutCompletePage]...");
            EnsureInitialized();
            await BackHomeButton.ClickAsync();
            _logger.Information("Clicked [Back Home] button on [CheckoutCompletePage]. Navigating to [ProductsPage]...");
            return await ProductsPage.InitAsync(_page, _logger);
        }
    }
}
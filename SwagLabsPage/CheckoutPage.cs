using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public class CheckoutPage : BasePage
    {
        private readonly TextBox _firstNameTextBox;
        private readonly TextBox _lastNameTextBox;
        private readonly TextBox _postalCodeTextBox;
        private readonly Button _cancelButton;
        private readonly Button _continueButton;
        private readonly TextBox _errorMessageTextBox;

        public TextBox FirstNameTextBox => _firstNameTextBox;
        public TextBox LastNameTextBox => _lastNameTextBox;
        public TextBox PostalCodeTextBox => _postalCodeTextBox;
        public Button CancelButton => _cancelButton;
        public Button ContinueButton => _continueButton;
        public TextBox ErrorMessageTextBox => _errorMessageTextBox;

        public CheckoutPage(IPage page, ILogger logger) : base(page, "CheckoutPage", logger)
        {
            _firstNameTextBox = new TextBox(_page, GetBy.Placeholder, "First Name");
            _lastNameTextBox = new TextBox(_page, GetBy.Placeholder, "Last Name");
            _postalCodeTextBox = new TextBox(_page, GetBy.Placeholder, "Zip/Postal Code");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel");
            _continueButton = new Button(_page, GetBy.Role, "Continue");
            _errorMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h3");
        }

        public override async Task InitAsync()
        {
            _logger?.Information("Initializing [CheckoutPage]...");
            await FirstNameTextBox.WaitToBeVisibleAsync();
            await LastNameTextBox.WaitToBeVisibleAsync();
            await PostalCodeTextBox.WaitToBeVisibleAsync();
            await CancelButton.WaitToBeVisibleAsync();
            await ContinueButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[CheckoutPage] initialized successfully.");
        }

        public static async Task<CheckoutPage> InitAsync(IPage page, ILogger logger)
        {
            CheckoutPage checkoutPage = new(page, logger);
            await checkoutPage.InitAsync();
            return checkoutPage;
        }

        public async Task<CheckoutPage> FillCheckoutInformationAsync(string firstName = "", string lastName = "", string postalCode = "")
        {
            _logger?.Information("Filling checkout information...");
            EnsureInitialized();
            if (!string.IsNullOrWhiteSpace(firstName))
                await FirstNameTextBox.EnterTextAsync(firstName);
            if (!string.IsNullOrWhiteSpace(lastName))
                await LastNameTextBox.EnterTextAsync(lastName);
            if (!string.IsNullOrWhiteSpace(postalCode))
                await PostalCodeTextBox.EnterTextAsync(postalCode);
            _logger.Information("Checkout information filled.");
            return await InitAsync(_page, _logger);
        }

        public async Task<CheckoutOverviewPage> ClickContinueAsync()
        {
            _logger?.Information("Clicking Continue button...");
            EnsureInitialized();
            await ContinueButton.ClickAsync();
            _logger.Information("Navigated to Checkout Overview Page.");
            return await CheckoutOverviewPage.InitAsync(_page, _logger);
        }

        public async Task<CheckoutPage> ClickContinueErrorExpectedAsync()
        {
            _logger?.Information("Clicking Continue button expecting error...");
            EnsureInitialized();
            await ContinueButton.ClickAsync();
            _logger.Information("Staying on Checkout Page due to expected error.");
            return await InitAsync(_page, _logger);
        }

        public async Task<CartPage> ClickCancelAsync()
        {
            _logger?.Information("Clicking Cancel button...");
            EnsureInitialized();
            await CancelButton.ClickAsync();
            _logger.Information("Navigated back to Cart Page.");
            return await CartPage.InitAsync(_page, _logger);
        }
    }
}
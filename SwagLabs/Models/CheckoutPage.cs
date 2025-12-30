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

        public CheckoutPage(IPage page) : base(page)
        {
            _firstNameTextBox = new TextBox(_page, GetBy.Placeholder, "First Name");
            _lastNameTextBox = new TextBox(_page, GetBy.Placeholder, "Last Name");
            _postalCodeTextBox = new TextBox(_page, GetBy.Placeholder, "Zip/Postal Code");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel");
            _continueButton = new Button(_page, GetBy.Role, "Continue");
        }

        public override async Task InitAsync()
        {
            await _firstNameTextBox.CheckIsVisibleAsync();
            await _lastNameTextBox.CheckIsVisibleAsync();
            await _postalCodeTextBox.CheckIsVisibleAsync();
            await _cancelButton.CheckIsVisibleAsync();
            await _continueButton.CheckIsVisibleAsync();
            _isInitialized = true;
        }

        public static async Task<CheckoutPage> InitAsync(IPage page)
        {
            CheckoutPage checkoutPage = new(page);
            await checkoutPage.InitAsync();
            return checkoutPage;
        }

        public async Task FillCheckoutInformationAsync(string firstName, string lastName, string postalCode)
        {
            EnsureInitialized();
            await _firstNameTextBox.EnterTextAsync(firstName);
            await _lastNameTextBox.EnterTextAsync(lastName);
            await _postalCodeTextBox.EnterTextAsync(postalCode);
        }

        public async Task<CheckoutOverviewPage> ClickContinueAsync()
        {
            EnsureInitialized();
            await _continueButton.ClickAsync();
            return await CheckoutOverviewPage.InitAsync(_page);
        }

        public async Task<CartPage> ClickCancelAsync()
        {
            EnsureInitialized();
            await _cancelButton.ClickAsync();
            return await CartPage.InitAsync(_page);
        }
    }
}
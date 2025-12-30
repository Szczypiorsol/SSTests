using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CheckoutCompletePage : BasePage
    {
        private readonly TextBox _thankYouMessageTextBox;
        private readonly Button _backHomeButton;

        public CheckoutCompletePage(IPage page) : base(page)
        {
            _thankYouMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h2.complete-header");
            _backHomeButton = new Button(_page, GetBy.Role, "Back Home");
        }

        public override async Task InitAsync()
        {
            await _thankYouMessageTextBox.CheckIsVisibleAsync();
            await _backHomeButton.CheckIsVisibleAsync();
            _isInitialized = true;
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page)
        {
            CheckoutCompletePage checkoutCompletePage = new(page);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task<string> GetThankYouMessageAsync()
        {
            EnsureInitialized();
            return await _thankYouMessageTextBox.GetTextAsync();
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            EnsureInitialized();
            await _backHomeButton.ClickAsync();
            return await ProductsPage.InitAsync(_page);
        }
    }
}
using Controls;
using Microsoft.Playwright;
using Serilog;
using static Controls.Control;

namespace SwagLabs.Pages
{
    public class CartPage : BasePage
    {
        private readonly ListControl _productsListControl;
        private readonly Button _continueShoppingButton;
        private readonly Button _checkoutButton;

        public ListControl ProductsListControl => _productsListControl;
        public Button ContinueShoppingButton => _continueShoppingButton;
        public Button CheckoutButton => _checkoutButton;

        public CartPage(IPage page, ILogger logger) : base(page, "CartPage", logger)
        {
            _productsListControl = new ListControl(
                page: _page,
                getByList: GetBy.CssSelector,
                listName: "div.cart_list",
                getByItem: GetBy.CssSelector,
                itemName: "div.cart_item"
                );
            _continueShoppingButton = new Button(_page, GetBy.Role, "Continue Shopping");
            _checkoutButton = new Button(_page, GetBy.Role, "Checkout");
        }

        public override async Task InitAsync()
        {
            _logger?.Information("Initializing [CartPage]...");
            await ProductsListControl.WaitToBeVisibleAsync();
            await ContinueShoppingButton.WaitToBeVisibleAsync();
            await CheckoutButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[CartPage] initialized successfully.");
        }

        public static async Task<CartPage> InitAsync(IPage page, ILogger logger)
        {
            CartPage cartPage = new(page, logger);
            await cartPage.InitAsync();
            return cartPage;
        }

        public ILocator GetProductNameLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<CartPage> RemoveCartItemAsync(int ordinalNumber)
        {
            _logger.Information("Removing item #{OrdinalNumber} from the cart...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            _logger.Information("Item #{OrdinalNumber} removed from the cart.", ordinalNumber);
            return await InitAsync(_page, _logger);
        }

        public async Task<ProductsPage> ClickContinueShoppingAsync()
        {
            _logger.Information("Clicking 'Continue Shopping' button...");
            EnsureInitialized();
            await ContinueShoppingButton.ClickAsync();
            _logger.Information("'Continue Shopping' button clicked.");
            return await ProductsPage.InitAsync(_page, _logger);
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            _logger.Information("Clicking 'Checkout' button...");
            EnsureInitialized();
            await CheckoutButton.ClickAsync();
            _logger.Information("'Checkout' button clicked.");
            return await CheckoutPage.InitAsync(_page, _logger);
        }
    }
}

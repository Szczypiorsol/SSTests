using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CartPage : BasePage
    {
        private readonly ListControl _cartList;
        private readonly Button _continueShoppingButton;
        private readonly Button _checkoutButton;

        public CartPage(IPage page) : base(page)
        {
            _cartList = new ListControl(_page, GetBy.CssSelector, "div.cart_list", GetBy.CssSelector, "div.cart_item");
            _continueShoppingButton = new Button(_page, GetBy.Role, "Continue Shopping");
            _checkoutButton = new Button(_page, GetBy.Role, "Checkout");
        }

        public override async Task InitAsync()
        {
            await _cartList.CheckIsVisibleAsync();
            await _continueShoppingButton.CheckIsVisibleAsync();
            await _checkoutButton.CheckIsVisibleAsync();
            _isInitialized = true;
        }

        public static async Task<CartPage> InitAsync(IPage page)
        {
            CartPage cartPage = new(page);
            await cartPage.InitAsync();
            return cartPage;
        }

        public async Task<int> GetCartItemsCountAsync()
        {
            EnsureInitialized();
            return await _cartList.GetItemCountAsync();
        }

        public async Task<string> GetCartItemNameAsync(int OrdinalNumber)
        {
            EnsureInitialized();
            return await _cartList.GetItemElementTextAsync(OrdinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public async Task<string> GetCartItemPriceAsync(int OrdinalNumber)
        {
            EnsureInitialized();
            return await _cartList.GetItemElementTextAsync(OrdinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task RemoveCartItemAsync(int OrdinalNumber)
        {
            EnsureInitialized();
            await _cartList.ClickOnItemElementAsync(OrdinalNumber, "button");
        }

        public async Task ClickContinueShoppingAsync()
        {
            EnsureInitialized();
            await _continueShoppingButton.ClickAsync();
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            EnsureInitialized();
            await _checkoutButton.ClickAsync();
            return await CheckoutPage.InitAsync(_page);
        }

        public ILocator GetCartListLocatorAsync()
        {
            EnsureInitialized();
            return _cartList.Locator;
        }

        public ILocator GetCartItemLocatorAsync()
        {
            EnsureInitialized();
            return _cartList.ListItemLocator;
        }

        public ILocator GetCartItemLocatorByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            return _cartList.GetItemLocatorByOrdinalNumber(ordinalNumber);
        }

        public ILocator GetCartItemNameLocatorByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            return _cartList.GetItemLocatorByOrdinalNumber(ordinalNumber).Locator("div.inventory_item_name");
        }

        public ILocator GetCartItemPriceLocatorByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            return _cartList.GetItemLocatorByOrdinalNumber(ordinalNumber).Locator("div.inventory_item_price");
        }
    }
}

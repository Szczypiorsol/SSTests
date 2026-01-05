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
            try
            {
                await _cartList.CheckIsVisibleAsync();
                await _continueShoppingButton.CheckIsVisibleAsync();
                await _checkoutButton.CheckIsVisibleAsync();
            }
            catch (PlaywrightException ex)
            {
                throw new Exception("Cart Page did not load correctly.", ex);
            }

            _isInitialized = true;
        }

        public static async Task<CartPage> InitAsync(IPage page)
        {
            CartPage cartPage = new(page);
            await cartPage.InitAsync();
            return cartPage;
        }

        public async Task AssertCartItemsCountAsync(int expectedCount)
        {
            EnsureInitialized();
            await _cartList.AssertItemCountAsync(expectedCount);
        }

        public async Task AssertCartItemAsync(int ordinalNumber, string expecterCartItemName, string expecterCartItemPrice)
        {
            EnsureInitialized();
            await _cartList.AssertItemElementTextAsync(expecterCartItemName, ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
            await _cartList.AssertItemElementTextAsync(expecterCartItemPrice, ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task RemoveCartItemAsync(int ordinalNumber)
        {
            EnsureInitialized();
            await _cartList.ClickOnItemElementAsync(ordinalNumber, "button");
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
    }
}

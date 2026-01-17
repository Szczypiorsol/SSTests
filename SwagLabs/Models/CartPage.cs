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

        public CartPage(IPage page, int defaultTimeout = 300) : base(page, "[CartPage]", defaultTimeout)
        {
            _cartList = new ListControl(
                page: _page,
                getByList: GetBy.CssSelector,
                listName: "div.cart_list",
                listDescription: $"{_pageName}_[ProductsList]",
                getByItem: GetBy.CssSelector,
                itemName: "div.cart_item",
                listItemDescription: $"{_pageName}_[Product]"
                );
            _continueShoppingButton = new Button(_page, GetBy.Role, "Continue Shopping", $"{_pageName}_[ContinueShoppingButton]");
            _checkoutButton = new Button(_page, GetBy.Role, "Checkout", $"{_pageName}_[CheckoutButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _cartList.CheckIsVisibleAsync();
                await _continueShoppingButton.CheckIsVisibleAsync();
                await _checkoutButton.CheckIsVisibleAsync();
            }
            catch (Exception ex) when (ex is AssertionException || ex is PlaywrightException)
            {
                throw new AssertionException($"{_pageName} did not load correctly.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"{_pageName} did not load within {_defaultTimeout} miliseconds.", ex);
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
            await _cartList.AssertItemElementTextAsync(expecterCartItemName, ordinalNumber, GetBy.CssSelector, "div.inventory_item_name", "Name");
            await _cartList.AssertItemElementTextAsync(
                expecterCartItemPrice, 
                ordinalNumber, 
                GetBy.CssSelector, 
                "div.inventory_item_price", 
                "Price"
                );
        }

        public async Task<CartPage> RemoveCartItemAsync(int ordinalNumber)
        {
            EnsureInitialized();
            try
            {
                await _cartList.ClickOnItemElementAsync(ordinalNumber, "button");
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to remove cart item at ordinal number {ordinalNumber} within {_defaultTimeout} miliseconds.", ex);
            }
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> ClickContinueShoppingAsync()
        {
            EnsureInitialized();
            try
            {
                await _continueShoppingButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click Continue Shopping button within {_defaultTimeout} miliseconds.", ex);
            }
            return await ProductsPage.InitAsync(_page);
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            EnsureInitialized();
            try
            {
                await _checkoutButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click Checkout button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutPage.InitAsync(_page);
        }
    }
}

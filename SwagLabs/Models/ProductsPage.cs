using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class ProductsPage : BasePage
    {
        private readonly Button _cartButton;
        private readonly ComboBox _sortComboBox;
        private readonly ListControl _productsList;

        public ProductsPage(IPage page, int defaultTimeout = 300) : base(page, "[ProductsPage]", defaultTimeout)
        {
            _cartButton = new Button(_page, GetBy.CssSelector, "a.shopping_cart_link", "ProductsPageCartButton");
            _sortComboBox = new ComboBox(
                _page, 
                GetBy.CssSelector, 
                "select.product_sort_container",
                $"{_pageName}_[SortComboBox]", 
                GetBy.CssSelector, 
                "option",
                $"{_pageName}_[SortOption]"
                );
            _productsList = new ListControl(
                _page, 
                GetBy.CssSelector, 
                "div.inventory_list",
                $"{_pageName}_[ProductsList]", 
                GetBy.CssSelector, 
                "div.inventory_item",
                $"{_pageName}_[Product]"
                );
        }

        public override async Task InitAsync()
        {
            try
            {
                await _cartButton.CheckIsVisibleAsync();
                await _sortComboBox.CheckIsVisibleAsync();
                await _productsList.CheckIsVisibleAsync();
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

        public static async Task<ProductsPage> InitAsync(IPage page)
        {
            ProductsPage productsPage = new(page);
            await productsPage.InitAsync();
            return productsPage;
        }

        public async Task AssertProductsCountAsync(int expectedCount)
        {
            EnsureInitialized();
            await _productsList.AssertItemCountAsync(expectedCount);
        }

        public async Task AssertProductByOrdinalNumberAsync(int ordinalNumber, string expectedProductName, string expectedPrice)
        {
            EnsureInitialized();
            await _productsList.AssertItemElementTextAsync(expectedProductName, ordinalNumber, GetBy.CssSelector, "div.inventory_item_name ", "Name");
            await _productsList.AssertItemElementTextAsync(expectedPrice, ordinalNumber, GetBy.CssSelector, "div.inventory_item_price", "Price");
        }

        public async Task<ProductsPage> ClickOnProductByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            try
            {
                await _productsList.ClickOnItemElementAsync(ordinalNumber, "button");
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click on the product button at position {ordinalNumber} within {_defaultTimeout} miliseconds.", ex);
            }
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> SelectSortOptionAsync(string optionText)
        {
            EnsureInitialized();
            try
            {
                await _sortComboBox.SelectItemByTextAsync(optionText);
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to select sort option '{optionText}' within {_defaultTimeout} miliseconds.", ex);
            }
            return await InitAsync(_page);
        }

        public async Task<CartPage> ClickOnCartButtonAsync()
        {
            EnsureInitialized();
            try
            {
                await _cartButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click on the cart button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CartPage.InitAsync(_page);
        }
    }
}

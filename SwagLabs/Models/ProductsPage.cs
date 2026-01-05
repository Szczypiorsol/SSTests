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

        public ProductsPage(IPage page) : base(page)
        {
            _cartButton = new Button(_page, GetBy.CssSelector, "a.shopping_cart_link");
            _sortComboBox = new ComboBox(_page, GetBy.CssSelector, "select.product_sort_container", GetBy.CssSelector, "option");
            _productsList = new ListControl(_page, GetBy.CssSelector, "div.inventory_list", GetBy.CssSelector, "div.inventory_item");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _cartButton.CheckIsVisibleAsync();
                await _sortComboBox.CheckIsVisibleAsync();
                await _productsList.CheckIsVisibleAsync();
            }
            catch (PlaywrightException ex)
            {
                throw new Exception("Products Page did not load correctly.", ex);
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
            await _productsList.AssertItemElementTextAsync(expectedProductName, ordinalNumber, GetBy.CssSelector, "div.inventory_item_name ");
            await _productsList.AssertItemElementTextAsync(expectedPrice, ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task ClickOnProductByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            await _productsList.ClickOnItemElementAsync(ordinalNumber, "button");
        }

        public async Task SelectSortOptionAsync(string optionText)
        {
            EnsureInitialized();
            await _sortComboBox.SelectItemByTextAsync(optionText);
        }

        public async Task<CartPage> ClickOnCartButtonAsync()
        {
            EnsureInitialized();
            await _cartButton.ClickAsync();
            return await CartPage.InitAsync(_page);
        }
    }
}

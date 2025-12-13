using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    internal class ProductsPage
    {
        private readonly IPage _page;
        private readonly Button _cartButton;
        private readonly ComboBox _sortComboBox;
        private readonly ListControl _productsList;

        public ProductsPage(IPage page)
        {
            _page = page;
            _cartButton = new Button(_page, GetBy.CssSelector, "a.shopping_cart_link");
            _sortComboBox = new ComboBox(_page, GetBy.CssSelector, "select.product_sort_container", GetBy.CssSelector, "option");
            _productsList = new ListControl(_page, GetBy.CssSelector, "div.inventory_list", GetBy.CssSelector, "div.inventory_item");
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _productsList.GetItemCountAsync();
        }

        public async Task<string> GetProductNameByOrdinalNumberAsync(int ordinalNumber)
        {
            return await _productsList.GetItemElementTextAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name ");
        }

        public async Task ClickOnProductByOrdinalNumberAsync(int ordinalNumber)
        {
            await _productsList.ClickOnItemElementAsync(ordinalNumber, "button");
        }

        public async Task<string> GetPriceByOrdinalNumberAsync(int ordinalNumber)
        {
            return await _productsList.GetItemElementTextAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task SelectSortOptionAsync(string optionText)
        {
            await _sortComboBox.SelectItemByTextAsync(optionText);
        }

        public async Task ClickOnCartButtonAsync()
        {
            await _cartButton.ClickAsync();
        }

        public async Task CheckIfViewIsVisibleAsync()
        {
            await _cartButton.CheckIsVisibleAsync();
            await _sortComboBox.CheckIsVisibleAsync();
            await _productsList.CheckIsVisibleAsync();
        }
    }
}

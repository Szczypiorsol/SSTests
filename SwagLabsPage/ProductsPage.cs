using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly Button _cartButton;
        private readonly ComboBox _sortComboBox;
        private readonly ListControl _productsListControl;
        private readonly TextBox _numberOfProductsInCartTextBox;

        public Button CartButton => _cartButton;
        public ComboBox SortComboBox => _sortComboBox;
        public ListControl ProductsListControl => _productsListControl;
        public TextBox NumberOfProductsInCartTextBox => _numberOfProductsInCartTextBox;

        public ProductsPage(IPage page, ILogger logger) : base(page, "[ProductsPage]", logger)
        {
            _cartButton = new Button(_page, GetBy.CssSelector, "a.shopping_cart_link");
            _sortComboBox = new ComboBox(_page, GetBy.CssSelector, "select.product_sort_container", GetBy.CssSelector, "option");
            _productsListControl = new ListControl(_page, GetBy.CssSelector, "div.inventory_list", GetBy.CssSelector, "div.inventory_item");
            _numberOfProductsInCartTextBox = new TextBox(_page, GetBy.CssSelector, "span.shopping_cart_badge");
        }

        public override async Task InitAsync()
        {
            _logger?.Information("Initializing [ProductsPage]...");
            await CartButton.WaitToBeVisibleAsync();
            await SortComboBox.WaitToBeVisibleAsync();
            await ProductsListControl.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[ProductsPage] initialized successfully.");
        }

        public static async Task<ProductsPage> InitAsync(IPage page, ILogger logger)
        {
            ProductsPage productsPage = new(page, logger);
            await productsPage.InitAsync();
            return productsPage;
        }

        public ILocator GetProductNameLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<ProductsPage> ClickOnProductByOrdinalNumberAsync(int ordinalNumber)
        {
            _logger.Information("Clicking on product button at ordinal number {OrdinalNumber}...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            _logger.Information("Clicked on product button at ordinal number {OrdinalNumber}.", ordinalNumber);
            return await InitAsync(_page, _logger);
        }

        public async Task<ProductsPage> RemoveProductByOrdinalNumberAsync(int ordinalNumber)
        {
            _logger.Information("Removing product at ordinal number {OrdinalNumber}...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            _logger.Information("Removed product at ordinal number {OrdinalNumber}.", ordinalNumber);
            return await InitAsync(_page, _logger);
        }

        public async Task<ProductsPage> SelectSortOptionAsync(string optionText)
        {
            _logger.Information("Selecting sort option '{OptionText}'...", optionText);
            EnsureInitialized();
            await SortComboBox.SelectItemByTextAsync(optionText);
            _logger.Information("Selected sort option '{OptionText}'.", optionText);
            return await InitAsync(_page, _logger);
        }

        public async Task<CartPage> ClickOnCartButtonAsync()
        {
            _logger.Information("Clicking on Cart button...");
            EnsureInitialized();
            await CartButton.ClickAsync();
            _logger.Information("Clicked on Cart button.");
            return await CartPage.InitAsync(_page, _logger);
        }
    }
}

using Microsoft.Playwright;
using SwagLabs.Models;

namespace SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task SellItemPositivePath()
        {
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            LoginPage loginPage = await LoginPage.InitAsync(page);
            ProductsPage productsPage = await loginPage.LoginAsync("standard_user", "secret_sauce");
            await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Expect(cartPage.GetCartItemLocatorAsync()).ToHaveCountAsync(1);
            await Expect(cartPage.GetCartItemNameLocatorByOrdinalNumberAsync(0)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Expect(cartPage.GetCartItemPriceLocatorByOrdinalNumberAsync(0)).ToHaveTextAsync("$9.99");
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            productsPage = await checkoutCompletePage.ClickBackHomeAsync();
        }
    }
}

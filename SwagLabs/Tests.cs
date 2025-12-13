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

            LoginPage loginPage = new(page);
            await loginPage.CheckIfViewIsVisibleAsync();
            ProductsPage productsPage = await loginPage.LoginAsync("standard_user", "secret_sauce");
            await productsPage.CheckIfViewIsVisibleAsync();
            await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            await productsPage.ClickOnCartButtonAsync();

            //// Expect a title "to contain" a substring.
            //await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

            //// create a locator
            //var getStarted = Page.Locator("text=Get Started");

            //// Expect an attribute "to be strictly equal" to the value.
            //await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            //// Click the get started link.
            //await getStarted.ClickAsync();

            //// Expects the URL to contain intro.
            //await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        }
    }
}

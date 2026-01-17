using Microsoft.Playwright;
using SwagLabs.Models;

namespace SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : BaseTest
    {
        [Test]
        public async Task When_UserIsLockedOut_Should_DisplayErrorMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(Users["LockedOutUser"], "secret_sauce");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        public async Task When_UserEntersWrongPassword_Should_DisplayInvalidCredentialsMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(UserLogin, "wrong_password");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task When_UserEntersWrongLogin_Should_DisplayInvalidCredentialsMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            loginPage = await loginPage.LoginWithInvalidCredentialsAsync("admin_user", "secret_sauce");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task When_SingleProductIsBought_Should_ValidateDetailsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(1);
            await cartPage.AssertCartItemAsync(0, "Sauce Labs Bike Light", "$9.99");
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(1);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Bike Light", "$9.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $9.99");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $0.80");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $10.79");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task When_UserSortsProductsByNameAndPrice_Should_DisplayCorrectOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");

            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await productsPage.AssertProductsCountAsync(6);
            await productsPage.SelectSortOptionAsync("Name (Z to A)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Backpack", "$29.99");
            await productsPage.SelectSortOptionAsync("Price (low to high)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.SelectSortOptionAsync("Price (high to low)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Onesie", "$7.99");
            await productsPage.SelectSortOptionAsync("Name (A to Z)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Test.allTheThings() T-Shirt (Red)", "$15.99");
        }

        [Test]
        public async Task When_SixProductsAreOrdered_Should_ValidateTotalsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");

            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await productsPage.AssertProductsCountAsync(6);
            for (int i = 0; i < 6; i++)
            {
                await productsPage.ClickOnProductByOrdinalNumberAsync(i);
            }
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(6);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(6);
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $129.94");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $10.40");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $140.34");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }


    }
}

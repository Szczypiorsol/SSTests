using Microsoft.Playwright;
using SwagLabs.Pages;

namespace SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : BaseTest
    {
        [Test]
        public async Task T001_When_UserIsLockedOut_Should_DisplayErrorMessage()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(Users["LockedOutUser"], "secret_sauce");
            await Assertions.Expect(loginPage.ErrorMessageTextBox.Locator)
                .ToHaveTextAsync("Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        public async Task T002_When_UserEntersWrongPassword_Should_DisplayInvalidCredentialsMessage()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(UserLogin, "wrong_password");
            await Assertions.Expect(loginPage.ErrorMessageTextBox.Locator)
                .ToHaveTextAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task T003_When_UserEntersWrongLogin_Should_DisplayInvalidCredentialsMessage()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            loginPage = await loginPage.LoginWithInvalidCredentialsAsync("admin_user", "secret_sauce");
            await Assertions.Expect(loginPage.ErrorMessageTextBox.Locator)
                .ToHaveTextAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task T004_When_SingleProductIsBought_Should_ValidateDetailsAndConfirmOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(1);
            await Assertions.Expect(cartPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(cartPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");

            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");

            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(1);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $9.99");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $0.80");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $10.79");

            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T005_When_UserSortsProductsByNameAndPrice_Should_DisplayCorrectOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await Assertions.Expect(productsPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(6);

            productsPage = await productsPage.SelectSortOptionAsync("Name (Z to A)");
            await Assertions.Expect(productsPage.GetProductNameLocator(0)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(productsPage.GetProductPriceLocator(0)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(productsPage.GetProductPriceLocator(1)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(2)).ToHaveTextAsync("Sauce Labs Fleece Jacket");
            await Assertions.Expect(productsPage.GetProductPriceLocator(2)).ToHaveTextAsync("$49.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(3)).ToHaveTextAsync("Sauce Labs Bolt T-Shirt");
            await Assertions.Expect(productsPage.GetProductPriceLocator(3)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(4)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(productsPage.GetProductPriceLocator(4)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(5)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(productsPage.GetProductPriceLocator(5)).ToHaveTextAsync("$29.99");

            productsPage = await productsPage.SelectSortOptionAsync("Price (low to high)");
            await Assertions.Expect(productsPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(productsPage.GetProductPriceLocator(0)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(productsPage.GetProductPriceLocator(1)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(2)).ToHaveTextAsync("Sauce Labs Bolt T-Shirt");
            await Assertions.Expect(productsPage.GetProductPriceLocator(2)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(3)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(productsPage.GetProductPriceLocator(3)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(4)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(productsPage.GetProductPriceLocator(4)).ToHaveTextAsync("$29.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(5)).ToHaveTextAsync("Sauce Labs Fleece Jacket");
            await Assertions.Expect(productsPage.GetProductPriceLocator(5)).ToHaveTextAsync("$49.99");
            
            productsPage = await productsPage.SelectSortOptionAsync("Price (high to low)");
            await Assertions.Expect(productsPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Fleece Jacket");
            await Assertions.Expect(productsPage.GetProductPriceLocator(0)).ToHaveTextAsync("$49.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(productsPage.GetProductPriceLocator(1)).ToHaveTextAsync("$29.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(2)).ToHaveTextAsync("Sauce Labs Bolt T-Shirt");
            await Assertions.Expect(productsPage.GetProductPriceLocator(2)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(3)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(productsPage.GetProductPriceLocator(3)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(4)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(productsPage.GetProductPriceLocator(4)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(5)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(productsPage.GetProductPriceLocator(5)).ToHaveTextAsync("$7.99");

            productsPage = await productsPage.SelectSortOptionAsync("Name (A to Z)");
            await Assertions.Expect(productsPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(productsPage.GetProductPriceLocator(0)).ToHaveTextAsync("$29.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(productsPage.GetProductPriceLocator(1)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(2)).ToHaveTextAsync("Sauce Labs Bolt T-Shirt");
            await Assertions.Expect(productsPage.GetProductPriceLocator(2)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(3)).ToHaveTextAsync("Sauce Labs Fleece Jacket");
            await Assertions.Expect(productsPage.GetProductPriceLocator(3)).ToHaveTextAsync("$49.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(4)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(productsPage.GetProductPriceLocator(4)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(productsPage.GetProductNameLocator(5)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(productsPage.GetProductPriceLocator(5)).ToHaveTextAsync("$15.99");
        }

        [Test]
        public async Task T006_When_SixProductsAreOrdered_Should_ValidateTotalsAndConfirmOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await Assertions.Expect(productsPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(6);
            for (int i = 0; i < 6; i++)
            {
                productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(i);
            }

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(6);
            
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(6);
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $129.94");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $10.40");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $140.34");
            
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T007_When_ItemIsDeletedFromCartOnProductPage_Should_ReflectCorrectTotalsAndConfirmOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(0);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);
            productsPage = await productsPage.RemoveProductByOrdinalNumberAsync(2);

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(2);

            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");

            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(2);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$29.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(1)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $37.98");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $3.04");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $41.02");

            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T008_When_ItemIsDeletedFromCartOnCartPage_Should_ReflectCorrectTotalsAndConfirmOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(3);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(3);
            cartPage = await cartPage.RemoveCartItemAsync(1);

            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");

            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(2);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(1)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(1)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $25.98");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $2.08");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $28.06");

            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T009_When_CheckoutFormIsIncomplete_Should_DisplayValidationErrors()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(0);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(3);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(3);

            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.ClickContinueErrorExpectedAsync();
            await Assertions.Expect(checkoutPage.ErrorMessageTextBox.Locator).ToHaveTextAsync("Error: First Name is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(firstName: "John");
            checkoutPage = await checkoutPage.ClickContinueErrorExpectedAsync();
            await Assertions.Expect(checkoutPage.ErrorMessageTextBox.Locator).ToHaveTextAsync("Error: Last Name is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(lastName: "Doe");
            checkoutPage = await checkoutPage.ClickContinueErrorExpectedAsync();
            await Assertions.Expect(checkoutPage.ErrorMessageTextBox.Locator).ToHaveTextAsync("Error: Postal Code is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(postalCode: "12345");

            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(3);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$29.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Fleece Jacket");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(1)).ToHaveTextAsync("$49.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(2)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(2)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $87.97");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $7.04");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $95.01");

            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T10_When_SameUserReauthentication_Should_PreserveCartContentsAndConfirmOrder()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);

            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(3);
            
            loginPage = await cartPage.LogoutAsync();
            
            productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            
            cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(3);

            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(3);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(1)).ToHaveTextAsync("Sauce Labs Bolt T-Shirt");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(1)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(2)).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(2)).ToHaveTextAsync("$15.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $41.97");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $3.36");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $45.33");
            
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T11_When_DifferentUserReauthentication_Should_NotRetainPreviousCart()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);
            
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(3);
            
            loginPage = await cartPage.LogoutAsync();
            string userToLogin = UserLogin == Users["StandardUser"] ? Users["VisualUser"] : Users["StandardUser"];
            
            productsPage = await loginPage.LoginAsync(userToLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);
            
            cartPage = await productsPage.ClickOnCartButtonAsync();
            await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(1);
            
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(1);
            await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Onesie");
            await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$7.99");
            await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
            await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
            await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $7.99");
            await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $0.64");
            await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $8.63");

            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T12_When_PageIsRefreshed_CartShouldRemainUnchanged()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(0);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(3);
            await Assertions.Expect(productsPage.NumberOfProductsInCartTextBox.Locator).ToHaveTextAsync("2");
            await productsPage.RefreshAsync();
            await Assertions.Expect(productsPage.NumberOfProductsInCartTextBox.Locator).ToHaveTextAsync("2");
        }

        [Test]
        public async Task T13_When_UnauthenticatedUserAccessesCheckout_ShouldBeRedirectedToLogin()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync("https://www.saucedemo.com/checkout-step-two.html");
            await Assertions.Expect(loginPage.ErrorMessageTextBox.Locator)
                .ToHaveTextAsync("Epic sadface: You can only access '/checkout-step-two.html' when you are logged in.");
        }

        [Test]
        public async Task T14_When_MultipleUsersPerformActions_Should_IsolateSessionsCorrectly()
        {
            LoginPage loginPage = await NavigateToLoginPageAsync();

            IBrowserContext? browserContext2 = null;
            
            try
            {
                browserContext2 = await Browser.NewContextAsync();
                IPage pageInstance2 = await browserContext2.NewPageAsync();
                await pageInstance2.GotoAsync("https://www.saucedemo.com/");
                LoginPage loginPage2 = await LoginPage.InitAsync(pageInstance2, Logger);
                string userToLogin = UserLogin == Users["StandardUser"] ? Users["VisualUser"] : Users["StandardUser"];
            
                ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
                ProductsPage productsPage2 = await loginPage2.LoginAsync(userToLogin, "secret_sauce");
                productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
                productsPage2 = await productsPage2.ClickOnProductByOrdinalNumberAsync(0);
         
                CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
                CartPage cartPage2 = await productsPage2.ClickOnCartButtonAsync();
                await Assertions.Expect(cartPage.ProductsListControl.ItemsLocator).ToHaveCountAsync(1);
                await Assertions.Expect(cartPage2.ProductsListControl.ItemsLocator).ToHaveCountAsync(1);
                await Assertions.Expect(cartPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
                await Assertions.Expect(cartPage2.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Backpack");
                await Assertions.Expect(cartPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");
                await Assertions.Expect(cartPage2.GetProductPriceLocator(0)).ToHaveTextAsync("$29.99");
            
                CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
                CheckoutPage checkoutPage2 = await cartPage2.ClickCheckoutAsync();
                checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
                checkoutPage2 = await checkoutPage2.FillCheckoutInformationAsync("Jane", "Smith", "54321");
            
                CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
                CheckoutOverviewPage checkoutOverviewPage2 = await checkoutPage2.ClickContinueAsync();
                await Assertions.Expect(checkoutOverviewPage.ProductsItemList.ItemsLocator).ToHaveCountAsync(1);
                await Assertions.Expect(checkoutOverviewPage2.ProductsItemList.ItemsLocator).ToHaveCountAsync(1);
                await Assertions.Expect(checkoutOverviewPage.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Bike Light");
                await Assertions.Expect(checkoutOverviewPage2.GetProductNameLocator(0)).ToHaveTextAsync("Sauce Labs Backpack");
                await Assertions.Expect(checkoutOverviewPage.GetProductPriceLocator(0)).ToHaveTextAsync("$9.99");
                await Assertions.Expect(checkoutOverviewPage2.GetProductPriceLocator(0)).ToHaveTextAsync("$29.99");
                await Assertions.Expect(checkoutOverviewPage.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
                await Assertions.Expect(checkoutOverviewPage2.PaymentInformationTextBox.Locator).ToHaveTextAsync("SauceCard #31337");
                await Assertions.Expect(checkoutOverviewPage.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
                await Assertions.Expect(checkoutOverviewPage2.ShippingInformationTextBox.Locator).ToHaveTextAsync("Free Pony Express Delivery!");
                await Assertions.Expect(checkoutOverviewPage.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $9.99");
                await Assertions.Expect(checkoutOverviewPage2.SummarySubtotalTextBox.Locator).ToHaveTextAsync("Item total: $29.99");
                await Assertions.Expect(checkoutOverviewPage.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $0.80");
                await Assertions.Expect(checkoutOverviewPage2.SummaryTaxTextBox.Locator).ToHaveTextAsync("Tax: $2.40");
                await Assertions.Expect(checkoutOverviewPage.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $10.79");
                await Assertions.Expect(checkoutOverviewPage2.SummaryTotalTextBox.Locator).ToHaveTextAsync("Total: $32.39");
            
                CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
                CheckoutCompletePage checkoutCompletePage2 = await checkoutOverviewPage2.ClickFinishAsync();
                await Assertions.Expect(checkoutCompletePage.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
                await Assertions.Expect(checkoutCompletePage2.ThankYouMessageTextBox.Locator).ToHaveTextAsync("Thank you for your order!");
                _ = await checkoutCompletePage.ClickBackHomeAsync();
                _ = await checkoutCompletePage2.ClickBackHomeAsync();
            }
            finally
            {
                if (browserContext2 != null)
                {
                    await browserContext2.CloseAsync();
                }
            }
        }
    }
}

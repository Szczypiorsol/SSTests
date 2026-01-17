using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class LoginPage : BasePage
    {
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly TextBox _errorMessageTextBox;
        private readonly Button _loginButton;

        public LoginPage(IPage page, int defaultTimeout = 300) : base(page, "[LoginPage]", defaultTimeout)
        {
            _usernameTextBox = new TextBox(_page, GetBy.Role, "Username", $"{_pageName}_[UsernameTextBox]");
            _passwordTextBox = new TextBox(_page, GetBy.Role, "Password", $"{_pageName}_[PasswordTextBox]");
            _errorMessageTextBox = new TextBox(_page, GetBy.CssSelector, "div.error-message-container", $"{_pageName}_[ErrorMessageTextBox]");
            _loginButton = new Button(_page, GetBy.Role, "Login", $"{_pageName}_[LoginButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _usernameTextBox.CheckIsVisibleAsync();
                await _passwordTextBox.CheckIsVisibleAsync();
                await _loginButton.CheckIsVisibleAsync();
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

        public static async Task<LoginPage> InitAsync(IPage page)
        {
            LoginPage loginPage = new(page);
            await loginPage.InitAsync();
            return loginPage;
        }

        public async Task AssertErrorMessageAsync(string expectedText)
        {
            EnsureInitialized();
            await _errorMessageTextBox.CheckIsVisibleAsync();
            await _errorMessageTextBox.AssertTextAsync(expectedText);
        }

        public async Task<ProductsPage> LoginAsync(string username, string password)
        {
            EnsureInitialized();
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            try
            {
                return await ProductsPage.InitAsync(_page);
            }
            catch (AssertionException ex)
            {
                if (ex.Message.Contains("[ProductsPage] did not load correctly."))
                {
                    await _errorMessageTextBox.CheckIsVisibleAsync();
                    string errorText = await _errorMessageTextBox.GetTextAsync();
                    throw new AssertionException($"[{_pageName}] Login failed. Error message displayed: '{errorText}'", ex);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<LoginPage> LoginWithInvalidCredentialsAsync(string username, string password)
        {
            EnsureInitialized();
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            return await InitAsync(_page);
        }

        private async Task ClickLoginButton()
        {
            try
            {
                await _loginButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click on the login button within {_defaultTimeout} miliseconds.", ex);
            }
        }
    }
}
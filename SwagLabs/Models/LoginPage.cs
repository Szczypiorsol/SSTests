using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    internal class LoginPage
    {
        private readonly IPage _page;
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly Button _loginButton;

        public LoginPage(IPage page)
        {
            _page = page;
            _usernameTextBox = new TextBox(_page, GetBy.Role , "Username");
            _passwordTextBox = new TextBox(_page, GetBy.Role, "Password");
            _loginButton = new Button(_page, GetBy.Role, "Login");
        }

        public async Task CheckIfViewIsVisibleAsync()
        {
            await _usernameTextBox.CheckIsVisibleAsync();
            await _passwordTextBox.CheckIsVisibleAsync();
            await _loginButton.CheckIsVisibleAsync();
        }


        public async Task<ProductsPage> LoginAsync(string username, string password)
        {
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await _loginButton.ClickAsync();

            return new ProductsPage(_page);
        }

        public async Task<LoginPage> LoginWithInvalidCredentialsAsync(string username, string password)
        {
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await _loginButton.ClickAsync();

            return this;
        }
    }
}

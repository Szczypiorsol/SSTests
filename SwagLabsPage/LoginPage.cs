using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
{
    public class LoginPage : BasePage
    {
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly TextBox _errorMessageTextBox;
        private readonly Button _loginButton;

        public TextBox UsernameTextBox => _usernameTextBox;
        public TextBox PasswordTextBox => _passwordTextBox;
        public TextBox ErrorMessageTextBox => _errorMessageTextBox;
        public Button LoginButton => _loginButton;

        public LoginPage(IPage page, ILogger logger) : base(page, "LoginPage", logger)
        {
            _usernameTextBox = new TextBox(_page, GetBy.Role, "Username");
            _passwordTextBox = new TextBox(_page, GetBy.Role, "Password");
            _errorMessageTextBox = new TextBox(_page, GetBy.CssSelector, "div.error-message-container");
            _loginButton = new Button(_page, GetBy.Role, "Login");
        }

        public override async Task InitAsync()
        {
            _logger?.Information("Initializing [LoginPage]...");
            await UsernameTextBox.WaitToBeVisibleAsync();
            await PasswordTextBox.WaitToBeVisibleAsync();
            await LoginButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Information("[LoginPage] initialized successfully.");
        }

        public static async Task<LoginPage> InitAsync(IPage page, ILogger logger)
        {
            LoginPage loginPage = new(page, logger);
            await loginPage.InitAsync();
            return loginPage;
        }

        public async Task<ProductsPage> LoginAsync(string username, string password)
        {
            _logger?.Information("Performing login with username: {Username}", username);
            EnsureInitialized();
            await UsernameTextBox.EnterTextAsync(username);
            await PasswordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            _logger.Information("Login successful, navigating to ProductsPage.");
            return await ProductsPage.InitAsync(_page, _logger);
        }

        public async Task<LoginPage> LoginWithInvalidCredentialsAsync(string username, string password)
        {
            _logger?.Information("Attempting login with invalid credentials. Username: {Username}", username);
            EnsureInitialized();
            await UsernameTextBox.EnterTextAsync(username);
            await PasswordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            _logger.Information("Login attempt with invalid credentials completed.");
            return await InitAsync(_page, _logger);
        }

        private async Task ClickLoginButton()
        {
            await LoginButton.ClickAsync();
        }
    }
}
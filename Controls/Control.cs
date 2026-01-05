using Microsoft.Playwright;

namespace Controls
{
    public class Control
    {
        public enum GetBy
        {
            Role,
            TestId,
            Text,
            CssSelector,
            Placeholder
        }

        protected readonly ILocator _locator;
        protected readonly string _name;

        public Control(ILocator locator, string name)
        {
            if (locator is null)
            {
                throw new ArgumentNullException(nameof(locator), "Locator cannot be null.");
            }

            _locator = locator;
            _name = name;
        }

        public static ILocator GetLocator(IPage page, GetBy getBy, AriaRole ariaRole, string name)
        {
            return getBy switch
            {
                GetBy.Role => page.GetByRole(ariaRole, new PageGetByRoleOptions { Name = name }),
                GetBy.TestId => page.Locator($"[data-test='{name}']"),
                GetBy.Text => page.GetByText(name),
                GetBy.CssSelector => page.Locator(name),
                GetBy.Placeholder => page.GetByPlaceholder(name),
                _ => throw new Exception("GetBy method not recognized."),
            };
        }

        public async Task CheckIsVisibleAsync()
        {
           if (_locator.IsVisibleAsync().GetAwaiter().GetResult() != true)
            {
                await Assertions.Expect(_locator).ToBeVisibleAsync();
                //string TypeName = this.GetType().Name;
                //throw new Exception($"{TypeName} {_name} is not visible.");
            }
        }

        public async Task ClickAsync()
        {
            await _locator.ClickAsync();
        }
    }
}

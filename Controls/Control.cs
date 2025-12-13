using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Controls
{
    public class Control(ILocator locator, string name)
    {
        public enum GetBy
        {
            Role,
            TestId,
            Text,
            CssSelector
        }

        protected readonly ILocator _locator = locator;
        protected readonly string _name = name;

        public static ILocator GetLocator(IPage page, GetBy getBy, AriaRole ariaRole, string name)
        {
            return getBy switch
            {
                GetBy.Role => page.GetByRole(ariaRole, new PageGetByRoleOptions { Name = name }),
                GetBy.TestId => page.Locator($"[data-testid='{name}']"),
                GetBy.Text => page.GetByText(name),
                GetBy.CssSelector => page.Locator(name),
                _ => throw new Exception("GetBy method not recognized."),
            };
        }

        public async Task CheckIsVisibleAsync()
        {
            if(_locator.IsVisibleAsync().GetAwaiter().GetResult() != true)
            {
                string TypeName = this.GetType().Name;
                throw new Exception($"{TypeName} {_name} is not visible.");
            }
        }

        public async Task ClickAsync()
        {
            await _locator.ClickAsync();
        }
    }
}

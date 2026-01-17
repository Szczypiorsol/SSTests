using Microsoft.Playwright;
using NUnit.Framework;

namespace Controls
{
    public class TextBox(IPage page, Control.GetBy getBy, string name, string description) 
        : Control(GetLocator(page, getBy, AriaRole.Textbox, name), name, description)
    {
        public async Task EnterTextAsync(string text)
        {
            await _locator.FillAsync(text);
        }

        public async Task AssertTextAsync(string expectedText)
        {
            try
            {
                await Assertions.Expect(_locator).ToHaveTextAsync(expectedText);
            }
            catch (PlaywrightException ex)
            {
                string actualText = await _locator.InnerTextAsync();
                throw new AssertionException($"TextBox {_description} should have text '{expectedText}', but has '{actualText}'", ex);
            }
        }

        public async Task<string> GetTextAsync()
        {
            return await _locator.InnerTextAsync();
        }
    }
}

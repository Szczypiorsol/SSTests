using Microsoft.Playwright;
using static Controls.Control;

namespace Controls
{
    public class Button(IPage page, GetBy getBy, string name) : Control(GetLocator(page, getBy, AriaRole.Button, name), name)
    {
    }
}

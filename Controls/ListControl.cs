using Microsoft.Playwright;
using System.Threading.Tasks;
using static Controls.Control;

namespace Controls
{
    public class ListControl(IPage page, GetBy getByList, string listName, GetBy getByItem, string itemName) : Control(GetLocator(page, getByList, AriaRole.List, listName), listName)
    {
        private readonly IPage _page = page;
        private readonly ILocator _listItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);
        private readonly string _listItemName = itemName;

        public ILocator ListItemLocator => _listItemLocator;

        public async Task<int> GetItemCountAsync()
        {
            return await _listItemLocator.CountAsync();
        }

        public async Task<string> GetItemElementTextAsync(int OrdinalNumber, GetBy getBy, string name)
        {
            await CheckIfItemIsVisibleAsync(OrdinalNumber);
            var text = await _listItemLocator.Nth(OrdinalNumber).And(GetLocator(_page, getBy, AriaRole.None, name)).TextContentAsync();
            return text ?? string.Empty;
        }

        public async Task ClickOnItemElementAsync(int OrdinalNumber, string name)
        {
            await CheckIfItemIsVisibleAsync(OrdinalNumber);
            await _listItemLocator.Nth(OrdinalNumber).Locator(name).ClickAsync();
        }

        public async Task CheckIfItemIsVisibleAsync(int OrdinalNumber)
        {
            if (_listItemLocator.Nth(OrdinalNumber).IsVisibleAsync().GetAwaiter().GetResult() != true)
            {
                throw new Exception($"ListItem {_listItemName}_{OrdinalNumber} is not visible.");
            }
        }

        public ILocator GetItemLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _listItemLocator.Nth(ordinalNumber);
        }
    }
}

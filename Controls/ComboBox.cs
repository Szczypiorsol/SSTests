using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using static Controls.Control;

namespace Controls
{
    public class ComboBox(IPage page, GetBy getByList, string listName, GetBy getByItem, string itemName) : Control(GetLocator(page, getByList, AriaRole.List, listName), listName)
    {
        private readonly ILocator _listItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);
        private readonly string _listItemName = itemName;

        public async Task SelectItemByTextAsync(string itemText)
        {
            await _locator.SelectOptionAsync(new SelectOptionValue { Label = itemText });
        }

        public async Task CheckIfItemIsVisibleAsync(int OrdinalNumber)
        {
            if (_listItemLocator.Nth(OrdinalNumber).IsVisibleAsync().GetAwaiter().GetResult() != true)
            {
                throw new Exception($"ListItem {_listItemName}_{OrdinalNumber} is not visible.");
            }
        }
    }
}

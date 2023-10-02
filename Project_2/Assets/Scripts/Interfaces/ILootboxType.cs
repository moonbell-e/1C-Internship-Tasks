using System;
using System.Collections.Generic;

public interface ILootboxType
{
    void HandleItems(List<Item> itemsFromLootbox, List<Item> itemsToReturn, Dictionary<Item, ItemTextData> itemUIObjects);
    void HandleTakeItemsButtonClicked(Action<List<Item>, Item, string> takeItemsButtonClicked,
        List<Item> items, Item lootbox, string lootboxType);
}

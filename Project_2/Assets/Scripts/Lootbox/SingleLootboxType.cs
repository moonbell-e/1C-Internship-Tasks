using System;
using System.Collections.Generic;

public class SingleLootboxType : ILootboxType
{
    public void HandleItems(List<Item> itemsFromLootbox, List<Item> itemsToReturn,
        Dictionary<Item, ItemTextData> itemUIObjects)
    {
        itemsToReturn.Add(itemsFromLootbox[0]);
        itemUIObjects[itemsToReturn[0]].GetItemButton().interactable = false;
    }

    public void HandleTakeItemsButtonClicked(Action<List<Item>, Item, string> takeItemsButtonClicked,
        List<Item> itemsToReturn, Item lootbox,
        string lootboxType)
    {
        takeItemsButtonClicked?.Invoke(itemsToReturn, lootbox, lootboxType);
    }
}
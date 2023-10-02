using System;
using System.Collections.Generic;

public class MultipleLootboxType : ILootboxType
{
    public void HandleItems(List<Item> itemsFromLootbox, List<Item> itemsToReturn,
        Dictionary<Item, ItemTextData> itemUIObjects)
    {
        DeactivateButtons(itemsFromLootbox, itemUIObjects);
    }

    public void HandleTakeItemsButtonClicked(Action<List<Item>, Item, string> takeItemsButtonClicked,
        List<Item> itemsFromLootbox, Item lootbox, string lootboxType)
    {
        takeItemsButtonClicked?.Invoke(itemsFromLootbox, lootbox, lootboxType);
    }

    private static void DeactivateButtons(List<Item> lootboxItems,
        IReadOnlyDictionary<Item, ItemTextData> itemUIObjects)
    {
        foreach (var item in lootboxItems)
        {
            itemUIObjects[item].GetItemButton().interactable = false;
        }
    }
}
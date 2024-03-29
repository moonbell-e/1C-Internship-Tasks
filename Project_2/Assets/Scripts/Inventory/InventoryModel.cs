using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class InventoryModel : IModel
{
    public List<Item> Items { get; set; }
    public int money;

    public bool IsAnyLootboxes()
    {
        return Items.Any(item => item.config.lootbox != null);
    }

    public Item GetFirstLootbox()
    {
        var firstItemWithLootbox = Items.FirstOrDefault(item => item.config.lootbox != null);
        return firstItemWithLootbox;
    }
}
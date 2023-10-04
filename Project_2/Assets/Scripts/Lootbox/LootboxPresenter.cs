using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class LootboxPresenter
{
    private readonly LootboxModel _lootboxModel;
    
    public LootboxPresenter(LootboxModel lootboxModel)
    {
        _lootboxModel = lootboxModel;
    }

    public List<Item> OpenLootbox(Item item)
    {
        _lootboxModel.Items = GetItemsFromLootbox(item);
        return _lootboxModel.Items;
    }
    
    public void TakeItemsFromMultipleLootbox(Item item, Action<List<Item>, Item, LootboxType> takeLootboxClicked)
    {
        takeLootboxClicked?.Invoke(_lootboxModel.Items, item, item.config.lootbox.type);
    }

    public void TakeItemsFromSingleLootbox(Item item, Action<List<Item>, Item, LootboxType> takeLootboxClicked)
    {
        takeLootboxClicked?.Invoke(_lootboxModel.ItemsToReturn, item, item.config.lootbox.type);
    }

    public void PurchaseItem(Item item)
    {
        _lootboxModel.ItemsToReturn.Add(item);
    }

    private static List<Item> GetItemsFromLootbox(Item item)
    {
        var itemsToReturn = new List<Item>();
        var slotCount = GetSelectedSlotCount(item);

        while (itemsToReturn.Count < slotCount)
        {
            PickItemsFromLootbox(itemsToReturn, item);
        }

        return itemsToReturn;
    }

    private static void PickItemsFromLootbox(ICollection<Item> itemsToReturn, Item item)
    {
        var tempSum = 0f;

        foreach (var lootboxItem in item.config.lootbox.content)
        {
            tempSum += lootboxItem.dropChance;

            if (tempSum < CalculateItemProbability(item)) continue;

            itemsToReturn.Add(new Item
            {
                id = lootboxItem.item.id,
                quantity = lootboxItem.item.quantity,
                config = lootboxItem.item.config
            });
            
            break;
        }
    }

    private static int GetSelectedSlotCount(Item item)
    {
        var tempSum = 0f;
        foreach (var slot in item.config.lootbox.slots)
        {
            tempSum += slot.weight;
            if (tempSum < CalculateSlotProbability(item)) continue;
            return slot.slotCount;
        }

        return 0;
    }

    private static float CalculateItemProbability(Item item)
    {
        var sumWeight = item.config.lootbox.content.Sum(lootboxItem => lootboxItem.dropChance);
        var randomNum = Random.Range(0, sumWeight);
        return randomNum;
    }

    private static float CalculateSlotProbability(Item item)
    {
        float totalWeight = item.config.lootbox.slots.Sum(slot => slot.weight);
        var randomNum = Random.Range(0, totalWeight);
        return randomNum;
    }
}
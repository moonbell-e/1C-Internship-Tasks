using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootboxPresenter
{
    private LootboxView _lootboxView;

    public LootboxPresenter(LootboxView lootboxView)
    {
        _lootboxView = lootboxView;
    }
    
    public static List<Item> OpenLootbox(Item item)
    {
        return GetItemsFromLootbox(item);
    }

    private static float CalculateProbability(Item item)
    {
        var sumWeight = item.config.content.Sum(lootboxItem => lootboxItem.config.dropChance);
        var randomNum = Random.Range(0, sumWeight);
        return randomNum;
    }

    private static List<Item> GetItemsFromLootbox(Item item)
    {
        var tempSum = 0f;
        List<Item> itemsToReturn = new();
        foreach (var lootboxItem in item.config.content)
        {
            tempSum += lootboxItem.config.dropChance;
            if (tempSum >= CalculateProbability(item))
            {
                itemsToReturn.Add(item);
            }
        }

        return itemsToReturn;
    }
}
    
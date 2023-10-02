using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootboxPresenter
{
    private LootboxStaticDataModel _staticDataModel;

    private readonly string _lootboxesStaticFileName = $"{Application.dataPath}/Configs/LootboxesStaticConfig.json";

    public Lootbox GetLootboxData(Item item)
    {
        var staticData = _staticDataModel.Lootboxes.Find(staticItem => staticItem.id == item.id);
        return staticData;
    }

    public void LoadLootboxData()
    {
        _staticDataModel = JsonHandler.LoadJson<LootboxStaticDataModel>(_lootboxesStaticFileName);
    }

    public static List<Item> OpenLootbox(Item item)
    {
        return GetItemsFromLootbox(item);
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

        return 1;
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
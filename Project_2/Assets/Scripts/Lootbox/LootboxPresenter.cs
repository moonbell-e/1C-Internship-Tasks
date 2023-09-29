using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootboxPresenter
{
    private LootboxView _lootboxView;
    private StaticDataModel _staticDataModel;

    private readonly string _lootboxesStaticFileName = $"{Application.dataPath}/Configs/LootboxesStaticConfig.json";

    public LootboxPresenter(LootboxView lootboxView)
    {
        _lootboxView = lootboxView;
    }

    public ItemStaticData GetLootboxStaticData(Item item)
    {
        var staticData = _staticDataModel.Items.Find(staticItem => staticItem.id == item.id);
        return staticData;
    }

    public void LoadLootboxData()
    {
        _staticDataModel = JsonHandler.LoadJson<StaticDataModel>(_lootboxesStaticFileName);
    }

    public static List<Item> OpenLootbox(Item item)
    {
        return GetItemsFromLootbox(item);
    }

    private static float CalculateProbability(Item item)
    {
        var sumWeight = item.config.content.Sum(lootboxItem => lootboxItem.dropChance);
        var randomNum = Random.Range(0, sumWeight);
        return randomNum;
    }

    private static List<Item> GetItemsFromLootbox(Item item)
    {
        var itemsToReturn = new List<Item>();

        while (itemsToReturn.Count < 3)
        {
            PickItemsFromLootbox(itemsToReturn, item);
        }

        return itemsToReturn;
    }

    private static void PickItemsFromLootbox(ICollection<Item> itemsToReturn, Item item)
    {
        var tempSum = 0f;

        foreach (var lootboxItem in item.config.content)
        {
            tempSum += lootboxItem.dropChance;

            if (tempSum < CalculateProbability(item)) continue;

            itemsToReturn.Add(new Item
            {
                id = lootboxItem.id,
                quantity = lootboxItem.quantity,
                config = lootboxItem.config
            });

            break;
        }
    }
}
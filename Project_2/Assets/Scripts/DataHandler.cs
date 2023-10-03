using UnityEngine;

public static class DataHandler
{
    private static readonly string InventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private static readonly string ShopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
    private static readonly string ItemsStaticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";
    private static readonly string LootboxesStaticFileName = $"{Application.dataPath}/Configs/LootboxesStaticConfig.json";

    public static StaticDataModel LoadStaticData()
    {
        var staticDataModel = LoadItemsStaticDataModel();
        var lootboxStaticData = LoadLootboxData();
        UpdateLootboxData(staticDataModel, lootboxStaticData);

        return staticDataModel;
    }

    public static InventoryModel LoadInventoryData()
    {
        return JsonHandler.LoadJson<InventoryModel>(InventoryFilePath);
    }

    public static ShopModel LoadShopData()
    {
        return JsonHandler.LoadJson<ShopModel>(ShopFilePath);
    }

    public static void SaveData(InventoryModel inventoryModel, ShopModel shopModel)
    {
        JsonHandler.SaveJson(inventoryModel, InventoryFilePath);
        JsonHandler.SaveJson(shopModel, ShopFilePath);
    }

    private static StaticDataModel LoadItemsStaticDataModel()
    {
        return JsonHandler.LoadJson<StaticDataModel>(ItemsStaticFileName);
    }

    private static LootboxStaticDataModel LoadLootboxData()
    {
        return JsonHandler.LoadJson<LootboxStaticDataModel>(LootboxesStaticFileName);
    }

    private static void UpdateLootboxData(StaticDataModel itemsData, LootboxStaticDataModel lootboxData)
    {
        foreach (var item in itemsData.Items)
        {
            var staticData = lootboxData.Lootboxes.Find(staticItem => staticItem.id == item.id);
            item.lootbox = staticData;

            if (staticData == null) continue;
            {
                foreach (var lootboxItem in staticData.content)
                {
                    lootboxItem.item.config = itemsData.Items.Find(staticItem => staticItem.id == lootboxItem.item.id);
                }
            }
        }
    }
}
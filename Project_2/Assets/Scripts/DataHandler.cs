using UnityEngine;

public static class DataHandler
{
    private static readonly string InventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private static readonly string ShopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
    private static readonly string ItemsStaticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";

    public static StaticDataModel LoadStaticData()
    {
        return JsonHandler.LoadJson<StaticDataModel>(ItemsStaticFileName);
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
}
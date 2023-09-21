using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;  
    
    private readonly InventoryView _inventoryView;
    private readonly ShopView _shopView;

    private readonly string _inventoryFilePath =  $"{Application.dataPath}/Configs/InventoryConfig.json";
    private readonly string _shopFilePath =  $"{Application.dataPath}/Configs/ShopConfig.json";
    private readonly string _staticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";

    public Presenter(InventoryView inventoryView, ShopView shopView, ShopModel shopModel, InventoryModel inventoryModel)
    {
        _inventoryView = inventoryView;
        _shopView = shopView;
        _shopModel = shopModel;
        _inventoryModel = inventoryModel;
        
        _inventoryView.OnSellButtonClicked += SellItem;
        _shopView.OnBuyButtonClicked += BuyItem;
    }

    ~Presenter()
    {
        _inventoryView.OnSellButtonClicked -= SellItem;
        _shopView.OnBuyButtonClicked -= BuyItem;
    }
    
    public ItemStaticData GetStaticData(Item item)
    {
        return _staticDataModel.Items.FirstOrDefault(staticItem => staticItem.id == item.id);
    }

    public void LoadData()
    {
        _staticDataModel = LoadJson<StaticDataModel>(_staticFileName);
        _inventoryModel = LoadJson<InventoryModel>(_inventoryFilePath);
        _shopModel = LoadJson<ShopModel>(_shopFilePath);
        
        PrepareViews();
    }
    
    private void BuyItem(Item item)
    {
        var staticItemData = GetStaticData(item);
        if (staticItemData != null)
        {
            var itemPrice = staticItemData.price;

            if (_inventoryModel.money >= itemPrice)
            {
                _inventoryModel.money -= itemPrice;
                _inventoryModel.Items.Add(item);
                _shopModel.Items.Remove(item);
                
                _inventoryView.UpdateViewAdd(item, _inventoryModel.money);
                _shopView.UpdateViewRemove(item);
                _inventoryView.UpdateButtons();
                
                _shopView.UpdateButtons();
                SaveData();
            }
            else
            {
                Debug.LogError("Not enough money to buy this item!");
            }
        }
        else
        {
            Debug.LogError($"Static data not found for item with id: {item.id}");
        }
    }

    private void SellItem(Item item)
    {
        var staticItemData = GetStaticData(item);
        if (staticItemData != null)
        {
            var itemPrice = staticItemData.price;
            _inventoryModel.money += itemPrice;
            
            _inventoryModel.Items.Remove(item);
            _shopModel.Items.Add(item);
            
            _inventoryView.UpdateViewRemove(item, _inventoryModel.money);
            _shopView.UpdateViewAdd(item);
            _inventoryView.UpdateButtons();
            
            _shopView.UpdateButtons();
            SaveData();
        }
        else
        {
            Debug.LogError($"Static data not found for item with id: {item.id}");
        }
    }


    private static T LoadJson<T>(string filePath)
    {
        if (!IsFileExists(filePath)) return default;
        var jsonData = System.IO.File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    private static void SaveJson(object data, string filePath)
    {
        if (!IsFileExists(filePath)) return;
        var jsonData = JsonConvert.SerializeObject(data);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    private static bool IsFileExists(string filePath)
    {
        return System.IO.File.Exists(filePath);
    }

    private void SaveData()
    {
        SaveJson(_inventoryModel, _inventoryFilePath);
        SaveJson(_shopModel, _shopFilePath);
    }

    private void PrepareViews()
    {
        _inventoryView.PrepareView(_inventoryModel);
        _shopView.PrepareView(_shopModel);
    }
}
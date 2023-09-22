using Newtonsoft.Json;
using UnityEngine;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;

    private readonly InventoryView _inventoryView;
    private readonly ShopView _shopView;

    private JsonHandler _jsonHandler;

    private readonly string _inventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private readonly string _shopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
    private readonly string _staticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";

    public Presenter(InventoryView inventoryView, ShopView shopView, ShopModel shopModel, InventoryModel inventoryModel, JsonHandler jsonHandler)
    {
        _inventoryView = inventoryView;
        _shopView = shopView;
        _shopModel = shopModel;
        _inventoryModel = inventoryModel;
        _jsonHandler = jsonHandler;

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
        return _staticDataModel.Items.Find(staticItem => staticItem.id == item.id);
    }

    public void LoadData()
    {
        _staticDataModel = _jsonHandler.LoadJson<StaticDataModel>(_staticFileName);
        _inventoryModel = _jsonHandler.LoadJson<InventoryModel>(_inventoryFilePath);
        _shopModel = _jsonHandler.LoadJson<ShopModel>(_shopFilePath);

        PrepareViews();
    }

    private void BuyItem(Item item)
    {
        var itemPrice = GetItemPrice(item);
        
        if (CanAffordItem(item, itemPrice))
        {
            _inventoryModel.money -= itemPrice;
            BuyOneItem(item);
            _inventoryView.UpdateButtons();
            _shopView.UpdateButtons();
            SaveData();
        }
        else
        {
            Debug.LogError("Not enough money to buy this item!");
        }
    }

    private void SellItem(Item item)
    {
        _inventoryModel.money += GetItemPrice(item);
        SellOneItem(item);
        _inventoryView.UpdateButtons();
        _shopView.UpdateButtons();
        SaveData();
    }

    private int GetItemPrice(Item item)
    {
        var staticItemData = GetStaticData(item);
        var itemPrice = staticItemData.price;
        return itemPrice;
    }
    
    private void SaveData()
    {
        _jsonHandler.SaveJson(_inventoryModel, _inventoryFilePath);
        _jsonHandler.SaveJson(_shopModel, _shopFilePath);
    }

    private void PrepareViews()
    {
        _inventoryView.PrepareView(_inventoryModel);
        _shopView.PrepareView(_shopModel);
    }
    
    private bool CanAffordItem(Item item, int itemPrice)
    {
        return _inventoryModel.money >= itemPrice;
    }

    private void BuyOneItem(Item item)
    {
        var existingItem = _inventoryModel.Items.Find(i => i.id == item.id);
        if (existingItem != null)
        {
            existingItem.quantity++;
            _inventoryView.UpdateViewAdd(existingItem, _inventoryModel.money);
        }
        else
        {
            var newItem = new Item
            {
                id = item.id,
                quantity = 1
            };

            _inventoryModel.Items.Add(newItem);
            _inventoryView.UpdateViewAdd(newItem, _inventoryModel.money);
        }

        item.quantity--;
        if (item.quantity < 1)
        {
            _shopModel.Items.Remove(item);
        }

        _shopView.UpdateViewRemove(item);
    }

    private void SellOneItem(Item item)
    {
        var existingItem = _shopModel.Items.Find(i => i.id == item.id);

        if (existingItem != null)
        {
            existingItem.quantity++;
            _shopView.UpdateViewAdd(existingItem);
        }
        else
        {
            var newItem = new Item
            {
                id = item.id,
                quantity = 1
            };

            _shopModel.Items.Add(newItem);
            _shopView.UpdateViewAdd(newItem);
        }

        item.quantity--;

        if (item.quantity < 1)
        {
            _inventoryModel.Items.Remove(item);
        }

        _inventoryView.UpdateViewRemove(item, _inventoryModel.money);
    }
}
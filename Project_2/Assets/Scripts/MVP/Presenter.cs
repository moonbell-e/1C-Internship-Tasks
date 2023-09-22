using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;

    private readonly InventoryView _inventoryView;
    private readonly ShopView _shopView;

    private readonly string _inventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private readonly string _shopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
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
        return _staticDataModel.Items.Find(staticItem => staticItem.id == item.id);
    }

    public void LoadData()
    {
        _staticDataModel = JsonHandler.LoadJson<StaticDataModel>(_staticFileName);
        _inventoryModel = JsonHandler.LoadJson<InventoryModel>(_inventoryFilePath);
        _shopModel = JsonHandler.LoadJson<ShopModel>(_shopFilePath);

        PrepareViews();
    }

    private void SaveData()
    {
        JsonHandler.SaveJson(_inventoryModel, _inventoryFilePath);
        JsonHandler.SaveJson(_shopModel, _shopFilePath);
    }

    private void BuyItem(Item item)
    {
        var itemPrice = GetItemPrice(item);
        if (CanAffordItem(itemPrice))
        {
            _inventoryModel.money -= itemPrice;
            BuyItemOrPackOfItems(item);
            UpdateShopItemAfterPurchase(item);
            UpdateButtons();
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
        UpdateButtons();
        SaveData();
    }
    
    private int GetItemPrice(Item item)
    {
        var staticItemData = GetStaticData(item);
        var itemPrice = staticItemData.price;
        return itemPrice;
    }

    private bool CanAffordItem(int itemPrice)
    {
        return _inventoryModel.money >= itemPrice;
    }

    private void BuyItemOrPackOfItems(Item item)
    {
        var staticData = GetStaticData(item);
        if (staticData.content is { Count: > 0 })
        {
            foreach (var contentItem in staticData.content)
            {
                BuyOneItem(contentItem);
            }
        }
        else
        {
            BuyOneItem(item);
        }
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

        UpdateInventoryItemAfterPurchase(item);
    }

    private void PrepareViews()
    {
        _inventoryView.PrepareView(_inventoryModel);
        _shopView.PrepareView(_shopModel);
    }

    private void UpdateButtons()
    {
        _inventoryView.UpdateButtons();
        _shopView.UpdateButtons();
    }
    
    private void UpdateShopItemAfterPurchase(Item item)
    {
        item.quantity--;
        if (item.quantity < 1)
        {
            _shopModel.Items.Remove(item);
        }

        _shopView.UpdateViewRemove(item);
    }

    private void UpdateInventoryItemAfterPurchase(Item item)
    {
        item.quantity--;
        if (item.quantity < 1)
        {
            _inventoryModel.Items.Remove(item);
        }

        _inventoryView.UpdateViewRemove(item, _inventoryModel.money);
    }

}
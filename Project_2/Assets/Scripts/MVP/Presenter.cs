using System.Collections.Generic;
using UnityEngine;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;

    private readonly InventoryView _inventoryView;
    private readonly ShopView _shopView;

    private readonly LootboxPresenter _lootboxPresenter;
    private readonly LootboxView _lootboxView;

    public Presenter(InventoryView inventoryView, ShopView shopView, ShopModel shopModel, InventoryModel inventoryModel,
        LootboxView lootboxView, LootboxPresenter lootboxPresenter)
    {
        _inventoryView = inventoryView;
        _shopView = shopView;

        _shopModel = shopModel;
        _inventoryModel = inventoryModel;

        _lootboxPresenter = lootboxPresenter;
        _lootboxView = lootboxView;

        SubscribeToEvents();
    }

    ~Presenter()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        _inventoryView.SellButtonClicked += SellItem;
        _shopView.BuyButtonClicked += BuyItem;
        _lootboxView.TakeItemsButtonClicked += AddLootboxItems;
    }

    private void UnsubscribeFromEvents()
    {
        _inventoryView.SellButtonClicked -= SellItem;
        _shopView.BuyButtonClicked -= BuyItem;
        _lootboxView.TakeItemsButtonClicked -= AddLootboxItems;
    }

    public ItemStaticData GetItemStaticData(Item item)
    {
        return _staticDataModel.Items.Find(staticItem => staticItem.id == item.id);
    }

    public void LoadData()
    {
        SetModelsData();
        LoadStaticData(_inventoryModel);
        LoadStaticData(_shopModel);

        PrepareViews();
    }

    public void SetFirstLootboxForView()
    {
        if (_inventoryModel.IsAnyLootboxes())
            _lootboxView.SetCurrentLootbox(_inventoryModel.GetFirstLootbox());
    }

    private void SaveData()
    {
        DataHandler.SaveData(_inventoryModel, _shopModel);
    }

    private void SetModelsData()
    {
        _staticDataModel = DataHandler.LoadStaticData();
        _inventoryModel = DataHandler.LoadInventoryData();
        _shopModel = DataHandler.LoadShopData();
    }

    private void LoadStaticData(IModel model)
    {
        foreach (var item in model.Items)
        {
            item.config = GetItemStaticData(item);

            LoadItemPackConfigs(item);
        }
    }

    private void LoadItemPackConfigs(Item item)
    {
        if (item.config.itemPack == null) return;

        foreach (var itemInPack in item.config.itemPack.content)
        {
            itemInPack.config = GetItemStaticData(itemInPack);
        }
    }

    private void AddLootboxItems(List<Item> items, Item lootbox, LootboxType lootboxType)
    {
        for (var i = 0; i < items.Count; i++)
        {
            AddItemToInventory(items[i]);

            if (i > 0 && lootboxType != LootboxType.Multiple)
            {
                _inventoryModel.money -= items[i].config.price;
            }
        }

        UpdateInventoryItemAfterPurchase(lootbox);
        SaveData();
    }

    private void BuyItem(Item item)
    {
        var price = item.config.price;
        if (CanAffordItem(price))
        {
            _inventoryModel.money -= price;
            BuyItemOrPackOfItems(item);
            SaveData();
        }
        else
        {
            Debug.LogError("Not enough money to buy this item!");
        }
    }

    private void SellItem(Item item)
    {
        item.config = GetItemStaticData(item);
        var price = item.config.price;
        _inventoryModel.money += price;
        SellOneItem(item);
        SaveData();
    }

    private static bool IsLootboxItem(Item item) => item.config.lootbox != null;

    private bool CanAffordItem(int itemPrice)
    {
        return _inventoryModel.money >= itemPrice;
    }

    private void BuyItemOrPackOfItems(Item item)
    {
        var itemPack = item.config.itemPack;
        if (itemPack != null)
        {
            foreach (var contentItem in itemPack.content)
            {
                BuyOneItem(contentItem);
            }
        }
        else
        {
            BuyOneItem(item);
        }

        UpdateShopItemAfterPurchase(item);
    }

    private void BuyOneItem(Item item)
    {
        var existingItem = _inventoryModel.Items.Find(i => i.id == item.id);
        if (existingItem != null)
        {
            existingItem.quantity++;
            _inventoryView.UpdateViewAdd(existingItem, _inventoryModel.money);
            CheckLootboxItem(existingItem);
        }
        else
        {
            var newItem = CreateNewItem(item);
            _inventoryModel.Items.Add(newItem);
            _inventoryView.UpdateViewAdd(newItem, _inventoryModel.money);
            CheckLootboxItem(newItem);
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
            var newItem = CreateNewItem(item);
            _shopModel.Items.Add(newItem);
            _shopView.UpdateViewAdd(newItem);
        }

        UpdateInventoryItemAfterPurchase(item);
    }

    private void CheckLootboxItem(Item item)
    {
        if (!IsLootboxItem(item)) return;
        _inventoryView.DeactivateLootboxButton(item);
        _lootboxView.SetCurrentLootbox(item);
    }

    private void PrepareViews()
    {
        _inventoryView.PrepareView(_inventoryModel);
        _shopView.PrepareView(_shopModel);

        SetFirstLootboxForView();
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

    private void AddItemToInventory(Item item)
    {
        _inventoryModel.Items.Add(item);
        _inventoryView.UpdateViewAdd(item);
    }

    private static Item CreateNewItem(Item item)
    {
        return new Item
        {
            id = item.id,
            quantity = 1,
            config = item.config
        };
    }
}
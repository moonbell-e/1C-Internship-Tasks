using UnityEngine;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;
    private StaticDataModel _lootboxesStaticDataModel;

    private readonly InventoryView _inventoryView;
    private readonly ShopView _shopView;

    private readonly LootboxPresenter _lootboxPresenter;
    private readonly LootboxView _lootboxView;

    private readonly string _inventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private readonly string _shopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
    private readonly string _itemsStaticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";

    public Presenter(InventoryView inventoryView, ShopView shopView, ShopModel shopModel, InventoryModel inventoryModel,
        LootboxView lootboxView, LootboxPresenter lootboxPresenter)
    {
        _inventoryView = inventoryView;
        _shopView = shopView;

        _shopModel = shopModel;
        _inventoryModel = inventoryModel;

        _lootboxPresenter = lootboxPresenter;
        _lootboxView = lootboxView;

        _inventoryView.SellButtonClicked += SellItem;
        _shopView.BuyButtonClicked += BuyItem;
    }

    ~Presenter()
    {
        _inventoryView.SellButtonClicked += SellItem;
        _shopView.BuyButtonClicked -= BuyItem;
    }

    private ItemStaticData GetItemStaticData(Item item)
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

    private void SaveData()
    {
        JsonHandler.SaveJson(_inventoryModel, _inventoryFilePath);
        JsonHandler.SaveJson(_shopModel, _shopFilePath);
    }

    private void SetModelsData()
    {
        _staticDataModel = JsonHandler.LoadJson<StaticDataModel>(_itemsStaticFileName);
        _inventoryModel = JsonHandler.LoadJson<InventoryModel>(_inventoryFilePath);
        _shopModel = JsonHandler.LoadJson<ShopModel>(_shopFilePath);
    }

    private void LoadStaticData(IModel model)
    {
        foreach (var item in model.Items)
        {
            item.config = IsLootboxItem(item) ? _lootboxPresenter.GetLootboxStaticData(item) : GetItemStaticData(item);
        }
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

    private static bool IsLootboxItem(Item item) => item.id.Contains("lootbox");

    private bool CanAffordItem(int itemPrice)
    {
        return _inventoryModel.money >= itemPrice;
    }

    private void BuyItemOrPackOfItems(Item item)
    {
        var staticData = item.config.content;
        if (staticData?.Count > 0 && !IsLootboxItem(item))
        {
            foreach (var contentItem in staticData)
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
            var newItem = new Item
            {
                id = item.id,
                quantity = 1,
                config = item.config
            };
            
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
            var newItem = new Item
            {
                id = item.id,
                quantity = 1,
                config = item.config
            };

            _shopModel.Items.Add(newItem);
            _shopView.UpdateViewAdd(newItem);
        }

        UpdateInventoryItemAfterPurchase(item);
    }

    private void CheckLootboxItem(Item item)
    {
        if (!IsLootboxItem(item)) return;
        _inventoryView.DeactivateLootboxButton(item);
        _lootboxView.AddLootbox(item);
    }

    private void PrepareViews()
    {
        _inventoryView.PrepareView(_inventoryModel);
        _shopView.PrepareView(_shopModel);
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
using UnityEngine;

public class Presenter
{
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private StaticDataModel _staticDataModel;

    private readonly InventoryView _inventoryView;
    private readonly View _shopView;

    private readonly string _inventoryFilePath = $"{Application.dataPath}/Configs/InventoryConfig.json";
    private readonly string _shopFilePath = $"{Application.dataPath}/Configs/ShopConfig.json";
    private readonly string _staticFileName = $"{Application.dataPath}/Configs/ItemsStaticConfig.json";

    public Presenter(InventoryView inventoryView, View shopView, ShopModel shopModel, InventoryModel inventoryModel)
    {
        _inventoryView = inventoryView;
        _shopView = shopView;
        _shopModel = shopModel;
        _inventoryModel = inventoryModel;

        _inventoryView.ButtonClicked += OnButtonClicked;
        _shopView.ButtonClicked += OnButtonClicked;
    }

    ~Presenter()
    {
        _inventoryView.ButtonClicked -= OnButtonClicked;
        _shopView.ButtonClicked -= OnButtonClicked;
    }

    public ItemStaticData GetStaticData(Item item)
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
        _staticDataModel = JsonHandler.LoadJson<StaticDataModel>(_staticFileName);
        _inventoryModel = JsonHandler.LoadJson<InventoryModel>(_inventoryFilePath);
        _shopModel = JsonHandler.LoadJson<ShopModel>(_shopFilePath);
    }

    private void LoadStaticData(IModel model)
    {
        foreach (var item in model.Items)
        {
            item.config = GetStaticData(item);
        }
    }

    private void OnButtonClicked(Item item, ButtonType buttonType)
    {
        if (buttonType == ButtonType.Buy)
        {
            BuyItem(item);
        }
        else
        {
            SellItem(item);
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
        item.config = GetStaticData(item);
        var price = item.config.price;
        _inventoryModel.money += price;
        SellOneItem(item);
        SaveData();
    }

    private bool CanAffordItem(int itemPrice)
    {
        return _inventoryModel.money >= itemPrice;
    }

    private void BuyItemOrPackOfItems(Item item)
    {
        var staticData = item.config.content;
        if (staticData?.Count > 0)
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
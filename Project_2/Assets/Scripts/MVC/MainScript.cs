using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private string _inventoryConfigPath;
    [SerializeField] private string _shopItemsConfigPath;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private ShopController _shopController;
    private ShopModel _shopModel;

    private InventoryController _inventoryController;
    private InventoryModel _inventoryModel;


    private void Start()
    {
        InitializeInventory();
        InitializeShop();
    }

    private void InitializeInventory()
    {
        _inventoryModel = new InventoryModel();
        _inventoryController = new InventoryController(_inventoryView, _inventoryModel, _inventoryConfigPath);
        _inventoryController.LoadData();
        AddListenersToButtons();
    }

    private void SelectItem()
    {
    }

    private void InitializeShop()
    {
        _shopModel = new ShopModel();
        _shopController = new ShopController(_shopView, _shopModel, _shopItemsConfigPath);
        _shopController.LoadData();
    }

    private void AddListenersToButtons()
    {
        var item = new Item();

        _buyButton.onClick.AddListener(() => { _inventoryController.BuyItem(item); });

        _sellButton.onClick.AddListener(() => { _inventoryController.SellItem(item); });
    }
}
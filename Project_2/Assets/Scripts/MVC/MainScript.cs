using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private ShopController _shopController;
    private ShopModel _shopModel;

    private InventoryController _inventoryController;
    private InventoryModel _inventoryModel;
    private int _currentItemId;

    private const string InventoryConfigPath = "InventoryConfig.json";
    private const string ShopItemsConfigPath = "ItemsConfig.json";


    private void Start()
    {
        InitializeInventory();
        InitializeShop();
        AddListenersToButtons();
    }

    private void Update()
    {
        SelectItem();
    }

    private void BuyCurrentItem()
    {
        var item = _shopController.Model.items[_currentItemId];
        if (_inventoryController.Model.money <= item.price) return;
        _inventoryController.BuyItem(item);
        _shopController.SellItem(item);
    }

    private void SellCurrentItem()
    {
        if (IsSellAvailable())
        {
            var item = _inventoryController.Model.items[_currentItemId];
            _inventoryController.SellItem(item);
            _shopController.BuyItem(item);
        }
    }

    private void SelectItem()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var data = PointerRaycast(Input.mousePosition);

        if (data.transform.IsChildOf(_shopView.GridLayout) || data.transform.IsChildOf(_inventoryView.GridLayout))
        {
            _currentItemId = data.transform.GetSiblingIndex();
        }
    }

    private static GameObject PointerRaycast(Vector2 position)
    {
        var pointerData = new PointerEventData(EventSystem.current);
        var resultsData = new List<RaycastResult>();
        pointerData.position = position;
        EventSystem.current.RaycastAll(pointerData, resultsData);

        return resultsData.Count > 0 ? resultsData[0].gameObject : null;
    }

    private void InitializeInventory()
    {
        _inventoryModel = new InventoryModel();
        _inventoryController = new InventoryController(_inventoryView, _inventoryModel, InventoryConfigPath);
        _inventoryController.LoadData();
    }

    private void InitializeShop()
    {
        _shopModel = new ShopModel();
        _shopController = new ShopController(_shopView, _shopModel, ShopItemsConfigPath);
        _shopController.LoadData();
    }

    private void AddListenersToButtons()
    {
        _buyButton.onClick.AddListener(BuyCurrentItem);
        _sellButton.onClick.AddListener(SellCurrentItem);
    }

    private bool IsSellAvailable() => (_inventoryController.Model.items.Count > 0 &&
                                      _inventoryController.Model.items.Count >= _currentItemId);
}
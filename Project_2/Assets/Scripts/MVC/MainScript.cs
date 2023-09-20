using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private string _inventoryConfigPath;
    [SerializeField] private string _shopItemsConfigPath;
    
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private void Awake()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        var inventoryModel = new InventoryModel();
        var inventoryController = new InventoryController(_inventoryView, inventoryModel, _inventoryConfigPath);
        inventoryController.LoadData();
        AddListenersToButtons(inventoryController);
    }

    private void SelectItem()
    {
        
    }

    private void AddListenersToButtons(InventoryController inventoryController)
    {
        Item item = new Item();
        
        _buyButton.onClick.AddListener(() =>
        {
            inventoryController.BuyItem(item);
        });
        
        _sellButton.onClick.AddListener(() =>
        {
            inventoryController.SellItem(item);
        });
    }
    
}

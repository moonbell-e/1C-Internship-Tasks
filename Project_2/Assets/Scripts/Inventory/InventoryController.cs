using UnityEngine;

public class InventoryController : Controller
{
    private InventoryView _inventoryView;
    private InventoryModel _inventoryModel;
    private string _fileName;
    
    public InventoryController(InventoryView view, InventoryModel model, string fileName) : base(view, model, fileName)
    {
        _inventoryView = view;
        _inventoryModel = model;
        _fileName = fileName;
    }

    public override void LoadData()
    {
        _inventoryModel = LoadJson<InventoryModel>();
    }

    public override void SaveData()
    {
        SaveJson(_inventoryModel);
    }

    public override void BuyItem(Item item)
    {
        _inventoryModel.items.Add(item);
        _inventoryModel.money -= item.price;
        SaveData();
    }

    public override void SellItem(Item item)
    {
        _inventoryModel.items.Remove(item);
        _inventoryModel.money += item.price;
        SaveData();
    }
}

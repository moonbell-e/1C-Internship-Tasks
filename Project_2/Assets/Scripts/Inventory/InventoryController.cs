public class InventoryController : Controller<InventoryModel>
{
    private readonly InventoryView _inventoryView;
    private InventoryModel _inventoryModel;
    
    public InventoryController(InventoryView view, InventoryModel model, string fileName) : base(view, model, fileName)
    {
        _inventoryView = view;
        _inventoryModel = model;
    }

    public override void LoadData()
    {
        _inventoryModel = LoadJson();
        _inventoryView.UpdateView(_inventoryModel);
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

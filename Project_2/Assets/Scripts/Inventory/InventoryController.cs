public class InventoryController : Controller<InventoryModel>
{
    private readonly InventoryView _inventoryView;

    public InventoryModel Model { get; private set; }

    public InventoryController(InventoryView view, InventoryModel model, string fileName) : base(view, model, fileName)
    {
        _inventoryView = view;
        Model = model;
    }

    public override void LoadData()
    {
        Model = LoadJson();
        _inventoryView.PrepareView(Model);
    }

    protected override void SaveData(Item item)
    {
        SaveJson(Model);
    }

    public override void BuyItem(Item item)
    {
        Model.items.Add(item);
        Model.money -= item.price;

        var moneyValue = Model.money;
        _inventoryView.UpdateViewAdd(item, moneyValue);
        SaveData(item);
    }

    public override void SellItem(Item item)
    {
        Model.items.Remove(item);
        Model.money += item.price;
        
        var moneyValue = Model.money;
        _inventoryView.UpdateViewAdd(item, moneyValue);
        SaveData(item);
    }
}
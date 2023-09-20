public class ShopController : Controller<ShopModel>
{
    private readonly ShopView _shopView;
    public ShopModel Model { get; private set; }

    public ShopController(ShopView view, ShopModel model, string fileName) : base(view, model, fileName)
    {
        _shopView = view;
        Model = model;
    }

    public override void LoadData()
    {
        Model = LoadJson();
        _shopView.PrepareView(Model);
    }

    protected override void SaveData(Item item)
    {
        SaveJson(Model);
    }

    public override void BuyItem(Item item)
    {
        Model.items.Add(item);
        _shopView.UpdateViewAdd(item);
        SaveData(item);
    }

    public override void SellItem(Item item)
    {
        Model.items.Remove(item);
        _shopView.UpdateViewRemove(item);
        SaveData(item);
    }
}
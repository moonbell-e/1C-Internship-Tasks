public class ShopController : Controller<ShopModel>
{
    private readonly ShopView _shopView;
    private ShopModel _shopModel;
    
    public ShopController(ShopView view, ShopModel model, string fileName) : base(view, model, fileName)
    {
        _shopView = view;
        _shopModel = model;
    }

    public override void LoadData()
    {
        _shopModel = LoadJson();
        _shopView.UpdateView(_shopModel);
    }

    public override void SaveData()
    {
        SaveJson(_shopModel);
    }

    public override void BuyItem(Item item)
    {   
        //
    }

    public override void SellItem(Item item)
    {
        //
    }
}

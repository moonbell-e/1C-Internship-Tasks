public class ShopView : View<ShopModel>
{
    public override void PrepareView(ShopModel model)
    {
        PrepareItemsUI(model.items);
    }
}
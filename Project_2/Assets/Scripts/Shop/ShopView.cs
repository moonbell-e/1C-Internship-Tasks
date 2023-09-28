using System;

public class ShopView : View
{
    public event Action<Item> BuyButtonClicked;

    protected override void HandleButtonClick(Item item)
    {
        BuyButtonClicked?.Invoke(item);
    }
}

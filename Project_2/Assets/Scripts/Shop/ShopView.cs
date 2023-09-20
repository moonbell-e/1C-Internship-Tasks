using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopView : View<ShopModel>
{
    public override void UpdateView(ShopModel model)
    {
        UpdateItemsUI(model.items);
    }
}
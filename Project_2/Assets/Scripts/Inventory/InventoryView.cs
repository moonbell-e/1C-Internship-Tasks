using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryView : View<InventoryModel>
{
    [SerializeField] private TextMeshProUGUI _moneyValue;

    public override void UpdateView(InventoryModel model)
    {
        _moneyValue.text = model.money.ToString();
        UpdateItemsUI(model.items);
    }
}
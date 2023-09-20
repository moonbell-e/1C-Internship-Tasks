using TMPro;
using UnityEngine;

public class InventoryView : View<InventoryModel>
{
    [SerializeField] private TextMeshProUGUI _moneyValue;

    protected internal void UpdateViewAdd(Item item, InventoryModel model)
    {
        base.UpdateViewAdd(item);
        _moneyValue.text = model.money.ToString();
    }

    protected internal void UpdateViewRemove(Item item, InventoryModel model)
    {
        base.UpdateViewRemove(item);
        _moneyValue.text = model.money.ToString();
    }

    public override void PrepareView(InventoryModel model)
    {
        PrepareItemsUI(model.items);
        _moneyValue.text = model.money.ToString();
    }
}
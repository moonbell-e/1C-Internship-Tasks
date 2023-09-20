using TMPro;
using UnityEngine;

public class InventoryView : View<InventoryModel>
{
    [SerializeField] private TextMeshProUGUI _moneyValue;

    public void UpdateViewAdd(Item item, int moneyValue)
    {
        base.UpdateViewAdd(item);
        _moneyValue.text = moneyValue.ToString();
    }

    public void UpdateViewRemove(Item item, int moneyValue)
    {
        base.UpdateViewRemove(item);
        _moneyValue.text = moneyValue.ToString();_moneyValue.text = moneyValue.ToString();
    }
    
    public override void PrepareView(InventoryModel model)
    {
        PrepareItemsUI(model.items);
        _moneyValue.text = model.money.ToString();
    }
}
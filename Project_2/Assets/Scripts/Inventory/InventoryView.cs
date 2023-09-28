using System;
using TMPro;
using UnityEngine;

public class InventoryView : View
{
    public event Action<Item> SellButtonClicked; 
    
    [SerializeField] private TextMeshProUGUI _moneyValue;

    public void UpdateViewAdd(Item item, int moneyValue)
    {
        base.UpdateViewAdd(item);
        _moneyValue.text = moneyValue.ToString();
    }

    public void UpdateViewRemove(Item item, int moneyValue)
    {
        base.UpdateViewRemove(item);
        _moneyValue.text = moneyValue.ToString();
    }

    public void PrepareView(InventoryModel model)
    {
        PrepareItemsUI(model.Items);
        _moneyValue.text = model.money.ToString();
    }

    public void DeactivateLootboxButton(Item item)
    {
        var button = itemUIObjects[item].GetItemButton();
        button.onClick.RemoveAllListeners();
    }

    protected override void HandleButtonClick(Item item)
    {
        SellButtonClicked?.Invoke(item);
    }
}
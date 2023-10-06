using System;
using TMPro;
using UnityEngine;

public class InventoryView : View
{
    public event Action<Item> SellButtonClicked; 
    
    [SerializeField] private TextMeshProUGUI _moneyValue;
    [SerializeField] private LootboxView _lootboxView;

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
    
    protected override void HandleButtonClick(Item item)
    {
        if (item.config.lootbox != null)
        {
            _lootboxView.HandleOpenButtonClick(item);
        }
        else
        {
            SellButtonClicked?.Invoke(item);
        }
    }
}
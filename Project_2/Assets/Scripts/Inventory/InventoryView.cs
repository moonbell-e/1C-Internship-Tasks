using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    public event Action<Item> OnSellButtonClicked;
    
    [SerializeField] private TextMeshProUGUI _moneyValue;

    private void Start()
    {
        UpdateButtons();
    }
    
    public void UpdateButtons()
    {
        foreach (var item in itemUIObjects)
        {
            if (IsHasListeners(item.Value.GetComponent<Button>())) continue;
            
            var button = item.Value.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => HandleButtonClick(item.Key, OnSellButtonClicked));
        }
    }

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
}
using System;
using UnityEngine.UI;

public class ShopView : View
{
    public event Action<Item> OnBuyButtonClicked;

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
            button.onClick.AddListener(() => BuyButtonClick(item.Key));
        }
    }

    private void BuyButtonClick(Item item)
    {
        OnBuyButtonClicked?.Invoke(item);
    }

    private static bool IsHasListeners(Button button)
    {
        return button.onClick.GetPersistentEventCount() > 0;
    }
}
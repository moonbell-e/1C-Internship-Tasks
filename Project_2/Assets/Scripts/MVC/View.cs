using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class View<T> : MonoBehaviour where T : Model
{
    [SerializeField] protected GameObject _gridLayout;
    [SerializeField] protected GameObject _itemPrefab;

    public abstract void UpdateView(T model);

    protected void UpdateItemsUI(List<Item> items)
    {
        foreach (var item in items)
        {
            Object itemUI = Instantiate(_itemPrefab, _gridLayout.transform);

            var textComponents = itemUI.GetComponentsInChildren<TextMeshProUGUI>();

            if (textComponents.Length >= 3)
            {
                var itemNameText = textComponents[0];
                var itemPriceText = textComponents[1];
                var itemQuantityText = textComponents[2];

                itemNameText.text = item.name;
                var priceText = string.Format("{0} руб.", item.price);
                itemPriceText.text = priceText;
                itemQuantityText.text = item.quantity.ToString();
            }
            else
            {
                Debug.LogWarning("Not enough TextMeshProUGUI components found in the item UI.");
            }
        }
    }
}
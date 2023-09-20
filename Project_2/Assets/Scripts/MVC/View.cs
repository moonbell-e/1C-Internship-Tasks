using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class View<T> : MonoBehaviour where T : Model
{
    [SerializeField] protected Transform _gridLayout;
    [SerializeField] protected GameObject _itemPrefab;
    public Transform GridLayout => _gridLayout;

    private readonly Dictionary<Item, GameObject> _itemUIObjects = new();

    public abstract void PrepareView(T model);

    protected internal void UpdateViewAdd(Item item)
    {
        UpdateItemsUI(item);
    }

    protected internal void UpdateViewRemove(Item item)
    {
        var go = _itemUIObjects[item];
        _itemUIObjects.Remove(item);
        Destroy(go);
    }

    protected void PrepareItemsUI(List<Item> items)
    {
        _itemUIObjects.Clear();

        foreach (var item in items)
        {
            UpdateItemsUI(item);
        }
    }

    private void UpdateItemsUI(Item item)
    {
        var itemUI = Instantiate(_itemPrefab, _gridLayout);
        HandleTextValues(itemUI, item);
        _itemUIObjects[item] = itemUI;
    }

    private static void HandleTextValues(GameObject itemUI, Item item)
    {
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
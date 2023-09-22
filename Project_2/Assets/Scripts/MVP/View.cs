using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{
    [SerializeField] private Transform _gridLayout;
    [SerializeField] protected GameObject _itemPrefab;

    private Presenter _presenter;
    protected readonly Dictionary<Item, GameObject> itemUIObjects = new();
    
    public void Init(Presenter presenter)
    {
        _presenter = presenter;
    }

    public void PrepareView(IModel model)
    {
        PrepareItemsUI(model.Items);
    }
    
    protected static bool IsHasListeners(Button button)
    {
        return button.onClick.GetPersistentEventCount() > 0;
    }
    
    protected static void HandleButtonClick(Item item, Action<Item> buttonAction)
    {
        buttonAction?.Invoke(item);
    }

    protected internal void UpdateViewAdd(Item item)
    {
        if (itemUIObjects.ContainsKey(item))
        {
            UpdateItemsUI(item);
        }
        else
        {
            CreateItemUI(item);
        }
    }

    protected internal void UpdateViewRemove(Item item)
    {
        var go = itemUIObjects[item];
        if (item.quantity > 0)
        {
            HandleTextValues(go, item);
        }
        else
        {
            itemUIObjects.Remove(item);
            Destroy(go);
        }
    }

    protected void PrepareItemsUI(List<Item> items)
    {
        itemUIObjects.Clear();

        foreach (var item in items)
        {
            UpdateItemsUI(item);
        }
    }
    
    private void UpdateItemsUI(Item item)
    {
        if (!itemUIObjects.ContainsKey(item))
        {
            CreateItemUI(item);
        }
        else
        {
            var existingUI = itemUIObjects[item];
            HandleTextValues(existingUI, item);
        }
    }
    
    private void CreateItemUI(Item item)
    {
        var itemUI = Instantiate(_itemPrefab, _gridLayout);
        HandleTextValues(itemUI, item);
        itemUIObjects[item] = itemUI;
    }
    
    private void HandleTextValues(GameObject itemUI, Item item)
    {
        var textData = itemUI.GetComponent<ItemTextData>();
        var staticData = _presenter.GetStaticData(item);
        textData.SetItemTextData(staticData.name, staticData.price, item.quantity);
    }
}
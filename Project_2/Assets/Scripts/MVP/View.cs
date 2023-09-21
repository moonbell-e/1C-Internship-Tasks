using System;
using System.Collections.Generic;
using UnityEngine;

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

    protected internal void UpdateViewAdd(Item item)
    {
        UpdateItemsUI(item);
    }

    protected internal void UpdateViewRemove(Item item)
    {
        var go = itemUIObjects[item];
        itemUIObjects.Remove(item);
        Destroy(go);
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
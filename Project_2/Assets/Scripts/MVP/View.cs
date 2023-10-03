using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] private Transform _gridLayout;
    [SerializeField] private GameObject _itemPrefab;

    protected readonly Dictionary<Item, ItemTextData> itemUIObjects = new();
    private Presenter _presenter;

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
        var textData = itemUIObjects[item];
        if (item.quantity > 0)
        {
            HandleTextValues(textData, item);
        }
        else
        {
            itemUIObjects.Remove(item);
            Destroy(textData.gameObject);
        }
    }

    protected void PrepareItemsUI(List<Item> items)
    {
        foreach (var item in items)
        {
            CreateItemUI(item);
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
        var itemUI = Instantiate(_itemPrefab, _gridLayout).GetComponent<ItemTextData>();
        itemUIObjects[item] = itemUI;
        AddButtonListener(itemUI, item);
        HandleTextValues(itemUIObjects[item], item);
    }

    private void HandleTextValues(ItemTextData textData, Item item)
    {
        var staticData = _presenter.GetItemStaticData(item);
        textData.SetItemTextData(staticData.name, staticData.price, item.quantity);
    }

    private void AddButtonListener(ItemTextData itemTextData, Item item)
    {
        var button = itemTextData.GetItemButton();
        button.onClick.AddListener(() => HandleButtonClick(item));
    }

    protected abstract void HandleButtonClick(Item item);
}
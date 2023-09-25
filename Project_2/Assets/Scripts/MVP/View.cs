using System;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public event Action<Item, ButtonType> ButtonClicked;

    [SerializeField] private Transform _gridLayout;
    [SerializeField] protected GameObject _itemPrefab;
    [SerializeField] private ButtonType _buttonType;

    private Presenter _presenter;
    private readonly Dictionary<Item, ItemTextData> _itemUIObjects = new();

    public Transform GridLayout => _gridLayout;

    public void Init(Presenter presenter)
    {
        _presenter = presenter;
    }

    public void PrepareView(IModel model)
    {
        PrepareItemsUI(model.Items);
    }

    private void HandleButtonClick(Item item)
    {
        ButtonClicked?.Invoke(item, _buttonType);
    }

    protected internal void UpdateViewAdd(Item item)
    {
        if (_itemUIObjects.ContainsKey(item))
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
        var textData = _itemUIObjects[item];
        if (item.quantity > 0)
        {
            HandleTextValues(textData, item);
        }
        else
        {
            _itemUIObjects.Remove(item);
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
        if (!_itemUIObjects.ContainsKey(item))
        {
            CreateItemUI(item);
        }
        else
        {
            var existingUI = _itemUIObjects[item];
            HandleTextValues(existingUI, item);
        }
    }

    private void CreateItemUI(Item item)
    {
        var itemUI = Instantiate(_itemPrefab, _gridLayout).GetComponent<ItemTextData>();
        _itemUIObjects[item] = itemUI;
        AddListener(itemUI, item);
        HandleTextValues(_itemUIObjects[item], item);
    }

    private void HandleTextValues(ItemTextData textData, Item item)
    {
        var staticData = _presenter.GetStaticData(item);
        textData.SetItemTextData(staticData.name, staticData.price, item.quantity);
    }

    private void AddListener(ItemTextData itemTextData, Item item)
    {
        var button = itemTextData.GetItemButton();
        button.onClick.AddListener(() => HandleButtonClick(item));
    }
}
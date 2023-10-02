using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxView : View
{
    public event Action<List<Item>, Item, string> TakeItemsButtonClicked;

    [SerializeField] private Button _openButton;
    [SerializeField] private Button _takeItemsButton;
    [SerializeField] private GameObject _lootboxPanel;

    private readonly List<GameObject> _lootboxItemsUI = new();
    private List<Item> _itemsFromLootbox = new();
    private List<Item> _itemsToReturn = new();
    private Item _currentLootbox;

    private void Start()
    {
        _openButton.onClick.AddListener(HandleOpenButtonClick);
        _takeItemsButton.onClick.AddListener(HandleTakeItemsButtonClick);
    }

    public void ShowOpenButton(bool isShow)
    {
        _openButton.gameObject.SetActive(isShow);
    }

    public void SetCurrentLootbox(Item item)
    {
        _currentLootbox = item;
        _openButton.gameObject.SetActive(true);
    }

    private void HandleOpenButtonClick()
    {
        _itemsFromLootbox = LootboxPresenter.OpenLootbox(_currentLootbox);

        _lootboxPanel.SetActive(true);
        _openButton.gameObject.SetActive(false);

        foreach (var item in _itemsFromLootbox)
        {
            CreateItemUI(item);
            _lootboxItemsUI.Add(itemUIObjects[item].gameObject);
        }

        var lootboxTypeHandler = GetLootboxTypeHandler(_currentLootbox.config.lootbox.type);
        lootboxTypeHandler.HandleItems(_itemsFromLootbox, _itemsToReturn, itemUIObjects);
    }

    private void HandleTakeItemsButtonClick()
    {
        var lootboxTypeHandler = GetLootboxTypeHandler(_currentLootbox.config.lootbox.type);
        lootboxTypeHandler.HandleTakeItemsButtonClicked(TakeItemsButtonClicked, _itemsFromLootbox, _currentLootbox,
            _currentLootbox.config.lootbox.type);

        _itemsFromLootbox = new List<Item>();
        _itemsToReturn = new List<Item>();
        UpdateGridView();
    }

    protected override void HandleButtonClick(Item item)
    {
        _itemsToReturn.Add(item);
        itemUIObjects[item].GetItemButton().interactable = false;
    }

    private void UpdateGridView()
    {
        foreach (var itemUI in _lootboxItemsUI)
        {
            Destroy(itemUI);
        }

        _lootboxPanel.SetActive(false);
        _lootboxItemsUI.Clear();
    }

    private static ILootboxType GetLootboxTypeHandler(string lootboxType)
    {
        switch (lootboxType)
        {
            case "multiple":
                return new MultipleLootboxType();
            case "single":
                return new SingleLootboxType();
            default:
                return null;
        }
    }
}
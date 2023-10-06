using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootboxView : MonoBehaviour
{
    public event Action<List<Item>, Item, LootboxType> TakeItemsButtonClicked;

    [SerializeField] private Transform _gridLayout;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _lootboxPanel;
    [SerializeField] private Button _takeItemsButton;

    private readonly List<GameObject> _lootboxItemsUI = new();
    private readonly Dictionary<Item, ItemTextData> _itemsTextData = new();
    private LootboxPresenter _lootboxPresenter;
    private Presenter _presenter;
    private Item _currentLootbox;

    public void Init(LootboxPresenter lootboxPresenter, Presenter presenter)
    {
        _lootboxPresenter = lootboxPresenter;
        _presenter = presenter;
    }

    private void Start()
    {
        _takeItemsButton.onClick.AddListener(HandleTakeItemsButtonClick);
    }

    public void SetCurrentLootbox(Item item)
    {
        _currentLootbox = item;
    }

    public void HandleOpenButtonClick(Item item)
    {
        var itemsFromLootbox = _lootboxPresenter.OpenLootbox(item);

        _lootboxPanel.SetActive(true);

        foreach (var lootboxItem in itemsFromLootbox)
        {
            CreateItemUI(lootboxItem);
        }
    }

    private void HandleTakeItemsButtonClick()
    {
        switch (_currentLootbox.config.lootbox.type)
        {
            case LootboxType.Multiple:
                _lootboxPresenter.TakeItemsFromMultipleLootbox(_currentLootbox, TakeItemsButtonClicked);
                break;
            case LootboxType.Single:
                _lootboxPresenter.TakeItemsFromSingleLootbox(_currentLootbox, TakeItemsButtonClicked);
                break;
        }

        UpdateGridView();
    }

    private void UpdateGridView()
    {
        foreach (var itemUI in _lootboxItemsUI)
        {
            Destroy(itemUI);
        }

        _lootboxPanel.SetActive(false);
        _lootboxItemsUI.Clear();
        _itemsTextData.Clear();
        _presenter.SetFirstLootboxForView();
    }

    private void CreateItemUI(Item item)
    {
        var itemUI = Instantiate(_itemPrefab, _gridLayout).GetComponent<ItemTextData>();
        _lootboxItemsUI.Add(itemUI.gameObject);
        _itemsTextData[item] = itemUI;
        AddButtonListener(itemUI, item);
        HandleTextValues(itemUI, item);
    }

    private void HandleTextValues(ItemTextData textData, Item item)
    {
        var staticData = _presenter.GetItemStaticData(item);
        textData.SetItemTextData(staticData.name, staticData.price, item.quantity);
    }

    private void AddButtonListener(ItemTextData itemTextData, Item item)
    {
        var button = itemTextData.GetItemButton();
        if (_currentLootbox.config.lootbox.type != LootboxType.Multiple)
            button.onClick.AddListener(() => HandleButtonClick(item));
    }

    private void HandleButtonClick(Item item)
    {
        _lootboxPresenter.PurchaseItem(item);
        _itemsTextData[item].GetItemButton().interactable = false;
    }
}
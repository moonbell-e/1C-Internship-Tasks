using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxView: View
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _takeItemsButton;
    
    private List<Item> _itemsToReturn;
    private Item _currentLootbox;

    private void Start()
    {
        _openButton.onClick.AddListener(HandleOpenButtonClick);
    }

    public void SetCurrentLootbox(Item item)
    {
        _currentLootbox = item;
    }
    
    public void SetActiveLootboxButton(bool isShow)
    {
        _openButton.gameObject.SetActive(isShow);
    }

    private void HandleOpenButtonClick()
    {
        _itemsToReturn = LootboxPresenter.OpenLootbox(_currentLootbox);
        SetActiveLootboxButton(false);
        PrepareItemsUI(_itemsToReturn);
    }

    protected override void HandleButtonClick(Item item)
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxView: View
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _takeItemsButton;

    private readonly List<Item> _lootboxesUI = new();
    
    private List<Item> _itemsToReturn;
    private Item _currentLootbox;

    private void Start()
    {
        _openButton.onClick.AddListener(HandleOpenButtonClick);
    }

    private void Update()
    {
        _openButton.gameObject.SetActive(IsAnyLootboxes());
    }

    public void AddLootbox(Item  item)
    {
        _lootboxesUI.Add(item);
        _currentLootbox = item;
    }
    

    private bool IsAnyLootboxes()
    {
        return _lootboxesUI.Count > 0;
    }

    private void HandleOpenButtonClick()
    {
        _itemsToReturn = LootboxPresenter.OpenLootbox(_currentLootbox);
        foreach (var variable in _itemsToReturn)
        {
            Debug.Log(variable.id);
            Debug.Log(variable.quantity);
        }
    }

    protected override void HandleButtonClick(Item item)
    {
        throw new System.NotImplementedException();
    }
}

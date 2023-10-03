using UnityEngine;

public class MainScript : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private LootboxView _lootboxView;

    private Presenter _presenter;
    private LootboxPresenter _lootboxPresenter;
    private LootboxModel _lootboxModel;
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private int _currentItemId;

    private void Start()
    {
        InitializePresenter();
        InitializeViews();
        _presenter.LoadData();

        // _presenter.BuyItem(_presenter.ShopModel.Items[6]);
        // _lootboxPresenter.OpenLootbox(_presenter.InventoryModel.Items[0]);
        //
        // foreach (var item in _presenter.InventoryModel.Items)
        // {
        //      Debug.Log(item.id);   
        //      Debug.Log(item.quantity);   
        // }

        _presenter.BuyItem(_presenter.ShopModel.Items[7]);
        _lootboxPresenter.OpenComplexLootbox(_presenter.InventoryModel.Items[0]);
        _lootboxPresenter.PurchaseItem(_lootboxPresenter.LootboxModel.Items[0]);
        _lootboxPresenter.PurchaseItem(_lootboxPresenter.LootboxModel.Items[2]);
        _lootboxPresenter.TakeItemsFromComplexLootbox(_presenter.InventoryModel.Items[0]);
    }

    private void InitializeViews()
    {
        _inventoryView.Init(_presenter);
        _shopView.Init(_presenter);
        _lootboxView.Init(_presenter);
    }

    private void InitializePresenter()
    {
        _inventoryModel = new InventoryModel();
        _shopModel = new ShopModel();
        _lootboxModel = new LootboxModel();
        _lootboxPresenter = new LootboxPresenter(_lootboxModel);
        _presenter = new Presenter(_inventoryView, _shopView, _shopModel, _inventoryModel, _lootboxView,
            _lootboxPresenter);
    }
}
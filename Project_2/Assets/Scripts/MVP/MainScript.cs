using UnityEngine;

public class MainScript : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private View _shopView;

    private Presenter _presenter;
    private InventoryModel _inventoryModel;
    private ShopModel _shopModel;
    private int _currentItemId;

    private void Start()
    {
        InitializePresenter();
        InitializeViews();
        _presenter.LoadData();
    }

    private void InitializeViews()
    {
        _inventoryView.Init(_presenter);
        _shopView.Init(_presenter);
    }

    private void InitializePresenter()
    {
        _inventoryModel = new InventoryModel();
        _shopModel = new ShopModel();
        _presenter = new Presenter(_inventoryView, _shopView, _shopModel, _inventoryModel);
    }
}
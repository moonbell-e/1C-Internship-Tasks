using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTextData : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _quantity;
    
    public void SetItemTextData(string name, int price, int quantity)
    {
        _name.text = name;
        _price.text = $"{price} руб";
        _quantity.text = quantity.ToString();
    }

    public Button GetItemButton()
    {
        return _button;
    }
}
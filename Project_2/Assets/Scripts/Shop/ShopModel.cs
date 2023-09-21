using System.Collections.Generic;

[System.Serializable]
public class ShopModel: IModel
{
    public List<Item> Items { get; set; }
}

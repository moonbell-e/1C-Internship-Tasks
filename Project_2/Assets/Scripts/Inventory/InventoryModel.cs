using System.Collections.Generic;

[System.Serializable]
public class InventoryModel : IModel
{
    public List<Item> Items { get; set; }
    public int money;
}
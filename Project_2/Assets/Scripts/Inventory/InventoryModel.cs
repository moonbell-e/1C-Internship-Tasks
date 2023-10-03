using System;
using System.Collections.Generic;

[Serializable]
public class InventoryModel : IModel
{
    public List<Item> Items { get; set; }
    public int money;
}
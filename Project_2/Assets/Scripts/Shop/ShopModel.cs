using System;
using System.Collections.Generic;

[Serializable]
public class ShopModel : IModel
{
    public List<Item> Items { get; set; } 
}
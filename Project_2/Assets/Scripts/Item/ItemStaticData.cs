using System;
using System.Collections.Generic;

[Serializable]
public class ItemStaticData
{
    public string id;
    public string name;
    public int price;
    public float dropChance;
    public List<Item> content;
}
using System;

[Serializable]
public class Item
{
    public string id;
    public int quantity;
    [NonSerialized] public ItemStaticData config;
}
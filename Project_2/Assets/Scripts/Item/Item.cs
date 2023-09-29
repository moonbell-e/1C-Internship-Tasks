using System;

[Serializable]
public class Item
{
    public string id;
    public int quantity;
    public float dropChance;
    [NonSerialized] public ItemStaticData config;
}
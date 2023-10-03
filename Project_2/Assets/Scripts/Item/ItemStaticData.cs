using System;

[Serializable]
public class ItemStaticData
{
    public string id;
    public string name;
    public int price;
    public Pack itemPack;
    [NonSerialized] public Lootbox lootbox;
}
using System;

[Serializable]
public class ItemStaticData
{
    public string id;
    public string name;
    public int price;
    [NonSerialized] public Lootbox lootbox;
    public Pack itemPack;
}
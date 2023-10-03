using System;
using System.Collections.Generic;

[Serializable]
public class Lootbox
{
    public string id;
    public string type;
    public List<LootboxSlot> slots;
    public List<LootboxItem> content;
}
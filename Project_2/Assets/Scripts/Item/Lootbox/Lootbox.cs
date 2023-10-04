using System;
using System.Collections.Generic;

[Serializable]
public class Lootbox
{
    public string id;
    public LootboxType type;
    public List<LootboxSlot> slots;
    public List<LootboxItem> content;
}
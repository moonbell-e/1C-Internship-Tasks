using System;
using System.Collections.Generic;

[Serializable]
public class LootboxItemStaticData : ItemStaticData
{
    public string type;
    public new List<LootboxItem> content;
}

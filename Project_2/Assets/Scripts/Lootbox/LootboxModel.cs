using System.Collections.Generic;

public class LootboxModel : IModel
{
    public List<Item> Items { get; set; } = new();
    public List<Item> ItemsToReturn { get; set; } = new();
}
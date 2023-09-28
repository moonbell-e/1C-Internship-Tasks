using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Item
{
    public string id;
    public int quantity;
    public ItemStaticData config;
}
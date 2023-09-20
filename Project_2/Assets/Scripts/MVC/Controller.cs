using System;
using Newtonsoft.Json;
using UnityEngine;

public abstract class Controller<T> where T : Model
{
    protected View<T> view;
    protected T model;
    private readonly string _fileName;

    protected Controller(View<T> view, T model, string fileName)
    {
        this.view = view;
        this.model = model;
        _fileName = fileName;
    }

    protected T LoadJson()
    {
        if (IsFileExists() == false) return default;
        var jsonData = System.IO.File.ReadAllText(_fileName);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }


    protected void SaveJson(object data)
    {
        if (IsFileExists() == false) return;
        var jsonData = JsonConvert.SerializeObject(data);
        System.IO.File.WriteAllText(_fileName, jsonData);
    }

    private bool IsFileExists()
    {
        return System.IO.File.Exists(_fileName);
    }


    public abstract void LoadData();

    protected abstract void SaveData(Item item);

    public abstract void BuyItem(Item item);

    public abstract void SellItem(Item item);
}
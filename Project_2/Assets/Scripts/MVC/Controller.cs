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
        var jsonData = System.IO.File.ReadAllText(_fileName);
        return System.IO.File.Exists(_fileName) ? JsonConvert.DeserializeObject<T>(jsonData) : default(T);
    }

    protected void SaveJson(object data)
    {
        var jsonData = JsonConvert.SerializeObject(data);
        System.IO.File.WriteAllText(_fileName, jsonData);
    }
    
    public abstract void LoadData();
    
    public abstract void SaveData();
    
    public abstract void BuyItem(Item item);
    
    public abstract void SellItem(Item item);

}

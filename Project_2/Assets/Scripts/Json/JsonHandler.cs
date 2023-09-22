using Newtonsoft.Json;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{
    public static T LoadJson<T>(string filePath)
    {
        if (!IsFileExists(filePath)) return default;
        var jsonData = System.IO.File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public static void SaveJson(object data, string filePath)
    {
        if (!IsFileExists(filePath)) return;
        var jsonData = JsonConvert.SerializeObject(data);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    private static bool IsFileExists(string filePath)
    {
        return System.IO.File.Exists(filePath);
    }
}

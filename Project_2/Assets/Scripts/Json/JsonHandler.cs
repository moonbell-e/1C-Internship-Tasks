using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{
    public static T LoadJson<T>(string filePath)
    {
        if (!IsFileExists(filePath)) return default;

        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented, 
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy() 
            }
        };

        var jsonData = System.IO.File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(jsonData, settings);
    }

    public static void SaveJson(object data, string filePath)
    {
        if (!IsFileExists(filePath)) return;

        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy() 
            }
        };

        var jsonData = JsonConvert.SerializeObject(data, settings);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    private static bool IsFileExists(string filePath)
    {
        return System.IO.File.Exists(filePath);
    }
}
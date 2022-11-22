using Newtonsoft.Json;
using UnityEngine;

public class PersistentDataManager
{
    PersistentData _data;

    public PersistentData data => _data ??= ParseData();

    const string _key = "SavedData";

    public void Save()
    {
        var jsonString = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    public void Reset()
    {
        PlayerPrefs.SetString(_key, "");
        PlayerPrefs.Save();
        _data = ParseData();
    }

    static PersistentData ParseData()
    {
        var jsonString = PlayerPrefs.GetString(_key);
        return jsonString == ""
            ? new PersistentData()
            : JsonConvert.DeserializeObject<PersistentData>(jsonString);
    }
}
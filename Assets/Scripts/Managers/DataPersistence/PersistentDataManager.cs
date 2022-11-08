using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class PersistentDataManager : IInitializable
{
    [Inject] IEnumerable<IPersistableData> _persistableDatas;

    PersistentData _data;

    public PersistentData data => _data ??= ParseData();

    const string _key = "SavedData";

    public void Initialize()
    {
        LoadObjects();
    }

    public void Save()
    {
        foreach (var dataPersistence in AllPersistableDatas())
        {
            dataPersistence.Save(_data);
        }

        var jsonString = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    public static void Reset()
    {
        PlayerPrefs.SetString(_key, "");
        PlayerPrefs.Save();
    }

    void LoadObjects()
    {
        foreach (var dataPersistence in AllPersistableDatas())
        {
            dataPersistence.Load(data);
        }
    }

    IEnumerable<IPersistableData> AllPersistableDatas()
    {
        var objects = Object
            .FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IPersistableData>()
            .Where(persistableData => !_persistableDatas.Contains(persistableData));

        return _persistableDatas.Concat(objects);
    }

    static PersistentData ParseData()
    {
        var jsonString = PlayerPrefs.GetString(_key);
        return jsonString == ""
            ? new PersistentData()
            : JsonConvert.DeserializeObject<PersistentData>(jsonString);
    }
}
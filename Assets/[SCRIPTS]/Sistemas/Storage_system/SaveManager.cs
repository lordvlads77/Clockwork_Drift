using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/race_records.json";

    [System.Serializable]
    private class RaceRecordList
    {
        public List<RaceRecord> records = new List<RaceRecord>();
    }

    public static void SaveRecord(RaceRecord newRecord)
    {
        RaceRecordList data = LoadAllInternal();
        data.records.Add(newRecord);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static List<RaceRecord> LoadAll()
    {
        return LoadAllInternal().records;
    }

    private static RaceRecordList LoadAllInternal()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<RaceRecordList>(json);
        }
        return new RaceRecordList();
    }

    public static void ClearAll()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
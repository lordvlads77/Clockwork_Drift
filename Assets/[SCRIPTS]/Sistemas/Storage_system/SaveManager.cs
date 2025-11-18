using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RaceResult
{
    public string trackName;
    public float time;
    public int score;
    public string date;
}

[Serializable]
public class RaceResultList
{
    public List<RaceResult> results = new List<RaceResult>();
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

    }

    public void SaveResult(string trackName, float time, int score)
    {
        var list = LoadAllResultsInternal();
        var result = new RaceResult
        {
            trackName = trackName,
            time = time,
            score = score,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        list.results.Add(result);
        try
        {
            string json = JsonUtility.ToJson(list, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Saved result to {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save results: " + e);
        }
    }

    public List<RaceResult> LoadAllResults()
    {
        return LoadAllResultsInternal().results;
    }

    private RaceResultList LoadAllResultsInternal()
    {
        if (!File.Exists(filePath))
        {
            return new RaceResultList();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(json))
                return new RaceResultList();

            return JsonUtility.FromJson<RaceResultList>(json) ?? new RaceResultList();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load results: " + e);
            return new RaceResultList();
        }
    }

    public void ClearAllResults()
    {
        try
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            Debug.Log("Cleared saved scores.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to clear results: " + e);
        }
    }
}

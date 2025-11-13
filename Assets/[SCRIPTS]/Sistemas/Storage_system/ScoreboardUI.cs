using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject recordPrefab;     
    [SerializeField] private Transform contentParent;     
    [SerializeField] private TMP_Dropdown sortDropdown;   

    private List<RaceRecord> records;

    private void Start()
    {
      
        if (sortDropdown != null)
        {
            sortDropdown.ClearOptions();
            sortDropdown.AddOptions(new List<string> { "Por mejor tiempo", "Por más puntos", "Por fecha" });
            sortDropdown.onValueChanged.AddListener(OnSortChanged);
        }

        LoadAndDisplay();
    }

    public void LoadAndDisplay()
    {
        records = SaveManager.LoadAll();
        SortRecords(0); 
    }

    public void OnSortChanged(int index)
    {
        SortRecords(index);
    }

    private void SortRecords(int mode)
    {
        if (records == null || records.Count == 0) return;

        switch (mode)
        {
            case 0:
                records = records.OrderBy(r => r.trackTime).ToList();
                break;
            case 1: 
                records = records.OrderByDescending(r => r.challengePoints).ToList();
                break;
            case 2: 
                records = records.OrderByDescending(r => r.date).ToList();
                break;
        }

        DisplayRecords();
    }

    private void DisplayRecords()
    {
        // Limpiar contenido previo
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var record in records)
        {
            GameObject entry = Instantiate(recordPrefab, contentParent);
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();

            text.text = 
                $"<b>{record.trackName}</b>\n" +
                $"<color=#00FFAA>Tiempo:</color> {record.trackTime:F2}s\n" +
                $"<color=#FFD700>Puntos:</color> {record.challengePoints}\n" +
                $"<size=85%><color=#AAAAAA>{record.date}</color></size>";
        }
    }

    public void ClearAllRecords()
    {
        SaveManager.ClearAll();
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }
}

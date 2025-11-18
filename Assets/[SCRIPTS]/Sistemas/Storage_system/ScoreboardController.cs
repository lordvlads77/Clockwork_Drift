using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject entryPrefab;

    private void Start()
    {
        // DEBUG
        var results = SaveManager.Instance?.LoadAllResults();

        Debug.Log("----------- SCOREBOARD DEBUG ----------");
        Debug.Log("SaveManager.Instance = " + SaveManager.Instance);
        Debug.Log("Resultados cargados = " + (results != null ? results.Count : -1));

        // Si hay resultados, pintarlos
        if (results != null)
        {
            foreach (var r in results)
            {
                var go = Instantiate(entryPrefab, contentPanel);
                go.GetComponent<ScoreEntryUI>().Setup(r);
            }
        }
    }
}
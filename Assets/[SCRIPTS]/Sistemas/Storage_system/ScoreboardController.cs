using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour
{
    [Header("References")]
    public RectTransform contentPanel;     
    public GameObject entryPrefab;         
    public ScrollRect scrollRect;          

    private IEnumerator Start()
    {
        // Esperar un frame para que ScrollView inicialice
        yield return new WaitForEndOfFrame();

        if (SaveManager.Instance == null)
        {
            Debug.LogError("ScoreboardController: SaveManager.Instance es null.");
            yield break;
        }

        // Obtener resultados
        var results = SaveManager.Instance.LoadAllResults();
        Debug.Log("----------- SCOREBOARD DEBUG ----------");
        Debug.Log("SaveManager.Instance = " + SaveManager.Instance);
        Debug.Log("Resultados cargados = " + (results != null ? results.Count : -1));

        // Ordenar por fecha (más reciente primero)
        results.Sort((a, b) =>
            System.DateTime.Parse(b.date).CompareTo(System.DateTime.Parse(a.date))
        );

        // Limpiar el Content
        for (int i = contentPanel.childCount - 1; i >= 0; i--)
            Destroy(contentPanel.GetChild(i).gameObject);

        if (results == null || results.Count == 0)
        {
            Debug.Log("No hay resultados para mostrar.");
            yield break;
        }

        // Crear entradas
        foreach (var r in results)
        {
            GameObject go = Instantiate(entryPrefab, contentPanel, false);

            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt != null)
                rt.localScale = Vector3.one;

            ScoreEntryUI entryUI = go.GetComponent<ScoreEntryUI>();
            if (entryUI != null)
                entryUI.Setup(r);
        }
        
        yield return null; // esperar 1 frame más
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);

        yield return null; // esperar 1 frame más
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);

        // Posicionar el scroll en la parte superior
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
            Debug.Log("ScrollRect seteado a top.");
        }
        else
        {
            Debug.LogWarning("ScoreboardController: ScrollRect NO asignado en inspector.");
        }
        
    }
    
    public void OnClickClearScores()
    {
        // Borra el archivo JSON y todos los resultados
        SaveManager.Instance.ClearAllResults();

        // Recargar la escena actual (Scoreboard)
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}

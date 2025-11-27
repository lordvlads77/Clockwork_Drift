using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ScrollRect
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ScoreboardController : MonoBehaviour
{
    [Header("References")]
    public RectTransform contentPanel;     // asignar: ScrollView/Viewport/Content (RectTransform)
    public GameObject entryPrefab;         // asignar: ScoreEntryPrefab
    public ScrollRect scrollRect;          // asignar: Scroll_View_score (ScrollRect)

    private IEnumerator Start()
    {
        
        yield return null;

        if (SaveManager.Instance == null)
        {
            Debug.LogError("ScoreboardController: SaveManager.Instance es null.");
            yield break;
        }

        var results = SaveManager.Instance.LoadAllResults();
        Debug.Log("----------- SCOREBOARD DEBUG ----------");
        Debug.Log("SaveManager.Instance = " + SaveManager.Instance);
        Debug.Log("Resultados cargados = " + (results != null ? results.Count : -1));

        // limpia hijos previos
        for (int i = contentPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }

        if (results == null || results.Count == 0)
        {
            Debug.Log("No hay resultados para mostrar.");
            yield break;
        }

        // Instanciar Entries
        foreach (var r in results)
        {
            GameObject go = Instantiate(entryPrefab);
            go.transform.SetParent(contentPanel, false); // importante: false para mantener rect transform
            // asegurar escala y anchura correctas
            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
            }
            var entryUI = go.GetComponent<ScoreEntryUI>();
            if (entryUI != null)
                entryUI.Setup(r);

            Debug.Log("Instancié: " + go.name);
        }

        // Forzar actualización del layout y canvas para que Unity calcule tamaños
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);

        // Mover scroll al top (mejor tiempo primero)
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
}

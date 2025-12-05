using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LeaderboardBackController : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;

    public void OnBackToVictoryPanel()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
        
        StartCoroutine(BackToTrackRoutine());
    }

    private IEnumerator BackToTrackRoutine()
    {
        // Queremos que al volver, la pista esté en estado Finished
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Finished);

        AsyncOperation op = SceneManager.LoadSceneAsync(GameManager.LastTrackSceneIndex);
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            // Barra de Progreso aquí pal futuro.
            // float progress = Mathf.Clamp01(op.progress / 0.9f);

            yield return null;  // seguir esperando al siguiente frame
        }
    }
}


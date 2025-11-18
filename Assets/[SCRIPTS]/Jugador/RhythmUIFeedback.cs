using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class RhythmUIFeedback : MonoBehaviour
{
    [Header("Referencias")]
    public RhythmBoost rhythm;        // Tu script de ritmo
    public Image[] uiElements;        // Las imágenes de la interfaz (barras, fondo, etc.)

    [Header("Colores de Feedback")]
    public Color normalColor = Color.white;
    public Color successColor = Color.cyan;
    public Color failColor = Color.red;

    [Header("Tiempos de Feedback")]
    public float flashDuration = 0.6f;  // Cuánto dura el brillo
    public float failDuration = 0.8f;   // Cuánto dura el color rojo

    private bool isFlashing;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }

    void Start()
    {
        if (rhythm != null)
        {
            rhythm.OnSuccessfulBoost += OnSuccess;
            rhythm.OnFailedBoost += OnFail;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        if (rhythm != null)
        {
            rhythm.OnSuccessfulBoost -= OnSuccess;
            rhythm.OnFailedBoost -= OnFail;
        }
    }

    private void OnSuccess()
    {
        if (!isFlashing)
            StartCoroutine(FlashColor(successColor, flashDuration));
    }

    private void OnFail()
    {
        if (!isFlashing)
            StartCoroutine(FlashColor(failColor, failDuration));
    }

    private IEnumerator FlashColor(Color targetColor, float duration)
    {
        isFlashing = true;
        foreach (var img in uiElements)
            img.color = targetColor;

        yield return new WaitForSeconds(duration);

        foreach (var img in uiElements)
            img.color = normalColor;

        isFlashing = false;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

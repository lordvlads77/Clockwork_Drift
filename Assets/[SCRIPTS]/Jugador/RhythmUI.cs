using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
public class RhythmUI : MonoBehaviour
{ 
    [Header("Referencias")]
    public RhythmBoost rhythm;   // Script de ritmo
    public RectTransform leftBar;
    public RectTransform rightBar;
    public Image[] uiElements;   // Imágenes a colorear

    [Header("Movimiento")]
    public float maxDistance = 300f; // qué tan lejos empiezan las barras
    public float minDistance = 0f;   // dónde se juntan (centro)

    [Header("Feedback visual")]
    public Color normalColor = Color.white;
    public Color successColor = Color.cyan;
    public Color failColor = Color.red;
    public float flashDuration = 0.6f;
    public float failDuration = 0.8f;

    private Vector2 leftStart;
    private Vector2 rightStart;
    private bool isFlashing;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }
    

    void Start()
    {
        if (leftBar != null) leftStart = leftBar.anchoredPosition;
        if (rightBar != null) rightStart = rightBar.anchoredPosition;

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

    void Update()
    {
        if (rhythm == null) return;

        // progreso del beat (0–1)
        float progress = rhythm.GetBeatProgress();

        // movimiento lineal hacia el centro
        float distance = Mathf.Lerp(maxDistance, minDistance, progress);

        if (leftBar != null)
            leftBar.anchoredPosition = new Vector2(-distance, leftStart.y);

        if (rightBar != null)
            rightBar.anchoredPosition = new Vector2(distance, rightStart.y);
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

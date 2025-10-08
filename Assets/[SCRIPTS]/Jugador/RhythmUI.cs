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

[Header("Movimiento")]
public float maxDistance = 300f; // qué tan lejos empiezan las barras
public float minDistance = 0f;   // dónde se juntan (centro)

private Vector2 leftStart;
private Vector2 rightStart;

void Start()
{
    if (leftBar != null) leftStart = leftBar.anchoredPosition;
    if (rightBar != null) rightStart = rightBar.anchoredPosition;
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
}

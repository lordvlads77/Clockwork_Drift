using System;
using UnityEngine;

[RequireComponent(typeof(ObstacleDetector))]
public class ObstaclePenalty : MonoBehaviour
{
    [SerializeField] private int penaltyPoints = 5;
    private ObstacleDetector detector;

    private void Awake()
    {
        detector = GetComponent<ObstacleDetector>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnEnable()
    {
        if (detector != null)
            detector.OnPlayerHit += ApplyPenalty;
    }

    private void OnDisable()
    {
        if (detector != null)
            detector.OnPlayerHit -= ApplyPenalty;
    }

    private void ApplyPenalty()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SubtractScore(penaltyPoints);
            Debug.Log($"Jugador golpeó obstáculo: -{penaltyPoints} puntos");
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
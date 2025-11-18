using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackSection : MonoBehaviour
{
    [Header("Configuración de sección")]
    [SerializeField] private int pointsForPerfectRun = 100;
    [SerializeField] private List<GameObject> obstacles; // Conos y charcos de esta sección

    private bool playerInside = false;
    private bool playerHitSomething = false;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = GameStateManager.Instance.CurrentGameState == GameState.Gameplay;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            playerHitSomething = false;

            foreach (var obs in obstacles)
            {
                if (obs == null) continue;
                var detector = obs.GetComponent<ObstacleDetector>();
                if (detector != null)
                    detector.OnPlayerHit += MarkObstacleHit;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            UnsubscribeFromObstacles();

            if (!playerHitSomething)
            {
                ScoreManager.Instance.AddScore(pointsForPerfectRun);
                Debug.Log($"Perfect Section! +{pointsForPerfectRun}");
            }
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromObstacles();
    }

    private void OnDestroy()
    {
        UnsubscribeFromObstacles();
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void UnsubscribeFromObstacles()
    {
        foreach (var obs in obstacles)
        {
            if (obs == null) continue;
            var detector = obs.GetComponent<ObstacleDetector>();
            if (detector != null)
                detector.OnPlayerHit -= MarkObstacleHit;
        }
    }

    private void MarkObstacleHit()
    {
        playerHitSomething = true;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class TrackSection : MonoBehaviour
{
    [Header("Configuración de sección")]
    [SerializeField] private int pointsForPerfectRun = 100;
    [SerializeField] private List<GameObject> obstacles; // Conos y charcos de esta sección

    private bool playerInside = false;
    private bool playerHitSomething = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            playerHitSomething = false;

            foreach (var obs in obstacles)
            {
                var detector = obs.GetComponent<ObstacleDetector>();
                if (detector != null)
                    detector.OnPlayerHit += MarkObstacleHit;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerInside)
        {
            playerInside = false;

            foreach (var obs in obstacles)
            {
                var detector = obs.GetComponent<ObstacleDetector>();
                if (detector != null)
                    detector.OnPlayerHit -= MarkObstacleHit;
            }

            if (!playerHitSomething)
            {
                ScoreManager.Instance.AddScore(pointsForPerfectRun);
                Debug.Log($"Perfect Section! +{pointsForPerfectRun}");
            }
        }
    }

    private void MarkObstacleHit()
    {
        playerHitSomething = true;
    }
}

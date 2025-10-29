using UnityEngine;
using System;

public class ObstacleDetector : MonoBehaviour
{
    public event Action OnPlayerHit;
    private bool alreadyHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerHit(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePlayerHit(collision);
    }

    private void HandlePlayerHit(Collider2D collider)
    {
        if (alreadyHit) return;

        if (collider.CompareTag("Player"))
        {
            alreadyHit = true;
            OnPlayerHit?.Invoke();
        }
    }
}
using UnityEngine;
using System;

public class ObstacleDetector : MonoBehaviour
{
    public event Action OnPlayerHit;
    private bool alreadyHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alreadyHit) return;

        if (collision.collider.CompareTag("Player"))
        {
            alreadyHit = true;
            OnPlayerHit?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyHit) return;

        if (collision.CompareTag("Player"))
        {
            alreadyHit = true;
            OnPlayerHit?.Invoke();
        }
    }
}
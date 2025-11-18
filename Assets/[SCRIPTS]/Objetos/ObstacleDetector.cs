using UnityEngine;
using System;

public class ObstacleDetector : MonoBehaviour
{
    public event Action OnPlayerHit;
    private bool alreadyHit = false;
    public Animator animator;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

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
            animator.SetTrigger("hit");
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidoStopping : MonoBehaviour
{
    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalConstraints = rb.constraints;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        bool shouldFreeze =
            newState == GameState.Paused ||
            newState == GameState.Menu   ||
            newState == GameState.Finished;

        if (shouldFreeze)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (newState == GameState.Gameplay)
        {
            rb.constraints = originalConstraints;
        }
    }
}

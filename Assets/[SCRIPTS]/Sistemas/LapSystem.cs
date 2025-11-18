using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapSystem : MonoBehaviour
{
    [SerializeField] private string tagObjetivo = "Player";
    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = GameStateManager.Instance.CurrentGameState == GameState.Gameplay;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagObjetivo))
        {
            GameManager.Instance.IncrementLap();
        }
    }
    
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
    
}

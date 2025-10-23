using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private int _totalLaps = default;
    [SerializeField] private int _currentLap = default;

    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        /*GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Paused
            ? GameState.Gameplay
            : GameState.Paused;
        GameStateManager.Instance.SetState(newGameState);*/
        
    }

    public void IncrementLap()
    {
        _currentLap++;
        lapText.text = "" + _currentLap + "/" + _totalLaps;
        if (_currentLap >= _totalLaps)
        {
            //Win Event here
            Debug.Log("Finalizaste la carrera!");
        }
    }

    /*private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }*/
    
    /*private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }*/
}

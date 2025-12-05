using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int LastTrackSceneIndex { get; private set; }

    // Evento para los conos 
    public static event System.Action<int> OnLapCompleted;

    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private int _totalLaps = 3;
    [SerializeField] private int _currentLap = 0;
    [SerializeField] private string trackName;
    
    private Timer timer;
    private ScoreManager scoreManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LastTrackSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        scoreManager = FindObjectOfType<ScoreManager>();

        // Detectar nombre del Track automáticamente
        trackName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        lapText.text = _currentLap + "/" + _totalLaps;
    }


    public void IncrementLap()
    {
        _currentLap++;
        lapText.text = _currentLap + "/" + _totalLaps;

       
        OnLapCompleted?.Invoke(_currentLap);

        if (_currentLap >= _totalLaps)
        {
            UIController.Instance.ShowFinishedTrackPanel();
            Debug.Log("Finalizaste la carrera!");

            // Guardar datos de carrera
            SaveRaceResult();
        }
    }

    private void SaveRaceResult()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no está en la escena.");
            return;
        }

        float finalTime = timer != null ? timer.GetTime() : 0f;
        int finalScore = scoreManager != null ? scoreManager.GetCurrentScore() : 0;

        SaveManager.Instance.SaveResult(trackName, finalTime, finalScore);

        Debug.Log("Datos guardados exitosamente.");
    }
}
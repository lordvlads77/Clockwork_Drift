using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private int _totalLaps = 3;
    [SerializeField] private int _currentLap = 0;

    // ⬇ NUEVO: nombre de la pista (puedes editarlo en el Inspector)
    [SerializeField] private string trackName = "Pista 1";

    private Timer timer;               // ⬇ NUEVO
    private ScoreManager scoreManager; // ⬇ NUEVO

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
    }

    private void Start()
    {
        // ⬇ NUEVO: Buscar Timer y ScoreManager al iniciar
        timer = FindObjectOfType<Timer>();
        scoreManager = FindObjectOfType<ScoreManager>();

        // Mostrar texto inicial de vuelta
        lapText.text = _currentLap + "/" + _totalLaps;
    }


    public void IncrementLap()
    {
        _currentLap++;
        lapText.text = _currentLap + "/" + _totalLaps;

        if (_currentLap >= _totalLaps)
        {
            //Win Event
            UIController.Instance.ShowFinishedTrackPanel();
            Debug.Log("Finalizaste la carrera!");

            // ⬇ NUEVO: guardar datos de la carrera
            SaveRaceResult();
        }
    }

    // ⬇ NUEVO: método para guardar el resultado
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

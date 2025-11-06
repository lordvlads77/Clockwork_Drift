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

    public void IncrementLap()
    {
        _currentLap++;
        lapText.text = "" + _currentLap + "/" + _totalLaps;
        if (_currentLap >= _totalLaps)
        {
            //Win Event
            UIController.Instance.ShowFinishedTrackPanel();
            Debug.Log("Finalizaste la carrera!");
        }
    }
   
}

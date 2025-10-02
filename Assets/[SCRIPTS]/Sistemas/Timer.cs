using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    public float timerLap;
    private int  minutes, seconds, cents;

    private void FixedUpdate()
    {
        timerLap += Time.fixedDeltaTime;
        minutes = (int)(timerLap / 60f);
        seconds = (int)(timerLap - minutes * 60f);
        cents = (int)((timerLap - (int)timerLap) * 100f);
      
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }
} 
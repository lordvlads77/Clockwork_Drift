using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.IncrementLap();
    }
}

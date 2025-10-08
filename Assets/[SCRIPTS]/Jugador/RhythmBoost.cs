using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class RhythmBoost : MonoBehaviour
{
    [Header("Ritmo")]
    public float beatInterval = 1.0f;   // Duración del beat en segundos
    public float tolerance = 0.2f;      // Margen de error
    public float boostForce = 200f;     // Fuerza extra en boost
    public float penaltySlowdown = 0.5f;// Multiplicador de freno (0 = detiene por completo)

    private float beatTimer = 0f;
    private Rigidbody2D rb;
    private Movement movementScript;
    
    public delegate void BeatEvent();
    public event BeatEvent OnBeat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<Movement>();
    }

    void Update()
    {
        float prevTime = beatTimer;
        beatTimer += Time.deltaTime;
        if (beatTimer > beatInterval)
        {
            beatTimer -= beatInterval;
            // Aquí ha ocurrido un “nuevo beat”
            OnBeat?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
            CheckBeat();
    }

    void CheckBeat()
    {
        // ✅ Dentro del margen correcto
        if (beatTimer <= tolerance || beatTimer >= beatInterval - tolerance)
        {
            Vector2 boostDir = rb.velocity.normalized; // dirección del movimiento actual
            if (boostDir.sqrMagnitude > 0.1f)
            {
                rb.AddForce(boostDir * boostForce, ForceMode2D.Impulse);
                Debug.Log("Perfecto! BOOST 🚀");
            }
        }
        else
        {
            // ❌ Fallo: frenar al carro
            rb.velocity *= penaltySlowdown; 
            beatTimer = 0; // Reiniciar timing
            Debug.Log("Fallo! 🚫");
        }
    }
    public float GetBeatProgress()
    {
        return beatTimer / beatInterval;
    }

    public bool IsInTimingWindow()
    {
        return (beatTimer <= tolerance || beatTimer >= beatInterval - tolerance);
    }
}

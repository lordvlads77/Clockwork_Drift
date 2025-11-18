using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]

public class RhythmBoost : MonoBehaviour
{
    [Header("Ritmo")]
    public float songBPM = 70f;
    public float beatInterval;
    public float tolerance = 0.2f;      // Margen de error
    public float boostForce = 200f;     // Fuerza extra en boost
    public float penaltySlowdown = 0.5f;// Multiplicador de freno (0 = detiene por completo)
    public ParticleSystem ParticulaBoost;
    [SerializeField] private AudioClip BoostSoundClip;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip failSoundClip;
    private AudioSource failSource;
    private double musicStartDSP;
    public AudioSource musicSource;
    private bool restarting = false;

    private float beatTimer = 0f;
    private Rigidbody2D rb;
    //private Movement movementScript;
    
    public delegate void BeatEvent();
    public event BeatEvent OnBeat;
    
    public event Action OnSuccessfulBoost;
    public event Action OnFailedBoost;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        failSource = GetComponent<AudioSource>();
        beatInterval = 60f / songBPM;
        double startTime = AudioSettings.dspTime + 0.2; // arranca en tiempo DSP real
        musicSource.PlayScheduled(startTime);
        musicStartDSP = startTime;
    }

    void Update()
    {
        float prevTime = beatTimer;
        beatTimer = (float)(AudioSettings.dspTime - musicStartDSP) % beatInterval;
        if (beatTimer > beatInterval)
        {
            beatTimer -= beatInterval;
            OnBeat?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
            CheckBeat();
        
        
    }
    void crearParticula()
    {
        ParticulaBoost.Play();
    }
    void CheckBeat()
    {
        if (beatTimer <= tolerance || beatTimer >= beatInterval - tolerance)
        {
            Vector2 boostDir = rb.velocity.normalized; // dirección del movimiento actual
            if (boostDir.sqrMagnitude > 0.1f)
            {
                rb.AddForce(boostDir * boostForce, ForceMode2D.Impulse);
                crearParticula();
                _audioSource.clip = BoostSoundClip;
                _audioSource.Play();
                OnSuccessfulBoost?.Invoke();
                Debug.Log("Perfecto! BOOST");
            }
        }
        else
        {
            rb.velocity *= penaltySlowdown; 
            beatTimer = 0; // Reiniciar timing
            Debug.Log("Fallo!");
            OnFailedBoost?.Invoke();
            failSource.clip = failSoundClip;
            failSource.Play();
            if (!restarting)
                StartCoroutine(RestartMusic());
        }
    }
    IEnumerator RestartMusic()
    {
        restarting = true;

        // Silenciar la música de inmediato
        musicSource.Stop();

        // Esperar medio segundo
        yield return new WaitForSeconds(0.5f);

        // Programamos inicio preciso con DSP
        double startTime = AudioSettings.dspTime + 0.05;
        musicSource.PlayScheduled(startTime);

        musicStartDSP = startTime;

        // Resetear beat timer para mantener sincronía
        beatTimer = 0f;

        restarting = false;
    }
    public float GetBeatProgress()
    {
        return beatTimer / beatInterval;
    }

    public bool IsInTimingWindow()
    {
        return (beatTimer <= tolerance || beatTimer >= beatInterval - tolerance);
    }
    IEnumerator StartMusicSync()
    {
        yield return new WaitForSeconds(0.1f); // asegura que todo esté listo
        _audioSource.Play();
        beatTimer = 0f;
    }
    
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

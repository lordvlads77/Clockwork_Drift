using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{
    public static CarInputController Instance { get; private set; }
    public ParticleSystem Particuladerrape;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip DriftSoundClip;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        
        TopDownMovement.Instance.SetInputVector(inputVector);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TopDownMovement.Instance.Handbrake(true);
            crearParticula();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            TopDownMovement.Instance.Handbrake(false);
        }
    }

    void crearParticula()
    {
        Particuladerrape.Play();
        _audioSource.clip = DriftSoundClip;
        _audioSource.Play();
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

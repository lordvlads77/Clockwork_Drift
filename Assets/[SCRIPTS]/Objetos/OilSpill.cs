using System;
using UnityEngine;
public class OilSpill : MonoBehaviour
{
    [SerializeField] private float slipDuration = 2f;
    [SerializeField] private float slipForce = 2f;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip ImpactClip;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        enabled = (GameStateManager.Instance.CurrentGameState == GameState.Gameplay);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        TopDownMovement car = collision.GetComponent<TopDownMovement>();
        if (car != null)
        {
            car.Slip(slipDuration, slipForce);
            _audioSource.clip = ImpactClip;
            _audioSource.Play();
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}


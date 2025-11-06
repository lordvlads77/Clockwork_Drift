using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashEffect : MonoBehaviour
{
    [SerializeField] private string tagObjetivo = "Wall";
    public ParticleSystem crashParticle;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip CrashSoundClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag(tagObjetivo))
        {
            _audioSource.clip = CrashSoundClip;
            _audioSource.Play();
            crearParticula();
        }
    }

    void crearParticula ()
    {
        crashParticle.Play();
    }
}

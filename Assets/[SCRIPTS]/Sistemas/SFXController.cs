using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXController : MonoBehaviour
{
    public static SFXController Instance { get; private set; }
    [SerializeField] private AudioSource _sfxAudioSource = default;
    [SerializeField] private AudioClip _choqueCharcoClip = default;
    [SerializeField] private AudioClip _ChoqueConoClip = default;
    [FormerlySerializedAs("menu_MovClip")] [SerializeField] private AudioClip _menu_MovClip = default;
    [FormerlySerializedAs("menu_SelectClip")] [SerializeField] private AudioClip _menu_SelectClip = default;
    [SerializeField] private AudioClip _pauseClip = default;
    

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

    public void PlayChoqueCharcoSFX()
    {
        _sfxAudioSource.PlayOneShot(_choqueCharcoClip, 1f);
    }
    
    public void PlayChoqueConoSFX()
    {
        _sfxAudioSource.PlayOneShot(_ChoqueConoClip, 1f);
    }
    public void PlayMenuMoveSFX()
    {
        _sfxAudioSource.PlayOneShot(_menu_MovClip, 1f);
    }
    public void PlayMenuSelectSFX()
    {
        _sfxAudioSource.PlayOneShot(_menu_SelectClip, 1f);
    }

    public void PlayPauseSFX()
    {
        _sfxAudioSource.PlayOneShot(_pauseClip, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]

public class RhythmBoost : MonoBehaviour
{
[Header("Beatmap")]
    public List<float> beatTimes = new List<float>(); 
    private int currentBeatIndex = 0;
    private float songTime = 0f;

    [Header("Timing")]
    public float tolerance = 0.15f; 

    [Header("Boost")]
    public float boostForce = 200f;
    public float penaltySlowdown = 0.5f;
    public ParticleSystem ParticulaBoost;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip BoostSoundClip;
    public AudioClip failSoundClip;
    private AudioSource sfxSource;

    [Header("Score")]
    public int pointsForTiming = 5;

    private Rigidbody2D rb;
    private double musicStartDSP;
    private bool restarting = false;
    private bool waitingForMusicStart = false;

    private enum BeatState { Pending, Triggered, Processed }
    private BeatState beatState = BeatState.Pending;

    public event Action OnBeat;
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
        sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource != null)
        {
            double startTime = AudioSettings.dspTime + 0.2;
            musicSource.PlayScheduled(startTime);
            musicStartDSP = startTime;
        }
    }

    void Update()
    {
        if (waitingForMusicStart)
            return;

        if (musicSource == null || beatTimes == null || beatTimes.Count == 0)
            return;

        songTime = Mathf.Max(0f, (float)(AudioSettings.dspTime - musicStartDSP));

        currentBeatIndex = Mathf.Clamp(currentBeatIndex, 0, beatTimes.Count - 1);
        float nextBeat = beatTimes[currentBeatIndex];

        // Trigger de beat
        if (beatState == BeatState.Pending &&
            songTime >= nextBeat - tolerance &&
            songTime <= nextBeat + tolerance)
        {
            beatState = BeatState.Triggered;
            OnBeat?.Invoke();
        }

        // Se pasó la ventana -> ignorado
        if ((beatState == BeatState.Pending || beatState == BeatState.Triggered) &&
            songTime > nextBeat + tolerance)
        {
            ProcessNextBeat();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            CheckBeat();
    }

    // NUEVO — reinicio correcto al acabar todos los beats
    private void ProcessNextBeat()
    {
        beatState = BeatState.Processed;
        currentBeatIndex++;

        if (currentBeatIndex >= beatTimes.Count)
        {
            // Reinicio del beatmap sin fallar y sin reiniciar música
            currentBeatIndex = 0;
            beatState = BeatState.Pending;
            songTime = 0f;

            // Ajustamos el DSP para que coincida con tiempo 0 sin cortar la música
            musicStartDSP = AudioSettings.dspTime;

            return;
        }

        beatState = BeatState.Pending;
    }

    void CheckBeat()
    {
        float nextBeat = beatTimes[currentBeatIndex];

        if (Mathf.Abs(songTime - nextBeat) <= tolerance)
        {
            Vector2 dir = rb.velocity.normalized;

            if (dir.sqrMagnitude > 0.05f)
            {
                rb.AddForce(dir * boostForce, ForceMode2D.Impulse);
                ParticulaBoost?.Play();

                if (BoostSoundClip != null)
                    sfxSource.PlayOneShot(BoostSoundClip);

                OnSuccessfulBoost?.Invoke();
                ScoreManager.Instance?.AddScore(pointsForTiming);
            }

            beatState = BeatState.Processed;
            currentBeatIndex++;

            if (currentBeatIndex >= beatTimes.Count)
            {
                currentBeatIndex = 0;
                beatState = BeatState.Pending;
                songTime = 0f;
                musicStartDSP = AudioSettings.dspTime;
                return;
            }

            beatState = BeatState.Pending;
        }
        else
        {
            if (beatState != BeatState.Processed)
            {
                beatState = BeatState.Processed;
                HandleMiss();
            }
        }
    }

    private void HandleMiss()
    {
        rb.velocity *= penaltySlowdown;
        OnFailedBoost?.Invoke();

        if (failSoundClip != null)
            sfxSource.PlayOneShot(failSoundClip);

        StartCoroutine(RestartMusic());
    }

    IEnumerator RestartMusic()
    {
        if (restarting) yield break;
        restarting = true;

        musicSource.Stop();
        yield return new WaitForSeconds(0.25f);

        waitingForMusicStart = true;
        currentBeatIndex = 0;
        beatState = BeatState.Pending;
        songTime = 0f;

        double startTime = AudioSettings.dspTime + 0.05;
        musicSource.PlayScheduled(startTime);
        musicStartDSP = startTime;

        yield return new WaitForSeconds(0.05f);
        waitingForMusicStart = false;
        restarting = false;
    }

    public float GetBeatProgress()
    {
        int idxNext = Mathf.Clamp(currentBeatIndex, 0, beatTimes.Count - 1);
        float prevTime = (idxNext > 0) ? beatTimes[idxNext - 1] : 0f;
        float nextTime = beatTimes[idxNext];
        float duration = Mathf.Max(0.0001f, nextTime - prevTime);
        return Mathf.Clamp01((songTime - prevTime) / duration);
    }

    public bool IsInTimingWindow()
    {
        float nextBeat = beatTimes[Mathf.Clamp(currentBeatIndex, 0, beatTimes.Count - 1)];
        return Mathf.Abs(songTime - nextBeat) <= tolerance;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

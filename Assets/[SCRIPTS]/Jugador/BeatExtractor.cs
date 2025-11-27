using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
[RequireComponent(typeof(AudioSource))]
public class BeatExtractor : MonoBehaviour
{
    public float sensitivity = 0.03f;
    public float minTimeBetweenBeats = 0.2f;
    public AudioSource musicSource;

    private float[] samples = new float[1024];
    private float lastBeatTime = 0f;

    public List<float> detectedBeats = new List<float>();
    private bool recording = false;

    void Start()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!recording)
            return;

        float songTime = musicSource.time;
        musicSource.GetOutputData(samples, 0);

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
            sum += Mathf.Abs(samples[i]);

        float amplitude = sum / samples.Length;

        if (amplitude > sensitivity && songTime - lastBeatTime > minTimeBetweenBeats)
        {
            lastBeatTime = songTime;
            detectedBeats.Add(songTime);
            Debug.Log($"Beat detectado en: {songTime:F3} s");
        }
    }

    public void StartRecording()
    {
        detectedBeats.Clear();
        lastBeatTime = 0f;
        recording = true;
        Debug.Log("Grabación de beats iniciada.");
    }

    public void StopRecording()
    {
        recording = false;

        Debug.Log("Grabación detenida. Beats detectados:");
        foreach (float t in detectedBeats)
            Debug.Log(t.ToString("F3"));
    }
}

// ------------------
//   CUSTOM EDITOR
// ------------------
#if UNITY_EDITOR
[CustomEditor(typeof(BeatExtractor))]
public class BeatExtractorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BeatExtractor extractor = (BeatExtractor)target;

        GUILayout.Space(15);
        GUILayout.Label("Beat Detector Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("▶ Start Recording Beats"))
        {
            extractor.StartRecording();
            extractor.musicSource.Play();       // empieza la música automáticamente
        }

        if (GUILayout.Button("⏹ Stop Recording Beats"))
        {
            extractor.StopRecording();
            extractor.musicSource.Stop();
        }

        if (extractor.detectedBeats.Count > 0)
        {
            GUILayout.Space(10);

            if (GUILayout.Button("📋 Copy Beats to Clipboard"))
            {
                string beatText = "";
                foreach (float t in extractor.detectedBeats)
                    beatText += t.ToString("F3") + "f,\n";

                GUIUtility.systemCopyBuffer = beatText;
                Debug.Log("Beats copiados al portapapeles.");
            }

            if (GUILayout.Button("💾 Export Beats to .txt"))
            {
                string path = EditorUtility.SaveFilePanel("Save Beat Times", "", "beats.txt", "txt");

                if (!string.IsNullOrEmpty(path))
                {
                    List<string> lines = new List<string>();
                    foreach (float t in extractor.detectedBeats)
                        lines.Add(t.ToString("F3"));

                    File.WriteAllLines(path, lines);
                    Debug.Log("Archivo exportado a: " + path);
                }
            }
        }
    }
}
#endif

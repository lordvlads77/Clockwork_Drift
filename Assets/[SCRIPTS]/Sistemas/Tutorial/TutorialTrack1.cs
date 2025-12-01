using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialTrack1 : MonoBehaviour
{
    [Header("UI del Tutorial")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 4f;

    private bool tutorialStarted = false;
    private bool tutorialFinished = false;
    private bool paused = false;

    private CanvasGroup canvasGroup;

    private string[] instructions = new string[]
    {
        "HOLA @MIGO  BIENBENIDO AL TUTORIAL :) ",
        "USAR <b>W A S D</b> PARA CONTROLAR EL VEHÍCULO",
        "PRESIONA <b>ESPACIO</b> PARA FRENAR",
        "EVITA CONOS Y CHARCOS DE ACITE PARA NO PERDER PUNTOS",
        "SI LOGRAS EVITAR LOS OBSTACULOS GANARAS PUNTOS",
        "HAZ CLICK IZQUIERDO PARA SEGUIR EL RITMO DE LA MUSICA",
        "SI HACIERTAS EL RITMO EN LA BARRA INFERIOR IRAS MAS RAPIDO",
        "MANTÉN EL RITMO PARA GANAR PUNTOS",
        "COMPLETA LAS VUELTAS SIN CHOCAR",
        "SUERTE :)"
    };

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialTrack1", 0) == 0)
        {
            StartTutorial();
        }
        else
        {
            canvasGroup.alpha = 0f;
            gameplayUI.SetActive(true);
        }
    }

    public void StartTutorial()
    {
        tutorialStarted = true;
        tutorialFinished = false;
        paused = false;

        gameplayUI.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true; // permite interacción si quieres

        StartCoroutine(ShowInstructionsSequentially());
    }

    private IEnumerator ShowInstructionsSequentially()
    {
        foreach (string instruction in instructions)
        {
            tutorialText.text = instruction;

            // Fade In
            yield return StartCoroutine(FadeCanvasGroup(0f, 1f, fadeDuration));

            // Esperar displayDuration segundos respetando pausa
            float timer = 0f;
            while (timer < displayDuration)
            {
                if (!paused)
                    timer += Time.deltaTime;
                yield return null;
            }

            // Fade Out
            yield return StartCoroutine(FadeCanvasGroup(1f, 0f, fadeDuration));
        }

        FinishTutorial();
    }

    private IEnumerator FadeCanvasGroup(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (!paused)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            }
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    public void FinishTutorial()
    {
        tutorialFinished = true;
        canvasGroup.alpha = 0f;
        gameplayUI.SetActive(true);

        PlayerPrefs.SetInt("TutorialTrack1", 1);
        PlayerPrefs.Save();
    }

    public void PauseTutorial()
    {
        paused = true;
    }

    public void ResumeTutorial()
    {
        paused = false;
    }

    public bool IsTutorialActive()
    {
        return tutorialStarted && !tutorialFinished;
    }
}

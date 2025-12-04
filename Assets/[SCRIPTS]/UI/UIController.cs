using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Main Menu UI Panel")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Pause Menu UI Panel")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Settings Menu UI Panel")]
    [SerializeField] private GameObject settingsMenuPanel;

    [Header("Finished Track UI Panel")]
    [SerializeField] private GameObject finishedtrackPanel;

    [Header("Tutorial Track 1 Panel")]
    [SerializeField] private GameObject tutorialPanel;

    private TutorialTrack1 tutorialScript;
    [Header("Settings Menu from Pause Panel GObject")]
    [SerializeField] private GameObject settingsMenuFromPause = default;
    [SerializeField] private GameObject LevelsPanel = default;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (tutorialPanel != null)
            tutorialScript = tutorialPanel.GetComponent<TutorialTrack1>();

        if (GameStateManager.Instance != null)
            HandleGameStateChanged(GameStateManager.Instance.CurrentGameState);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        bool paused = newState == GameState.Paused;
        bool inMenu = newState == GameState.Menu;
        bool finished = newState == GameState.Finished;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(inMenu);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(paused);
        if (finishedtrackPanel != null) finishedtrackPanel.SetActive(finished);
        
        if (SFXController.Instance != null)
        {
            SFXController.Instance.PlayPauseSFX();
        }
        Cursor.visible = inMenu || paused || finished;
        Cursor.lockState = (inMenu || paused || finished) ?
            CursorLockMode.Confined : CursorLockMode.Locked;

        // --------- Mostrar/Ocultar texto de tutorial ----------
        if (tutorialScript != null)
        {
            if (paused)
                tutorialScript.PauseTutorial();   // oculta panel de tutorial
            else if (newState == GameState.Gameplay)
                tutorialScript.ResumeTutorial();  // muestra panel de tutorial
        }

        // Ocultar settings si no estamos en pausa
        if (!paused)
        {
            if (settingsMenuFromPause != null)
                settingsMenuFromPause.SetActive(false);

            if (settingsMenuPanel != null &&
                (mainMenuPanel == null || !mainMenuPanel.activeInHierarchy))
                settingsMenuPanel.SetActive(false);
        }
    }

    // --- PRESIONASTE PLAY EN EL MENÚ ---
    public void OnPlayFromMainMenu()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        // Mostrar tutorial solamente la primera vez
        if (PlayerPrefs.GetInt("TutorialTrack1", 0) == 0)
        {
            if (tutorialScript != null)
                tutorialScript.StartTutorial();
        }
    }

    public void OnResumeButton()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void ShowPauseMenu() => pauseMenuPanel.SetActive(true);
    public void HidePauseMenu() => pauseMenuPanel.SetActive(false);

    public void OnOpenSettingsFromPause()
    {
        if (settingsMenuFromPause != null) settingsMenuFromPause.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    /*public void OnBackFromSettingsToPause()
    {
        if (settingsMenuFromPause != null) settingsMenuFromPause.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }*/

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
        if (mainMenuPanel != null && mainMenuPanel.activeInHierarchy == true)
        {
            mainMenuPanel.SetActive(false);
        }
        else if (pauseMenuPanel.activeInHierarchy == true)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
    
    public void ToLevelsMenu()
    {
        LevelsPanel.SetActive(true);
    }
    
    public void BackFromLevelsMenu()
    {
        LevelsPanel.SetActive(false);
    }
    
    public void ShowSettingsMenuFromPause()
    {
        settingsMenuFromPause.SetActive(true);
        //pauseMenuPanel.SetActive(false);
    }

    public void HideSettingsMainMenu()
    {
        settingsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void HideSettingsPauseMenu()
    {
        settingsMenuFromPause.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitGame() => Application.Quit();

    public void GameReplay()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void ShowFinishedTrackPanel()
    {
        if (finishedtrackPanel != null)
            finishedtrackPanel.SetActive(true);

        GameStateManager.Instance.SetState(GameState.Finished);
    }

    public void Score_board()
    {
        SceneManager.LoadScene(1);
    }
}

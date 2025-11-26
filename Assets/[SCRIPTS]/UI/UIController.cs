using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [FormerlySerializedAs("_mainMenuPanel")]
    [Header("Main Menu UI Panel")]
    [SerializeField] private GameObject mainMenuPanel;
    [Header("Pause Menu UI Panel")]
    [SerializeField] private GameObject pauseMenuPanel;
    [Header("Settings Menu UI Panel")]
    [SerializeField] private GameObject settingsMenuPanel;
    [Header("Settings Menu from Pause Panel GObject")]
    [SerializeField] private GameObject settingsMenuFromPause = default;
    [SerializeField] private GameObject finishedtrackPanel = default;

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
    
    private void OnEnable()
    {
        // Nos suscribimos al cambio de estado del juego
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (GameStateManager.Instance != null)
        {
            HandleGameStateChanged(GameStateManager.Instance.CurrentGameState);
        }
    }

    //TODO: Handling of States UI Controller va primero
    
    private void HandleGameStateChanged(GameState newState)
    {
        bool paused = newState == GameState.Paused;
        bool inMenu = newState == GameState.Menu;
        bool finished = newState == GameState.Finished;
        
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(inMenu);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(paused);
        }
        
        if (finishedtrackPanel != null)
        {
            finishedtrackPanel.SetActive(finished);
        }

        if (SFXController.Instance != null)
        {
            SFXController.Instance.PlayPauseSFX();
        }
        
        bool uiActive = inMenu || paused || finished;
        Cursor.visible = uiActive;
        Cursor.lockState = uiActive ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (!paused)
        {
            if(settingsMenuFromPause != null)
                settingsMenuFromPause.SetActive(false);
            if (settingsMenuPanel != null && (mainMenuPanel == null || !mainMenuPanel.activeInHierarchy))
            {
                settingsMenuPanel.SetActive(false);
            }
        }
    }
    
    public void OnPlayFromMainMenu()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Gameplay);
    }
    
    public void OnResumeButton()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Gameplay);
    }

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
    
    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
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

    public void QuitGame()
    {
        Application.Quit();
    }
    

    public void GameReplay()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ShowFinishedTrackPanel()
    {
        if (finishedtrackPanel != null)
        {
            finishedtrackPanel.SetActive(true);
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Finished);
        }
    }

    public void Score_board()
    {
        SceneManager.LoadScene(1);
    }
    
}

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }
    }

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
        if (mainMenuPanel.activeInHierarchy == true)
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

    public void UISoundOn()
    {
        //TODO: Add sound on functionality
    }
    
    public void UISoundOff()
    {
        // TODO: Add sound off functionality
    }

    public void GameReplay()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ShowFinishedTrackPanel()
    {
        finishedtrackPanel.SetActive(true);
    }
    
}

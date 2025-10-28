using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Header("Pause Button GObject")]
    [SerializeField] private GameObject pauseButton;
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

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
    }
    
    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
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

    public void HideSettingsMenu()
    {
        settingsMenuPanel.SetActive(false);
        // TODO: Dynamic going back to the appropriate UI Panel.
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
    
}

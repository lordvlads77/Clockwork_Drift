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
    [Header("Pause Button GObject")]
    [SerializeField] private GameObject pauseButton;
    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
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

    public void QuitGame()
    {
        Application.Quit();
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameStatus status;
    
    public GameObject pausePanel;
    public GameObject mainMenuPanel;
    public GameObject pauseButton;
    public GameObject playButton;

    

    public static event Action<GameStatus> onStatus;

    private void OnEnable()
    {
        GameManager.onMainMenu += OnMainMenu;
    }

    private void OnDisable()
    {
        GameManager.onMainMenu -= OnMainMenu;
    }

    void OnMainMenu()
    {
        mainMenuPanel.SetActive(true);
        onStatus?.Invoke(GameStatus.MainMenu);
    }

    void OnGamePlay()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false); 
        pauseButton.SetActive(true);
        
        onStatus?.Invoke(GameStatus.InGame);

    }

    // ReSharper disable Unity.PerformanceAnalysis
    void OnPause()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(true); 
        pauseButton.SetActive(false);
        onStatus?.Invoke(GameStatus.Pause);

    }

    #region Click Buttons

    public void ClickStartButton()
    {
        status = GameStatus.InGame;
        OnGamePlay();
        onStatus?.Invoke(status);
    }
    
    public void ClickPauseButton()
    {
        status = GameStatus.Pause;
        OnPause();
        onStatus?.Invoke(status);
    }
    
    public void ClickContinueButton()
    {
        status = GameStatus.InGame;
        OnGamePlay();
        onStatus?.Invoke(status);
    }


    public void ClickRestratButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        status = GameStatus.InGame;
        onStatus?.Invoke(status);
    }

    #endregion
    


}

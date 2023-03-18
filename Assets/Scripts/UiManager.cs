using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameStatus status;
    public GameColors color; 
   
    [Header("Game Panels")]
    public GameObject pausePanel;
    public GameObject mainMenuPanel;
    public GameObject selectColorPanel;
    [Header("Game Buttons")]
    public GameObject pauseButton;
    public GameObject playButton;
    
    [Header("Color Buttons")]
    public Button selectColorButton;
    public Button redColorButton;
    public Button yellowColorButton; 
    public Button blueColorButton;
    public Button whiteColorButton;
    
    public static event Action<GameStatus> onStatus;
    public static event Action<GameColors> onColorChanged;

    private void Start()
    {
        
        selectColorButton.onClick.AddListener(ClickSelectColorButton);
        redColorButton.onClick.AddListener(ClickRedColorButton);
        yellowColorButton.onClick.AddListener(ClickYellowColorButton);
        blueColorButton.onClick.AddListener(ClickBlueColorButton);
        whiteColorButton.onClick.AddListener(ClickWhiteColorButton);
    }

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
        
        pausePanel.SetActive(false);
        selectColorPanel.SetActive(false);
        
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
        status = GameStatus.MainMenu;
        onStatus?.Invoke(status);
    }

    
    #endregion
    
    #region ColorClicks
    public void ClickRedColorButton()
    {
        
        
        color = GameColors.Red;
        redColorButton.transform.DOShakeScale(0.5f, 0.6f, 2, 0f)
            .OnComplete(() =>
            {
                selectColorPanel.SetActive(false);
                onColorChanged?.Invoke(color);
            });
        

    }
    public void ClickYellowColorButton()
    {
       
        color = GameColors.Yellow;
        yellowColorButton.transform.DOShakeScale(0.5f, 0.6f, 2, 0f)
            .OnComplete(() =>
            {
                selectColorPanel.SetActive(false);
                onColorChanged?.Invoke(color);
            });
    }
    public void ClickBlueColorButton()
    {
        
        color = GameColors.Blue;
        blueColorButton.transform.DOShakeScale(0.5f, 0.6f, 2, 0f)
            .OnComplete(() =>
            {
                selectColorPanel.SetActive(false);
                onColorChanged?.Invoke(color);
            });

    }
    public void ClickWhiteColorButton()
    {
      
        color = GameColors.White; 
        whiteColorButton.transform.DOShakeScale(0.5f, 0.6f, 2, 0f)
            .OnComplete(() =>
            {
                selectColorPanel.SetActive(false);
                onColorChanged?.Invoke(color);
            });
    }

    public void ClickSelectColorButton()
    {
        selectColorPanel.SetActive(true);
    }

    #endregion
    


}

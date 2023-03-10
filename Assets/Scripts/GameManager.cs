using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> Balls;

    int activeBallIndex;
    public Transform nextBallTransform;
    
    public Transform slider;
    public Transform startPosition;

    private bool isMove; 
    public static GameManager Instance;

    public TextMeshProUGUI remainingBallText;
    public int ballCount;

    private Tween _sliderTween;

    public GameObject pausePanel;
    public GameObject mainMenuPanel;
    public GameObject pauseButton;
    
    public GameObject playButton; 
    
    
    public GameStatus status; 
    
    private void Awake()
    {
        Instance = this;
    }

    public void ClickPauseButton()
    {
       status = GameStatus.Pause;
    }
    public void ClickStartButton()
    {
        status = GameStatus.InGame;
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
        isMove = true;
        _sliderTween.Play(); 
    }
    
    public void ClickContinueButton()
    {
        status = GameStatus.InGame;
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
        isMove = true;
        _sliderTween.Play(); 
    }

    private void Start()
    {
        status = GameStatus.MainMenu;
        
        isMove = true;

        _sliderTween = slider.DOLocalMoveX(3.2f, 1f).SetLoops(-1, LoopType.Yoyo);
        
        ballCount = BallPooler.Instance.ballCount;
        remainingBallText.text = ballCount.ToString();

        
        
        Balls = BallPooler.Instance.Balls;

        //index = 0
        var ball1 = Balls[activeBallIndex]; // -> index of 0
        var ball2 = Balls[activeBallIndex + 1]; // -> index of 1

        ball1.GetComponent<Ball>().ballNumber = 2;
        ball2.GetComponent<Ball>().ballNumber = 2;

        ball1.transform.position = startPosition.position;
        ball1.transform.SetParent(slider);

        ball2.transform.position = nextBallTransform.position;
        
        ball1.SetActive(true);
        ball2.SetActive(true);

    }
    
    
    
    
    private void FixedUpdate()
    {
        StartCoroutine(nameof(Shoot));
        
        if (status == GameStatus.Pause)
        {
            pausePanel.SetActive(true);
            pauseButton.SetActive(false); 
            isMove = false;
            _sliderTween.Pause();
        }
        if (status == GameStatus.MainMenu)
        {
            mainMenuPanel.SetActive(true);
            pauseButton.SetActive(false); 
            pausePanel.SetActive(false);
            isMove = false;
            _sliderTween.Pause();
        }
        
    }

    
    IEnumerator Shoot()
    {
        

        if (Input.touchCount > 0 && isMove && status == GameStatus.InGame)
        {
            var touch = Input.GetTouch(0);
            var touchPos = Camera.main.ScreenToViewportPoint(touch.position); //screen sized between 0-1
            
            
            if (touch.phase == TouchPhase.Began && touchPos.y < 0.8f)
            {
                 isMove = false; 
                 _sliderTween.Pause();
            
            if (ballCount == 0)
            {
                Debug.Log("@@--DONE!");
                slider.gameObject.SetActive(false);
            }
                        
            
            
            
            Balls[activeBallIndex].transform.SetParent(null);
            Balls[activeBallIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (!isMove && Balls.Count > activeBallIndex + 1)
            {
                Balls[activeBallIndex + 1].transform.DOMove(startPosition.position, 0.75f).OnComplete(() =>
                    {
                        Balls[activeBallIndex].transform.SetParent(slider);
                        isMove = true;
                    });
            }
            
            

            activeBallIndex++;
            
            ballCount--;
            remainingBallText.text = ballCount.ToString();
            
            
            if (!isMove && Balls.Count > activeBallIndex + 1)
            {
                Balls[activeBallIndex].transform.DOMove(startPosition.position, 0.75f).OnComplete(() =>
                {
                    _sliderTween.Play();
                    Balls[activeBallIndex].transform.SetParent(slider);
                    isMove = true;
                    
                }); 
                
                Balls[activeBallIndex + 1].transform.position = nextBallTransform.position;
                Balls[activeBallIndex + 1].transform.SetParent(null);
            
                Balls[activeBallIndex].SetActive(true);
                Balls[activeBallIndex + 1].SetActive(true);
                
                
                Balls[activeBallIndex + 1].transform.DOScale(Vector3.zero, 1f).From();
            }
            yield return new WaitForSeconds(0.25f);
                
            }
            
            
            
        }

    }
    
    
}

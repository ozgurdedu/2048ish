using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    
    public GameStatus status;
    
    
    //***
    public static event UnityAction onMainMenu;

    private void Awake()
    {
        Instance = this;
    }
    
    

    private void Start()
    {
        status = GameStatus.MainMenu;
        onMainMenu?.Invoke();
        isMove = true;
        
        _sliderTween = slider.DOLocalMoveX(3.2f, 1f).SetLoops(-1, LoopType.Yoyo);
        _sliderTween.Pause();
        
        
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
        
    }
    
    IEnumerator Shoot()
    {
        if (Input.touchCount > 0 && isMove && status == GameStatus.InGame)
        {
            var touch = Input.GetTouch(0);
            var touchPos = Camera.main.ScreenToViewportPoint(touch.position); //screen sized between 0-1
            if (touch.phase == TouchPhase.Began && touchPos.y < 0.8f)
            {
                
                CanTheBallCollide();
                _sliderTween.Pause();
                isMove = false;
                
                // if (ballCount == 0)
            // {
            //     Debug.Log("@@--DONE!");
            //     slider.gameObject.SetActive(false);
            // }
            
            Balls[activeBallIndex].transform.SetParent(BallPooler.Instance.transform);
            Balls[activeBallIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (!isMove && Balls.Count > activeBallIndex + 1)
            {
                Balls[activeBallIndex + 1].transform.DOMove(startPosition.position, 0.75f).OnComplete(() =>
                    {
                        Balls[activeBallIndex].transform.SetParent(slider);
                        isMove = true;
                        _sliderTween.Play();
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
                Balls[activeBallIndex + 1].transform.SetParent(BallPooler.Instance.transform);
            
                Balls[activeBallIndex].SetActive(true);
                Balls[activeBallIndex + 1].SetActive(true);
                
                
                Balls[activeBallIndex + 1].transform.DOScale(Vector3.zero, 1f).From();
                
                
            }

            }

            yield return new WaitForSeconds(0.25f);

           
            
        }

    }


    public void CanTheBallCollide()
    {
        foreach (var ball in Balls)
        {
            var _transform = ball.transform;
            if (_transform.position.y > 1.3f)
                ball.GetComponent<Ball>().canCollide = false;
            else
                ball.GetComponent<Ball>().canCollide = true;
        }
        
    }
    
    private void OnEnable()
    {
        UiManager.onStatus += OnStatus;
    }
    
    private void OnDisable()
    {
        UiManager.onStatus -= OnStatus;
    }
    
    private void OnStatus(GameStatus _status)
    {
        status = _status;
        switch (status)
        {
            case GameStatus.MainMenu:
                _sliderTween.Pause();
                isMove = false;
                break; 
            case GameStatus.InGame:
                _sliderTween.Play();
                isMove = true;
                break; 
            case GameStatus.Pause:
                _sliderTween.Pause();
                isMove = false;
                break; 
        }

    }
    
}


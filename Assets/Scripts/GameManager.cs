using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        ballCount = BallPooler.Instance.ballCount;
        remainingBallText.text = ballCount.ToString();

        isMove = true;
        
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
        SliderMove();
        StartCoroutine(nameof(Shoot));

    }


    IEnumerator Shoot()
    {
        if (Input.GetMouseButtonDown(0) && isMove)
        {
            ballCount--;
            remainingBallText.text = ballCount.ToString();
            
            if (ballCount == 0)
            {
                Debug.Log("@@--DONE!");
            }
            
            isMove = false;
            
            Balls[activeBallIndex].transform.SetParent(null);
            Balls[activeBallIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (!isMove)
            {
                Balls[activeBallIndex + 1].transform.DOMove(startPosition.position, 0.75f).OnComplete(() =>
                {
                    Balls[activeBallIndex].transform.SetParent(slider);
                    isMove = true;
                }); 
            }
            
            
            //Index was out of range.
            if (activeBallIndex <= Balls.Count)
                activeBallIndex++;
            else
            {
                Debug.Log("abi" + activeBallIndex);
                Debug.Log("bc" + ballCount);
                Debug.Log("ballsC" + Balls.Count);
            }
            
            
            if (!isMove)
            {
                Balls[activeBallIndex].transform.DOMove(startPosition.position, 0.75f).OnComplete(() =>
                {
                    Balls[activeBallIndex].transform.SetParent(slider);
                    isMove = true;
                }); 
            }
            
            Balls[activeBallIndex + 1].transform.position = nextBallTransform.position;
            Balls[activeBallIndex + 1].transform.SetParent(null);
            
            Balls[activeBallIndex].SetActive(true);
            
            Balls[activeBallIndex + 1].SetActive(true);
            Balls[activeBallIndex + 1].transform.DOScale(Vector3.zero, 1f).From();


        
            
            
            yield return new WaitForSeconds(1f);
        }

    }
    
    
    private void SliderMove()
    {
        float h = Input.GetAxis("Mouse X");
        slider.Translate(isMove ? (h * 0.25f) : 0, 0, 0);
        
        float x = Mathf.Clamp(slider.position.x, -1.5f, 1.5f);
        slider.position = new Vector3(x, 0.85f);
    }
}

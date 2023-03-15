using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class BallPooler : MonoBehaviour
{
    public static BallPooler Instance;
    
    public GameObject BallPrefab;
    [HideInInspector] public GameObject Ball;
    public int ballCount;
    public List<GameObject> Balls = new List<GameObject>();
    [HideInInspector] public int activeBallIndex = 0;
    public Transform startPosition;
    public Transform sliderPosition;
    [HideInInspector]public GameObject ball1, ball2;
   
    
    
    [SerializeField] private List<Sprite> sprites;
    public IDictionary<int, Sprite> BallDictionary = new Dictionary<int, Sprite>();

    private void Awake()
    {
        Instance = this;
        
        
        
        for (int i = 0; i < sprites.Count; i++)
        {
            BallDictionary.Add((int)Mathf.Pow(2,i + 1), sprites[i]);
        }
        
        for (int i = 0; i < ballCount; i++)
        {
            Ball = Instantiate(BallPrefab, transform);
            Ball.SetActive(false);
            
            List<int> numberList = new List<int>(BallDictionary.Keys);
            var ni = Random.Range(0, numberList.Count);
          
            Ball.GetComponent<Ball>().ballNumber = numberList[ni] > 1024 ? 1024 : numberList[ni] ;
            
            Balls.Add(Ball);
        }
    }
    
  
    
    public GameObject GetBall()
    {
        foreach (var ball in Balls)
        {
            if (!ball.activeInHierarchy)
                return ball;
        }
        return null;
    }
}

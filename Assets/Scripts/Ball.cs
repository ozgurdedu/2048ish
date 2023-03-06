 using System;
 using System.Collections;
 using System.Collections.Generic;
 using TMPro;
 using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ballNumberText;
    public int ballNumber;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    

    void Start()
    {
        gameObject.tag = ballNumber.ToString();
        ballNumberText.text = ballNumber.ToString(); 
       
        foreach (var dict in BallPooler.Instance.BallDictionary)
        {
            if (ballNumber == dict.Key)
                spriteRenderer.sprite = dict.Value;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ballNumber.ToString()) && ballNumber <= 1024)
        {
            
            if (!effect.isPlaying)
                effect.Play();
            collision.gameObject.SetActive(false);
            ballNumber += ballNumber;
            gameObject.tag = ballNumber.ToString();
            ballNumberText.text = ballNumber.ToString();
            spriteRenderer.sprite = BallPooler.Instance.BallDictionary[ballNumber];
            

        }
        if (collision.gameObject.CompareTag(ballNumber.ToString()) && ballNumber > 1024)
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(300f,300f));    
            //bomb effect
            //2048 - 2048 çarpışırsa bombaya dönüşsün ve değdiğinde ortalığı patlatsın.
        }
        
    }
}

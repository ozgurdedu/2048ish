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
    [SerializeField] private ParticleSystem effect2048;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool canCollide;

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
        if (collision.gameObject.CompareTag(ballNumber.ToString()) && ballNumber <= 1024 && canCollide)
        {
            
            if (!effect.isPlaying)
                effect.Play();
            collision.gameObject.SetActive(false);
            ballNumber += ballNumber;
            gameObject.tag = ballNumber.ToString();
            ballNumberText.text = ballNumber.ToString();
            spriteRenderer.sprite = BallPooler.Instance.BallDictionary[ballNumber];
            

        }
        if (collision.gameObject.CompareTag(ballNumber.ToString()) && ballNumber > 1024 && canCollide)
        {
            Debug.Log("2048ler degdi");
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(300f,300f));    
            //bomb effect
            //2048 - 2048 çarpışırsa bombaya dönüşsün ve değdiğinde ortalığı patlatsın.

            effect2048.transform.position = transform.position; 
            effect2048.gameObject.SetActive(true);
            if (!effect2048.isPlaying)
                effect2048.Play();
            
            
            var bp = BallPooler.Instance.gameObject;
            foreach (var b in bp.GetComponentsInChildren<Transform>())
            { 
                if(b.gameObject.activeInHierarchy)
                {
                    if (b.gameObject.name == "Ball(Clone)" && b.transform.position.y < 1f)
                    {
                        b.gameObject.SetActive(false);
                       
                    }
                }
            }

            
            
        }
        
    }
}

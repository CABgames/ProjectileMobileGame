/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Enemy.cs
///Developed by Charlie Bullock
///This class is checks for cannon projectile position and when hit will plaly a death animation
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Sprite[] deathSprites;
    [SerializeField]
    private float time;
    [SerializeField]
    private AudioClip deathSound;
    private AudioManager aM;
    private float tempTime;
    private int spriteCounter = 0;
    private bool death = false;
    private LevelManager lM;
    #endregion Variables

    //Start function assigns aM variable the AudioManager class, tempTime is set to the value of time and the first sprite for the spriterenderer is assigned
    private void Start()
    {
        aM = GameObject.FindObjectOfType<AudioManager>();
        lM = GameObject.FindObjectOfType<LevelManager>();
        tempTime = time;
        GetComponent<SpriteRenderer>().sprite = sprites[spriteCounter];
    }

    //In this update function the sprites array is looped through and then reset continuosly until the death variable is true that at which point deathSprites array is played through once
    void Update()
    {
        tempTime -= Time.deltaTime;
        if (tempTime <= 0)
        {
            //If death is false the sprites array is to be looped through constantly
            if (death == false)
            {
                if (spriteCounter >= sprites.Length - 1)
                {
                    spriteCounter = 0;
                }
                else
                {
                    spriteCounter++;
                }
                GetComponent<SpriteRenderer>().sprite = sprites[spriteCounter];
            }
            //Else the deathSprites is looped through once
            else
            {
                if (spriteCounter != deathSprites.Length -1)
                {
                    spriteCounter++;
                    GetComponent<SpriteRenderer>().sprite = deathSprites[spriteCounter];
                }
            }
            tempTime = time;
        }
    }
    //OnCollisionEnter function checks for collision with a projectile and if death variable not true this enemy goes through the death process
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" && death == false)
        {
            death = true;
            spriteCounter = 0;
            tempTime = time;
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.025f, -0.1591432f);
            GetComponent<BoxCollider2D>().size = new Vector2 (0.155f,0.002f);
            StartCoroutine(DestroyInTime());
        }
    }

    //Coroutine plays audio clip then destroys this enemy gameobject after three seconds
    IEnumerator DestroyInTime()
    {
        aM.PlayClip(deathSound);
        yield return new WaitForSeconds(3);
        lM.timer += 150;
        Destroy(gameObject);
    }
}

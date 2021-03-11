/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///SpritePlayer.cs
///Developed by Charlie Bullock
///This class can play sprites as an animation at a given speed
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePlayer : MonoBehaviour
{
    //Variables
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private float time;
    [SerializeField]
    private bool destroyOnCompletion;
    private float tempTime;
    private int spriteCounter = 0;
    
    //At the start the temporary time is assigned the value of time and the sprite renderer is assigned a sprite from the sprites array
    private void Start()
    {
        tempTime = time;
        GetComponent<SpriteRenderer>().sprite = sprites[spriteCounter];
    }

    //In this update function sprite frames are looped through with the set time variable determining loop speed and a spritecounter changing as the array is cycled through
    void Update()
    {
        //Subtract delta time from the size of time remaining in the variable
        tempTime -= Time.deltaTime;
        //If no time is remaining
        if (tempTime <= 0)
        {
            if (spriteCounter >= sprites.Length - 1)
            {
                //If destroyOnCompletion enabled the gameobject if destroyed after the sprite array animation has played
                if (destroyOnCompletion)
                {
                    Destroy(gameObject);
                }
                //Reset sprite counter back to zero
                else
                {
                    spriteCounter = 0;
                }
            }
            //Increment sprite counter
            else
            {
                spriteCounter++;
            }
            //Set the sprite image for the sprite renderer to the correct element sprite from the array
            GetComponent<SpriteRenderer>().sprite = sprites[spriteCounter];
            //Set time back to the assigned default size
            tempTime = time;
        }
    }
}

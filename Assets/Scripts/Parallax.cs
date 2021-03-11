/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Parallax.cs
///Developed by Charlie Bullock
///This class tracks the cameras position and adjusts itself with a variable offset
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //Variables
    private float length, startPos;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private float parallaxEffect;


    //Start function sets the startPos variable to the gameobject current x axis value and the length to the size of the sprite renderer x axis bounds
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    //This update function sets the parallax gameobject to the it's original position offset by the distance
    void Update()
    {
        float distance = (camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}

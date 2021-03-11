/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: LaunchPlayer.cs 
//Author: Charlie Bullock, based upon and using help from CT5009 intro to Unity physics tutorial
///Description: Script for launching different types of cannon projectiles
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LaunchProjectile : MonoBehaviour
{
    //Variables
    #region Variables
    private bool preparing = false;
    private bool bTouchedStageFloor = true;
    [SerializeField]
    private float defaultPower;
    [SerializeField]
    private GameObject explosionPoint;
    [SerializeField]
    private GameObject explosion;
    private float temporaryPower;
    private float distance;
    private Vector3 priorVelocity;
    private Vector3 velocity;
    private Prediction prediction;
    [SerializeField]
    private GameObject[] projectiles;
    private int currentProjectile;
    private GameObject cannon;
    private LevelManager lM;
    private AudioManager aM;
    [SerializeField]
    private AudioClip fireSound;
    #endregion Variables

    //Start function gets the rigidbody and prediction component
    private void Start()
    {
        lM = FindObjectOfType<LevelManager>();
        aM = GameObject.FindObjectOfType<AudioManager>();
        currentProjectile = 0;
        prediction = GetComponent<Prediction>();
        if (projectiles.Length <= 0)
        {
            Debug.Log("More elements need adding to array.");
        }
        lM.RemainingShots(projectiles.Length - currentProjectile);
    }

    //Function checks if the game has not ended when the mouse is down along with if the player ball is touching floor and if so will freeze the gameobject before it will fire it
    private void Update()
    {
        if (lM.GetDistance() <= 1.2f && Input.GetMouseButtonDown(0) && lM.gameState == 0)
        {
            lM.NowFiring(false);
            if (currentProjectile <= projectiles.Length - 1)
            {
                switch (projectiles[currentProjectile].GetComponent<Projectile>().GetProjectileType())
                {         
                    //BluePotion
                    case 0:
                        temporaryPower = defaultPower * 6;
                        break;
                    //Brick
                    case 1:
                        temporaryPower = defaultPower * 4;
                        break;
                    //Bullet
                    case 2:
                        temporaryPower = defaultPower * 11;
                        break;
                    //Cannon ball
                    case 3:
                        temporaryPower = defaultPower * 4.25f;
                        break;
                    //Cone
                    case 4:
                        temporaryPower = defaultPower * 9f;
                        break;
                    //GreenPotion
                    case 5:
                        temporaryPower = defaultPower * 6;
                        break;
                    //HolyGrenade
                    case 6:
                        temporaryPower = defaultPower * 6.5f;
                        break;
                    //MagmaBall
                    case 7:
                        temporaryPower = defaultPower * 3.5f;
                        break;
                    //PurplePotion
                    case 8:
                        temporaryPower = defaultPower * 6;
                        break;
                    //RedPotion
                    case 9:
                        temporaryPower = defaultPower * 6;
                        break;
                    //Skull
                    case 10:
                        temporaryPower = defaultPower * 7;
                        break;
                    //Wing
                    case 11:
                        temporaryPower = defaultPower * 10f;
                        break;
                }
                //Turn on the update frame mouse direction
                preparing = true;
            }
        }
        if (preparing)
        {
            //Get camera's negatize z axis pos (or distance from camera to our target)
            float z = -Camera.main.transform.position.z;
            //Screen to world point converts our X and Y screen position into world space 'Z' distance away from our camera
            //As our camera is -z from 0, this will end up on the XY plane (or where Z is equal to 0)
            Vector3 mousePositionInWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z));
            //Calculate the distance from the plater position from the mouse position in world space
            Vector3 differenceDistance = transform.position - mousePositionInWorldSpace;
            //Draw the mouse line
            Debug.DrawLine(mousePositionInWorldSpace, transform.position, Color.white);
            distance = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
            if (distance < 1.18f && distance > 1f)
            {
                velocity = differenceDistance * temporaryPower;
                priorVelocity = velocity;
            }
            else
            {
                priorVelocity = differenceDistance * temporaryPower;
                velocity =Vector3.Lerp(transform.position,priorVelocity,0.9f - (distance * 0.1f)) ;
            }

            if (Input.GetMouseButtonUp(0) && cannon == null)
            {
                preparing = false;
                Fire(velocity);
                bTouchedStageFloor = false;
            }
            else if (cannon == null || currentProjectile == 0)
            {
                //transform.right = new Vector3(transform.position.x - 180, transform.position.y, transform.position.z);
                if (prediction != null /*&& lM.enablePrediction*/)
                {

                    prediction.Predict(velocity);
                }

                if (GetComponent<LineRenderer>().positionCount >= 1)
                {
                    transform.right = GetComponent<LineRenderer>().GetPosition(1) - transform.position;
                }
            }
        }
    }

    //Returns position of projectile
    public Vector3 ProjectilePosition()
    {
        if (cannon != null)
        {
            return cannon.transform.position;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    //Function which when called is fired in the specified direction
    private void Fire(Vector2 velocity)
    {
        aM.PlayClip(fireSound);
        lM.NowFiring(true);
        gameObject.GetComponent<LineRenderer>().positionCount = 0;
        cannon = Instantiate(projectiles[currentProjectile], transform.position,transform.rotation);
        Instantiate(explosion, explosionPoint.transform.position, explosionPoint.transform.rotation);
        cannon.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
        
        currentProjectile++;
        lM.RemainingShots(projectiles.Length - currentProjectile);
        StartCoroutine(WaitForSeconds());

    }

    //Coroutine for waiting 5 seconds
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(5);
        cannon = null;
    }
}

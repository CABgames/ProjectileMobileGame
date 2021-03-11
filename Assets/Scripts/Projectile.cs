/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Projecile.cs
///Developed by Charlie Bullock
///This class is on all cannon projectiles and ensures the correct projectile attributes can be accessed by other classes
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private int projectileType;
    private bool countDown = false;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject dummyPotion;
    [SerializeField]
    private AudioClip abilitySound;
    [SerializeField]
    private AudioClip collisionSound;
    private bool duplicated = false;
    private LevelManager lM;
    private AudioManager aM;
    #endregion Variables

    //In this start function the aM variable is assigned the audio manager class, the particle system is halted and the lM variable is assigned the level manager class
    private void Start()
    {
        aM = GameObject.FindObjectOfType<AudioManager>();
        gameObject.GetComponent<ParticleSystem>().Stop();
        lM = FindObjectOfType<LevelManager>();
    }

    //In this update function when the player clicks or taps the projectile will use it's unique ability type which can be seen below and they will all emit a particle effect along with play the ability sound
    private void Update()
    {
        //If the player clicks or pressed and has not yet already then this projectile can utilise it's ability
        if (Input.GetMouseButtonDown(0) && duplicated == false)
        {
            aM.PlayClip(abilitySound);
            gameObject.GetComponent<ParticleSystem>().Emit(100);
            duplicated = true;
            switch (projectileType)
            {
                //BluePotion
                case 0:
                    if (dummyPotion != null)
                    {
                        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                        Instantiate(dummyPotion, transform.position, transform.rotation);
                    }
                    break;
                //Brick
                case 1:
                    transform.localScale = transform.localScale * 2;
                    GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10, ForceMode2D.Impulse);
                    break;
                //Bullet
                case 2:
                    transform.rotation = Quaternion.Euler (0,0,0);
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                    break;
                //Cannon
                case 3:
                    GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 8, ForceMode2D.Impulse);
                    break;
                //Cone
                case 4:
                    transform.localScale = transform.localScale * 0.5f;
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    break;
                //GreenPotion
                case 5:
                    transform.localScale = transform.localScale * 2f;
                    GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 20, ForceMode2D.Impulse);
                    break;
                //HolyGrenade
                case 6:
                    PushOrPullObjects(false);
                    lM.Shake(1, 0.05f, gameObject);
                    break;
                //MagmaBall
                case 7:
                    PushOrPullObjects(true);
                    break;
                //PurplePotion
                case 8:
                    transform.localScale = transform.localScale * 0.75f;
                    break;
                //RedPotion
                case 9:
                    transform.localScale = transform.localScale * 1.25f;
                    break;
                //Skull
                case 10:
                    transform.localScale = transform.localScale * 1.5f;
                    break;
                //Wing
                case 11:
                    GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 50, ForceMode2D.Impulse);
                    break;
            }
        }
    }

    //This function simply pulls rigidbody 2D objects within a overlap distance of this projectile inwards or outwards depending on the state of the boolean parameter given when this function is called
    private void PushOrPullObjects(bool pulling)
    {
        //Get gameobjects with colliders within the phyics 2D overlap sphere distance of 5 from this gameobjects position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);
        //Foreach loop for objectives
        foreach (Collider2D collide in colliders)
        {
            //If this element from the colliders array also has a rigidbody 2D then apply force inward or outward from this gameobject depending on the state of the boolean parameter of this function
            if (collide.GetComponent<Rigidbody2D>())
            {
                Vector2 force;
                if (pulling)
                {
                    force = (collide.transform.position + transform.position) * 70;
                }
                else
                {
                    force = (collide.transform.position - transform.position) * 70;
                }
                Rigidbody2D rb = collide.transform.GetComponent<Rigidbody2D>();
                rb.AddForce(force);
            }
        }
    }

    //This function is used to get the correct projectile type in before it is launched in order to correctly give a correct arch prediction for the line renderer of the projectiles path
    public int GetProjectileType()
    {
        return projectileType;
    }

    //Function which allows player to fire again when they collide with Stages and reset the players rigidbody states
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (projectileType == 11)
        {
            //Check if gone beyond array's size
            lM.GameLost();
        }
        //If this object is not tagged as either player or projectile
        else if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile")
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Enemy>() == null)
            {
                //If projectiles
                switch (projectileType)
                {
                    //Purple potion
                    case 8:
                        collision.gameObject.transform.localScale = collision.gameObject.transform.localScale * 0.75f;
                        break;
                    //RedPotion
                    case 9:
                        collision.gameObject.transform.localScale = collision.gameObject.transform.localScale * 1.25f;
                        break;
                    //Skull
                    case 10:
                        Destroy(collision.gameObject);
                        break;
                    default:
                        break;
                }
            }
            //Setting the collided gameobject tag to projectile and adding the SetProjectile class to it
            collision.gameObject.tag = "Projectile";
            collision.gameObject.AddComponent<SetProjectile>();
            //If the  explosion and countdown are false then these both must happen
            if (explosion != null && countDown == false)
            {
                //Sound played, explosion instantiated, countdown set to true and screen will shake
                aM.PlayClip(collisionSound);
                Instantiate(explosion, collision.transform.position,collision.transform.rotation);
                collision.transform.gameObject.SetActive(false);
                countDown = true;
                collision.transform.gameObject.SetActive(true);
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
                StartCoroutine( lM.Shake(1.5f, 0.2f, gameObject));
            }
        }
    }
}

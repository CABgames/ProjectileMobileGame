/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///SetProjectile.cs
///Developed by Charlie Bullock
///This class lets the gameobject it is attached to add the script to other gameobjects it collides to and set them to projectiles
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProjectile : MonoBehaviour
{

    //Function which allows player to fire again when they collide with Stages and reset the players rigidbody states
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision object not player or projectile then said object becomes a projectile (this allows moved/hit blocks to cause damage to enemies) 
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile")
        {
            collision.gameObject.tag = "Projectile";
            collision.gameObject.AddComponent<SetProjectile>();
        }
    }

}

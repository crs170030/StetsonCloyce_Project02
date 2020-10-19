using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *TODO:
 * Make Projectile move in direction its facing 
 * Make parent projectile not disappear
*/

public class LazerController : MonoBehaviour
{
    //Need to make sure it is facing a correct inherited direction when fired
    public Transform target;
    //add reference to impact particle system when lazers make their impact?

    //public float projectileSpeed = .2f;
    public int projectileDamage = 20;
    //public float lifeTime = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Lazer Hit : " + other.name);

        //detect if it's the player
        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();
        //if it is a targeted block, get component
        Target _target = other.gameObject.GetComponent<Target>();

        //if valid:
        if (_player != null)
        {
            Debug.Log("Lazer has hit " + _player.name);

            _player.TakeDamage(projectileDamage);

        }else if(_target != null)
        {
            Debug.Log("Lazer has hit " + _target.name);

            _target.TakeDamage(projectileDamage);
        }

        //destroy lazer when it hits any object
        Destroy(gameObject);
    }
}
